using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxCloud : ParallaxBackground
{
    [SerializeField] private float _speedWind;
    [SerializeField] Wind _wind;
    private int _windForce;

    private void OnEnable()
    {
        _wind.WindChanged += WindChanged;
    }

    private void OnDisable()
    {
        _wind.WindChanged -= WindChanged;
    }

    void Update()
    {
        _rectPosition += (_speed  - _windForce * _speedWind)* Time.deltaTime;
        RectPosition(_rectPosition);
    }

    private void WindChanged(Vector2Int wind)
    {
        _windForce=wind.x;
    }
}
