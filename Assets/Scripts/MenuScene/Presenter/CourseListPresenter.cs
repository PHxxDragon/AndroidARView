using UnityEngine;
using EAR.View;
using EAR.WebRequest;
using EAR.SceneChange;

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

        private int currentPage;
        private int currentLimit;
        private CourseListView.CourseType currentType;
        private string currentKeyword;

        void Start()
        {
            courseListView.CourseListRefreshEvent += (page, limit, type, keyword) =>
            {
                if (CheckCache(page, limit, type, keyword))
                {
                    courseListView.KeepData();
                    return;
                }

                MenuSceneParam.Reset();
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
                    ApplyCache(page, limit, type, keyword);
                }, (error) =>
                {
                    modelShower.ShowErrorModal(error);
                });
            };
        }

        private bool CheckCache(int page, int limit, CourseListView.CourseType type, string keyword)
        {
            if (page == currentPage && limit == currentLimit && type == currentType && keyword == currentKeyword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ApplyCache(int page, int limit, CourseListView.CourseType type, string keyword)
        {
            currentPage = page;
            currentLimit = limit;
            currentType = type;
            currentKeyword = keyword;
        }
    }
}

