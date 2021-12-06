using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowOnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _arrow;
    private Player _player;
    private readonly Camera cam;
    private int camHight;
    private int camWidth;
    private float camSize;

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        camHight = cam.scaledPixelHeight;
        camWidth = cam.scaledPixelWidth;
        camSize = cam.orthographicSize;
    }

    public void SetStartParameters(Player player)
    {
        _player = player;
    }

    void Update()
    {

        Vector3 playerPos = cam.WorldToViewportPoint(_player.transform.position);
        if (playerPos.x > 0 && playerPos.x < 1 && playerPos.y > 0 && playerPos.y < 1)
        {
            if (_arrow.activeSelf) _arrow.SetActive(false);
            return;
        }
        else
        {
            if (!_arrow.activeSelf) _arrow.SetActive(true);
            _arrow.transform.localPosition= PositionArrow(playerPos);
            Vector3 directionToArrow = _player.transform.position - _arrow.transform.position;
            float rotationArrow = Mathf.Atan2(directionToArrow.y, directionToArrow.x) * Mathf.Rad2Deg;
            _arrow.transform.rotation = Quaternion.Euler(0f,0f, rotationArrow); 
        }

    }

    private Vector2 PositionArrow(Vector3 playerPos)
    {
        if (playerPos.x < 0 && playerPos.y > 0 && playerPos.y < 1)//левый край
        {

            return new Vector2( - camWidth * camSize / camHight, _player.transform.position.y);
        }
        if (playerPos.x > 1 && playerPos.y > 0 && playerPos.y < 1)//правый край
        {
            return new Vector2(camWidth * camSize / camHight, _player.transform.position.y);
        }
        if (playerPos.y < 0 && playerPos.x > 0 && playerPos.x < 1)//нижеий край
        {
            return new Vector2(_player.transform.position.x,  - camSize);
        }
        if (playerPos.y > 1 && playerPos.x > 0 && playerPos.x < 1)//верхний край
        {
            return new Vector2(_player.transform.position.x, camSize);
        }
        if (playerPos.x < 0 && playerPos.y < 0)//левый нижний край
        {
            return new Vector2(- camWidth * camSize / camHight, - camSize);
        }
        if (playerPos.x < 0 && playerPos.y > 1)//левый верхний край
        {
            return new Vector2(- camWidth * camSize / camHight, camSize);
        }
        if (playerPos.x > 1 && playerPos.y < 0)//правый нижний край
        {
            return new Vector2(camWidth * camSize / camHight, camSize);
        }
        if (playerPos.x > 1 && playerPos.y > 1)//правый верхний край
        {
            return new Vector2(camWidth * camSize / camHight, camSize);
        }
        return Vector2.zero;


    }
}
