//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesDisabler : MonoBehaviour
{
    [SerializeField] private GameplayLogic _gmLogic;
    [SerializeField] private EnemyPlacement _EnemiesPerArea;
    private int _ActualZone;

    private void Update()
    {
        _ActualZone = _gmLogic.ZoneIndex;

        if (_ActualZone == 0 && _EnemiesPerArea.Areas[0].Enemies.Count <= 0)    //zone 0 = dungeon
        {
            for (int i = 0; i < _EnemiesPerArea.Areas[1].Enemies.Count; i++)
            {
                _EnemiesPerArea.Areas[1].Enemies[i].SetActive(true);
            }
        }


        else if (_ActualZone == 1 && _EnemiesPerArea.Areas[1].Enemies.Count <= 0)   //zone 1 = Hallway
        {
            for (int i = 0; i < _EnemiesPerArea.Areas[2].Enemies.Count; i++)
            {
                _EnemiesPerArea.Areas[2].Enemies[i].SetActive(true);
            }
        }


        else if (_ActualZone == 2 && _EnemiesPerArea.Areas[2].Enemies.Count <= 0)   //zone 2 = First Building
        {
            for (int i = 0; i < _EnemiesPerArea.Areas[3].Enemies.Count; i++)
            {
                _EnemiesPerArea.Areas[3].Enemies[i].SetActive(true);
            }
        }

        else if (_ActualZone == 4)
        {
            for (int i = 0; i < _EnemiesPerArea.Areas[4].Enemies.Count; i++)    //zone 4 = Main Patio
            {
                _EnemiesPerArea.Areas[4].Enemies[i].SetActive(true);
            }
        }
    }

}
