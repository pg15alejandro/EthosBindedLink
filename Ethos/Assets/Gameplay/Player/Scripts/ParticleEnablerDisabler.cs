//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEnablerDisabler : MonoBehaviour
{
    [SerializeField] private MeleeWeaponTrail _TrailParticle;


    private void EnableTrailParticle()
    {
        _TrailParticle.Emit = true;
    }

    private void DisableTrailParticle()
    {
        _TrailParticle.Emit = false;
    }
}
