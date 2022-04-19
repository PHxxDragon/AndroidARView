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
        public NullableBool playAtStart;
        public NullableBool loop;
    }

    [Serializable]
    public class ImageData : EntityData
    {
        public string assetId;
        public NullableBool isVisible;
    }

    [Serializable]
    public struct NullableBool
    {
        public bool Value;
        public bool HasValue;

        public NullableBool(bool value)
        {
            Value = value;
            HasValue = true;
        }

        public static implicit operator NullableBool(NullableNull _) => new NullableBool();
        public static implicit operator NullableBool(bool value) => new NullableBool(value);
        public static implicit operator bool(NullableBool value) => value.Value;
    }

    public sealed class NullableNull
    {
        private NullableNull()
        { }
    }

    [Serializable]
    public struct NullableInt
    {
        public int Value;
        public bool HasValue;

        public NullableInt(int value)
        {
            Value = value;
            HasValue = true;
        }

        public static implicit operator NullableInt(NullableNull _) => new NullableInt();
        public static implicit operator NullableInt(int value) => new NullableInt(value);
        public static implicit operator int(NullableInt value) => value.Value;
    }

    [Serializable]
    public struct NullableFloat
    {
        public float Value;
        public bool HasValue;

        public NullableFloat(float value)
        {
            Value = value;
            HasValue = true;
        }

        public static implicit operator NullableFloat(NullableNull _) => new NullableFloat();
        public static implicit operator NullableFloat(float value) => new NullableFloat(value);
        public static implicit operator float(NullableFloat value) => value.Value;
    }

    [Serializable]
    public struct NullableColor
    {
        public Color Value;
        public bool HasValue;

        public NullableColor(Color value)
        {
            Value = value;
            HasValue = true;
        }

        public static implicit operator NullableColor(NullableNull _) => new NullableColor();
        public static implicit operator NullableColor(Color value) => new NullableColor(value);
        public static implicit operator Color(NullableColor value) => value.Value;
    }

    [Serializable]
    public struct NullableVector4
    {
        public Vector4 Value;
        public bool HasValue;

        public NullableVector4(Vector4 value)
        {
            Value = value;
            HasValue = true;
        }

        public static implicit operator NullableVector4(NullableNull _) => new NullableVector4();
        public static implicit operator NullableVector4(Vector4 value) => new NullableVector4(value);
        public static implicit operator Vector4(NullableVector4 value) => value.Value;
    }

    [Serializable]
    public class ModelData : EntityData
    {
        public NullableInt defaultAnimation;
        public string assetId;
        public NullableBool isVisible;
    }

    [Serializable]
    public class NoteData : EntityData
    {
        public NullableBool isVisible;

        public RectTransformData noteContentRectTransformData;

        public string noteContent;
        public NullableColor textBackgroundColor;

        public NullableVector4 borderWidth;
        public NullableVector4 textBorderRadius;
        public NullableColor borderColor;

        public NullableInt fontSize;
        public NullableColor textColor;

        public NullableFloat boxWidth;
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

