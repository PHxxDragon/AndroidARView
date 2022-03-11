using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EAR.Tutorials
{
    public class ControlTutorial : MonoBehaviour
    {
        [SerializeField]
        private GameObject groundPlane;
        [SerializeField]
        private GameObject midAir;
        [SerializeField]
        private GameObject image;

        public enum ControlTutorialEnum
        {
            GroundPlane, MidAir, Image
        }

        void Start()
        {
            DisableAll();
            groundPlane.SetActive(true);
        }

        private void DisableAll()
        {
            groundPlane.SetActive(false);
            midAir.SetActive(false);
            image.SetActive(false);
        }

        public void ChangeTutorial(ControlTutorialEnum control)
        {
            switch(control)
            {
                case ControlTutorialEnum.GroundPlane:
                    DisableAll();
                    groundPlane.SetActive(true);
                    break;
                case ControlTutorialEnum.MidAir:
                    DisableAll();
                    midAir.SetActive(true);
                    break;
                case ControlTutorialEnum.Image:
                    DisableAll();
                    image.SetActive(true);
                    break;
            }
        }
    }

}
