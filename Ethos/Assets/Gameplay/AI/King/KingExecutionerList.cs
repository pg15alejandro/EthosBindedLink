//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingExecutionerList : MonoBehaviour
{
    [SerializeField] public List<GameObject> ExecutionerAttacking;
    [SerializeField] public List<GameObject> ExecutionerDefending;
    [SerializeField] private int _AmmountOfExecutionersAttacking;
    [NonSerialized] public bool ExecutionersAlive = true;
    private void Update()
    {
        ExecutionerAttackingChecker();

        if (ExecutionerAttacking.Count <= 0)
            ExecutionersAlive = false;
    }

    private void ExecutionerAttackingChecker()
    {
        foreach (var item in ExecutionerDefending){
            item.GetComponent<ExecutionerHealthKing>().Invulnerable = true;
            item.layer = LayerMask.NameToLayer("Default");
        }
        
        if (ExecutionerAttacking.Count < _AmmountOfExecutionersAttacking)
        {
            if (ExecutionerDefending.Count > 0)
            {
                foreach (var item in ExecutionerDefending)
                {
                    if (!ExecutionerAttacking.Contains(item) && ExecutionerAttacking.Count < _AmmountOfExecutionersAttacking)
                    {
                        ExecutionerAttacking.Add(item);
                        ExecutionerDefending.Remove(item);
                        item.GetComponent<ExecutionerHealthKing>().Invulnerable = false;
                        item.layer = LayerMask.NameToLayer("ExecutionerBoss");
                    }
                }

            }
        }
    }
}
