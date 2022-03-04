using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GlobalLight : MonoBehaviour
{
    [SerializeField] Light2D _light;
    [SerializeField] Light2D _lightBackground;

    public void SetLight(LevelParameters.TimesDay timesDay)
    {
        switch (timesDay)
        {
            case LevelParameters.TimesDay.Day:
                {
                    _light.intensity = 1;
                    _lightBackground.intensity = 1;
                    _lightBackground.color = Color.white;
                    return;
                }
            case LevelParameters.TimesDay.Evening:
                {
                    _light.intensity = 0.5f;
                    _lightBackground.intensity = 0.75f;
                    _lightBackground.color = new Color(1,0.4f,0.4f);
                    return;
                }
            case LevelParameters.TimesDay.Night:
                {
                    _light.intensity = 0f;
                    _lightBackground.intensity = 0.3f;
                    _lightBackground.color = Color.white;
                    return;
                }
            case LevelParameters.TimesDay.Morning:
                {
                    _light.intensity = 0.5f;
                    _lightBackground.intensity = 0.75f;
                    _lightBackground.color = new Color(1, 0.6f, 0.3f);
                    return;
                }
        }    
    }
}
