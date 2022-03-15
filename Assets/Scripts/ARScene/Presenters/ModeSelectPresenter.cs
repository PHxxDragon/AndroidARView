using UnityEngine;
using UnityEngine.SceneManagement;
using EAR.AR;
using EAR.View;
using EAR.AnimationPlayer;
using TMPro;
using Vuforia;
using UnityEngine.UI;
using EAR.Tutorials;
using System;

namespace EAR.Presenter
{
    public class ModeSelectPresenter : MonoBehaviour
    {
        public event Action<int> OnModeSelected;

        [SerializeField]
        private ImageTargetCreator imageTargetCreator;
        private ImageTargetBehaviour imageTarget;
        private string imageTargetError;

        [SerializeField]
        private GameObject modelContainer;
        [SerializeField]
        private AnimPlayer animationPlayer;
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

        [SerializeField]
        private Button resetButton;

        [SerializeField]
        private ControlTutorial controlTutorial;

        void Awake()
        {
            if (dropdown == null || imageTargetCreator == null || groundPlaneController == null || groudPlaneStage == null)
            {
                Debug.Log("Unassigned references");
                return;
            }

            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
            imageTargetCreator.CreateTargetDoneEvent += () =>
            {
                imageTarget = imageTargetCreator.GetImageTarget().GetComponent<ImageTargetBehaviour>();
                if (VuforiaApplication.Instance.IsInitialized)
                {
                    CheckForSupportAnchorAndActiveImageTarget();
                }
                else
                {
                    VuforiaApplication.Instance.OnVuforiaInitialized += (VuforiaInitError err) =>
                    {
                        CheckForSupportAnchorAndActiveImageTarget();
                    };
                }
            };
            imageTargetCreator.CreateTargetErrorEvent += (string error) =>
            {
                Debug.Log("Error creating target image: " + error);
                imageTargetError = error;
                if (VuforiaApplication.Instance.IsInitialized)
                {
                    CheckForSupportAnchorOrShowErrorModal();
                }
                else
                {
                    VuforiaApplication.Instance.OnVuforiaInitialized += (VuforiaInitError err) =>
                    {
                        CheckForSupportAnchorOrShowErrorModal();
                    };
                }
            };
            resetButton.onClick.AddListener(OnResetButtonClick);
        }

        void Start()
        {
            ActiveGroundPlane();
        }

        private void OnResetButtonClick()
        {
            Modal modal = Instantiate<Modal>(modalPrefab, canvas);
            modal.SetModalContent(Utils.GetLocalizedText("ConfirmResetTitle"), Utils.GetLocalizedText("ConfirmResetMessage"));
            modal.OnConfirmButtonClick += ResetTrackingStatus;
            modal.OnCancelButtonClick += () =>
            {
                Destroy(modal.gameObject);
            };
        }

        private void ResetTrackingStatus()
        {
            groundPlaneController.ResetTrackingStatus();
            midAirController.ResetTrackingStatus();
            VuforiaBehaviour.Instance.DevicePoseBehaviour.Reset();
        }

        private void CheckForSupportAnchorAndActiveImageTarget()
        {
            if (!SupportAnchor())
            {
                Debug.Log("The device doesn't support anchors");
                ActiveImageTarget();
                header.gameObject.SetActive(false);
            }
        }

        private void CheckForSupportAnchorOrShowErrorModal()
        {
            if (!SupportAnchor())
            {
                Debug.Log("The device doesn't support anchors");
                Modal modal = Instantiate<Modal>(modalPrefab, canvas);
                string errorTextKey = imageTargetError == ImageTargetCreator.IMAGE_FORMAT_ERROR ? "ImageFormatError" : "NoImage";
                modal.SetModalContent(Utils.GetLocalizedText("Error"), Utils.GetLocalizedText(errorTextKey));
                modal.DisableCancelButton();
                modal.OnConfirmButtonClick += GoBackToMenu;
            }
        }

        private void GoBackToMenu()
        {
            Debug.Log("Back to menu");
            SceneManager.LoadScene("MenuScene");
        }

        private void ResetAll()
        {
            if (imageTarget != null)
            {
                imageTarget.gameObject.SetActive(false);
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
            Debug.Log("Set image target");
            if (imageTarget == null && SupportAnchor())
            {
                Modal modal = Instantiate(modalPrefab, canvas);
                string errorTextKey = imageTargetError == ImageTargetCreator.IMAGE_FORMAT_ERROR ? "ImageFormatError" : "NoImage";
                modal.SetModalContent(Utils.GetLocalizedText("Error"), Utils.GetLocalizedText(errorTextKey));
                modal.DisableCancelButton();
                modal.OnConfirmButtonClick += () =>
                {
                    dropdown.value = 0;
                };
                return;
            }
            ResetAll();
            imageTarget.gameObject.SetActive(true);
            modelContainer.transform.parent = imageTarget.transform;
            animationPlayer.ResumeAnimation();
            ResetTransform(modelContainer.transform);
            controlTutorial.ChangeTutorial(ControlTutorial.ControlTutorialEnum.Image);
            OnModeSelected?.Invoke(2);
        }

        private void ActiveMidAir()
        {
            Debug.Log("Set midair target");
            ResetAll();
            midAirController.gameObject.SetActive(true);
            modelContainer.transform.parent = midAirStage.transform;
            animationPlayer.ResumeAnimation();
            ResetTransform(modelContainer.transform);
            midAirController.AdjustModelPosition();
            controlTutorial.ChangeTutorial(ControlTutorial.ControlTutorialEnum.MidAir);
            OnModeSelected?.Invoke(1);
        }

        private void ActiveGroundPlane()
        {
            Debug.Log("Set groundplane target");
            ResetAll();
            groundPlaneController.gameObject.SetActive(true);
            modelContainer.transform.parent = groudPlaneStage.transform;
            animationPlayer.ResumeAnimation();
            ResetTransform(modelContainer.transform);
            controlTutorial.ChangeTutorial(ControlTutorial.ControlTutorialEnum.GroundPlane);
            OnModeSelected?.Invoke(0);
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
