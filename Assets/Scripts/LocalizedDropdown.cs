using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;
using System;

namespace EAR.Localization
{
    [RequireComponent(typeof(TMP_Dropdown))]
    [AddComponentMenu("Localization/Localize Dropdown")]
    public class LocalizedDropdown : MonoBehaviour
    {
        public List<LocalizedDropdownOption> options;
        public int selectedOptionIndex = 0;

        private Locale currentLocale = null;
        private TMP_Dropdown Dropdown => GetComponent<TMP_Dropdown>();
        private void Start()
        {
            PopulateDropdown();
        }

        private void OnEnable()
        {
            var locale = LocalizationSettings.SelectedLocale;
            if (currentLocale != null && locale != currentLocale)
            {
                UpdateDropdownOptions(locale);
                currentLocale = locale;
            }
            LocalizationSettings.SelectedLocaleChanged += UpdateDropdownOptions;
        }

        private void OnDisable() => LocalizationSettings.SelectedLocaleChanged -= UpdateDropdownOptions;

        private void OnDestroy() => LocalizationSettings.SelectedLocaleChanged -= UpdateDropdownOptions;


        private void PopulateDropdown()
        {
            // Clear any options that might be present
            selectedOptionIndex = Dropdown.value;

            Dropdown.ClearOptions();
            Dropdown.onValueChanged.RemoveListener(UpdateSelectedOptionIndex);

            for (var i = 0; i < options.Count; ++i)
            {
                var localizedText = options[i].text.GetLocalizedString();
                Dropdown.options.Add(new TMP_Dropdown.OptionData(localizedText));
                if (i == selectedOptionIndex)
                {
                    UpdateSelectedText(localizedText);
                }
            }

            // Update selected option, to make sure the correct option can be displayed in the caption
            Dropdown.value = selectedOptionIndex;
            Dropdown.onValueChanged.AddListener(UpdateSelectedOptionIndex);
            currentLocale = LocalizationSettings.SelectedLocale;
        }

        private void UpdateDropdownOptions(Locale locale)
        {
            // Updating all options in the dropdown
            // Assumes that this list is the same as the options passed on in the inspector window
            for (var i = 0; i < Dropdown.options.Count; ++i)
            {
                var localizedText = options[i].text.GetLocalizedString(locale);
                Dropdown.options[i].text = localizedText;
                if (i == selectedOptionIndex)
                {
                    UpdateSelectedText(localizedText);
                }
            }
        }

        private void UpdateSelectedOptionIndex(int index) => selectedOptionIndex = index;

        private void UpdateSelectedText(string text)
        {
            if (Dropdown.captionText != null)
            {
                Dropdown.captionText.text = text;
            }
        }



    }

    [Serializable]
    public class LocalizedDropdownOption
    {
        public LocalizedString text;
    }
}

