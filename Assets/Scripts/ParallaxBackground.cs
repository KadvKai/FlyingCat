using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] protected RawImage _image;
    [SerializeField] protected float _speed;
    protected float _startSpeed;
    protected float _rectPosition;
    protected void Start()
    {
        _rectPosition = _image.uvRect.x;
        _startSpeed = _speed;
    }

    private void Update()
    {
        _rectPosition += _speed*Time.deltaTime;
        RectPosition(_rectPosition);
    }
    protected void RectPosition(float rectPosition) 
    {
        _image.uvRect = new Rect(rectPosition, 0, _image.uvRect.width, _image.uvRect.height);
    }
    public void SetStart()
    {
        _speed = _startSpeed;
    }

    public void SetStop()
    {
        _speed = 0;
    }
}
