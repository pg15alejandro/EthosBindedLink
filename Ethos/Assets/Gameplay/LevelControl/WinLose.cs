//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WinLose : MonoBehaviour
{
    [SerializeField] SOResources _PlayerResources;
    [SerializeField] SOTransform _PlayerTransform;
    [SerializeField] EventSystem _EventSystem;
    [SerializeField] GameObject _WinScreen;
    [SerializeField] GameObject _RestartButtonW;
    [SerializeField] GameObject _LoseScreen;
    [SerializeField] GameObject _RestartButtonL;
    [SerializeField] UiController _UIController;
    [SerializeField] GameplayLogic _Gml;

    bool dead;

    /// <summary>
    /// Unity's Update method. Called once per frame
    /// </summary>
    private void Update()
    {
        //Check if the player is dead
        // if (_PlayerResources.CurrentHealth <= 0)
        // {
        //     //Pause the game and show the lose screen
        //     _
        //     Death();
        // }
    }

    public void Death()
    {
        if(dead) return;

        dead = true;
        _EventSystem.SetSelectedGameObject(_RestartButtonL);
        _LoseScreen.SetActive(true);
        Time.timeScale = 0f;
    }


    /// <summary>
    /// Called when the collider other enters the trigger
    /// </summary>
    /// <param name="other">The collision data associated with this collision</param>    
    private void OnTriggerEnter(Collider other)
    {
        //If the player touched this "Winnig Object"
        if (other.tag == "Player" && _Gml.CanOpenDoor)
        {
            _PlayerTransform.UseCheckpoint = false;
            _UIController.LoadScene("SCN_BossFight_SM");
        }
    }

    public void Win()
    {
        //Pause the game and show the win screen
        //_EventSystem.SetSelectedGameObject(_RestartButtonW);
        //Time.timeScale = 0f;

        StartCoroutine(LoadCreditsScene());
    }

    public IEnumerator LoadCreditsScene()
    {
        yield return new WaitForSeconds(2);
        _WinScreen.SetActive(true);
        yield return new WaitForSeconds(1);
        _UIController.LoadScene("SCN_CreditsScreen_SM");
    }
}
