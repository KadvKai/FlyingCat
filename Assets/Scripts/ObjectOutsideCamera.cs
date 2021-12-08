using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectOutsideCamera : MonoBehaviour
{
    protected bool _objectOutsideCamera;
    protected Camera _cam;
    protected Vector3 _objectPos;
    public event UnityAction<bool> ObjectOutsideCameraTrue;


    private void Start()
    {
        _cam = UnityEngine.Camera.main;
        _objectPos = _cam.WorldToViewportPoint(transform.position);
        if (_objectPos.x < -0 || _objectPos.x > 1 || _objectPos.y < -0 || _objectPos.y > 1) _objectOutsideCamera = false;
        else _objectOutsideCamera = true;
    }


    protected void Update()
    {
        _objectPos = _cam.WorldToViewportPoint(transform.position);
        if (_objectPos.x < -0 || _objectPos.x > 1 || _objectPos.y < -0 || _objectPos.y > 1)
        {

            if (_objectOutsideCamera ==false)
            {
                ObjectOutside(true);
                _objectOutsideCamera = true;
            }
        }
        else
        {
            if (_objectOutsideCamera == true)
            {
                ObjectOutside(false);
                _objectOutsideCamera = false;
            }
        }

    }

    protected virtual void ObjectOutside(bool objectOutsideCamera)
    {
        ObjectOutsideCameraTrue?.Invoke(objectOutsideCamera);
    }



}
