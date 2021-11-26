using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;

    private void Start()
    {
        gameOverScreen.SetActive(false);
    }
    private void GameOver()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;

    }

    public void ExitButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnEnable()
    {
        Player.GameOver += GameOver;
    }
    private void OnDisable()
    {
        Player.GameOver -= GameOver;
    }

}
