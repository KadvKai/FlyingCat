using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(BirdSound))]
public class BirdAction : MonoBehaviour
{
    [SerializeField] protected float _timeAction;
    [SerializeField] protected Collider2D _collider;
    protected Animator _animator;
    protected bool _triggerDisable;
    protected BirdSound _birdSound;
    protected PlayerMover _playerMover;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _collider.enabled = false;
        _birdSound = GetComponent<BirdSound>();
        _playerMover = GetComponent<PlayerMover>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_triggerDisable)
        {
            if (collision.gameObject.TryGetComponent<PlayerMover>(out _playerMover))
            {
                StartCoroutine(Action());
            }
        }
    }

    protected virtual IEnumerator Action()
    {
        _playerMover.Push((_playerMover.transform.position - transform.position).normalized);
        _birdSound.enabled = false;
        _animator.SetTrigger("ActionStart");
        _triggerDisable = true;
        yield return new WaitForSeconds(0.2f);
        _collider.enabled = true;
        yield return new WaitForSeconds(_timeAction);
        _animator.SetTrigger("ActionEnd");
        _triggerDisable = false;
        _collider.enabled = false;
        _birdSound.enabled = true;
    }
}
