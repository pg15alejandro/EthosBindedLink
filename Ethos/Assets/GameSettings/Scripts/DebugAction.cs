//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAction : MonoBehaviour
{
    private GameObject[] _Enemies;
    [SerializeField] private SOTransform _PlayerT;
    private Vector3 _SpawnPosition;
    [SerializeField] private GameObject _SwordEnemy;
    [SerializeField] private GameObject _SwordDummy;


    public void EnemySpawner(string type)
    {
        var direction = _PlayerT.value.forward;
        var startingPosition = _PlayerT.value.position;
        RaycastHit hitPoint;
        bool ray = Physics.Raycast(startingPosition, direction, out hitPoint, 10f);

        if (ray == false)
        {
            Quaternion rotation = Quaternion.AngleAxis(_PlayerT.value.rotation.y, Vector3.up);
            Vector3 addDistanceToDirection = rotation * transform.forward * 10f;
            _SpawnPosition = startingPosition + addDistanceToDirection;
            Debug.Log("NOT HIT");
        }
        else
        {
            Quaternion rotation = Quaternion.AngleAxis(_PlayerT.value.rotation.y, Vector3.up);
            Vector3 addDistanceToDirection = rotation * -hitPoint.transform.forward * 2.5f;
            _SpawnPosition = hitPoint.transform.position + addDistanceToDirection;
            Debug.Log("HIT");
        }

        switch (type)
        {
            case "Sword":
                Instantiate(_SwordEnemy, _SpawnPosition, new Quaternion(0, 0, 0, 0));
                Debug.Log("Sword Enemy Spawned");
                break;

            case "Axe":
                break;

            case "Shield":
                break;

            case "Executioner":
                break;
            case "Boss":
                break;

        }
    }

    public void DummySpawner(string type)
    {
        var direction = _PlayerT.value.forward;
        var startingPosition = _PlayerT.value.position;
        RaycastHit hitPoint;
        bool ray = Physics.Raycast(startingPosition, direction, out hitPoint, 10f);

        if (ray == false)
        {
            Quaternion rotation = Quaternion.AngleAxis(_PlayerT.value.rotation.y, Vector3.up);
            Vector3 addDistanceToDirection = rotation * transform.forward * 10f;
            _SpawnPosition = startingPosition + addDistanceToDirection;
        }
        else
        {
            Quaternion rotation = Quaternion.AngleAxis(_PlayerT.value.rotation.y, Vector3.up);
            Vector3 addDistanceToDirection = rotation * -hitPoint.transform.forward * 2.5f;
            _SpawnPosition = hitPoint.transform.position + addDistanceToDirection;
        }
        switch (type)
        {
            case "Sword":
                Instantiate(_SwordDummy, _SpawnPosition, new Quaternion(0, 0, 0, 0));
                Debug.Log("Sword Dummy Spawned");
                break;

            case "Axe":
                break;

            case "Shield":
                break;

            case "Executioner":
                break;
            case "Boss":
                break;

        }
    }

    public void KillAllEnemies()
    {
        _Enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var item in _Enemies)
        {
            Destroy(item);
        }

    }
}
