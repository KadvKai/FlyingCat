using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField] Text _textFood;
    [SerializeField] GameObject _pauseScreen;
    public event UnityAction PlayerCanvasExit;
    public event UnityAction PlayerCanvasContinue;

    private void Start()
    {
        _textFood.text = 0.ToString();
    }
   
    public void SetFoodQuantity(int food)
    {
       _textFood.text = food.ToString();
    }

    public void ShowPauseScreen(bool show)
    {
        _pauseScreen.SetActive(show);
    }

    public void ContinueButton()
    {
        PlayerCanvasContinue?.Invoke();
    }
    public void ExitButton()
    {
        PlayerCanvasExit?.Invoke();
    }
}
