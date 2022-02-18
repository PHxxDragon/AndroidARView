using UnityEngine;
using UnityEngine.EventSystems;

namespace EAR.View
{
    public class Dragger : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        public RectTransform dragArea;
        public RectTransform dragObject;
        public bool topOnClick = true;

        private Vector2 originalLocalPointerPosition;
        private Vector3 originalPanelLocalPosition;

        public void OnBeginDrag(PointerEventData data)
        {
            originalPanelLocalPosition = dragObject.localPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(dragArea, data.position, data.pressEventCamera, out originalLocalPointerPosition);
            gameObject.transform.SetAsLastSibling();

            if (topOnClick == true)
                dragObject.transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData data)
        {
            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(dragArea, data.position, data.pressEventCamera, out localPointerPosition))
            {
                Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
                dragObject.localPosition = originalPanelLocalPosition + offsetToOriginal;
            }

            ClampToArea();
        }

        void Start()
        {
            if (dragObject == null)
            {
                dragObject = transform as RectTransform;
            }
            if (dragArea == null)
            {
                RectTransform canvas = transform as RectTransform;
                while (canvas.parent != null && canvas.parent is RectTransform)
                {
                    canvas = canvas.parent as RectTransform;
                }
                dragArea = canvas;
            }
        }

        private void ClampToArea()
        {
            Vector3 pos = dragObject.localPosition;

            Vector3 minPosition = dragArea.rect.min - dragObject.rect.min;
            Vector3 maxPosition = dragArea.rect.max - dragObject.rect.max;

            pos.x = Mathf.Clamp(dragObject.localPosition.x, minPosition.x, maxPosition.x);
            pos.y = Mathf.Clamp(dragObject.localPosition.y, minPosition.y, maxPosition.y);

            dragObject.localPosition = pos;
        }
    }
}
