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

        private const string COURSE = "Course";

        private List<SectionData> sectionStack = new List<SectionData>();
        private List<SectionData> sections = new List<SectionData>();

        void Start()
        {
            moduleListView.ModuleListRefreshEvent += (courseId, courseName) => {
                sectionStack.Clear();
                breadCrumbView.PopulateData(sectionStack);
                webRequest.GetModuleList(courseId, 
                (response) => {
                    sections = response;
                    sections.Sort((sectionData1, sectionData2) => sectionData1.id - sectionData2.id);
                    sections.ForEach(sectionData => sectionData.modules.Sort((module1, module2) => module1.createdAt.CompareTo(module2.createdAt)));
                    SectionData dummySection = new SectionData();
                    dummySection.childrenSection = GetChildrenSection(0);
                    dummySection.name = LocalizationUtils.GetLocalizedText(COURSE);
                    moduleListView.SetHeaderTitle(courseName);
                    PushSection(dummySection);
                }, 
                (error) => {
                    Debug.Log(error);
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
                    screenNavigator.GoBack();
                }
            }
        }

        private void PushSection(SectionData sectionData)
        {
            sectionStack.Add(sectionData);
            breadCrumbView.PopulateData(sectionStack);
            PopulateSection(sectionData);
        }

        private List<SectionData> GetChildrenSection(int id)
        {
            List<SectionData> result = new List<SectionData>();
            foreach (SectionData data in sections)
            {
                if (data.parentSectionId == id)
                {
                    result.Add(data);
                }
            }
            return result;
        }

        private void PopulateSection(SectionData sectionData)
        {
            List<object> data = new List<object>();
            data.AddRange(sectionData.childrenSection);
            foreach(SectionData sectionData1 in sectionData.childrenSection)
            {
                sectionData1.sectionClickEvent = () =>
                {
                    sectionData1.childrenSection = GetChildrenSection(sectionData1.id);
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
                SceneManager.LoadScene("ARScene");
            }, (error) =>
            {
                modalShower.ShowErrorModal(error);
            });
        }
    }
}

