using UnityEngine.UI;
using UnityEngine;
using TMPro;

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
        private Canvas[] canvases;
        [SerializeField]
        private Camera eventCamera;

        void Start()
        {
            button.onClick.AddListener(() =>
            {
                noteContainer.gameObject.SetActive(!noteContainer.gameObject.activeSelf);
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
            return noteData;
        }

        public void PopulateData(NoteData data)
        {
            TransformData.TransformDataToTransfrom(data.noteTransformData, transform);
            RectTransformData.RectTransformDataToRectTransform(data.noteContentRectTransformData, noteContainer);
            text.text = data.noteContent;
            button_text.text = data.buttonTitle;
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

