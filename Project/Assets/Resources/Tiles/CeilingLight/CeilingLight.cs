using UnityEngine;
using System.Collections;

public class CeilingLight : Tile {
    public float DimIntensity;
    public float BrightIntensity;
    public float fadeRate;
    private LightingType intensity;
    Light spotlight;

    void Awake()
    {
        spotlight = transform.GetChild(0).GetComponent<Light>();
        spotlight.intensity = 0.0f;
        spotlight.enabled = false;
    }

    public void UpdateLightIntensity(LightingType intensity)
    {
        this.intensity = intensity;
        if (intensity != LightingType.Dark) spotlight.enabled = true;
    }

    public void SetRange(int length)
    {
        spotlight.range = (Mathf.Abs(spotlight.transform.localPosition.y) + length) / Mathf.Cos((90 - spotlight.transform.rotation.eulerAngles.x) * Mathf.Deg2Rad);
    }

    void Update()
    {
        switch (intensity)
        {
            case LightingType.Dark:
                spotlight.intensity = Mathf.Lerp(spotlight.intensity, 0.0f, Time.deltaTime * fadeRate);
                break;
            case LightingType.Dim:
                spotlight.intensity = Mathf.Lerp(spotlight.intensity, DimIntensity, Time.deltaTime * fadeRate);
                break;
            case LightingType.Bright:
                spotlight.intensity = Mathf.Lerp(spotlight.intensity, BrightIntensity, Time.deltaTime * fadeRate);
                break;
        }
    }
}
