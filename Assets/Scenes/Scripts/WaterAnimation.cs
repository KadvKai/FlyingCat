using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterAnimation : MonoBehaviour
{
    [SerializeField] private float _startScaleY;
    [SerializeField] private TileBase _tileOld;
    [SerializeField] private TileBase _tileNew;
    private TileBase _tileActive;
    private Wind _wind;
    private bool _windDirectionRight;
    private Tilemap _tilemap;

    private void Awake()
    {
        var cam = UnityEngine.Camera.main;
        _wind = cam.GetComponent<Wind>();
        _wind.WindChanged += WindChanged;
        transform.localScale = new Vector3(transform.localScale.x, _startScaleY , transform.localScale.z);
        _tilemap = GetComponent<Tilemap>();
        _tileActive = _tileOld;
    }

    private void WindChanged(Vector2 wind)
    {
        transform.localScale = new Vector3(transform.localScale.x, _startScaleY * (1 + Mathf.Abs( wind.x)), transform.localScale.z);
        if (_windDirectionRight == wind.x<0)
        {
            _windDirectionRight = !_windDirectionRight;
            if (_tileActive == _tileOld)
            {
                _tilemap.SwapTile(_tileActive, _tileNew);
                _tileActive = _tileNew;
            }
            else
            {
                _tilemap.SwapTile(_tileActive, _tileOld);
                _tileActive = _tileOld;
            }
           // transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z) ;
            //if (_windDirectionRight) transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
            //else transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
        }
    }

    private void OnDestroy()
    {
        _wind.WindChanged -= WindChanged;
    }
}
