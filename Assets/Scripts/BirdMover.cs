using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SpriteRenderer))]
public class BirdMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _way;
    [SerializeField] bool _random;
    private Vector3[] _flightPoints;
    private Vector3 _targetFlightPosition;
    private bool _directionRightOld;
    private int _newLightPosition;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _flightPoints = FlightPointsCreation(_way);
        _newLightPosition = 0;
        _targetFlightPosition = transform.position;
        _directionRightOld = true;
    }
    private void Update()
    {

        if (Vector3.Distance(transform.position, _targetFlightPosition) < 0.01f)
        {
            _targetFlightPosition = NewTargetPosition();
            bool directionRight = (_targetFlightPosition - transform.position).x > 0;
            Debug.Log("directionRight=" + directionRight);
            Debug.Log("directionRightOld=" + _directionRightOld);
            if (directionRight != _directionRightOld)
            {
                ChangeScale();
            }
            _directionRightOld = directionRight;
        }
        transform.position = Vector3.MoveTowards(transform.position, _targetFlightPosition, _speed * Time.deltaTime);
    }

    private void ChangeScale()
    {
        _spriteRenderer.flipX = !_spriteRenderer.flipX;
    }

    private Vector3 NewTargetPosition()
    {
        if (_random)
        {
            _newLightPosition = Random.Range(0, _flightPoints.Length);
        }
        else
        {
            _newLightPosition ++;
            if (_newLightPosition == _flightPoints.Length) _newLightPosition = 0;
        }
        return _flightPoints[_newLightPosition];
    }
    private Vector3[] FlightPointsCreation(GameObject way)
    {
        Transform[] points = way.GetComponentsInChildren<Transform>();
        Vector3[] flightPoints = new Vector3[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            flightPoints[i] = points[i].position;
        }
        return flightPoints;
    }



}
