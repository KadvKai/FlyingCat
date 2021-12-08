using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Eating : MonoBehaviour
{
    [SerializeField] int _foodQuantity;
    [SerializeField] private ParticleSystem _destroyEffect;
    public static event UnityAction<int> Ate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player= collision.GetComponent<Player>();
        if (player!=null)
        {
            Ate?.Invoke(_foodQuantity);
            Destroy(gameObject);
            Instantiate(_destroyEffect,transform.position,Quaternion.identity);
        }
    }

}
