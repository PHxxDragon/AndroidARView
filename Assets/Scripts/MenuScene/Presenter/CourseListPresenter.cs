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

        void Start()
        {
            if (courseListView != null && webRequest != null)
            {
                courseListView.CourseListRefreshEvent += CourseListRefreshEventSubscriber;
            }
            else
            {
                Debug.LogWarning("Unassigned references");
            }
        }

        private void CourseListRefreshEventSubscriber(int courseID)
        {
            string token = webRequest.GetAuthorizeToken();
            webRequest.GetCourseList(token, courseID, GetCourseListSuccessCallback, null);
        }

        private void GetCourseListSuccessCallback(List<CourseData> courseDatas)
        {
            foreach (CourseData courseData in courseDatas)
            {
                courseData.courseClickEvent += CourseClickEventSubscriber;
            }
            courseListView.PopulateData(courseDatas);
            foreach (CourseData courseData in courseDatas)
            {
                Utils.Instance.GetImageAsTexture2D(courseData.imageUrl, GetCourseImageSuccessCallback, null, null, courseData.id);
            }
        }

        private void GetCourseImageSuccessCallback(Texture2D texture, object param)
        {
            Sprite sprite = Utils.Instance.Texture2DToSprite(texture);
            courseListView.PopulateData(sprite, (int)param);
        }

        private void CourseClickEventSubscriber(int id)
        {
            screenNavigator.OpenView(NavigateCommandEnum.ToModuleList, id);
            moduleListView.Refresh();
        }
    }
}

