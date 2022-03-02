using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BirdMover))]
[RequireComponent(typeof(BirdSound))]
public class BirdController : MonoBehaviour
{
    private bool _birdOutsideCamera = true;
    private BirdMover _birdMover;
    private BirdSound _birdSound;

    private void OnEnable()
    {
        _birdMover = GetComponent<BirdMover>();
        _birdSound = GetComponent<BirdSound>();
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
        _birdSound.enabled = false;
        yield return new WaitUntil(() => (_birdOutsideCamera == false));
        StartCoroutine(WaitingBirdOffScreen());
    }

    private IEnumerator WaitingBirdOffScreen()
    {
        _birdMover.enabled = true;
        _birdSound.enabled = true;
        yield return new WaitUntil(() => (_birdOutsideCamera == true));
        Destroy(gameObject);
    }
}
