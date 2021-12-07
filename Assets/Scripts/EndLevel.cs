using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    [SerializeField] ParticleSystem _firework;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player!=null)
        {
            _firework.gameObject.SetActive(true);
        }
    }
}
