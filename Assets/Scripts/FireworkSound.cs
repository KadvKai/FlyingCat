using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
[RequireComponent(typeof(ParticleSystem))]

public class FireworkSound : MonoBehaviour
{
    [SerializeField] private AudioClip _launchSound;
    [Range(0, 1)] public float _launchSoundVolume;
    [SerializeField] private AudioClip _explosionSound;
    [Range(0, 1)] public float _explosionSoundVolume;

    private ParticleSystem _parentParticleSystem;
    private readonly IDictionary<uint, ParticleSystem.Particle> _trackedParticles = new Dictionary<uint, ParticleSystem.Particle>();

    void Start()
    {
        _parentParticleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        var liveParticles = new ParticleSystem.Particle[_parentParticleSystem.particleCount];
       _parentParticleSystem.GetParticles(liveParticles);


        if (AddParticle(liveParticles))
        {
            AudioSource.PlayClipAtPoint(_launchSound, transform.position, _launchSoundVolume);
        }
        
        if (RemoveParticle(liveParticles))
        {
            AudioSource.PlayClipAtPoint(_explosionSound, transform.position, _explosionSoundVolume);
        }
    }

    private bool AddParticle(ParticleSystem.Particle[] liveParticles)
    {
        int numberAddParticle = 0;
        foreach (var activeParticle in liveParticles)
        {
            if (_trackedParticles.TryGetValue(activeParticle.randomSeed, out ParticleSystem.Particle foundParticle) == false)
            {
                _trackedParticles.Add(activeParticle.randomSeed, activeParticle);
                numberAddParticle += 1;
            }
        }
        if (numberAddParticle == 0) return false;
        else return true;
    }

    private bool RemoveParticle(ParticleSystem.Particle[] liveParticles)
    {
        int numberRemoveParticle = 0;
        var updatedParticleAsDictionary = liveParticles.ToDictionary(x => x.randomSeed, x => x);
        var dictionaryKeysAsList = _trackedParticles.Keys.ToList();

        foreach (var dictionaryKey in dictionaryKeysAsList)
        {
            if (updatedParticleAsDictionary.ContainsKey(dictionaryKey) == false)
            {
                _trackedParticles.Remove(dictionaryKey);
                numberRemoveParticle += 1;
            }
        }
        if (numberRemoveParticle == 0) return false;
        else return true;
    }
}
