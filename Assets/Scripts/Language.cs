using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Language", menuName = "Language", order = 51)]

public class Language : ScriptableObject
{
    [SerializeField] private Dictionary<string, string> Translation;
    public enum Season
    {
        Spring,
        Summer,
        Autumn,
        Winter
    }
    [SerializeField] private Season se;
    /*= new Dictionary<string, string>()
{
    {"Play","Play" },
    {"Exit","Exit" }
};*/
}
