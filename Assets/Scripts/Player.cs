using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerOutsideCamera))]
[RequireComponent(typeof(PlayerMover))]
public class Player : MonoBehaviour
{
   
    [SerializeField] private GameObject[] balloon;
    [SerializeField] private GameObject[] balloonBurst;
    [SerializeField] private PlayerCanvas _canvas;
    public event UnityAction EndLevel;
    public event UnityAction GameOver;
    private int lives;
    public void SetStartParameters()
    {
        transform.position = new Vector3(-2, -8, 0);
        lives = balloon.Length;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var endLevel = collision.GetComponent<EndLevel>();
        if (endLevel != null) PlayerEndLevel();
        var eating= collision.GetComponent<Eating>();
        if (eating != null) _canvas.SetFoodQuantity(eating.GetFoodQuantity());
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
        GetComponent<PlayerMover>().enabled = false;
        GameOver?.Invoke();
    }

    private void PlayerEndLevel()
    {
        GetComponent<PlayerMover>().enabled = false;
        EndLevel?.Invoke();
    }
}
