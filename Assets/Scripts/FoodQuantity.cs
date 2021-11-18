using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodQuantity : MonoBehaviour
{
    [SerializeField] Text text;
    private int food;

    private void Start()
    {
        food = 0;
        text.text = food.ToString();
    }
    private void OnEnable()
    {
        Eating.Ate += FoodQuantityChanget;
    }

    private void FoodQuantityChanget(int newFood)
    {
        food += newFood;
        text.text = food.ToString();
    }

    private void OnDisable()
    {
        Eating.Ate -= FoodQuantityChanget;
    }
}
