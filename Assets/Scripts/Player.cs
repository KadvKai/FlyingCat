using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerOutsideCamera))]
[RequireComponent(typeof(PlayerMover))]
public class Player : MonoBehaviour
{

    [SerializeField] private GameObject[] _balloon;
    [SerializeField] private GameObject[] _balloonBurst;
    [SerializeField] private PlayerCanvas _canvas;
    public event UnityAction EndLevel;
    public event UnityAction GameOver;
    public event UnityAction Exit;
    private int lives;
    private bool _pause;
    private int _food;
    delegate void MultiDelegate();
  


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
        transform.position = new Vector3(-2, -7, 0);
        lives = _balloon.Length;
        foreach (var balloon in _balloon)
        {
            balloon.SetActive(true);
        }
        foreach (var balloon in _balloonBurst)
        {
            balloon.SetActive(false);
        }
        GetComponent<PlayerMover>().enabled = true;
    }

   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var eating = collision.gameObject.GetComponent<Eating>();
        if (eating != null)
            {
            _food += eating.GetFoodQuantity();
                _canvas.SetFoodQuantity(_food); 
            }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var endLevel = collision.gameObject.GetComponent<EndLevel>();
        if (endLevel != null) PlayerEndLevel();
        
    }

    public void TakeDamage()
    {
        lives -= 1;
        _balloon[lives].SetActive(false);
        _balloonBurst[lives].SetActive(true);
        if (lives < 1) PlayerGameOver();
    }

    private void PlayerGameOver()
    {
        //GetComponent<PlayerMover>().enabled = false;
        GameOver?.Invoke();
        gameObject.SetActive(false);

    }

    private void PlayerEndLevel()
    {
        //GetComponent<PlayerMover>().enabled = false;
        EndLevel?.Invoke();
        gameObject.SetActive(false);
        _canvas.gameObject.SetActive(false);
    }
    private void PlayerExit()
    {
        //GetComponent<PlayerMover>().enabled = false;
        Exit?.Invoke();
        gameObject.SetActive(false);
        _canvas.gameObject.SetActive(false);
    }
}
