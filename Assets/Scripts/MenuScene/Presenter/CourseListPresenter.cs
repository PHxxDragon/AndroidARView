using System.Collections.Generic;
using UnityEngine;
using EAR.View;
using EAR.WebRequest;

namespace EAR.MenuScene.Presenter
{
    public class CourseListPresenter : MonoBehaviour
    {
        [SerializeField]
        private CourseListView courseListView;

        [SerializeField]
        private WebRequestHelper webRequest;

        [SerializeField]
        private ScreenNavigator screenNavigator;

        [SerializeField]
        private ModuleListView moduleListView;

        [SerializeField]
        private ModalShower modelShower;

        void Start()
        {
            if (courseListView != null && webRequest != null)
            {
                courseListView.CourseListRefreshEvent += (page, limit) =>
                {
                    webRequest.GetCourseList(page, limit,
                    (response) =>
                    {
                        courseListView.PopulateData(response.courses, response.pageCount);
                        foreach (CourseData courseData in response.courses)
                        {
                            courseData.courseClickEvent += (id) =>
                            {
                                screenNavigator.PushView(moduleListView);
                                moduleListView.Refresh(id);
                            };
                        }
                    }, (error) =>
                    {
                        modelShower.ShowErrorModal(error);
                    });
                };
            }
            else
            {
                Debug.LogWarning("Unassigned references");
            }
        }
    }
}

