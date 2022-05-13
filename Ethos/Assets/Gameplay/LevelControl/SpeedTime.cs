//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpeedTime : MonoBehaviour
{
    [SerializeField] private float _Speed;
    [SerializeField] private UiController _UIController;

    private void Update()
    {
        if (Input.GetButton("BButton"))
        {
            Time.timeScale = 0f;
        }
        else if (Input.GetButton("AButton"))
        {
            Time.timeScale = _Speed;
        }
        else
        {
            Time.timeScale = 1f;
        }

        if (Input.GetButtonDown("XButton"))
        {
            GoMainScene();
        }
    }

    public void GoMainScene()
    {
        _UIController.PauseMainMenu();
    }
}
