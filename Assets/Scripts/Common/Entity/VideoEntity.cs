using UnityEngine;
using UnityEngine.Video;
using EAR.Container;

namespace EAR.Entity
{
    public class VideoEntity : VisibleEntity
    {
        private static int count = 1;
        [SerializeField]
        private VideoPlayer videoPlayer;
        [SerializeField]
        private GameObject plane;

        private string assetId = "";
        private bool playAtStart;

        public static int GetNextId()
        {
            return count++;
        }

/*        protected override string GetDefaultName()
        {
            return "New video " + count++;
        }*/

        public override void StartDefaultState()
        {
            base.StartDefaultState();
            if (playAtStart)
            {
                PlayVideo();
            }
        }

        public override void ResetEntityState()
        {
            base.ResetEntityState();
            StopVideo();
        }

        public void StopVideo()
        {
            videoPlayer.Pause();
            videoPlayer.time = 0;
        }

        public void PlayVideo()
        {
            videoPlayer.Play();
        }

        public override EntityData GetData()
        {
            VideoData videoData = new VideoData();
            videoData.assetId = assetId;
            videoData.id = GetId();
            videoData.name = GetEntityName();
            videoData.transform = TransformData.TransformToTransformData(transform);
            videoData.isVisible = isVisible;
            videoData.loop = videoPlayer.isLooping;
            videoData.playAtStart = playAtStart;
            return videoData;
        }

        public void SetVideo(string assetId)
        {
            if (this.assetId == assetId || assetId == null)
            {
                return;
            }

            this.assetId = assetId;

            string url = AssetContainer.Instance.GetVideo(assetId);
            videoPlayer.Stop();
            if (url != null)
            {
                videoPlayer.url = url;
                videoPlayer.Prepare();
                videoPlayer.prepareCompleted += (VideoPlayer source) =>
                {
                    Vector3 newScale = plane.transform.localScale;
                    newScale.x = ((float)videoPlayer.texture.width / videoPlayer.texture.height) * 1f;
                    plane.transform.localScale = newScale;
                    videoPlayer.Play();
                    if (!GlobalStates.IsPlayMode())
                    {
                        videoPlayer.time = 0;
                        videoPlayer.Pause();
                    }
                };
            }
        }

        public override void PopulateData(EntityData entityData)
        {
            if (entityData is VideoData videoData)
            {
                base.PopulateData(entityData);

                if (videoData.isVisible.HasValue)
                {
                    isVisible = videoData.isVisible.Value;
                }

                if (videoData.assetId != null)
                {
                    SetVideo(videoData.assetId);
                }

                if (videoData.playAtStart.HasValue)
                {
                    playAtStart = videoData.playAtStart.Value;
                }

                if (videoData.loop.HasValue)
                {
                    videoPlayer.isLooping = videoData.loop.Value;
                }
            }
            else
            {
                Debug.LogError("Wrong data class entity id " + entityData.id);
            }
        }

        public static VideoEntity InstantNewEntity(VideoData videoData)
        {
            VideoEntity videoPrefab = AssetContainer.Instance.GetVideoPrefab();
            VideoEntity videoEntity = Instantiate(videoPrefab);
            videoEntity.PopulateData(videoData);
            OnEntityCreated?.Invoke(videoEntity);
            return videoEntity;
        }
    }
}

