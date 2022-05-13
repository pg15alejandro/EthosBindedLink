//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem _BloodParticles;
    [SerializeField] private MeleeWeaponTrail _SlashParticle;

    private void Start()
    {
        _BloodParticles.Stop();
    }

    public void PlayBloodParticles()
    {
        _BloodParticles.Play();
    }

    public void PlaySlashParticles()
    {
        _SlashParticle.Emit = true;
    }

    public void StopSlashParticles()
    {
        _SlashParticle.Emit = false;
    }
}
