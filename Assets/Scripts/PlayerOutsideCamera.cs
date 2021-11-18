using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Player))]
public class PlayerOutsideCamera : MonoBehaviour
{
    [SerializeField] int startTimeToDestruction;
    private Player player;
    private Camera cam;
    private Coroutine coroutineCountdown;
    public static event UnityAction<int> TimeToDestruction;



    void Start()
    {
        player = gameObject.GetComponent<Player>();
        cam = UnityEngine.Camera.main;

    }


    void Update()
    {
        Vector3 playerPos = cam.WorldToViewportPoint(player.transform.position);
        if (playerPos.x < -0 || playerPos.x > 1 || playerPos.y < 0 || playerPos.y > 1)
        {
            if (coroutineCountdown == null)
            {
                coroutineCountdown = StartCoroutine(Countdown());
            }
        }
        else
        {
            if (coroutineCountdown != null)
            { 
                StopCoroutine(coroutineCountdown);
                coroutineCountdown = null;
                TimeToDestruction?.Invoke(0);
            }
        }

    }

    private IEnumerator Countdown()
    {
        if (startTimeToDestruction > 10)
        {
            yield return new WaitForSeconds(startTimeToDestruction - 10);
        }
        for (int i = 10; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
            TimeToDestruction?.Invoke(i);
        }
        player.TakeDamage();
        player.transform.position = cam.transform.position+new Vector3(0,0,10);
    }
}
