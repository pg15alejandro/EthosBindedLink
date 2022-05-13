//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOrbsToPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _HealthOrb;
    [SerializeField] private GameObject _EssencehOrb;

    [SerializeField] private Vector3 _OffsetHealthSpawn;
    [SerializeField] private Vector3 _OffsetEssenceSpawn;

    [SerializeField] private PlayerController _PlayerController;
    [SerializeField] private PlayerHealth _PlayerHealth;

    [SerializeField] private ParticleSystem _ExplodeEssenceParticle;
    [SerializeField] private ParticleSystem _ExplodeHealthParticle;

    public void MoveHealthOrbToPlayer(Transform enemy, int health)
    {
        var x = Instantiate(_HealthOrb, enemy.position + _OffsetHealthSpawn, enemy.rotation);
        var y = x.GetComponent<OrbFollowsPlayer>();
        y.AmountOfHealth = health;
        y.PlayerController = _PlayerController;
        y.PlayerHealth = _PlayerHealth;
        y.ExplodeParticle = _ExplodeHealthParticle;

    }

    public void MoveEssenceOrbToPlayer(Transform enemy, int essence)
    {
        var x = Instantiate(_EssencehOrb, enemy.position + _OffsetEssenceSpawn, enemy.rotation);
        var y = x.GetComponent<OrbFollowsPlayer>();
        y.AmountOfEssence = essence;
        y.PlayerController = _PlayerController;
        y.PlayerHealth = _PlayerHealth;
        y.ExplodeParticle = _ExplodeEssenceParticle;
    }
}
