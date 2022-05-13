//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyRegisterModifier : MonoBehaviour
{
    [SerializeField] private SOEnemyRegister _EnemyList;
    private List<Transform> _VisibleEnemies = new List<Transform>();
    [SerializeField] private FieldOfView _FoV;
    // Update is called once per frame
    void Update()
    {
        _FoV.UpdateList();
        _VisibleEnemies = _FoV.VisibleTargets.OrderBy(
            x =>
                Vector2.Distance(this.transform.position, x.transform.position)
        ).ToList();

        for (int i = 0; i < _VisibleEnemies.Count; i++)
        {
            if (_VisibleEnemies[i].gameObject == null) return;
            if(!_VisibleEnemies[i].gameObject.GetComponent<AIHealth>().IsAlive) return;

            if (!_EnemyList.EnemiesInside.Contains(_VisibleEnemies[i].gameObject) && _EnemyList.EnemiesInside.Count < _EnemyList.MaxiumEnemiesInside && _EnemyList.Executioners.Count <= 0)
            {
                Debug.Log($"Adding {_VisibleEnemies[i].gameObject} to the list");
                _EnemyList.MoveEnemyFromAlertedToInside(gameObject);
            }
        }
    }
}
