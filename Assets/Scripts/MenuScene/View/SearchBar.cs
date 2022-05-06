using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class SearchBar : MonoBehaviour
{
    public event Action<string> OnSearch;

    [SerializeField]
    private TMP_InputField searchInputField;
    [SerializeField]
    private Button searchButton;

    void Awake()
    {
        searchButton.onClick.AddListener(() =>
        {
            OnSearch?.Invoke(searchInputField.text);
        });

        searchInputField.onSubmit.AddListener((text) =>
        {
            OnSearch?.Invoke(text);
        });
    }

    public string GetText()
    {
        return searchInputField.text;
    }
}
