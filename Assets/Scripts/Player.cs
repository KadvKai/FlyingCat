using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Player : MonoBehaviour
{
    [SerializeField] private float speedMax;
    [SerializeField] private float forceUpRatio;
    [SerializeField] private float forceHorizontalRatio;
    [SerializeField] private float forceWindRatio;
    [SerializeField] private GameObject player;
    private Rigidbody2D rb;
    private float playerScale;
    private bool MouseButtonDown=false;
    private Vector2 windForceVector;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerScale = player.transform.localScale.x;
        windForceVector = Vector2.zero;
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
        if (windForceVector.magnitude>0)
        {
        rb.AddForce(windForceVector);

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

    public void Wind(Vector2Int wind)
    {
        Debug.Log("�����" + wind);
        windForceVector = new Vector2(wind.x * forceWindRatio, wind.y * forceWindRatio);
    }
}
