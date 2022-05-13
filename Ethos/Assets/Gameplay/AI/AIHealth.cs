//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIHealth : AHealth
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------
    [SerializeField] private SOEnemyRegister _EnemyList;                //List of all the enemies
    [SerializeField] private EnemyPlacement _EnemiesPerArea;
    Animator _Anim;
    [SerializeField] private float _TimeToDestroyRagDoll;
    [SerializeField] private MoveOrbsToPlayer _Orbs;
    [SerializeField] private bool _PlayAudioOnDeath;
    public bool IsAlive = true;

    /// <summary>
    /// Deals Damage to the player and checks for death
    /// </summary>
    /// <param name="amount">The amount of damage to be dealt</param>
    public override void Damage(int amount)
    {
        //Decrease the enemy health
        _CurrentHealth -= amount;

        TriggerOnHealthChanged(healthPercentage);

        AkSoundEngine.PostEvent("Enemy_Takesdamage", this.gameObject);

        //Check if the enemy is still alive
        if (_CurrentHealth <= 0)
        {
            Death();
            return;
        }
    }

    public override void AddHealth(int amount)
    {
        _CurrentHealth += amount;
    }



    /// <summary>
    /// Called when the character dies
    /// </summary>
    public override void Death()
    {
        IsAlive = false;
        AkSoundEngine.PostEvent("Play_Enemy_Death_Drop", this.gameObject);
        AkSoundEngine.PostEvent("Play_Enemy_Death_Sigh", this.gameObject);

        if(_PlayAudioOnDeath)
            AkSoundEngine.PostEvent("Dungeon_Arcadia_018", this.gameObject);

        _Anim = gameObject.GetComponent<Animator>();
        _Anim.SetTrigger("Death");
        if (_EnemyList.EnemiesAlerted.Contains(gameObject))
        {
            _EnemyList.EnemiesAlerted.Remove(gameObject);
        }

        if (_EnemyList.EnemiesInside.Contains(gameObject))
        {
            _EnemyList.EnemiesInside.Remove(gameObject);
            // Debug.Log("Removing", this);
        }

        foreach (var area in _EnemiesPerArea.Areas)
        {
            if (area.Enemies.Contains(gameObject)) area.Enemies.Remove(gameObject);
        }
        var ragDoll = GetComponent<RagdollEnemy>();
        if (ragDoll != null)
        {
            ragDoll.ActivateRagdoll();
            Destroy(gameObject, _TimeToDestroyRagDoll);
        }
        else
        {
            Destroy(gameObject);
        }
        //Destroy the enemy                
    }

    public void DestroyGo()
    {
    }
}
