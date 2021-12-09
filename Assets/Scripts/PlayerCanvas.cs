using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField] Text _textFood;

    private void Start()
    {
        _textFood.text = 0.ToString();
    }
   
    public void SetFoodQuantity(int food)
    {
       _textFood.text = food.ToString();
    }
}
