using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ABar : MonoBehaviour
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------
    protected Image _Image;             //Get the image to be filled
    

    /// <summary>
    /// Called when the script instance is being loaded
    /// </summary>
    private void Awake()
    {
        _Image = GetComponent<Image>();
    }


    /// <summary>
    /// Unity's Update method. Called once per frame
    /// </summary>
    public abstract void Update();
}
