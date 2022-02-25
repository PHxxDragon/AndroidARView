using EAR.AR;
using UnityEngine;
using EAR.AnimationPlayer;
using System;

namespace EAR.Editor.Presenter
{
    public class AnimationPresenter : MonoBehaviour
    {
        [SerializeField]
        private ModelLoader modelLoader;
        [SerializeField]
        private AnimPlayer animationPlayer;
        [SerializeField]
        private Camera _camera;

        private bool hasAnimation = false;

        void Start()
        {
            if (modelLoader != null && animationPlayer != null)
            {
                modelLoader.OnLoadEnded += OnLoadEnded;
            } else
            {
                Debug.Log("Unassigned references");
            }
            if (_camera == null)
            {
                _camera = Camera.main;
            }
        }

        private void OnLoadEnded()
        {
            hasAnimation = animationPlayer.SetModel(modelLoader.GetModel());
            if (hasAnimation)
            {
                animationPlayer.PlayAnimation(1);
            }
        }

/*        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlayNextAnimation();
            }
        }*/

        private void PlayNextAnimation()
        {
            if (hasAnimation)
            {
                int current = animationPlayer.GetCurrentIndex();
                int max = animationPlayer.GetMaxIndex();
                animationPlayer.PlayAnimation((current + 1) % max);
            }
        }
    }
}

