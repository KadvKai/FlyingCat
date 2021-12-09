using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameObject _endLevelScreen;
    
    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2);
        _gameOverScreen.SetActive(true);
        //Time.timeScale = 0;

    }
    public IEnumerator EndLevel()
    {
        yield return new WaitForSeconds(2);
        _endLevelScreen.SetActive(true);
        //Time.timeScale = 0;

    }

    public void ExitButton()
    {
        //Time.timeScale = 1;
        _gameOverScreen.SetActive(false);
        _endLevelScreen.SetActive(false);
    }
}
