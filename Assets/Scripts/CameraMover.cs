using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    public void SetStartParameters()
    {
        gameObject.transform.position = new Vector3(0f, 0f, -10f);
        this.enabled = true;
    }

    private void Update()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.right);
    }
}
