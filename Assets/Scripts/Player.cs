using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]

public class Player : MonoBehaviour
{
    [SerializeField] private float speedMax;
    [SerializeField] private float forceUpRatio;
    [SerializeField] private float forceHorizontalRatio;
    [SerializeField] private float forceWindRatio;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] balloon;
    [SerializeField] private GameObject[] balloonBurst;
    public static event UnityAction GameOver;
    public event UnityAction EndLevel;
    private int lives;
    private Rigidbody2D rb;
    private float playerScale;
    private bool MouseButtonDown=false;
    private Vector2 windForceVector;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerScale = player.transform.localScale.x;
    }
    
    public void WindChanged(Vector2Int wind)
    {
        windForceVector = new Vector2(wind.x * forceWindRatio, wind.y * forceWindRatio);
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
        SpeedCorrection();
        WindForce();
    }
    public void SetStartParameters()
    {
        transform.position = new Vector3(-2, -8, 0);
        lives = balloon.Length;
        windForceVector = Vector2.zero;
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


    public void TakeDamage()
    {
        lives -= 1;
        balloon[lives].SetActive(false);
        balloonBurst[lives].SetActive(true);
        if (lives < 1) PlayerGameOver();
    }

    private void PlayerGameOver()
    {
        GameOver?.Invoke();
    }

    private void PlayerEndLevel()
    {
        EndLevel?.Invoke();
    }
}
