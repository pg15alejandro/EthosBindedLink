//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections.Generic;
using UnityEngine;

public class SOEnemyRegister : MonoBehaviour
{
    [Header("DEBUG")]
    public List<GameObject> EnemiesAlerted = new List<GameObject>();
    public List<GameObject> EnemiesInside = new List<GameObject>();
    public List<GameObject> Executioners = new List<GameObject>();
    public int AmmountOfEnemiesAlerted;
    public int AmmountOfEnemiesInside;
    public int AmmountOfExecutioners;

    [Header("IMPORTANT BEHAVIOR")]
    public int MaxiumEnemiesInside;
    public float OutsideMaxAngleEnemies;
    public float InsideMaxAngleEnemies;
    public float DistanceToRunInnerEnemies;
    public float DistanceToRunOuterEnemies;
    public float DistanceInnerEnemies;
    public float DistanceOutterEnemies;
    public float DistanceToMoveBackInnerEnemies;
    public float DistanceToMoveBackOuterEnemies;

    private void Update()
    {
        AmmountOfEnemiesAlerted = EnemiesAlerted.Count;
        AmmountOfEnemiesInside = EnemiesInside.Count;
        AmmountOfExecutioners = Executioners.Count;
    }

    public void MoveEnemyFromAlertedToInside(GameObject go)
    {
        if (go == null) return;
        if (!go.GetComponent<AIHealth>().IsAlive) return;
        if (EnemiesAlerted.Contains(go) == false) return;
        var hlt = go.GetComponent<AHealth>();
        if (hlt == null || hlt.healthPercentage <= 0f) return;

        EnemiesAlerted.Remove(go);
        EnemiesInside.Add(go);
        // Debug.Log("*** MoveEnemyFromAlertedToInside", go);
    }

    public void TryMoveAIFromInsideToAlerted(GameObject go)
    {
        if (go == null) return;
        if (!go.GetComponent<AIHealth>().IsAlive) return;
        if (EnemiesInside.Contains(go) == false) return;
        var hlt = go.GetComponent<AHealth>();
        if (hlt == null || hlt.healthPercentage <= 0f) return;

        EnemiesInside.Remove(go);
        EnemiesAlerted.Add(go);
        //Debug.Log("*** MoveEnemyFromInsideToAlert", go);
    }

    public void TryRemoveAIFromInsideList(GameObject go)
    {
        if (go == null) return;
        if (!go.GetComponent<AIHealth>().IsAlive) return;
        //Debug.Log("*** TryRemoveAIFromInsideList", go);
        if (EnemiesInside.Contains(go)) EnemiesInside.Remove(go);
    }

    public void ExecutionerInside(GameObject go)
    {
        if (Executioners.Count > 0)
        {
            foreach (var item in EnemiesInside)
            {
                if (!EnemiesAlerted.Contains(item))
                {
                    EnemiesInside.Remove(item);
                    EnemiesAlerted.Add(item);
                }

            }
        }
    }


}
