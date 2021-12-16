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
    public event UnityAction Exit;
    private int lives;
    private bool _pause;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    private void Pause()
    {
        if (_pause)
        {
            PlayerCanvasContinue();
        }
        else
        {
            _canvas.PlayerCanvasContinue += PlayerCanvasContinue;
            _canvas.PlayerCanvasExit += PlayerCanvasExit;
            _canvas.ShowPauseScreen(true);
            Time.timeScale = 0;
            _pause = true;
        }
    }

    private void PlayerCanvasExit()
    {
        PlayerCanvasContinue();
        PlayerExit();
    }

    private void PlayerCanvasContinue()
    {
        _canvas.PlayerCanvasContinue -= PlayerCanvasContinue;
        _canvas.PlayerCanvasExit -= PlayerCanvasExit;
        _canvas.ShowPauseScreen(false);
        Time.timeScale = 1;
        _pause = false;
    }

    public void SetStartParameters()
    {
        _canvas.gameObject.SetActive(true);
        transform.position = new Vector3(-2, -8, 0);
        lives = balloon.Length;
        GetComponent<PlayerMover>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var endLevel = collision.GetComponent<EndLevel>();
        if (endLevel != null) PlayerEndLevel();
        var eating = collision.GetComponent<Eating>();
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
    private void PlayerExit()
    {
        GetComponent<PlayerMover>().enabled = false;
        Exit?.Invoke();
    }
}
