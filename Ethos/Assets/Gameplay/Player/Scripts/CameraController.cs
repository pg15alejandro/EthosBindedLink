using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------

    [SerializeField] private float _CameraRotVel = 2f;
    [SerializeField] private SOTransform _PlayerTransform;    
    [SerializeField] private SOGameState _GameState;
    [SerializeField] public bool IsPlayerCamera;
    [NonSerialized] public bool PlayerLockOn;
    [NonSerialized] public bool PlayerPossesing;
    [NonSerialized] public bool MoveCamera = true;
    private Vector3 _EnemyPos;

    [SerializeField] private float _RotX;
    private float _RotContrX;
    private float _RotContrY;
    private float _RotY;


    /// <summary>
    /// Unity's Update method. Called once per frame
    /// </summary>
    private void Update()
    {
        if(_GameState.GamePaused) return;

        if(_PlayerTransform != null && IsPlayerCamera && !PlayerPossesing){
            transform.position = _PlayerTransform.value.position;
            transform.position = new Vector3(transform.position.x, transform.position.y + 1.6f, transform.position.z); //Take it 1.6 units up
        }        
        
        if(!IsPlayerCamera){
            transform.position = _EnemyPos;
            transform.position = new Vector3(transform.position.x, transform.position.y + 1.6f, transform.position.z); //Take it 1.6 units up
        }
        

        //If the player locked on an enemy, update the camera base's rotation values and don't update the rotation with the controller
        if (PlayerLockOn)
        {
            ExitLockOn();
            return;
        }

        ReadRotInputs();

        if(MoveCamera)
            UpdateCameraRot();
    }


    /// <summary>
    /// Set's the cameraBase position to the given position
    /// <param name="position">The position of the enemy</param>
    /// </summary>
    public void SetEnemyPos(Vector3 position)
    {
        _EnemyPos = position;
    }


    /// <summary>
    /// Reads the the player rotation inputs
    /// </summary>
    private void ReadRotInputs()
    {
        //Read the values of the left joystick
        _RotContrX = Input.GetAxis("RotHorizontal");
        _RotContrY = Input.GetAxis("RotVertical");

        //Invert the input and mutiply it by the camera base velocity
        _RotContrX *= -1 * _CameraRotVel;
        _RotContrY *= -1 * _CameraRotVel;
    }


    /// <summary>
    /// Updates the rotation of the camera base
    /// </summary>
    private void UpdateCameraRot()
    {
        //Add our input to the camera base rotation values
        _RotX += _RotContrX;
        _RotY += _RotContrY;

        _RotX = _RotX % 360;                    //RotX = [0, 360)

        if (_RotY > 180f)
            _RotY -= 360f;

        _RotY = Mathf.Clamp(_RotY, -10, 90);    //RotY = [-10, 90]

        //Set the new rotation to the camera base
        transform.localEulerAngles = new Vector3(_RotY, _RotX, 0);
    }


    /// <summary>
    /// Updates the camera base rotation values when exiting LockOn
    /// </summary>
    private void ExitLockOn()
    {
        //Get the camera base's rotation and store it
        _RotY = transform.localEulerAngles.x;
        _RotX = transform.localEulerAngles.y;
    }

    /// <summary>
    /// Returns the camera base actual rotations
    /// </summary>
    public Vector2 GetRotations()
    {
        return new Vector2(_RotX, _RotY);
    }


    /// <summary>
    /// Sets the camera base rotation values to the given ones
    /// </summary>
    /// <param name="rotations">New rotation values</param>
    public void SetRotations(Vector2 rotations)
    {
        _RotX = rotations.x;
        _RotY = rotations.y;
    }

}
