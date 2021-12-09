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
        ObjectOutside(_objectPos.x < -0 || _objectPos.x > 1 || _objectPos.y < -0 || _objectPos.y > 1);
    }


    protected void Update()
    {
        _objectPos = _cam.WorldToViewportPoint(transform.position);
        if (_objectPos.x < -0 || _objectPos.x > 1 || _objectPos.y < -0 || _objectPos.y > 1)
        {

            if (_objectOutsideCamera == false)
            {
                ObjectOutside(true);
            }
        }
        else
        {
            if (_objectOutsideCamera == true)
            {
                ObjectOutside(false);
            }
        }

    }

    protected virtual void ObjectOutside(bool objectOutsideCamera)
    {
        _objectOutsideCamera = objectOutsideCamera;
        ObjectOutsideCameraTrue?.Invoke(_objectOutsideCamera);
    }



}
