using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectOutsideCamera : MonoBehaviour
{
    private bool _objectOutsideCamera;
    private Camera _cam;
    public event UnityAction<bool> ObjectOutsideCameraTrue;


    private void Start()
    {
        _cam = UnityEngine.Camera.main;

    }


    private void Update()
    {
        Vector3 objectPos = _cam.WorldToViewportPoint(transform.position);
        if (objectPos.x < -0.1 || objectPos.x > 1.1 || objectPos.y < -0.1 || objectPos.y > 1.1)
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

    protected void ObjectOutside(bool objectOutsideCamera)
    {
        ObjectOutsideCameraTrue?.Invoke(objectOutsideCamera);
    }



}
