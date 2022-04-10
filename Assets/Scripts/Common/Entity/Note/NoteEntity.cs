using UnityEngine.UI;
using UnityEngine;
using TMPro;
using EAR.Container;
using Nobi.UiRoundedCorners;

namespace EAR.Entity
{
    public class NoteEntity : VisibleEntity
    {
        private static int count = 1;

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
        private Canvas[] canvases;
        [SerializeField]
        private Camera eventCamera;

        private Vector3 originalScale;

        protected override string GetDefaultName()
        {
            return "New note " + count++;
        }

        public static NoteEntity InstantNewEntity(NoteData noteData)
        {
            NoteEntity notePrefab = AssetContainer.Instance.GetNotePrefab();
            NoteEntity noteEntity = Instantiate(notePrefab);
            noteEntity.PopulateData(noteData);
            OnEntityCreated?.Invoke(noteEntity);
            return noteEntity;
        }

        public NoteData GetNoteData()
        {
            NoteData noteData = new NoteData();
            noteData.id = GetId();
            noteData.name = GetEntityName();
            noteData.noteContent = text.text;
            noteData.noteTransformData = TransformData.TransformToTransformData(transform);
            noteData.noteContentRectTransformData = RectTransformData.RectTransformToRectTransformData(noteContainer);
            if (noteData.noteContentRectTransformData.localScale == Vector3.zero)
            {
                noteData.noteContentRectTransformData.localScale = originalScale;
            }
            noteData.boxWidth = GetBoxWidth();

            noteData.fontSize = GetFontSize();
            noteData.textBackgroundColor = GetTextBackgroundColor();
            noteData.textBorderRadius = GetTextBorderRadius();
            noteData.textColor = GetTextColor();
            noteData.borderWidth = GetTextBorderWidth();
            noteData.borderColor = GetBorderColor();
            noteData.isVisible = isVisible;
            return noteData;
        }

        private void PopulateData(NoteData data)
        {
            if (data.noteTransformData != null)
            {
                TransformData.TransformDataToTransfrom(data.noteTransformData, transform);
            }

            if (data.noteContentRectTransformData != null)
            {
                RectTransformData.RectTransformDataToRectTransform(data.noteContentRectTransformData, noteContainer);
            }
            
            if (data.noteContent != null)
            {
                text.text = data.noteContent;
            }
            
            if (data.boxWidth > 0)
            {
                SetBoxWidth(data.boxWidth);
            }
            
            if (data.fontSize > 0)
            {
                SetFontSize(data.fontSize);
            }

            if (data.id != null)
            {
                SetId(data.id);
            }
            
            SetTextBackgroundColor(data.textBackgroundColor);
            SetTextBorderRadius(data.textBorderRadius);
            SetTextColor(data.textColor);
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
            return (int) text.fontSize;
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

        public void SetBorderColor(Color color)
        {
            borderBackground.color = color;
        }

        public Color GetBorderColor()
        {
            return borderBackground.color;
        }

        private void UpdateBackgroundCorners()
        {
            int remainDeg = Mathf.Max((int) (borderCorners.r.x - border.offsetMax.x), 0);
            imageCorners.r = new Vector4(remainDeg, remainDeg, remainDeg, remainDeg);
            imageCorners.Refresh();
            int padding = (int) imageCorners.GetRealR().x / 2;
            textHorizontalLayoutGroup.padding = new RectOffset(padding, padding, 3, 3);
        }
    }
}

