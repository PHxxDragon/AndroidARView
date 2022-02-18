using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace EAR.View
{
    public class CourseView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text title;

        [SerializeField]
        private TMP_Text course_id;

        [SerializeField]
        private TMP_Text ratings;

        [SerializeField]
        private TMP_Text courseType;

        [SerializeField]
        private TMP_Text teachers;

        [SerializeField]
        private Image courseImage;

        [SerializeField]
        private Button button;

        private int id;
        private Action<int> courseClickEvent;

        public void PopulateData(CourseData data)
        {
            id = data.id;
            title.text = data.title;
            course_id.text = "Course ID: " + data.id;
            ratings.text = data.rating + "";
            courseType.text = data.courseType;
            teachers.text = data.teachers;
            courseClickEvent = data.courseClickEvent;
        }

        public void PopulateData(Sprite sprite)
        {
            courseImage.sprite = sprite;
        }

        void Start()
        {
            button.onClick.AddListener(ButtonClickEventSubscriber);
        }

        private void ButtonClickEventSubscriber()
        {
            courseClickEvent?.Invoke(id);
        }
    }
}

