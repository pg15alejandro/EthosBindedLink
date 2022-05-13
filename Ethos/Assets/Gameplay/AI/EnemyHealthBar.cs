//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------
    [Header("HealthImages")]
    [SerializeField] Image _HealthLeftImage;
    [SerializeField] Image _HealthRightImage;

    /*[Header("SwordImages")]
    [SerializeField] Image _SwordLeftImage;
    [SerializeField] Image _SwordRightImage;*/
    [SerializeField] private float _UpdateSpeedSeconds = 0.5f;


    /// <summary>
    /// Unity's Start method. Called before the first frame
    /// </summary>
    private void Start()
    {
        //Subscribe the HandleHealthChanged method to the OnHealthPctChanged event
        GetComponentInParent<AHealth>().OnHealthPctChanged += HandleHealthChanged;
        

        //Subscribe the HanldeSwordChanged method to the OnSwordChanged event
//        transform.parent.GetComponentInChildren<FightSystemAI>().OnSwordChanged += HanldeSwordChanged;
    }

    private void OnEnable() {
        _HealthLeftImage.fillAmount = GetComponentInParent<AHealth>().healthPercentage;
        _HealthRightImage.fillAmount = _HealthLeftImage.fillAmount;

        // _SwordLeftImage.fillAmount = transform.parent.GetComponentInChildren<FightSystemAI>().swordPercentage;
        // _SwordRightImage.fillAmount = _SwordLeftImage.fillAmount;
    }

/*
    /// <summary>
    /// Calls the coroutine when the sword gets hit
    /// </summary>
    /// <param name="swordPct">The percentage of hp the sword has</param>
    private void HanldeSwordChanged(float swordPct)
    {        
        StartCoroutine(ChangeSwordPct(swordPct));
    }


    /// <summary>
    /// Lerps the sword's image fill amount when the enemy sword is hit
    /// </summary>
    /// <param name="swordPct">The percentage of hp the sword has</param>
    private IEnumerator ChangeSwordPct(float swordPct)
    {
        //Get the fill amount of the image before changin it
        float preChangePct = _SwordLeftImage.fillAmount;
        float elapsed = 0f;

        //While the elapsed time hasn't changed...
        while (elapsed < _UpdateSpeedSeconds)
        {
            //Increased the elapsed time
            elapsed += Time.deltaTime;

            //Lerp the images' fill amount between the previous and the new value
            _SwordLeftImage.fillAmount = Mathf.Lerp(preChangePct, swordPct, elapsed / _UpdateSpeedSeconds);
            _SwordRightImage.fillAmount = _SwordLeftImage.fillAmount;
            yield return null;
        }

        //Set the images' fill amount to the final given value
        _SwordLeftImage.fillAmount = swordPct;
        _SwordRightImage.fillAmount = _SwordLeftImage.fillAmount;
    }*/

    /// <summary>
    /// Calls the coroutine when the enemy gets hit
    /// </summary>
    /// <param name="healthPct">The percentage of hp the enemy has</param>
    private void HandleHealthChanged(float healthPct)
    {
        StartCoroutine(ChangeHealthPct(healthPct));
    }


    /// <summary>
    /// Lerps the enmey's hp image fill amount when the enemy is hit
    /// </summary>
    /// <param name="healthPct">The percentage of hp the enemy has</param>
    private IEnumerator ChangeHealthPct(float healthPct)
    {
        //Get the fill amount of the image before changin it
        float preChangePct = _HealthLeftImage.fillAmount;
        float elapsed = 0f;

        //While the elapsed time hasn't changed...
        while (elapsed < _UpdateSpeedSeconds)
        {
            //Increased the elapsed time
            elapsed += Time.deltaTime;

            //Lerp the images' fill amount between the previous and the new value
            _HealthLeftImage.fillAmount = Mathf.Lerp(preChangePct, healthPct, elapsed / _UpdateSpeedSeconds);
            _HealthRightImage.fillAmount = _HealthLeftImage.fillAmount;
            yield return null;
        }

        //Set the images' fill amount to the final given value
        _HealthLeftImage.fillAmount = healthPct;
        _HealthRightImage.fillAmount = _HealthLeftImage.fillAmount;
    }


    /// <summary>
    /// Called after the update method have been called
    /// </summary>
    private void LateUpdate()
    {
        //Make the canvas look at the camera
        transform.LookAt(Camera.main.transform);
    }

}
