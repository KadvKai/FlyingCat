using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameObject _endLevelScreen;
    public event UnityAction EndCanvasExit;
    public event UnityAction EndCanvasReiterate;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitButton();
        }
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0);
        _gameOverScreen.SetActive(true);
        _endLevelScreen.SetActive(false);

    }
    public IEnumerator EndLevel()
    {
        yield return new WaitForSeconds(0);
        _endLevelScreen.SetActive(true);
        _gameOverScreen.SetActive(false);
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
