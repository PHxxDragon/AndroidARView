using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

namespace EAR.View
{
    public class ImageList : MonoBehaviour
    {
        [SerializeField]
        private GameObject container;
        [SerializeField]
        private Image imagePrefab;

        private List<Image> images = new List<Image>();

        public void SetNumImage(int num)
        {
            foreach (Transform transform in container.transform)
            {
                Destroy(transform.gameObject);
                images.Clear();
            }

            if (num == 0) num = 1;

            for (int i = 0; i < num; i++)
            {
                images.Add(Instantiate(imagePrefab, container.transform));
            }
        }

        public void SetImage(Sprite image, int index)
        {
            if (index < images.Count && index >= 0)
            {
                images[index].sprite = image;
            }
        }

    }
}

