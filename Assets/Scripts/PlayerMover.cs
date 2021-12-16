using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float speedMax;
    [SerializeField] private float forceUpRatio;
    [SerializeField] private float forceHorizontalRatio;
    [SerializeField] private Wind _wind;
    [SerializeField] private float forceWindRatio;
    [SerializeField] private GameObject player;
    private Rigidbody2D rb;
    private float playerScale;
    private bool ButtonDown=false;
    private Vector2 windForceVector;

    private void Awake()
    {
     rb = GetComponent<Rigidbody2D>();   
    }
    private void OnEnable()
    {
        rb.velocity = Vector2.zero;
        windForceVector = Vector2.zero;
        _wind.WindChanged += WindChanged;
    }

    private void OnDisable()
    {
        _wind.WindChanged -= WindChanged;
    }
    public void Start()
    {
        playerScale = player.transform.localScale.x;
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0)==true && ButtonDown == false)
        {
            ButtonDown = true;
            AddForcePlayer();
        }
        if (Input.GetMouseButton(0) == false && ButtonDown == true)
        {
            ButtonDown = false;
        }
        SpeedCorrection();
        WindForce();
    }

   
    private void WindChanged(Vector2Int wind)
    {
        windForceVector = new Vector2(wind.x * forceWindRatio, wind.y * forceWindRatio);
    }

    private void SpeedCorrection()
    {
        if (rb.velocity.magnitude> speedMax)
        {
            rb.velocity = rb.velocity.normalized * speedMax;
        }
    }
    private void WindForce()
    {
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
}
