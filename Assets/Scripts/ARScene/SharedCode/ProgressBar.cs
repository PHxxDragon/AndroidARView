using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace EAR.View
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;
        [SerializeField]
        private TMP_Text sliderText;

        public void EnableProgressBar()
        {
            gameObject.SetActive(true);
        }

        public void DisableProgressBar()
        {
            gameObject.SetActive(false);
        }

        public void SetProgress(float percent, string text)
        {
            slider.value = percent;
            sliderText.text = text;
        }
    }

}
