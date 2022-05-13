//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIParticleManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem _Particle;
    private void Awake()
    {
        _Particle.Stop();
    }

    private void PlaySparks()
    {
        _Particle.Play();
    }
}
