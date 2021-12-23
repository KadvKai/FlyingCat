using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleCollider2D))]
public class Eating : MonoBehaviour
{
    [SerializeField] int _foodQuantity;
    [SerializeField] private ParticleSystem _destroyEffect;
    [SerializeField] private AudioClip _destroySound;

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            Destroy(gameObject);
            Instantiate(_destroyEffect, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_destroySound, transform.position);
        }
    }
    
    public int GetFoodQuantity()
    {
        return _foodQuantity;
    }

}
