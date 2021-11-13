using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Food : MonoBehaviour
{
    [SerializeField] int points;
    [SerializeField] float speed;
    [SerializeField] private ParticleSystem destroyEffect;
    public event UnityAction<int> AteFood;
    protected abstract void Move();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player= collision.GetComponent<Player>();
        if (player!=null)
        {
            AteFood?.Invoke(points);
            Destroy(gameObject);
            Instantiate(destroyEffect,transform.position,Quaternion.identity);
        }
    }

}
