using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject arrow;
    private Camera cam;
    private int camHight;
    private int camWidth;
    private float camSize;

    void Start()
    {
        cam = UnityEngine.Camera.main;
        camHight = cam.scaledPixelHeight;
        camWidth = cam.scaledPixelWidth;
        camSize = cam.orthographicSize;
    }

    void Update()
    {

        Vector3 playerPos = cam.WorldToViewportPoint(player.transform.position);
        if (playerPos.x > 0 && playerPos.x < 1 && playerPos.y > 0 && playerPos.y < 1)
        {
            if (arrow.activeSelf) arrow.SetActive(false);
            return;
        }
        else
        {
            if (!arrow.activeSelf) arrow.SetActive(true);
            arrow.transform.position= PositionArrow(playerPos);
            Vector3 directionToArrow = player.transform.position - arrow.transform.position;
            float rotationArrow = Mathf.Atan2(directionToArrow.y, directionToArrow.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0f,0f, rotationArrow); 
        }

    }

    private Vector2 PositionArrow(Vector3 playerPos)
    {
        if (playerPos.x < 0 && playerPos.y > 0 && playerPos.y < 1)//левый край
        {

            return new Vector2(cam.transform.position.x - camWidth * camSize / camHight, player.transform.position.y);
        }
        if (playerPos.x > 1 && playerPos.y > 0 && playerPos.y < 1)//правый край
        {
            return new Vector2(cam.transform.position.x + camWidth * camSize / camHight, player.transform.position.y);
        }
        if (playerPos.y < 0 && playerPos.x > 0 && playerPos.x < 1)//нижеий край
        {
            return new Vector2(player.transform.position.x, cam.transform.position.y - camSize);
        }
        if (playerPos.y > 1 && playerPos.x > 0 && playerPos.x < 1)//верхний край
        {
            return new Vector2(player.transform.position.x, cam.transform.position.y + camSize);
        }
        if (playerPos.x < 0 && playerPos.y < 0)//левый нижний край
        {
            return new Vector2(cam.transform.position.x - camWidth * camSize / camHight, cam.transform.position.y - camSize);
        }
        if (playerPos.x < 0 && playerPos.y > 1)//левый верхний край
        {
            return new Vector2(cam.transform.position.x - camWidth * camSize / camHight, cam.transform.position.y + camSize);
        }
        if (playerPos.x > 1 && playerPos.y < 0)//правый нижний край
        {
            return new Vector2(cam.transform.position.x + camWidth * camSize / camHight, cam.transform.position.y - camSize);
        }
        if (playerPos.x > 1 && playerPos.y > 1)//правый верхний край
        {
            return new Vector2(cam.transform.position.x + camWidth * camSize / camHight, cam.transform.position.y + camSize);
        }
        return Vector2.zero;


    }
}
