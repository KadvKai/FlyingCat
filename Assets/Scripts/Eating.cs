using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Eating : MonoBehaviour
{
    [SerializeField] int _foodQuantity;
    [SerializeField] private ParticleSystem _destroyEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player= collision.GetComponent<Player>();
        if (player!=null)
        {
            Destroy(gameObject);
            Instantiate(_destroyEffect,transform.position,Quaternion.identity);
        }
    }

    public int GetFoodQuantity()
    {
        return _foodQuantity;
    }

}
