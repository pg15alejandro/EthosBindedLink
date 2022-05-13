//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneChanger : MonoBehaviour
{
    [SerializeField] private GameplayLogic _gmLogic;
    [SerializeField] private SOTransform _PlayerTransform;
    [SerializeField] private int _ZoneValue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        if (_PlayerTransform.ZoneLoad <= _ZoneValue)
        {
            _gmLogic.ZoneIndex = _ZoneValue;
            _gmLogic.CanOpenDoor = false;

            _PlayerTransform.ZoneLoad = _ZoneValue;
        }

        gameObject.SetActive(false);
    }
}
