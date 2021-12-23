using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _speedMax;
    [SerializeField] private float _forceUpRatio;
    [SerializeField] private float _forceHorizontalRatio;
    [SerializeField] private Wind _wind;
    [SerializeField] private float _forceWindRatio;
    [SerializeField] private float _forcePushRatio;
    [SerializeField] private GameObject _player;
    private Rigidbody2D _rb;
    private float _playerScale;
    private bool _buttonDown=false;
    private Vector2 _windForceVector;
    private bool _controlDisable;

    private void Awake()
    {
     _rb = GetComponent<Rigidbody2D>();   
    }
    private void OnEnable()
    {
        _rb.velocity = Vector2.zero;
        _windForceVector = Vector2.zero;
        _wind.WindChanged += WindChanged;
    }

    private void OnDisable()
    {
        _wind.WindChanged -= WindChanged;
    }
    public void Start()
    {
        _playerScale = _player.transform.localScale.x;
    }

    private void FixedUpdate()
    {
        if (!_controlDisable)
        {
            if (Input.GetMouseButton(0) == true && _buttonDown == false)
            {
                _buttonDown = true;
                AddForcePlayer();
            }
            if (Input.GetMouseButton(0) == false && _buttonDown == true)
            {
                _buttonDown = false;
            }
            SpeedCorrection();
            WindForce(); 
        }
    }

   
    private void WindChanged(Vector2 wind)
    {
        _windForceVector = new Vector2(wind.x * _forceWindRatio, wind.y * _forceWindRatio);
    }

    private void SpeedCorrection()
    {
        if (_rb.velocity.magnitude> _speedMax)
        {
            _rb.velocity = _rb.velocity.normalized * _speedMax;
        }
    }
    private void WindForce()
    {
        if (_windForceVector.magnitude>0)
        {
        _rb.AddForce(_windForceVector);
        }
    }

    private void AddForcePlayer()
    {
        Vector2 forceVector = Vector2.up * _forceUpRatio + VectorHorizontalDisplacement(Camera.main.ScreenToWorldPoint(Input.mousePosition)) * _forceHorizontalRatio;
        _rb.AddForce(forceVector);
        if (forceVector.x < 0) _player.transform.localScale = new Vector3(-_playerScale, _player.transform.localScale.y, _player.transform.localScale.z);
        else _player.transform.localScale = new Vector3(_playerScale, _player.transform.localScale.y, _player.transform.localScale.z);

    }
    private Vector2 VectorHorizontalDisplacement(Vector3 vectorpoint)
    {
        return (vectorpoint - _rb.transform.position).x * Vector2.right;
    }
    public void Push(Vector2 vector)
    {
        _rb.velocity = Vector2.zero;
        _rb.AddForce(vector*_forcePushRatio);
        StartCoroutine(ControlDisable());
    }

    public IEnumerator ControlDisable()
    {
        _controlDisable = true;
        yield return new WaitForSeconds(0.5f);
        _controlDisable = false;
    }
}
