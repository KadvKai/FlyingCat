using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bird : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _way;
    private Vector3[] _flightPoints;
    private Vector3 _targetFlightPosition;
    private bool _directionRightOld;

    private void Start()
    {
        _flightPoints = FlightPointsCreation(transform.position, _way);
        _targetFlightPosition = _flightPoints[0];
        _directionRightOld = true;
    }
    private void Update()
    {

        if (Vector3.Distance(transform.position, _targetFlightPosition) < 0.01f)
        {
            _targetFlightPosition = NewTargetPosition();
            bool directionRight = (_targetFlightPosition - transform.position).x > 0;
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
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private Vector3 NewTargetPosition()
    {

        return _flightPoints[Random.Range(0, _flightPoints.Length)];
    }
    private Vector3[] FlightPointsCreation(Vector3 startPosition, GameObject way)
    {
        Transform[] points = way.GetComponentsInChildren<Transform>();
        Vector3[] flightPoints = new Vector3[points.Length + 1];
        flightPoints[0] = startPosition;
        for (int i = 0; i < points.Length; i++)
        {
            flightPoints[i + 1] = points[i].position;
        }
        return flightPoints;
    }



}
