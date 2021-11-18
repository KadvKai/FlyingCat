using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private float speed;
    void Start()
    {
        gameObject.transform.position = new Vector3(0f, 0f, -10f);  
    }

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.right);
    }
}
