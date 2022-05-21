using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;
using EAR.Localization;

namespace EAR.View
{
    public class CourseView : ListItemView<CourseData>
    {
        [SerializeField]
        private TMP_Text courseName;

        [SerializeField]
        private TMP_Text courseAccessibility;

        [SerializeField]
        private TMP_Text courseType;

        [SerializeField]
        private TMP_Text startDateText;

        [SerializeField]
        private TMP_Text endDateText;

        [SerializeField]
        private TMP_Text about;

        [SerializeField]
        private TMP_Text accessibility;

        [SerializeField]
        private Image courseImage;

        [SerializeField]
        private Button button;

        private int id;
        private UnityAction listener;

        private string UpperFirstLetter(string text)
        {
            if (text.Length == 0) return "";
            return char.ToUpper(text[0]) + text.Substring(1);
        }

        public override void PopulateData(CourseData data)
        {
            id = data.id;
            courseName.text = data.name;
            courseAccessibility.text = LocalizationUtils.GetLocalizedText("Accessibility" + UpperFirstLetter(data.accessibility));
            courseType.text = LocalizationUtils.GetLocalizedText("Pacing" + UpperFirstLetter(data.type));
            if (data.startDate != new DateTime())
            {
                startDateText.text = LocalizationUtils.GetLocalizedText("StartDate") + ": " + data.startDate.ToString("dd-MM-yyyy");
            } else
            {
                startDateText.gameObject.SetActive(false);
            }
            
            if (data.endDate != new DateTime())
            {
                endDateText.text = LocalizationUtils.GetLocalizedText("EndDate") + ": " + data.endDate.ToString("dd-MM-yyyy");
            } else
            {
                endDateText.gameObject.SetActive(false);
            }
            
            if (!string.IsNullOrEmpty(data.about))
            {
                about.text = LocalizationUtils.GetLocalizedText("About") + ": " + data.about;
            } else
            {
                about.text = "";
            }
            
            data.coverImage += (image) =>
            {
                if (courseImage)
                {
                    courseImage.sprite = image;
                }
            };
            if (listener != null)
            {
                button.onClick.RemoveListener(listener);
            }
            listener = () =>
            {
                data.courseClickEvent?.Invoke(id, courseName.text);
            };
            button.onClick.AddListener(listener);
        }
    }
}

