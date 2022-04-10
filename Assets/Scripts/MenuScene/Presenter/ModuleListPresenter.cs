using UnityEngine;
using EAR.View;
using EAR.WebRequest;
using System.Collections.Generic;
using EAR.Editor.Presenter;
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

        private List<SectionData> sectionStack = new List<SectionData>();
        private List<SectionData> sections = new List<SectionData>();

        void Start()
        {
            moduleListView.ModuleListRefreshEvent += (courseId) => {
                sectionStack.Clear();
                webRequest.GetModuleList(courseId, 
                (response) => {
                    sections = response;
                    SectionData dummySection = new SectionData();
                    dummySection.childrenSection = GetChildrenSection(0);
                    PushSection(dummySection);
                }, 
                (error) => {
                    Debug.Log(error);
                });
            };
            moduleListView.BackButtonClickEvent += () =>
            {
                sectionStack.RemoveAt(sectionStack.Count - 1);
                if (sectionStack.Count > 0)
                {
                    PopulateSection(sectionStack[sectionStack.Count - 1]);
                } else
                {
                    screenNavigator.GoBack();
                }
            };
        }

        private void PushSection(SectionData sectionData)
        {
            sectionStack.Add(sectionData);
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
                moduleData.moduleClickEvent = OpenARModule;
            }
            data.AddRange(sectionData.modules);
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

