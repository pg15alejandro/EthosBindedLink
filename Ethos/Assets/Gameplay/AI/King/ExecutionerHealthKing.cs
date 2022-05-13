//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionerHealthKing : AHealth
{
    [SerializeField] private KingExecutionerList _ExecList;
    [SerializeField] private float _TimeToDestroyRagDoll;
    public bool Invulnerable;

    /// <summary>
    /// Deals Damage to the player and checks for death
    /// </summary>
    /// <param name="amount">The amount of damage to be dealt</param>
    public override void Damage(int amount)
    {
        if(Invulnerable) return;

        //Decrease the enemy health
        _CurrentHealth -= amount;

        TriggerOnHealthChanged(healthPercentage);

        AkSoundEngine.PostEvent("Play_Executioner_TakeDamage", this.gameObject);

        //Check if the enemy is still alive
        if (_CurrentHealth <= 0)
        {
            Death();
            return;
        }
    }


    /// <summary>
    /// Called when the character dies
    /// </summary>
    public override void Death()
    {
        AkSoundEngine.PostEvent("Play_Executioner_Death", this.gameObject);
        AkSoundEngine.PostEvent("Play_Executioner_Death_BodyFall", this.gameObject);

        if (_ExecList.ExecutionerAttacking.Contains(gameObject))
        {
            _ExecList.ExecutionerAttacking.Remove(gameObject);
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
    } 

}
