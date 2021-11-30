using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Player))]
public class PlayerOutsideCamera : MonoBehaviour
{
    [SerializeField] int _startTimeToDestruction;
    private Player _player;
    private Camera _cam;
    private Coroutine _coroutineCountdown;
    public static event UnityAction<int> TimeToDestruction;



    void Start()
    {
        _player = gameObject.GetComponent<Player>();
        _cam = UnityEngine.Camera.main;

    }


    void Update()
    {
        Vector3 playerPos = _cam.WorldToViewportPoint(_player.transform.position);
        if (playerPos.x < -0 || playerPos.x > 1 || playerPos.y < 0 || playerPos.y > 1)
        {
            if (_coroutineCountdown == null)
            {
                _coroutineCountdown = StartCoroutine(Countdown());
            }
        }
        else
        {
            if (_coroutineCountdown != null)
            { 
                StopCoroutine(_coroutineCountdown);
                _coroutineCountdown = null;
                TimeToDestruction?.Invoke(0);
            }
        }

    }

    private IEnumerator Countdown()
    {
        if (_startTimeToDestruction > 10)
        {
            yield return new WaitForSeconds(_startTimeToDestruction - 10);
        }
        for (int i = 10; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
            TimeToDestruction?.Invoke(i);
        }
        _player.TakeDamage();
        _player.transform.position = _cam.transform.position+new Vector3(0,0,10);
    }
}
