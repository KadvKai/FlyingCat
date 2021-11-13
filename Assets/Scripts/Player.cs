using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Player : MonoBehaviour
{
    [SerializeField] private float speedMax;
    [SerializeField] private float forceUpRatio;
    [SerializeField] private float forceHorizontalRatio;
    [SerializeField] private GameObject player;
    private Rigidbody2D rb;
    private float playerScale;
    private bool MouseButtonDown=false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerScale = player.transform.localScale.x;
    }

   

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0)==true && MouseButtonDown == false)
        {
            MouseButtonDown = true;
            AddForcePlayer();
        }
        if (Input.GetMouseButton(0) == false && MouseButtonDown == true)
        {
            MouseButtonDown = false;
        }
        if (rb.velocity.magnitude> speedMax)
        {
            rb.velocity = rb.velocity.normalized * speedMax;
        }
    }

    private void AddForcePlayer()
    {
        Vector2 forceVector = Vector2.up * forceUpRatio + VectorHorizontalDisplacement(Camera.main.ScreenToWorldPoint(Input.mousePosition)) * forceHorizontalRatio;
        rb.AddForce(forceVector);
        if (forceVector.x < 0) player.transform.localScale = new Vector3(-playerScale, player.transform.localScale.y, player.transform.localScale.z);
        else player.transform.localScale = new Vector3(playerScale, player.transform.localScale.y, player.transform.localScale.z);

    }
    private Vector2 VectorHorizontalDisplacement(Vector3 vectorpoint)
    {
        return (vectorpoint - rb.transform.position).x * Vector2.right;
    }

}
