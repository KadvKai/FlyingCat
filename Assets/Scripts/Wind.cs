using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private ParticleSystem wind;
    [SerializeField] private int maxWindForce;
    private Camera cam;
    private int camHight;
    private int camWidth;
    private float camSize;
    private int windForce;
    private int windForceOld;
    private bool windDirectionRight;
    private ParticleSystemRenderer windRenderer;
    void Start()
    {
        cam = Camera.main;
        camHight = cam.scaledPixelHeight;
        camWidth = cam.scaledPixelWidth;
        camSize = cam.orthographicSize;
        windForce = 0;
        windForceOld = windForce;
        var windShape = wind.shape;
        windShape.scale = new Vector3(2, 2 * camSize, 1);
        windRenderer = wind.GetComponent<ParticleSystemRenderer>();
        StartCoroutine(WindMethod());
    }

    private void Update()
    {
        if (windForce>0)
        {
                if (windDirectionRight)
                {
                    wind.transform.position = new Vector3(cam.transform.position.x + 2 - camWidth * camSize / camHight, 0, 0);
                    windRenderer.flip = new Vector3(0, 0, 0);
                }
                else
                {
                    wind.transform.position = new Vector3(cam.transform.position.x - 2 + camWidth * camSize / camHight, 0, 0);
                    windRenderer.flip = new Vector3(1, 0, 0);
                }
        }
    }

    private IEnumerator WindMethod()
    {
        for (int i = 0; i < 50; i++)
        {
            windForce += Random.Range(-1, 2);
            if (windForce < 0) windForce = 0;
            if (windForce > maxWindForce) windForce = maxWindForce;
            if (windForce == 0)
            {
                if (Random.Range(0, 2) < 1) windDirectionRight = false;
                else windDirectionRight = true;
                wind.gameObject.SetActive(false);
            }
            else
            {
                wind.gameObject.SetActive(true);
                var windEmission = wind.emission;
                windEmission.rateOverTime = windForce;

            }
            if (windForceOld != windForce)
            {
                windForceOld = windForce;
                if (windDirectionRight) player.Wind(new Vector2Int(windForce,0));
                else player.Wind(new Vector2Int(-windForce, 0));
            }
            yield return new WaitForSeconds(10);
        }


    }
}
