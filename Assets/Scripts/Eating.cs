using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Eating : MonoBehaviour
{
    [SerializeField] int foodQuantity;
    [SerializeField] private ParticleSystem destroyEffect;
    public static event UnityAction<int> Ate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player= collision.GetComponent<Player>();
        if (player!=null)
        {
            Ate?.Invoke(foodQuantity);
            Destroy(gameObject);
            Instantiate(destroyEffect,transform.position,Quaternion.identity);
        }
    }

}
