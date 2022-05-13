//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayLogic : MonoBehaviour
{
    private EnemyPlacement _EnemiesPerArea;
    public int ZoneIndex = 0;
    public bool CanOpenDoor = true;
    public bool HasSword = false;
    public bool ExecStartAttack = false;
    [SerializeField] private GameObject _Door;
    [SerializeField] private int _AmmountOfReaminingEnemiesForExecActivation = 3;
    [SerializeField] private TextBoxEnablersDisablers _Ui;
    private bool _firstTime = true;
    [SerializeField] private bool _do = false;
    [SerializeField] private Animator _Animator;
    [SerializeField] private SOTransform _PlayerTransform;
    [SerializeField] private RuntimeAnimatorController _ArcadiaAnimator;
    [SerializeField] private MeshRenderer _SwordMesh;
    [SerializeField] private FightingSystem _Fighting;
    [SerializeField] private DungeonDisabler _DungeonDisabler;
    [NonSerialized] public bool CinematicReady;

    private void Start()
    {
        if (!_do) return;
        _EnemiesPerArea = gameObject.GetComponent<EnemyPlacement>();

        if (_PlayerTransform.ZoneLoad > 0)
        {
            CanOpenDoor = true;
        }
        if (_PlayerTransform.ZoneLoad > 0)
        {

            HasSword = true;
            _Fighting.CanAttack = true;

            _SwordMesh.enabled = true;
            _Animator.runtimeAnimatorController = _ArcadiaAnimator as RuntimeAnimatorController;

            _DungeonDisabler.DoDisableEnable();

            for (int i = 0; i < _PlayerTransform.ZoneLoad; i++)
            {
                foreach (var item in _EnemiesPerArea.Areas[i].Enemies)
                {
                    Destroy(item);
                }
                _EnemiesPerArea.Areas[i].Enemies = new List<GameObject>();
            }

            ZoneIndex = _PlayerTransform.ZoneLoad;
        }


        if (_PlayerTransform.ZoneLoad == 0)
        {
            foreach (var item in _EnemiesPerArea.Areas[0].Enemies)
            {
                item.SetActive(true);
            }
          
        }


        else if (_PlayerTransform.ZoneLoad == 1)
        {
            foreach (var item in _EnemiesPerArea.Areas[1].Enemies)
            {
                item.SetActive(true);
            }
        }


        else if (_PlayerTransform.ZoneLoad == 2)
        {
            foreach (var item in _EnemiesPerArea.Areas[2].Enemies)
            {
                item.SetActive(true);
            }
        }


        else if (_PlayerTransform.ZoneLoad == 3)
        {
            foreach (var item in _EnemiesPerArea.Areas[3].Enemies)
            {
                item.SetActive(true);
            }
        }


        else if (_PlayerTransform.ZoneLoad == 4)
        {
            foreach (var item in _EnemiesPerArea.Areas[4].Enemies)
            {
                item.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if (!_do) return;
        if (!HasSword)
        {
            return;
        }
        else
        {
            if (_EnemiesPerArea.Areas[0].Enemies.Count == 1)
            {
                GetTheOtherGuard();
                return;
            }
        }

        foreach (var item in _EnemiesPerArea.Areas[1].Enemies)
        {
            var mai = item.GetComponent<MovementAI>();
            if (mai != null)
            {
                if (mai._Approach)
                {
                    foreach (var item2 in _EnemiesPerArea.Areas[1].Enemies)
                    {
                        var mai2 = item2.GetComponent<MovementAI>();
                        if (mai2 != null)
                        {
                            mai2.PlayerDetected();
                        }
                    }
                }
            }
        }

        foreach (var item in _EnemiesPerArea.Areas[2].Enemies)
        {
            var mai = item.GetComponent<MovementAI>();
            if (mai != null)
            {
                if (mai._Approach)
                {
                    foreach (var item2 in _EnemiesPerArea.Areas[2].Enemies)
                    {
                        var mai2 = item2.GetComponent<MovementAI>();
                        if (mai2 != null)
                        {
                            mai2.PlayerDetected();
                        }
                    }
                }
            }
        }

        foreach (var item in _EnemiesPerArea.Areas[3].Enemies)
        {
            var mai = item.GetComponent<MovementAI>();
            if (mai != null)
            {
                if (mai._Approach)
                {
                    foreach (var item2 in _EnemiesPerArea.Areas[3].Enemies)
                    {
                        var mai2 = item2.GetComponent<MovementAI>();
                        if (mai2 != null)
                        {
                            mai2.PlayerDetected();
                        }
                    }
                }
            }
        }

        foreach (var item in _EnemiesPerArea.Areas[4].Enemies)
        {
            var mai = item.GetComponent<MovementAI>();
            if (mai != null)
            {
                if (mai._Approach)
                {
                    foreach (var item2 in _EnemiesPerArea.Areas[4].Enemies)
                    {
                        var mai2 = item2.GetComponent<MovementAI>();
                        if (mai2 != null)
                        {
                            mai2.PlayerDetected();
                        }
                    }
                }
            }
        }

        if (_EnemiesPerArea.Areas[4].Enemies.Count <= _AmmountOfReaminingEnemiesForExecActivation)           //makes the executioner join the fight at certain point
        {
            ExecStartAttack = true;
        }

        if (_EnemiesPerArea.Areas[4].Enemies.Count <= 0 && _EnemiesPerArea.Areas[3].Enemies.Count <= 0)
        {
            CinematicReady = true;
        }

        if (_EnemiesPerArea.Areas[ZoneIndex].Enemies.Count == 0)
        {
            for (int i = 0; i < _EnemiesPerArea.Areas[ZoneIndex].CanvasPerZone.Count; i++)
            {
                var item = _EnemiesPerArea.Areas[ZoneIndex].CanvasPerZone[i];
                item.SetActive(true);
                _EnemiesPerArea.Areas[ZoneIndex].CanvasPerZone.Remove(item);
            }

            Debug.Log($"The area {ZoneIndex} is empty");
            _Animator.SetBool("FightMode", false);
            CanOpenDoor = true;
        }
    }

    private void GetTheOtherGuard()
    {
        var Anim = _Door.GetComponent<Animator>();
        Anim.SetTrigger("OpenDoor");

        var guard = _EnemiesPerArea.Areas[0].Enemies[0];
        var movement = guard.GetComponent<MovementAI>();
        movement.PlayerDetected();

        if (_firstTime)
        {
            _Ui.Block();
            _firstTime = false;
        }
    }
}
