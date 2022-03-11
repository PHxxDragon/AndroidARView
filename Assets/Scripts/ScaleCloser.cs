using UnityEngine;
using DG.Tweening;

namespace EAR.View
{
    public class ScaleCloser : MonoBehaviour
    {
        [SerializeField]
        private bool closeFirst = true;

        private Vector3 originalScale;
        private bool isCompleted;

        void Start()
        {
            originalScale = gameObject.transform.localScale;
            isCompleted = true;
            if (closeFirst) Close();
        }

        public void Close()
        {
            if (!isCompleted) return;
            isCompleted = false;
            transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InQuart).OnComplete(() =>
            {
                isCompleted = true;
            });
        }

        public void Open()
        {
            if (!isCompleted) return;
            isCompleted = false;
            transform.DOScale(originalScale, 0.2f).SetEase(Ease.OutQuart).OnComplete(() =>
            {
                isCompleted = true;
            });
        }
    }
}