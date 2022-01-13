using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterAnimation : MonoBehaviour
{
    private float _startScaleY;
    private Wind _wind;
    private bool _windDirectionRight;

    private void Awake()
    {
        var cam = UnityEngine.Camera.main;
        _wind = cam.GetComponent<Wind>();
        _wind.WindChanged += WindChanged;
        _startScaleY = transform.lossyScale.y;
    }

    private void WindChanged(Vector2 wind)
    {
        transform.localScale = new Vector3(transform.localScale.x, _startScaleY * (1 + Mathf.Abs( wind.x)), transform.localScale.z);
        if (_windDirectionRight == wind.x<0)
        {
            _windDirectionRight = !_windDirectionRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z) ;
        }
    }

    private void OnDestroy()
    {
        _wind.WindChanged -= WindChanged;
    }
}
