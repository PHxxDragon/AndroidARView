using UnityEngine;
using Vuforia;

namespace EAR.AR
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class ImageHolder : MonoBehaviour
    {
        public float widthInMeter = 1;

        private Texture2D image;
        private float oldWidthInMeter = 1;
        private Mesh mesh;

        public void LoadImage(string imageUrl)
        {
            Utils.Instance.GetImageAsTexture2D(imageUrl, GenerateMesh);
        }

        void Update()
        {
            if (oldWidthInMeter != widthInMeter)
            {
                oldWidthInMeter = widthInMeter;
                UpdateMesh();
            }
        }

    private void GenerateMesh(Texture2D texture2D, object param)
        {
            mesh = new Mesh();
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            image = texture2D;
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material.mainTexture = texture2D;

            mesh.Clear();
            UpdateMesh();
        }

        private void UpdateMesh()
        {
            if (mesh == null || image == null)
                return;

            float heightInMeter = widthInMeter / image.width * image.height;
            Vector3[] vertices = new Vector3[]
            {
                new Vector3(-widthInMeter/2, 0, -heightInMeter/2),
                new Vector3(widthInMeter/2, 0, -heightInMeter/2),
                new Vector3(-widthInMeter/2, 0, heightInMeter/2),
                new Vector3(widthInMeter/2, 0, heightInMeter/2),
                new Vector3(-widthInMeter/2, 0, -heightInMeter/2),
                new Vector3(widthInMeter/2, 0, -heightInMeter/2),
                new Vector3(-widthInMeter/2, 0, heightInMeter/2),
                new Vector3(widthInMeter/2, 0, heightInMeter/2),
            };

            int[] triangles = new int[]
            {
                0, 2, 1, 1, 2, 3, 4, 5, 6, 5, 7, 6
            };

            Vector2[] uvs = new Vector2[]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();
        }
    }
}

