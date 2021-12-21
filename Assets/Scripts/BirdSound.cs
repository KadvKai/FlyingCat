using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BirdSound : MonoBehaviour
{
    [SerializeField] private AudioClip _birdSound;
    [SerializeField] private int _timeBirdSound;
    private Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
        StartCoroutine(PlayBirdSound());
    }

    private IEnumerator PlayBirdSound()
    {
        for (int i = 0; i < 50; i++)
        {
            yield return new WaitForSeconds(_timeBirdSound);
            AudioSource.PlayClipAtPoint(_birdSound, transform.position);
            _animator.SetTrigger("Sound");
        }
    }
}
