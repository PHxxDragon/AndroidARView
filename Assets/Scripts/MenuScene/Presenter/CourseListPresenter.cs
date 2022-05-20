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
                courseListView.CourseListRefreshEvent += (page, limit, type, keyword) =>
                {
                    webRequest.GetCourseList(page, limit, type.ToString(), keyword,
                    (response) =>
                    {
                        courseListView.PopulateData(response.courses, response.pageCount);
                        foreach (CourseData courseData in response.courses)
                        {
                            if (!string.IsNullOrEmpty(courseData.thumbnail))
                            {
                                Utils.Instance.GetImageAsTexture2D(courseData.thumbnail, (image) =>
                                {
                                    courseData.coverImage?.Invoke(Utils.Instance.Texture2DToSprite(image));
                                });
                            }
                            courseData.courseClickEvent += (id, name) =>
                            {
                                screenNavigator.OpenView(moduleListView);
                                moduleListView.Refresh((id, name));
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

