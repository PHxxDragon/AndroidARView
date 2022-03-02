using UnityEngine;
using UnityEngine.SceneManagement;
using EAR.AR;
using EAR.View;
using EAR.AnimationPlayer;
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
                Debug.Log("The device doesn't support anchors");
                ActiveImageTarget();
                header.gameObject.SetActive(false);
            }
        }

        private void CreateTargetErrorEventSubscriber(string error)
        {
            Debug.Log("Error creating target image: " + error);
            if (!SupportAnchor())
            {
                Debug.Log("The device doesn't support anchors");
                Modal modal = Instantiate<Modal>(modalPrefab, canvas);
                modal.SetModalContent(Utils.GetLocalizedText("Error"), Utils.GetLocalizedText("NoImage"));
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
            Debug.Log("Set image target");
            if (imageTarget == null && SupportAnchor())
            {
                Modal modal = Instantiate<Modal>(modalPrefab, canvas);
                modal.SetModalContent(Utils.GetLocalizedText("Error"), Utils.GetLocalizedText("NoImage"));
                modal.DisableCancelButton();
                return;
            }
            ResetAll();
            imageTarget.SetActive(true);
            modelContainer.transform.parent = imageTarget.transform;
            animationPlayer.ResumeAnimation();
            ResetTransform(modelContainer.transform);
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
        }

        private void ActiveGroundPlane()
        {
            Debug.Log("Set groundplane target");
            ResetAll();
            groundPlaneController.gameObject.SetActive(true);
            modelContainer.transform.parent = groudPlaneStage.transform;
            animationPlayer.ResumeAnimation();
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
