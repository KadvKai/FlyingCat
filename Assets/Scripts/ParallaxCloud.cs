using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxCloud : ParallaxBackground
{
    [SerializeField] private float _speedWind;
    private int _wind;

    void Update()
    {
        _rectPosition += (_speed  - _wind * _speedWind)* Time.deltaTime;
        RectPosition(_rectPosition);
    }

    public void WindChanged(Vector2Int wind)
    {
        _wind=wind.x;
    }
}
