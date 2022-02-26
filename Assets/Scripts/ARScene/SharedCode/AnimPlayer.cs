using Piglet;
using System.Collections.Generic;
using UnityEngine;
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
            _currentAnimationState.normalizedTime = value;
        }

        public bool SetModel(GameObject model)
        {
            _animation = model.GetComponent<Animation>();
            _animationList = model.GetComponent<AnimationList>();
            if (_animation == null || _animationList == null)
            {
                _currentIndex = -1;
                _clipCount = -1;
                return false;
            }
            if (!_animation.isPlaying)
            {
                _animation.Play(_animationList.Clips[STATIC_POSE_INDEX].name);
                _animation.Stop();
                AnimationStartEvent?.Invoke(false);
                _currentAnimationState = _animation[_animationList.Clips[0].name];
                _currentIndex = 0;
            } else
            {
                _currentIndex = -1;
                foreach (AnimationState state in _animation)
                {
                    _currentIndex++;
                    if (state.weight == 1)
                    {
                        _currentAnimationState = state;
                        break;
                    }
                }
                if (_currentAnimationState == null)
                {
                    Debug.LogError("Cannot find active animation state ");
                }
            }
            _clipCount = _animation.GetClipCount();
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

        public void ResumeAnimation()
        {
            if (_animation != null)
            {
                _animation.Play();
            }
        }

        public void PlayAnimation(int index)
        {
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
            } else
            {
                AnimationStartEvent?.Invoke(true);
            }
            _currentIndex = index;
        }

        public int GetCurrentIndex()
        {
            return _currentIndex;
        }

        public int GetMaxIndex()
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

