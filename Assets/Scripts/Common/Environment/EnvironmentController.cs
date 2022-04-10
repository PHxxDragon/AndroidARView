using EAR;
using System;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    public event Action<Color> OnAmbientLightChanged;
    public event Action<LightData> OnDirectionalLightDataChanged;
    [SerializeField]
    private Light directionalLight;

    public void SetAmbientLight(Color color)
    {
        RenderSettings.ambientLight = color;
        OnAmbientLightChanged?.Invoke(color);
    }

    public void SetDirectionalLight(LightData lightData)
    {
        SetDirectionalLightColor(lightData.color);
        SetDirectionalLightIntensity(lightData.intensity);
        SetDirectionalLightDirection(lightData.direction);
        OnDirectionalLightDataChanged?.Invoke(lightData);
    }

    public void SetDirectionalLightColor(Color color)
    {
        directionalLight.color = color;
    }

    public void SetDirectionalLightIntensity(float value)
    {
        directionalLight.intensity = value;
    }

    public void SetDirectionalLightDirection(Vector3 direction)
    {
        directionalLight.transform.forward = direction;
    }

    public LightData GetLightData()
    {
        return new LightData(LightType.Directional, directionalLight.color, directionalLight.intensity, directionalLight.transform.forward);
    }
}
