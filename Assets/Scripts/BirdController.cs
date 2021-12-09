using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BirdMover))]
public class BirdController : MonoBehaviour
{
    private bool _birdOutsideCamera = true;
    private BirdMover _birdMover;

    private void Awake()
    {
        _birdMover = GetComponent<BirdMover>();
        GetComponent<ObjectOutsideCamera>().ObjectOutsideCameraTrue += ObjectOutsideCameraTrue;
        StartCoroutine(WaitingBirdOnScreen());
    }

    private void OnDisable()
    {
        GetComponent<ObjectOutsideCamera>().ObjectOutsideCameraTrue -= ObjectOutsideCameraTrue;
    }

    private void ObjectOutsideCameraTrue(bool birdOutsideCamera)
    {
        _birdOutsideCamera = birdOutsideCamera;
    }

    private IEnumerator WaitingBirdOnScreen()
    {
        _birdMover.enabled = false;
        yield return new WaitUntil(() => (_birdOutsideCamera == false));
        StartCoroutine(WaitingBirdOffScreen());
    }

    private IEnumerator WaitingBirdOffScreen()
    {
        _birdMover.enabled = true;
        yield return new WaitUntil(() => (_birdOutsideCamera == true));
        Destroy(gameObject);
    }
}
