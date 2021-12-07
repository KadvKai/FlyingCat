using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerOutsideCamera))]

public class Player : MonoBehaviour
{
   
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] balloon;
    [SerializeField] private GameObject[] balloonBurst;
    [SerializeField] private Canvas _canvas;
    public static event UnityAction GameOver;
    public event UnityAction EndLevel;
    private int lives;
    public void SetStartParameters()
    {
        GetComponent<PlayerOutsideCamera>().TimeToDestruction += _canvas.GetComponent<PlayerOutsideCameraTextIndicator>().TimeToDestructionIndicator;
        transform.position = new Vector3(-2, -8, 0);
        lives = balloon.Length;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var endLevel = collision.GetComponent<EndLevel>();
        if (endLevel != null) PlayerEndLevel();
    }

    public void TakeDamage()
    {
        lives -= 1;
        balloon[lives].SetActive(false);
        balloonBurst[lives].SetActive(true);
        if (lives < 1) PlayerGameOver();
    }

    private void PlayerGameOver()
    {
        GameOver?.Invoke();
    }

    private void PlayerEndLevel()
    {
        EndLevel?.Invoke();
    }

    private void OnDisable()
    {

        GetComponent<PlayerOutsideCamera>().TimeToDestruction -= _canvas.GetComponent<PlayerOutsideCameraTextIndicator>().TimeToDestructionIndicator;
    }
}
