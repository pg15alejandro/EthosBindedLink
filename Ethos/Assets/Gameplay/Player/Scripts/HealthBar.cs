using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : ABar
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------
    [SerializeField] SOResources _PlayerResources;
    private float _LastHealth;
    private bool _LerpHealth;

    /// <summary>
    /// Unity's Update method. Called once per frame
    /// </summary>
    public override void Update()
    {
        //Check that the player resources is set in the inspector
        if (_PlayerResources == null)
        {
            _Image.fillAmount = 0f;
        }
        else
        {
            if (_LerpHealth)
            {
                if(_Image.fillAmount < _PlayerResources.healthPercentage)
                    _Image.fillAmount += .01f;
                else
                    _Image.fillAmount -= .01f;

                if(Mathf.Abs(_Image.fillAmount - _PlayerResources.healthPercentage) < .01)
                    _LerpHealth = false;
            }
            else
            {
                //Set the fill amount of the bar to the health percentage
                _Image.fillAmount = _PlayerResources.healthPercentage;
            }
        }
    }

    public void UpdateHealth()
    {        
        _LerpHealth = true;
    }
}
