using UnityEngine;
using EAR.AR;
using Vuforia;

namespace EAR.Presenter
{
    public class PlaybackPresenter : MonoBehaviour
    {
        [SerializeField]
        private ImageTargetCreator imageTargetCreator;
        private ImageTargetBehaviour imageTarget;

        [SerializeField]
        private GroundPlaneController groundPlaneController;
        [SerializeField]
        private MidAirController midAirController;

        void Awake()
        {
            groundPlaneController.OnStateChange += OnGroundPlaneStateChange;
            midAirController.OnStateChanged += OnMidAirStateChange;
            imageTargetCreator.CreateTargetDoneEvent += CreateTargetDoneEventSubscriber;
        }

        private void CreateTargetDoneEventSubscriber()
        {
            imageTarget = imageTargetCreator.GetImageTarget().GetComponent<ImageTargetBehaviour>();
            imageTarget.OnTargetStatusChanged += OnImageTargetStatusChanged;
        }

        private void OnImageTargetStatusChanged(ObserverBehaviour arg1, TargetStatus arg2)
        {
            if (arg2.Status != Status.NO_POSE && arg2.Status != Status.LIMITED)
            {
                GlobalStates.SetIsPlayMode(true);
            }
        }

        private void OnMidAirStateChange(MidAirController.MidAirStateEnum obj)
        {
            if (obj ==  MidAirController.MidAirStateEnum.Placed)
            {
                GlobalStates.SetIsPlayMode(true);
            }
        }

        private void OnGroundPlaneStateChange(GroundPlaneController.GroundPlaneStateEnum obj)
        {
            if(obj == GroundPlaneController.GroundPlaneStateEnum.Placed)
            {
                GlobalStates.SetIsPlayMode(true);
            }
        }
    }
}

