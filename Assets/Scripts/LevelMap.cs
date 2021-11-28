using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMap : MonoBehaviour
{
    [SerializeField] private int _numberLevelParts;
    [SerializeField] private GameObject _startPartLevel;
    [SerializeField] private GameObject[] _partLevel;
    [SerializeField] private GameObject _finishPartLevel;
    private readonly int _quantityuniqueNumber=3;//число не повторяющихся частей уровня
    private GameObject[] _levelGameObject;
    private readonly int _lengthPartLevel=61;
    private Transform _camTransform;
    private void Start()
    {
        _camTransform = Camera.main.transform;
        CreateNewLevel();
        StartCoroutine(LevelController());
    }
    public void CreateNewLevel()
    {
        _levelGameObject = new GameObject[_numberLevelParts + 2];
        _levelGameObject[0] = Instantiate(_startPartLevel,new Vector3(0,0,0),Quaternion.identity); 
        Queue<int> oldNumbersPartLevel=new Queue<int>();
        int partLevelLength = _partLevel.Length;
        for (int i = 1; i <= _numberLevelParts; i++)
        {
            int newNumber;
            do
            {
                newNumber = Random.Range(0, partLevelLength);
            } while (oldNumbersPartLevel.Contains(newNumber));
            oldNumbersPartLevel.Enqueue(newNumber);
            if (oldNumbersPartLevel.Count > _quantityuniqueNumber) oldNumbersPartLevel.Dequeue();
            _levelGameObject[i] = Instantiate(_partLevel[newNumber], new Vector3(i* _lengthPartLevel, 0, 0), Quaternion.identity);
        }
        _levelGameObject[_numberLevelParts + 1] = Instantiate(_finishPartLevel, new Vector3((_numberLevelParts + 1) * _lengthPartLevel, 0, 0), Quaternion.identity) ;

        for (int i = 2; i < _levelGameObject.Length; i++)
        {
            _levelGameObject[i].SetActive(false);
        }
    }

    private IEnumerator LevelController()
    {
        for (int i = 0; i < _levelGameObject.Length; i++)
        {
            Debug.Log("Стартует корутина"+i);
            yield return new WaitUntil(() => (_camTransform.position.x > i * _lengthPartLevel));
            if (i>0) _levelGameObject[i - 1].SetActive(false);
            if (i< _levelGameObject.Length-1) _levelGameObject[i + 1].SetActive(true);
            //StartCoroutine(WaitingPartLevel(i));
        }
    }

    
}
