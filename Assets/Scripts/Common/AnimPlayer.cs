using Piglet;
using System.Collections.Generic;
using UnityEngine;
using EAR.Entity;
using System;
using System.Linq;

namespace EAR.AnimationPlayer
{
    public class AnimPlayer : MonoBehaviour
    {
        public const int STATIC_POSE_INDEX = 0;
        public event Action<float> AnimationProgressChangeEvent;
        public event Action<bool> AnimationStartEvent;

        private Animation _animation;
        private AnimationList _animationList;

        private AnimationState _currentAnimationState;
        private int _currentIndex = -1;
        private int _clipCount = -1;

        void Awake()
        {
            SetModel(gameObject);
        }

        void OnEnable()
        {
            if (GlobalStates.IsPlayMode())
            {
                PlayAnimation(_currentIndex);
            }
        }

        public void ToggleAnimationPlay(bool play)
        {
            if (_animation != null)
            {
                if (play)
                {
                    _currentAnimationState.speed = 1;
                }
                else
                {
                    _currentAnimationState.speed = 0;
                }
            }
        }

        public void SetAnimationState(float value)
        {
            Debug.Log("Value: " + value + "Entity: " + GetComponentInParent<BaseEntity>().GetEntityName());
            if (_animation != null)
            {
                _currentAnimationState.normalizedTime = value;
            }
        }

        public bool SetModel(GameObject model)
        {
            _animation = model != null ? model.GetComponentInChildren<Animation>() : null ;
            _animationList = model != null ? model.GetComponentInChildren<AnimationList>() : null;
            if (_animation == null || _animationList == null)
            {
                _currentIndex = -1;
                _clipCount = -1;
                _currentAnimationState = null;
                return false;
            }
            if (!_animation.isPlaying)
            {
                _animation.Play(_animationList.Clips[STATIC_POSE_INDEX].name);
                _animation.Stop();
                _currentAnimationState = _animation[_animationList.Clips[0].name];
                _currentIndex = 0;
                _clipCount = _animation.GetClipCount();
                AnimationStartEvent?.Invoke(false);
            }
            else
            {
                _clipCount = _animation.GetClipCount();
                foreach (AnimationState state in _animation)
                {
                    if (state.weight == 1)
                    {
                        _currentAnimationState = state;
                        _currentIndex = -1;
                        for (int i = 0; i < _animationList.Clips.Count; i++)
                        {
                            if (state.name == _animationList.Clips[i].name)
                            {
                                _currentIndex = i;
                                break;
                            }
                        }
                        break;
                    }
                }
                if (_currentAnimationState == null)
                {
                    Debug.LogError("Cannot find active animation state ");
                }
            }
            return true;
        }

        public List<string> GetAnimationList()
        {
            if (_animationList != null)
            {
                return _animationList.Clips.Select(clip => clip.name).ToList();
            }
            return null;
        }

        public void PlayAnimation(int index)
        {
            _currentIndex = index;
            if (_animation != null && _animationList != null)
            {
                //reset the current animation state
                _currentAnimationState.time = 0;
                _currentAnimationState.speed = 1;

                //reset the model pose
                _animation.Play(_animationList.Clips[STATIC_POSE_INDEX].name);
                _animation.Sample();

                //Play the new animation state
                _currentAnimationState = _animation[_animationList.Clips[index].name];
                _currentAnimationState.time = 0;
                _currentAnimationState.speed = 1;
                _animation.Play(_animationList.Clips[index].name);
            }

            if (index == 0)
            {
                _animation.Stop();
                AnimationStartEvent?.Invoke(false);
            }
            else
            {
                AnimationStartEvent?.Invoke(true);
            }
        }

        public int GetCurrentIndex()
        {
            return _currentIndex;
        }

        public int GetAnimationCount()
        {
            return _clipCount;
        }

        void Update()
        {
            if (_animation != null && _animation.isPlaying)
            {
                AnimationProgressChangeEvent?.Invoke(_currentAnimationState.normalizedTime);
            }
        }
    }
}

