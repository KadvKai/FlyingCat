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
        _birdMover.enabled = false;
        GetComponent<ObjectOutsideCamera>().ObjectOutsideCameraTrue += ObjectOutsideCameraTrue;
        StartCoroutine(WaitingBirdOnScreen());
    }

    private void OnDisable()
    {
        GetComponent<ObjectOutsideCamera>().ObjectOutsideCameraTrue -= ObjectOutsideCameraTrue;
    }

    private void ObjectOutsideCameraTrue(bool birdOutsideCamera)
    {
        Debug.Log("За экраном"+ birdOutsideCamera);
        _birdOutsideCamera = birdOutsideCamera;
    }

    private IEnumerator WaitingBirdOnScreen()
    {
        yield return new WaitUntil(() => (_birdOutsideCamera == false));
        _birdMover.enabled = true;
        StartCoroutine(WaitingBirdOffScreen());
    }

    private IEnumerator WaitingBirdOffScreen()
    {
        yield return new WaitUntil(() => (_birdOutsideCamera == true));
        Destroy(gameObject);
    }
}
