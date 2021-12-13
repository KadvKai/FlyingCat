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

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2);
        _gameOverScreen.SetActive(true);

    }
    public IEnumerator EndLevel()
    {
        yield return new WaitForSeconds(2);
        _endLevelScreen.SetActive(true);
        EndCanvasExit?.Invoke();
    }

    public void ExitButton()
    {
        _gameOverScreen.SetActive(false);
        _endLevelScreen.SetActive(false);

    }

    public void ReiterateButton()
    {
        _gameOverScreen.SetActive(false);
        _endLevelScreen.SetActive(false);
        EndCanvasReiterate?.Invoke();
    }
}
