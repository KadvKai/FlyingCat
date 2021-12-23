using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BirdMover))]
public class EagleAction : BirdAction
{
    [SerializeField] private float _speedAction;
    private Vector3 _targetFlightPosition;
    protected override IEnumerator Action()
    {
        BirdMover birdMover = GetComponent<BirdMover>();
        birdMover.enabled = false;
        _targetFlightPosition = _playerMover.transform.position;
        _birdSound.enabled = false;
        _triggerDisable = true;
        while (transform.right!= (transform.position - _targetFlightPosition).normalized)
        {
            transform.right= Vector2.MoveTowards(transform.right,(transform.position- _targetFlightPosition), 10*Time.deltaTime);
            yield return null;
        }
        _animator.SetTrigger("ActionStart");
        yield return new WaitForSeconds(0.1f);

        _collider.enabled = true;
        while (transform.position != _targetFlightPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetFlightPosition, _speedAction * Time.deltaTime);
            yield return null;
        }
        _animator.SetTrigger("ActionEnd");
        _collider.enabled = false;
        _birdSound.enabled = true;
        birdMover.enabled = true;
        transform.right = Vector3.right;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.TakeDamage();
            _playerMover.Push((_playerMover.transform.position - transform.position).normalized);
            _targetFlightPosition = transform.position;
        }

    }

}
