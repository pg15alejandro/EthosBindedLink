using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaBar : ABar
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------
    [SerializeField] SOResources _PlayerResources;


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
            //Set the fill amount of the bar to the health percentage
            _Image.fillAmount = _PlayerResources.staminaPercentage;
        }
    }
}
