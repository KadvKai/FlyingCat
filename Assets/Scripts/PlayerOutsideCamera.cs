using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Player))]
public class PlayerOutsideCamera : ObjectOutsideCamera
{
    [SerializeField] int _startTimeToDestruction;
    private Coroutine _coroutineCountdown;
    public event UnityAction<int> TimeToDestruction;
    public event UnityAction<Vector3> PlayerOutsideCameraPosition;

    private new void Update()
    {
        base.Update();
        if (_objectOutsideCamera == true) PlayerOutsideCameraPosition(_objectPos);
    }
     protected override void ObjectOutside(bool objectOutsideCamera)
    {
        base.ObjectOutside(objectOutsideCamera);
        if (objectOutsideCamera == true)
            _coroutineCountdown = StartCoroutine(Countdown());
        else if (_coroutineCountdown != null)
        {
        StopCoroutine(_coroutineCountdown);
            TimeToDestruction?.Invoke(0);
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
        GetComponent<Player>().TakeDamage();
        transform.position = _cam.transform.position+new Vector3(0,0,10);
    }
}
