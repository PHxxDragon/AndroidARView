using EAR.Tutorials;
using EAR.AR;
using UnityEngine;
using Vuforia;

namespace EAR.Presenter
{
    public class TutorialPresenter : MonoBehaviour
    {
        [SerializeField]
        private ImageTargetCreator imageTargetCreator;
        private ImageTargetBehaviour imageTarget;

        [SerializeField]
        private GroundPlaneController groundPlaneController;
        [SerializeField]
        private MidAirController midAirController;
        [SerializeField]
        private Tutorial tutorial;
        [SerializeField]
        private ModeSelectPresenter modeSelectPresenter;
        [SerializeField]
        private InitializePresenter initializePresenter;

        private Tutorial.TutorialEnum currentMidAirTut;
        private Tutorial.TutorialEnum currentGroundPlaneTut;
        private Tutorial.TutorialEnum currentImageTut;

        void Awake()
        {
            if (groundPlaneController == null || imageTargetCreator == null || midAirController == null || tutorial == null || modeSelectPresenter == null)
            {
                Debug.Log("Unassigned references");
                return;
            }
            currentGroundPlaneTut = Tutorial.TutorialEnum.ScanEnvironmentGround;
            currentMidAirTut = Tutorial.TutorialEnum.TapToPlace;
            currentImageTut = Tutorial.TutorialEnum.ScanImage;
            groundPlaneController.OnStateChange += OnGroundPlaneStateChange;
            midAirController.OnStateChanged += OnMidAirStateChange;
            modeSelectPresenter.OnModeSelected += OnModeChange;
            imageTargetCreator.CreateTargetDoneEvent += CreateTargetDoneEventSubscriber;
            //TODO
            initializePresenter.LoadDone += OnEntityLoadDone;
            tutorial.gameObject.SetActive(false);
        }

        private void CreateTargetDoneEventSubscriber()
        {
            imageTarget = imageTargetCreator.GetImageTarget().GetComponent<ImageTargetBehaviour>();
            imageTarget.OnTargetStatusChanged += OnImageTargetStatusChanged;
        }

        private void OnImageTargetStatusChanged(ObserverBehaviour arg1, TargetStatus arg2)
        {
            if (arg2.Status == Status.NO_POSE || arg2.Status == Status.LIMITED)
            {
                tutorial.ShowTutorial(Tutorial.TutorialEnum.ScanImage);
                currentImageTut = Tutorial.TutorialEnum.ScanImage;
            } else
            {
                tutorial.ShowTutorial(Tutorial.TutorialEnum.None);
                currentImageTut = Tutorial.TutorialEnum.None;
            }
        }

        private void OnEntityLoadDone()
        {
            tutorial.gameObject.SetActive(true);
        }

        void Start()
        {
            tutorial.ShowTutorial(Tutorial.TutorialEnum.ScanEnvironmentGround);
        }

        private void OnModeChange(int arg0)
        {
            switch (arg0)
            {
                case 0:
                    tutorial.ShowTutorial(currentGroundPlaneTut);
                    break;
                case 1:
                    tutorial.ShowTutorial(currentMidAirTut);
                    break;
                case 2:
                    tutorial.ShowTutorial(currentImageTut);
                    break;
            }
        }

        private void OnMidAirStateChange(MidAirController.MidAirStateEnum obj)
        {
            switch(obj)
            {
                case MidAirController.MidAirStateEnum.NoPose:
                    tutorial.ShowTutorial(Tutorial.TutorialEnum.ScanEnvironmentMid);
                    currentMidAirTut = Tutorial.TutorialEnum.ScanEnvironmentMid;
                    break;
                case MidAirController.MidAirStateEnum.NotPlaced:
                    tutorial.ShowTutorial(Tutorial.TutorialEnum.TapToPlace);
                    currentMidAirTut = Tutorial.TutorialEnum.TapToPlace;
                    break;
                case MidAirController.MidAirStateEnum.Placed:
                    tutorial.ShowTutorial(Tutorial.TutorialEnum.None);
                    currentMidAirTut = Tutorial.TutorialEnum.None;
                    break;
            }
        }

        private void OnGroundPlaneStateChange(GroundPlaneController.GroundPlaneStateEnum obj)
        {
            switch (obj)
            {
                case GroundPlaneController.GroundPlaneStateEnum.PlaneNotDetected:
                    tutorial.ShowTutorial(Tutorial.TutorialEnum.ScanEnvironmentGround);
                    currentGroundPlaneTut = Tutorial.TutorialEnum.ScanEnvironmentGround;
                    break;
                case GroundPlaneController.GroundPlaneStateEnum.NotPlaced:
                    tutorial.ShowTutorial(Tutorial.TutorialEnum.TapToPlace);
                    currentGroundPlaneTut = Tutorial.TutorialEnum.TapToPlace;
                    break;
                case GroundPlaneController.GroundPlaneStateEnum.Placed:
                    tutorial.ShowTutorial(Tutorial.TutorialEnum.None);
                    currentGroundPlaneTut = Tutorial.TutorialEnum.None;
                    break;
            }
        }
    }
}

