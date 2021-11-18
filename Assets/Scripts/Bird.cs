using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] private GameObject bird;
    [SerializeField] private float speed;
    private float startOffsetX;
    private bool move;

    private void Start()
    {
        var cam = UnityEngine.Camera.main;
        var camHight = cam.scaledPixelHeight;
        var camWidth = cam.scaledPixelWidth;
        var camSize = cam.orthographicSize;
        startOffsetX = camWidth * camSize / camHight;
        bird.transform.localPosition = bird.transform.localPosition - bird.transform.right * startOffsetX;
        bird.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var camera = collision.GetComponent<CameraMover>();
        if (camera != null)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            bird.SetActive(true);
            move = true;
        }
    }
    private void Update()
    {
        if (move == true&& bird!=null)
        {
            bird.transform.Translate(speed * Time.deltaTime * bird.transform.right);

        }
    }


}
