using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AHealth : MonoBehaviour
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------
    [SerializeField] public int _CurrentHealth = 100;                        //Current health
    [SerializeField] protected int _MaxHealth = 100;                            //Max health
    public float healthPercentage => (float)_CurrentHealth / _MaxHealth;        //Health percentage
    public event Action<float> OnHealthPctChanged = delegate { };       //Event that will be called every time an enemy is hit
    protected void TriggerOnHealthChanged(float val)
    {
        OnHealthPctChanged(val);
    }


    /// <summary>
    /// Unity's Start method. Called before the first frame
    /// </summary>
    public virtual void Start()
    {
        _CurrentHealth = _MaxHealth;    //Set the health to full
    }


    /// <summary>
    /// Deals Damage to the player and checks for death
    /// </summary>
    /// <param name="amount">The amount of damage to be dealt</param>
    public virtual void Damage(int amount)
    {
        //Deal damage to the character
        _CurrentHealth -= amount;

        //Check if the character is still alive
        if (_CurrentHealth <= 0)
            Death();
    }

    public virtual void AddHealth(int amount)
    {
        if(_CurrentHealth + amount <= _MaxHealth)
            _CurrentHealth += amount;
        else
            _CurrentHealth = _MaxHealth;
    }


    /// <summary>
    /// Called when the character dies
    /// </summary>
    public virtual void Death()
    {
        //Destroy the gameObject
       // _Anim.SetTrigger("Death");
        Destroy(gameObject);
    }


    /// <summary>
    /// Returns the current health of the gameObject
    /// </summary>
    public int GetCurrentHealth()
    {
        return _CurrentHealth;
    }
}
