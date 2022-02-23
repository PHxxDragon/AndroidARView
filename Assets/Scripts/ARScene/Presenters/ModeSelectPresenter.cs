using UnityEngine;
using UnityEngine.SceneManagement;
using EAR.AR;
using EAR.View;
using TMPro;
using Vuforia;

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

        [SerializeField]
        private Modal modalPrefab;
        [SerializeField]
        private Transform canvas;
        [SerializeField]
        private GameObject header;

        void Awake()
        {
            if (dropdown == null || imageTargetCreator == null || groundPlaneController == null || groudPlaneStage == null)
            {
                Debug.Log("Unassigned references");
                return;
            }

            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
            ActiveGroundPlane();

            imageTargetCreator.CreateTargetDoneEvent += CreateTargetDoneEventSubscriber;
            imageTargetCreator.CreateTargetErrorEvent += CreateTargetErrorEventSubscriber;
            
        }
        private void CreateTargetDoneEventSubscriber()
        {
            imageTarget = imageTargetCreator.GetImageTarget();
            if (!SupportAnchor())
            {
                ActiveImageTarget();
                header.gameObject.SetActive(false);
            }
        }

        private void CreateTargetErrorEventSubscriber()
        {
            if (!SupportAnchor())
            {
                Modal modal = Instantiate<Modal>(modalPrefab, canvas);
                modal.SetModalContent("Error", "This module has no marker and this phone doesn't support ARCore");
                modal.DisableCancelButton();
                modal.OnConfirmButtonClick += GoBackToMenu;
            }
        }

        private void GoBackToMenu()
        {
            SceneManager.LoadScene("MenuScene");
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

        private bool SupportAnchor()
        {
            return VuforiaBehaviour.Instance.World.AnchorsSupported;
        }

        private void ActiveImageTarget()
        {
            if (imageTarget == null)
            {
                Modal modal = Instantiate<Modal>(modalPrefab, canvas);
                modal.SetModalContent("Error", "This module has no marker");
                modal.DisableCancelButton();
                return;
            }
            ResetAll();
            imageTarget.SetActive(true);
            modelContainer.transform.parent = imageTarget.transform;
            ResetTransform(modelContainer.transform);
        }

        private void ActiveMidAir()
        {
            ResetAll();
            midAirController.gameObject.SetActive(true);
            modelContainer.transform.parent = midAirStage.transform;
            ResetTransform(modelContainer.transform);
            midAirController.AdjustModelPosition();
        }

        private void ActiveGroundPlane()
        {
            ResetAll();
            groundPlaneController.gameObject.SetActive(true);
            modelContainer.transform.parent = groudPlaneStage.transform;
            ResetTransform(modelContainer.transform);
        }

        private void OnDropdownValueChanged(int mode)
        {
            switch (mode)
            {
                case 0:
                    ActiveGroundPlane();
                    break;
                case 1:
                    ActiveMidAir();
                    break;
                case 2:
                    ActiveImageTarget();
                    break;
                default:
                    break;
            }
        }

        private void ResetTransform(Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
    }
}
