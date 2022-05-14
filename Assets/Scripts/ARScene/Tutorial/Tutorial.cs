using UnityEngine;

namespace EAR.Tutorials
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField]
        private GameObject scanEnvironmentGroundTut;
        [SerializeField]
        private GameObject scanEnvironmentMidairTut;
        [SerializeField]
        private GameObject tapToPlaceTut;
        [SerializeField]
        private GameObject scanImage;

        public enum TutorialEnum
        {
            None, ScanEnvironmentGround, ScanEnvironmentMid, TapToPlace, ScanImage
        }

        private void HideAll()
        {
            scanEnvironmentGroundTut.SetActive(false);
            tapToPlaceTut.SetActive(false);
            scanEnvironmentMidairTut.SetActive(false);
            scanImage.SetActive(false);
        }

        public void ShowTutorial(TutorialEnum tut)
        {
            Debug.Log("Show tut: " + tut);
            switch (tut)
            {
                case TutorialEnum.None:
                    HideAll();
                    break;
                case TutorialEnum.ScanEnvironmentGround:
                    HideAll();
                    scanEnvironmentGroundTut.SetActive(true);
                    break;
                case TutorialEnum.ScanEnvironmentMid:
                    HideAll();
                    scanEnvironmentMidairTut.SetActive(true);
                    break;
                case TutorialEnum.TapToPlace:
                    HideAll();
                    tapToPlaceTut.SetActive(true);
                    break;
                case TutorialEnum.ScanImage:
                    HideAll();
                    scanImage.SetActive(true);
                    break;
            }
        }
    }
}

