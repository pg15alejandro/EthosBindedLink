//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reseter : MonoBehaviour
{
    [SerializeField] private SOResources _PlayerResources;

    /// <summary>
    /// Callback sent to all gameobjects before the application is quit
    /// </summary>
    private void OnApplicationQuit()
    {
        RestartResources();
    }

    /// <summary>
    /// Restarts all the player resources to it's maximum value
    /// </summary>
    public void RestartResources()
    {
        _PlayerResources.CurrentHealth = _PlayerResources.MaxHealth;
        _PlayerResources.CurrentEssence = _PlayerResources.MaxEssence;
        _PlayerResources.CurrentStamina = _PlayerResources.MaxStamina;
    }
}
