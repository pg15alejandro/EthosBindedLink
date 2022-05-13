using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure; // Required in C#

public class PlayerHealth : AHealth
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------
    [SerializeField] SOResources _PlayerResources;
    [SerializeField] WinLose _wl;
    [SerializeField] HealthBar _HealthBar;
    Animator _Anim;
    [SerializeField] private float _RumbleDuration;
    [SerializeField] private float _IntensityRightSide;
    [SerializeField] private float _IntensityLeftSide;
    [SerializeField] private ShakeUI _UIShaker;
    [SerializeField] private float _RotationRangeUI;
    [SerializeField] private float _RotationTimeUI;
    /// <summary>
    /// Unity's Start method. Called before the first frame
    /// </summary>
    public override void Start()
    {
        _CurrentHealth = _PlayerResources.CurrentHealth;
        _MaxHealth = _PlayerResources.MaxHealth;
        _UIShaker = gameObject.GetComponent<ShakeUI>();
    }

    private void Update()
    {
        _Anim = gameObject.GetComponent<Animator>();
    }

    /// <summary>
    /// Deals damage to the playyer and checks for death
    /// </summary>
    /// <param name="amount">The amount of damage to be dealt</param>
    public override void Damage(int amount)
    {

        //Deal damage
        _CurrentHealth -= amount;

        //Update the player's resources
        _PlayerResources.CurrentHealth = _CurrentHealth;
        _HealthBar.UpdateHealth();
        if (_CurrentHealth <= 0) Death();

        StartCoroutine(StartVibration());
        StartCoroutine(_UIShaker.FShakeUI(_RotationRangeUI,_RotationTimeUI));
        AkSoundEngine.PostEvent("Play_Arcadia_Takesdamage", this.gameObject);
        AkSoundEngine.SetRTPCValue("arcadia_health", _CurrentHealth);
    }

    private IEnumerator StartVibration()
    {
        GamePad.SetVibration(0, _IntensityLeftSide, _IntensityRightSide);
        yield return new WaitForSeconds(_RumbleDuration);
        GamePad.SetVibration(0,0,0);
    }

    public override void AddHealth(int amount)
    {
        if (_CurrentHealth + amount <= _MaxHealth)
            _CurrentHealth += amount;
        else
            _CurrentHealth = _MaxHealth;

        //Update the player's resources
        _PlayerResources.CurrentHealth = _CurrentHealth;
        _HealthBar.UpdateHealth();

        AkSoundEngine.SetRTPCValue("arcadia_health", _CurrentHealth);
    }

    public override void Death()
    {
        AkSoundEngine.PostEvent("Arcadia_Death", this.gameObject);
        _Anim.CrossFade("Death", .1f, 2);
    }

    public void EventCallerDeath()
    {
        _wl.Death();
    }

}
