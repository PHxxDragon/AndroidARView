using UnityEngine;
using EAR.View;
using EAR.WebRequest;
using System.Collections.Generic;
using EAR.SceneChange;
using UnityEngine.SceneManagement;

namespace EAR.MenuScene.Presenter
{
    public class ModuleListPresenter : MonoBehaviour
    {
        [SerializeField]
        private ModuleListView moduleListView;

        [SerializeField]
        private ScreenNavigator screenNavigator;

        [SerializeField]
        private WebRequestHelper webRequest;

        [SerializeField]
        private ModalShower modalShower;

        [SerializeField]
        private BreadCrumbView breadCrumbView;

        [SerializeField]
        private CourseListView courseListView;

        private const string COURSE = "Course";
        private const int DUMMY_SECTION_ID = -2345;

        private List<SectionData> sectionStack = new List<SectionData>();
        private List<SectionData> sections = new List<SectionData>();

        private int courseId = -1;
        private string courseName = "";

        void Start()
        {
            moduleListView.ModuleListRefreshEvent += (courseId, courseName) => {
                this.courseId = courseId;
                this.courseName = courseName;
                sectionStack.Clear();
                breadCrumbView.PopulateData(sectionStack);
                webRequest.GetModuleList(courseId, 
                (response) => {
                    sections = response;
                    sections.Sort((sectionData1, sectionData2) => sectionData1.id - sectionData2.id);
                    sections.ForEach(sectionData => sectionData.modules.Sort((module1, module2) => module1.createdAt.CompareTo(module2.createdAt)));
                    SectionData dummySection = new SectionData();
                    dummySection.name = LocalizationUtils.GetLocalizedText(COURSE);
                    dummySection.id = DUMMY_SECTION_ID;
                    moduleListView.SetHeaderTitle(courseName);
                    PushSection(dummySection);
                    LoadState();
                }, 
                (error) => {
                    modalShower.ShowErrorModal(error, () =>
                    {
                        BackToCourseList();
                    });
                });
            };
            moduleListView.BackButtonClickEvent += () =>
            {
                BackToSection(sectionStack.Count - 2);
            };
            breadCrumbView.OnBreadCrumbItemClick += (index) =>
            {
                BackToSection(index);
            };
        }

        private void BackToSection(int index)
        {
            if (index >= sectionStack.Count || index < -1)
            {
                Debug.LogError("Invalid index");
            } 
            else
            {
                if (index == sectionStack.Count - 1) return;

                int removeCount = sectionStack.Count - index - 1;

                for (int i = 0; i < removeCount; i++)
                {
                    sectionStack.RemoveAt(index + 1);
                }

                breadCrumbView.PopulateData(sectionStack);
                if (sectionStack.Count > 0)
                {
                    PopulateSection(sectionStack[sectionStack.Count - 1]);
                }
                else
                {
                    BackToCourseList();
                }
            }
        }
        
        private void BackToCourseList()
        {
            screenNavigator.OpenView(courseListView);
            courseListView.Refresh();
        }

        private void PushSection(SectionData sectionData)
        {
            sectionStack.Add(sectionData);
            breadCrumbView.PopulateData(sectionStack);
            PopulateSection(sectionData);
        }

        private void LoadState()
        {
            if (MenuSceneParam.sectionId == DUMMY_SECTION_ID) return;

            List<SectionData> traverseUpList = new List<SectionData>();
            SectionData sectionData = GetSection(MenuSceneParam.sectionId);
            while (sectionData != null)
            {
                traverseUpList.Add(sectionData);
                sectionData = GetSection(sectionData.parentSectionId);
            }
            traverseUpList.Reverse();
            sectionStack.AddRange(traverseUpList);
            breadCrumbView.PopulateData(sectionStack);
            PopulateSection(sectionStack[sectionStack.Count - 1]);
        }

        private SectionData GetSection(int id)
        {
            foreach (SectionData data in sections)
            {
                if (data.id == id)
                {
                    return data;
                }
            }
            return null;
        }

        private List<SectionData> GetChildrenSection(int id)
        {
            List<SectionData> result = new List<SectionData>();
            foreach (SectionData data in sections)
            {
                if (data.parentSectionId == id || (id == DUMMY_SECTION_ID && data.parentSectionId == 0))
                {
                    result.Add(data);
                }
            }
            return result;
        }

        private void PopulateSection(SectionData sectionData)
        {
            sectionData.childrenSection = GetChildrenSection(sectionData.id);

            List<object> data = new List<object>();
            data.AddRange(sectionData.childrenSection);
            foreach(SectionData sectionData1 in sectionData.childrenSection)
            {
                sectionData1.sectionClickEvent = () =>
                {
                    PushSection(sectionData1);
                };
            }
            foreach (ModuleData moduleData in sectionData.modules)
            {
                if (moduleData.moduleType == "ar")
                {
                    moduleData.moduleClickEvent = OpenARModule;
                    data.Add(moduleData);
                }
            }
            moduleListView.PopulateData(data);
        }

        private void OpenARModule(int id)
        {
            webRequest.GetARModuleData(id,
            (arModule) =>
            {
                ARSceneParam.assetInformation = arModule;
                MenuSceneParam.courseId = courseId;
                MenuSceneParam.courseName = courseName;
                MenuSceneParam.modelId = -1;
                MenuSceneParam.sectionId = sectionStack[sectionStack.Count - 1].id;
                SceneManager.LoadScene("ARScene");
            }, (error) =>
            {
                modalShower.ShowErrorModal(error);
            });
        }
    }
}

