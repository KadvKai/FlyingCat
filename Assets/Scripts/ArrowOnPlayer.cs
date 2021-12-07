using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowOnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _arrow;
    private Camera _cam;
    private float _camTopPosition;
    private float _camRightPosition;

    public void SetStartParameters()
    {
        _cam = GetComponent<Camera>();
        _camTopPosition=_cam.orthographicSize;
        _camRightPosition = _cam.scaledPixelWidth * _cam.orthographicSize / _cam.scaledPixelHeight;
        _arrow.SetActive(false);
    }
    public void PlayerOutsideCameraTrue(bool playerOutsideCamera, Vector3 playerPos)
    {
             _arrow.SetActive(playerOutsideCamera);
        if (_arrow.activeSelf)
        {
            _arrow.transform.localPosition = PositionArrow(playerPos);

            Vector2 directionToArrow = new Vector2((2 * playerPos.x - 1) * _camRightPosition, (2 * playerPos.y - 1) * _camTopPosition) - (Vector2)_arrow.transform.localPosition;

            float rotationArrow = Mathf.Atan2(directionToArrow.y, directionToArrow.x) * Mathf.Rad2Deg;
            _arrow.transform.rotation = Quaternion.Euler(0f, 0f, rotationArrow);
        }
    }
    private Vector3 PositionArrow(Vector3 playerPos)
    {
        if (playerPos.x < 0 && playerPos.y > 0 && playerPos.y < 1)//левый край
        {

            return new Vector3(-_camRightPosition, (2*playerPos.y-1)* _camTopPosition,10);
        }

        if (playerPos.x > 1 && playerPos.y > 0 && playerPos.y < 1)//правый край
        {
            return new Vector3(_camRightPosition, (2 * playerPos.y - 1) * _camTopPosition, 10);
        }

        if (playerPos.y < 0 && playerPos.x > 0 && playerPos.x < 1)//нижеий край
        {
            return new Vector3((2 * playerPos.x - 1) * _camRightPosition, -_camTopPosition, 10);
        }

        if (playerPos.y > 1 && playerPos.x > 0 && playerPos.x < 1)//верхний край
        {
            return new Vector3((2 * playerPos.x - 1) * _camRightPosition, _camTopPosition, 10);
        }

        if (playerPos.x < 0 && playerPos.y < 0)//левый нижний край
        {
            return new Vector3(-_camRightPosition, -_camTopPosition, 10);
        }

        if (playerPos.x < 0 && playerPos.y > 1)//левый верхний край
        {
            return new Vector3(-_camRightPosition, _camTopPosition, 10);
        }

        if (playerPos.x > 1 && playerPos.y < 0)//правый нижний край
        {
            return new Vector3(_camRightPosition, -_camTopPosition, 10);
        }

        if (playerPos.x > 1 && playerPos.y > 1)//правый верхний край
        {
            return new Vector3(_camRightPosition, _camTopPosition, 10);
        }
        return Vector3.zero;


    }
}
