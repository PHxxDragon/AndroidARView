using UnityEngine.UI;
using UnityEngine;
using TMPro;
using EAR.Localization;
using DG.Tweening;
using Nobi.UiRoundedCorners;

namespace EAR.View
{
    public class Note : MonoBehaviour
    {
        [SerializeField]
        private Button button;
        [SerializeField]
        private TMP_Text text;
        [SerializeField]
        private RectTransform noteContainer;
        [SerializeField]
        private HorizontalLayoutGroup textHorizontalLayoutGroup;
        [SerializeField]
        private RectTransform imageBox;
        [SerializeField]
        private Image textBackground;
        [SerializeField]
        private ImageWithIndependentRoundedCorners imageCorners;
        [SerializeField]
        private Image borderBackground;
        [SerializeField]
        private RectTransform border;
        [SerializeField]
        private ImageWithIndependentRoundedCorners borderCorners;


        [SerializeField]
        private TMP_Text button_text;
        [SerializeField]
        private HorizontalLayoutGroup buttonHorizontalLayoutGroup;
        [SerializeField]
        private Image buttonBackground;
        [SerializeField]
        private ImageWithIndependentRoundedCorners buttonImageCorners;
        [SerializeField]
        private Image buttonBorderBackground;
        [SerializeField]
        private RectTransform buttonBorder;
        [SerializeField]
        private ImageWithIndependentRoundedCorners buttonBorderCorners;


        [SerializeField]
        private Canvas[] canvases;
        [SerializeField]
        private Camera eventCamera;

        private Vector3 originalScale;
        private bool isCompleted = true;

        void Start()
        {
            button.onClick.AddListener(() =>
            {
                if (!isCompleted) return;

                if (noteContainer.transform.localScale != Vector3.zero)
                {
                    isCompleted = false;
                    originalScale = noteContainer.transform.localScale;
                    noteContainer.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InQuart).OnComplete(() =>
                    {
                        isCompleted = true;
                    });
                }
                else
                {
                    isCompleted = false;
                    noteContainer.transform.DOScale(originalScale, 0.2f).SetEase(Ease.OutQuart).OnComplete(() =>
                    {
                        isCompleted = true;
                    });
                }

            });
            if (eventCamera == null)
            {
                eventCamera = Camera.main;
                foreach (Canvas canvas in canvases)
                {
                    canvas.worldCamera = eventCamera;
                }
            }
        }

        public NoteData GetNoteData()
        {
            NoteData noteData = new NoteData();
            noteData.noteContent = text.text;
            noteData.buttonTitle = button_text.text;
            noteData.noteTransformData = TransformData.TransformToTransformData(transform);
            noteData.noteContentRectTransformData = RectTransformData.RectTransformToRectTransformData(noteContainer);
            if (noteData.noteContentRectTransformData.localScale == Vector3.zero)
            {
                noteData.noteContentRectTransformData.localScale = originalScale;
            }
            noteData.boxWidth = GetBoxWidth();
            noteData.height = GetHeight();
            noteData.fontSize = GetFontSize();
            noteData.buttonBackgroundColor = GetButtonBackgroundColor();
            noteData.buttonBorderRadius = GetButtonBorderRadius();
            noteData.textBackgroundColor = GetTextBackgroundColor();
            noteData.textBorderRadius = GetTextBorderRadius();
            noteData.textColor = GetTextColor();
            noteData.buttonTextColor = GetButtonTextColor();
            noteData.buttonFontSize = GetButtonFontSize();
            noteData.buttonBorderWidth = GetButtonBorderWidth();
            noteData.borderWidth = GetTextBorderWidth();
            noteData.buttonBorderColor = GetButtonBorderColor();
            noteData.borderColor = GetBorderColor();
            return noteData;
        }

        public void PopulateData(NoteData data)
        {
            TransformData.TransformDataToTransfrom(data.noteTransformData, transform);
            RectTransformData.RectTransformDataToRectTransform(data.noteContentRectTransformData, noteContainer);
            text.text = data.noteContent;
            button_text.text = data.buttonTitle;
            SetHeight(data.height);
            SetBoxWidth(data.boxWidth);
            SetFontSize(data.fontSize);
            SetButtonBackgroundColor(data.buttonBackgroundColor);
            SetButtonBorderRadius(data.buttonBorderRadius);
            SetTextBackgroundColor(data.textBackgroundColor);
            SetTextBorderRadius(data.textBorderRadius);
            SetTextColor(data.textColor);
            SetButtonTextColor(data.buttonTextColor);
            SetButtonFontSize(data.buttonFontSize);
            SetButtonBorderWidth(data.buttonBorderWidth);
            SetButtonBorderColor(data.buttonBorderColor);
            SetBorderColor(data.borderColor);
            SetTextBorderWidth(data.borderWidth);
        }

        public void SetHeight(float height)
        {
            Vector3 localPosition = noteContainer.localPosition;
            localPosition.y = height;
            noteContainer.localPosition = localPosition;
        }

        public float GetHeight()
        {
            return noteContainer.localPosition.y;
        }

        public void SetBoxWidth(float boxWidth)
        {
            imageBox.sizeDelta = new Vector2(boxWidth, imageBox.sizeDelta.y);
        }

        public float GetBoxWidth()
        {
            return imageBox.sizeDelta.x;
        }

        public void SetFontSize(int fontSize)
        {
            text.fontSize = fontSize;
        }

        public int GetFontSize()
        {
            return (int)text.fontSize;
        }

        public void SetText(string value)
        {
            text.text = value;
        }

        public string GetText()
        {
            return text.text;
        }

        public void SetTextBackgroundColor(Color color)
        {
            textBackground.color = color;
        }

        public Color GetTextBackgroundColor()
        {
            return textBackground.color;
        }

        public void SetTextBorderRadius(Vector4 r)
        {
            borderCorners.r = r;
            borderCorners.Refresh();
            UpdateBackgroundCorners();
        }

        public Vector4 GetTextBorderRadius()
        {
            return borderCorners.r;
        }

        public void SetTextColor(Color color)
        {
            text.color = color;
        }

        public Color GetTextColor()
        {
            return text.color;
        }

        public void SetTextBorderWidth(Vector4 width)
        {
            border.offsetMax = new Vector2(width.x, width.y);
            border.offsetMin = new Vector2(-width.z, -width.w);
            UpdateBackgroundCorners();
        }

        public Vector4 GetTextBorderWidth()
        {
            return new Vector4(border.offsetMax.x, border.offsetMax.y, -border.offsetMin.x, -border.offsetMin.y);
        }

        public void SetButtonText(string value)
        {
            button_text.text = value;
        }

        public string GetButtonText()
        {
            return button_text.text;
        }

        public void SetButtonBorderColor(Color color)
        {
            buttonBorderBackground.color = color;
        }

        public Color GetButtonBorderColor()
        {
            return buttonBorderBackground.color;
        }

        public void SetBorderColor(Color color)
        {
            borderBackground.color = color;
        }

        public Color GetBorderColor()
        {
            return borderBackground.color;
        }

        public void SetButtonBackgroundColor(Color color)
        {
            buttonBackground.color = color;
        }

        public Color GetButtonBackgroundColor()
        {
            return buttonBackground.color;
        }

        public void SetButtonBorderRadius(Vector4 r)
        {
            buttonBorderCorners.r = r;
            buttonBorderCorners.Refresh();
            UpdateButtonBackgroundCorners();
        }

        public Vector4 GetButtonBorderRadius()
        {
            return buttonBorderCorners.r;
        }

        public void SetButtonBorderWidth(Vector4 width)
        {
            buttonBorder.offsetMax = new Vector2(width.x, width.y);
            buttonBorder.offsetMin = new Vector2(-width.z, -width.w);
            UpdateButtonBackgroundCorners();
        }

        public Vector4 GetButtonBorderWidth()
        {
            return new Vector4(buttonBorder.offsetMax.x, buttonBorder.offsetMax.y, -buttonBorder.offsetMin.x, -buttonBorder.offsetMin.y);
        }

        public void SetButtonFontSize(int size)
        {
            button_text.fontSize = size;
        }

        public int GetButtonFontSize()
        {
            return (int)button_text.fontSize;
        }

        public void SetButtonTextColor(Color color)
        {
            button_text.color = color;
        }

        public Color GetButtonTextColor()
        {
            return button_text.color;
        }

        private void UpdateBackgroundCorners()
        {
            int remainDeg = Mathf.Max((int)(borderCorners.r.x - border.offsetMax.x), 0);
            imageCorners.r = new Vector4(remainDeg, remainDeg, remainDeg, remainDeg);
            imageCorners.Refresh();
            int padding = (int)imageCorners.GetRealR().x / 2;
            textHorizontalLayoutGroup.padding = new RectOffset(padding, padding, 3, 3);
        }

        private void UpdateButtonBackgroundCorners()
        {
            int remainDeg = Mathf.Max((int)(buttonBorderCorners.r.x - buttonBorder.offsetMax.x), 0);
            buttonImageCorners.r = new Vector4(remainDeg, remainDeg, remainDeg, remainDeg);
            buttonImageCorners.Refresh();
            int padding = (int)buttonImageCorners.GetRealR().x / 2;
            buttonHorizontalLayoutGroup.padding = new RectOffset(padding, padding, 3, 3);
        }
    }
}

