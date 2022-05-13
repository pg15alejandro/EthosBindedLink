//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesCanvas : MonoBehaviour
{
    [SerializeField] private float _Speed = 1f;
    Vector3 _Min, _Max;
    bool _Add = true;
    float _LerpAmount;

    private void Start() {
        _LerpAmount = 0f;
        _Min = new Vector3(0.01f, 0.01f, 0.01f);
        _Max = new Vector3(0.012f, 0.012f, 0.012f);
    }

    // Update is called once per frame
    void Update()
    {
        //Make the canvas look at the camera
        transform.LookAt(Camera.main.transform);
        
        transform.localScale = Vector3.Lerp(_Min, _Max, _LerpAmount);

        if(_LerpAmount <= 0)
            _Add = true;

        if(_LerpAmount >= 1)
            _Add = false;

        if(_Add)
            _LerpAmount += Time.deltaTime * _Speed;
        else
            _LerpAmount -= Time.deltaTime * _Speed;

    }
}
