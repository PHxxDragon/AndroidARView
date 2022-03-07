using UnityEngine.UI;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace EAR.View
{
    public class Note : MonoBehaviour
    {
        [SerializeField]
        private Button button;
        [SerializeField]
        private TMP_Text text;
        [SerializeField]
        private TMP_Text button_text;
        [SerializeField]
        private RectTransform noteContainer;
        [SerializeField]
        private RectTransform imageBox;

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
                    HideNote();
                } else
                {
                    ShowNote();
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
            HideNote();
        }

        private void HideNote()
        {
            isCompleted = false;
            originalScale = noteContainer.transform.localScale;
            noteContainer.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InQuart).OnComplete(() =>
            {
                isCompleted = true;
            });
        }

        private void ShowNote()
        {
            isCompleted = false;
            noteContainer.transform.DOScale(originalScale, 0.2f).SetEase(Ease.OutQuart).OnComplete(() =>
            {
                isCompleted = true;
            });
        }

        public NoteData GetNoteData()
        {
            NoteData noteData = new NoteData();
            noteData.noteContent = text.text;
            noteData.buttonTitle = button_text.text;
            noteData.noteTransformData = TransformData.TransformToTransformData(transform);
            if (noteData.noteTransformData.scale == Vector3.zero)
            {
                noteData.noteTransformData.scale = originalScale;
            }
            noteData.noteContentRectTransformData = RectTransformData.RectTransformToRectTransformData(noteContainer);
            noteData.boxWidth = GetBoxWidth();
            noteData.height = GetHeight();
            noteData.fontSize = GetFontSize();
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
            return (int) text.fontSize;
        }

        public void SetButtonText(string value)
        {
            button_text.text = value;
        }

        public string GetButtonText()
        {
            return button_text.text;
        }

        public void SetText(string value)
        {
            text.text = value;
        }

        public string GetText()
        {
            return text.text;
        }
    }
}

