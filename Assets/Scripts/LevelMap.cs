using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMap : MonoBehaviour
{
    private readonly int _quantityuniqueNumber=3;//число не повтор€ющихс€ частей уровн€
    private GameObject[] _levelGameObject;
    private readonly int _lengthPartLevel=61;
    private Transform _camTransform;

    public void SetCameraTransform(CameraMover camera)
    {
        _camTransform = camera.gameObject.transform;
    }
    public void CreateNewLevel(int numberLevelParts,GameObject startPartLevel,GameObject[] partLevel,GameObject finishPartLevel)
    {
        _levelGameObject = new GameObject[numberLevelParts + 2];
        _levelGameObject[0] = Instantiate(startPartLevel,new Vector3(0,0,0),Quaternion.identity); 
        Queue<int> oldNumbersPartLevel=new Queue<int>();
        int partLevelLength = partLevel.Length;
        for (int i = 1; i <= numberLevelParts; i++)
        {
            int newNumber;
            do
            {
                newNumber = Random.Range(0, partLevelLength);
            } while (oldNumbersPartLevel.Contains(newNumber));
            oldNumbersPartLevel.Enqueue(newNumber);
            if (oldNumbersPartLevel.Count > _quantityuniqueNumber) oldNumbersPartLevel.Dequeue();
            _levelGameObject[i] = Instantiate(partLevel[newNumber], new Vector3(i* _lengthPartLevel, 0, 0), Quaternion.identity);
        }
        _levelGameObject[numberLevelParts + 1] = Instantiate(finishPartLevel, new Vector3((numberLevelParts + 1) * _lengthPartLevel, 0, 0), Quaternion.identity) ;

        for (int i = 2; i < _levelGameObject.Length; i++)
        {
            _levelGameObject[i].SetActive(false);
        }
    }

    public IEnumerator LevelController()
    {
        for (int i = 1; i < _levelGameObject.Length; i++)
        {
            yield return new WaitUntil(() => (_camTransform.position.x > i * _lengthPartLevel));
            _levelGameObject[i - 1].SetActive(false);
            if (i< _levelGameObject.Length-1) _levelGameObject[i + 1].SetActive(true);
        }
    }

    
}
