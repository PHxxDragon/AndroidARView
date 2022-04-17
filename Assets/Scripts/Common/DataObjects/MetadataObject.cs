using System;
using System.Collections.Generic;
using UnityEngine;

namespace EAR
{
    [Serializable]
    public class MetadataObject
    {
        public List<ModelData> modelDatas = new List<ModelData>();
        public List<NoteData> noteDatas = new List<NoteData>();
        public List<LightData> lightDatas = new List<LightData>();
        public List<SoundData> soundDatas = new List<SoundData>();
        public List<ButtonData> buttonDatas = new List<ButtonData>();
        public List<ImageData> imageDatas = new List<ImageData>();
        public Color ambientColor = Color.white;
    }

    [Serializable]
    public class EntityData
    {
        public string id;
        public string name;
        public TransformData transform;
    }

    [Serializable]
    public class ButtonData : EntityData
    {
        public string activatorEntityId;
        public List<ButtonActionData> actionDatas;
    }

    [Serializable]
    public class ButtonActionData
    {
        public enum ActionType
        {
            Show, Hide, PlayAnimation, PlaySound, StopSound
        }

        public ActionType actionType;
        public string targetEntityId;
        public int animationIndex;
    }

    [Serializable]
    public class SoundData : EntityData
    {
        public string assetId;
        public bool? playAtStart;
        public bool? loop;
    }

    [Serializable]
    public class ImageData : EntityData
    {
        public string assetId;
        public bool? isVisible;
    }

    [Serializable]
    public class ModelData : EntityData
    {
        public int? defaultAnimation;
        public string assetId;
        public bool? isVisible;
    }

    [Serializable]
    public class NoteData : EntityData
    {
        public bool? isVisible;

        public RectTransformData noteContentRectTransformData;

        public string noteContent;
        public Color? textBackgroundColor;

        public Vector4? borderWidth;
        public Vector4? textBorderRadius;
        public Color? borderColor;

        public int? fontSize;
        public Color? textColor;

        public float? boxWidth;
    }

    [Serializable]
    public class LightData
    {
        public string id;
        public string name;

        public LightType lightType = LightType.Directional;
        public Color color = Color.white;
        public float intensity = 1f;
        public Vector3 direction = new Vector3(0, -1, 0);

        public LightData()
        {
        }

        public LightData(LightType lightType, Color color, float intensity, Vector3 direction)
        {
            this.lightType = lightType;
            this.color = color;
            this.intensity = intensity;
            this.direction = direction;
        }
    }
}

