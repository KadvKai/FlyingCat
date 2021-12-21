using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Camera))]
public class Wind : MonoBehaviour
{
    [SerializeField] private ParticleSystem _wind;
    private int _maxWindForce;
    private int _windForce;
    private int _windForceOld;
    private bool _windDirectionRight;
    private float _windRightPozition;
    private AudioSource _windSound;
    public event UnityAction<Vector2> WindChanged;

    public void SetStartParameters(int maxWindForce)
    {
        _windSound = _wind.GetComponent<AudioSource>();
        _maxWindForce = maxWindForce;
        _windForce = 0;
        WindChanged?.Invoke(new Vector2(0, 0));
        _windForceOld = _windForce;
        Camera cam = GetComponent<Camera>();
        _windRightPozition = 2 - cam.scaledPixelWidth * cam.orthographicSize / cam.scaledPixelHeight;
        var windShape = _wind.shape;
        windShape.scale = new Vector3(2, 2 * cam.orthographicSize, 1);
        StartCoroutine(WindChange());

    }
    private IEnumerator WindChange()
    {
        var windRenderer = _wind.GetComponent<ParticleSystemRenderer>();
        for (int i = 0; i < 50; i++)
        {
            if (_windForce == 0)
            {
                if (Random.Range(0, 2) < 1)
                {
                    _windDirectionRight = false;
                    _wind.transform.localPosition = new Vector3(-_windRightPozition, 0, 10);
                    windRenderer.flip = new Vector3(1, 0, 0);
                }
                else
                {
                    _windDirectionRight = true;
                    _wind.transform.localPosition = new Vector3(_windRightPozition, 0, 10);
                    windRenderer.flip = new Vector3(0, 0, 0);
                }
            }
            _windForce += Random.Range(-1, 2);
            if (_windForce < 0) _windForce = 0;
            if (_windForce > _maxWindForce) _windForce = _maxWindForce;

            if (_windForce != 0 && _windForceOld == 0)
            {
                _wind.gameObject.SetActive(true);
            }

            if (_windForceOld != _windForce)
            {
                var windEmission = _wind.emission;
                windEmission.rateOverTime = _windForce;
                StartCoroutine(SmoothWindChange(_windForce, _windForceOld));
                _windForceOld = _windForce;
            }

            yield return new WaitForSeconds(10);
        }
    }

    private IEnumerator SmoothWindChange(float windForce, float windForceOld)
    {
        var incrementWindForce = (windForce - windForceOld) / 10;
        float currentWindForce = windForceOld;
        int windDirection;
        if (_windDirectionRight) windDirection = 1;
        else windDirection = -1;
        while (Mathf.Abs(windForce - currentWindForce) > Mathf.Abs(incrementWindForce / 2))
        {
            currentWindForce += incrementWindForce;
            WindChanged?.Invoke(new Vector2(windDirection * currentWindForce, 0));
            _windSound.volume = 0.25f * currentWindForce;
            yield return new WaitForSeconds(0.2f);
        }
        if (_windForce == 0)
        {
            _wind.gameObject.SetActive(false);
        }
    }
}
