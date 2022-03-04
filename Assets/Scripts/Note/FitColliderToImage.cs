using UnityEngine.UI;
using UnityEngine;
using System.Collections;

namespace EAR.View
{
    public class FitColliderToImage : MonoBehaviour
    {
        private Image image;
        private BoxCollider boxCollider;
        void Start()
        {
            image = GetComponent<Image>();
            boxCollider = GetComponent<BoxCollider>();
            StartCoroutine(UpdateCollider());
        }

        private IEnumerator UpdateCollider()
        {
            while (image != null && boxCollider != null)
            {
                Vector3 minCorner = Vector3.positiveInfinity;
                Vector3 maxCorner = Vector3.negativeInfinity;
                Vector3[] points = new Vector3[4];
                image.rectTransform.GetLocalCorners(points);
                foreach(Vector3 point in points)
                {
                    minCorner = Vector3.Min(minCorner, point);
                    maxCorner = Vector3.Max(maxCorner, point);
                }
                Vector3 center = (maxCorner + minCorner) / 2;
                Vector3 size = maxCorner - minCorner;
                boxCollider.center = center;
                boxCollider.size = size;
                yield return new WaitForSeconds(1f);
            }
        }
    }

}
