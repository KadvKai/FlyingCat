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
    [SerializeField] private Button _nextLevelButton;
    public event UnityAction EndCanvasExit;
    public event UnityAction EndCanvasReiterate;
    public event UnityAction EndCanvasNextLevel;

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
    public void EndLevel(bool repeatButtonInteractable, bool nextLevelButtonActive)
    {
        _endLevelScreen.SetActive(true);
        _gameOverScreen.SetActive(false);
        _endLevelRepeatButton.interactable = repeatButtonInteractable;
        _nextLevelButton.interactable = repeatButtonInteractable;
        _nextLevelButton.gameObject.SetActive(nextLevelButtonActive);
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
    public void NextLevelButton()
    {
        gameObject.SetActive(false);
        EndCanvasNextLevel?.Invoke();
    }
}
