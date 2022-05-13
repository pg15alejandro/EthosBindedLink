//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingHealth : AHealth
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------
    public bool invulnerable;
    private Animator _Anim;

    public override void Start()
    {
        _CurrentHealth = _MaxHealth;
        _Anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Deals Damage to the player and checks for death
    /// </summary>
    /// <param name="amount">The amount of damage to be dealt</param>
    public override void Damage(int amount)
    {
        if (invulnerable) return;

        //Decrease the enemy health
        _CurrentHealth -= amount;

        TriggerOnHealthChanged(healthPercentage);

        AkSoundEngine.PostEvent("King_TakeDamage_VO", this.gameObject);

        //Check if the enemy is still alive
        if (_CurrentHealth <= 0)
        {
            Death();
            return;
        }
    }

    public override void Death()
    {
        invulnerable = true;
        _Anim.SetTrigger("Death");
    }
}
