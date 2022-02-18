using UnityEngine;
using EAR.AR;
using TMPro;
using System;

namespace EAR.Editor.Presenter
{
    public class ModeSelectPresenter : MonoBehaviour
    {
        [SerializeField]
        private ImageTargetCreator imageTargetCreator;
        private GameObject imageTarget;

        [SerializeField]
        private GameObject modelContainer;
        [SerializeField]
        private TMP_Dropdown dropdown;

        [SerializeField]
        private GroundPlaneController groundPlaneController;
        [SerializeField]
        private GameObject groudPlaneStage;

        [SerializeField]
        private MidAirController midAirController;
        [SerializeField]
        private GameObject midAirStage;

        

        void Start()
        {
            if (dropdown == null || imageTargetCreator == null || groundPlaneController == null || groudPlaneStage == null)
            {
                Debug.Log("Unassigned references");
                return;
            }

            imageTargetCreator.CreateTargetDoneEvent += CreateTargetDoneEventSubscriber;
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
            groundPlaneController.gameObject.SetActive(false);

        }

        private void ResetAll()
        {
            if (imageTarget != null)
            {
                imageTarget.SetActive(false);
            }
            groundPlaneController.gameObject.SetActive(false);
            midAirController.gameObject.SetActive(false);
        }

        private void OnDropdownValueChanged(int mode)
        {
            switch (mode)
            {
                case 0:
                    ResetAll();
                    imageTarget.SetActive(true);
                    modelContainer.transform.parent = imageTarget.transform;
                    ResetTransform(modelContainer.transform);
                    break;
                case 1:
                    ResetAll();
                    groundPlaneController.gameObject.SetActive(true);
                    modelContainer.transform.parent = groudPlaneStage.transform;
                    ResetTransform(modelContainer.transform);
                    break;
                case 2:
                    ResetAll();
                    midAirController.gameObject.SetActive(true);
                    modelContainer.transform.parent = midAirStage.transform;
                    ResetTransform(modelContainer.transform);
                    midAirController.AdjustModelPosition();
                    break;
                default:
                    break;
            }
        }

        private void CreateTargetDoneEventSubscriber()
        {
            imageTarget = imageTargetCreator.GetImageTarget();
            modelContainer.transform.parent = imageTarget.transform;
        }

        private void ResetTransform(Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
    }
}
