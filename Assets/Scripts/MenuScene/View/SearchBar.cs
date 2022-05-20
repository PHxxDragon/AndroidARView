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
    private Button button;

    private string keyword;

    void Awake()
    {
        button.onClick.AddListener(() =>
        {
            if (keyword != searchInputField.text)
            {
                keyword = searchInputField.text;
                OnSearch?.Invoke(keyword);
            }
        });
        searchInputField.onSubmit.AddListener((text) =>
        {
            keyword = text;
            OnSearch?.Invoke(text);
        });
    }

    public void SetText(string text)
    {
        searchInputField.text = text;
    }
    public string GetText()
    {
        return searchInputField.text;
    }
}
