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
    private ParticleSystemRenderer _windRenderer;
    public event UnityAction<Vector2Int> WindChanged;

    public void SetStartParameters(int maxWindForce)
    {
        _maxWindForce= maxWindForce;
        _windForce = 0;
        WindChanged?.Invoke(new Vector2Int(0,0));
        _windForceOld = _windForce;
        Camera cam = GetComponent<Camera>();
        _windRightPozition = 2 - cam.scaledPixelWidth * cam.orthographicSize / cam.scaledPixelHeight;
        var windShape = _wind.shape;
        windShape.scale = new Vector3(2, 2 * cam.orthographicSize, 1);
        StartCoroutine(WindChange());
    
    }
    private IEnumerator WindChange()
    {
        _windRenderer = _wind.GetComponent<ParticleSystemRenderer>();
        for (int i = 0; i < 50; i++)
        {
            if (_windForce == 0)
            {
                if (Random.Range(0, 2) < 1)
                { 
                    _windDirectionRight = false;
                    _wind.transform.localPosition = new Vector3(-_windRightPozition, 0, 10);
                    _windRenderer.flip = new Vector3(1, 0, 0);
                }
                else
                {
                    _windDirectionRight = true;
                    _wind.transform.localPosition = new Vector3(_windRightPozition, 0, 10);
                    _windRenderer.flip = new Vector3(0, 0, 0);
                }
            }
            _windForce += Random.Range(-1, 2);
            if (_windForce < 0) _windForce = 0;
            if (_windForce > _maxWindForce) _windForce = _maxWindForce;

            if (_windForce != 0)
            {
                _wind.gameObject.SetActive(true);
                var windEmission = _wind.emission;
                windEmission.rateOverTime = _windForce;
            }
            else
            {
                _wind.gameObject.SetActive(false);
            }
            if (_windForceOld != _windForce)
            {
                _windForceOld = _windForce;
                if (_windDirectionRight) WindChanged?.Invoke(new Vector2Int(_windForce, 0));
                else WindChanged?.Invoke(new Vector2Int(-_windForce, 0));
            }
            yield return new WaitForSeconds(10);
        }


    }
}
