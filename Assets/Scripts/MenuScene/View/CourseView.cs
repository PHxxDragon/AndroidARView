using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;

namespace EAR.View
{
    public class CourseView : ListItemView<CourseData>
    {
        [SerializeField]
        private TMP_Text courseName;

        [SerializeField]
        private TMP_Text courseId;

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
        private UnityAction listener;

        public override void PopulateData(CourseData data)
        {
            id = data.id;
            courseName.text = data.name;
            courseId.text = "Course ID: " + data.id;
            ratings.text = data.rating + "";
            courseType.text = data.courseType;
            teachers.text = data.teachers;
            if (listener != null)
            {
                button.onClick.RemoveListener(listener);
            }
            listener = () =>
            {
                data.courseClickEvent?.Invoke(id);
            };
            button.onClick.AddListener(listener);
        }
    }
}

