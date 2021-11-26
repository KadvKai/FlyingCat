using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    [SerializeField] private Bird _bird;
    private Camera _cam;
    private Vector3 _birdPos;
    void Start()
    {
        _cam = Camera.main;
        _bird.gameObject.SetActive(false);
        StartCoroutine(WaitingBirdOnScreen());
    }

    void Update()
    {
        if (_bird==null) Destroy(gameObject);
        else _birdPos = _cam.WorldToViewportPoint(_bird.transform.position);
    }

    private IEnumerator WaitingBirdOnScreen()
    {
        yield return new WaitUntil(() => (_birdPos.x > -0.1 && _birdPos.x < 1.1));
        _bird.gameObject.SetActive(true);
        StartCoroutine(WaitingBirdOffScreen());
    }

    private IEnumerator WaitingBirdOffScreen()
    {
        yield return new WaitUntil(() => (_birdPos.x < -0.1 || _birdPos.x > 1.1));
        Destroy(gameObject);
    }
}
