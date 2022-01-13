using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EndCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameObject _endLevelScreen;
    [SerializeField] private Button _gameOverScreenRepeatButton;
    [SerializeField] private Button _endLevelRepeatButton;
    public event UnityAction EndCanvasExit;
    public event UnityAction EndCanvasReiterate;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitButton();
        }
    }

    public void GameOver(bool repeatButtonInteractable)
    {
        _gameOverScreen.SetActive(true);
        _endLevelScreen.SetActive(false);
        _gameOverScreenRepeatButton.interactable = repeatButtonInteractable;
    }
    public void EndLevel(bool repeatButtonInteractable)
    {
        _endLevelScreen.SetActive(true);
        _gameOverScreen.SetActive(false);
        _endLevelRepeatButton.interactable = repeatButtonInteractable;
    }

    public void ExitButton()
    {
        gameObject.SetActive(false);
        EndCanvasExit?.Invoke();

    }

    public void ReiterateButton()
    {
        gameObject.SetActive(false);
        EndCanvasReiterate?.Invoke();
    }
}
