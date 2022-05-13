using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceBar : ABar
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------

    [SerializeField] SOResources _PlayerResources;
    private bool _LerpEssence;


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
            if (_LerpEssence)
            {
                if(_Image.fillAmount < _PlayerResources.essencePercentage)
                    _Image.fillAmount += .01f;
                else
                    _Image.fillAmount -= .01f;

                if(Mathf.Abs(_Image.fillAmount - _PlayerResources.essencePercentage) < .01)
                    _LerpEssence = false;
            }
            else
            {
                //Set the fill amount of the bar to the health percentage
                _Image.fillAmount = _PlayerResources.essencePercentage;
            }
        }
    }

    public void UpdateEssence()
    {        
        _LerpEssence = true;
    }
}
