//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System;
using UnityEngine;

public class ExecutionerHealth : AHealth
{
    [SerializeField] private SOEnemyRegister _EnemyList;
    [SerializeField] private EnemyPlacement _EnemiesPerArea;
    [SerializeField] private float _TimeToDestroyRagDoll;
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
        IsAlive = false;
        AkSoundEngine.PostEvent("Play_Executioner_Death_VO", this.gameObject);
        AkSoundEngine.PostEvent("Play_Executioner_Death_BodyFall", this.gameObject);
        AkSoundEngine.PostEvent("Patio_Arcadia_020", this.gameObject);


        _EnemyList.Executioners.Remove(gameObject);


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
    }

}
