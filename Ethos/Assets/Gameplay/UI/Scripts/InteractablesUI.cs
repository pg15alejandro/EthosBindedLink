//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesUI : MonoBehaviour
{
    public bool DependentInteractable;
    [SerializeField] private GameplayLogic _GameLogic;
    [SerializeField] GameObject _UI;
    bool _CanBeShowed = true;

    private void OnTriggerEnter(Collider other)
    {
        int playerL = LayerMask.NameToLayer("Player");

        if (other.gameObject.layer != playerL) return;

        if(DependentInteractable && !_GameLogic.CanOpenDoor) return;

        if(_CanBeShowed)
            _UI.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        int playerL = LayerMask.NameToLayer("Player");

        if (other.gameObject.layer != playerL) return;

        _UI.SetActive(false);
    }

    public void DisableUI()
    {
        _CanBeShowed = false;
        _UI.SetActive(false);
    }

    public void EnableUI()
    {
        _UI.SetActive(true);
    }
}
