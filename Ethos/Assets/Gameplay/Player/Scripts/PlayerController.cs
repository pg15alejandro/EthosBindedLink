using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Space(10)] SOTransform _PlayerValue;

    [Header("Serialized parameters")]
    [SerializeField] SOResources _PlayerResources;
    [SerializeField] SOGameState _GameState;
    [SerializeField] private bool _MainCharacter;
    [SerializeField] private Transform _CameraBase;
    [SerializeField] private SOAnimationHashes _AnimHash;
    [SerializeField] EssenceBar _EssenceBar;
    [SerializeField] Image _PossesionImage;

    [Header("Resources / costs")]
    [SerializeField] private int _PossesionCost = 40;
    [SerializeField] private int _EssenceRecoverAmount = 1;
    [SerializeField] private float _DeadRadius = .1f;
    private bool _PassiveEssence;

    [Header("Velocities / time")]
    [SerializeField] private float _PlayerSpeed = 10f;
    [SerializeField] private float _FightModeSpeed = 8f;
    [SerializeField] private float _LockSidePlayerSpeed = 9f;
    [SerializeField] private float _LockBackPlayerSpeed = 7f;
    [SerializeField] private float _DistanceToBreakLockOn = 10;
    [SerializeField] private float _PossesionDefaultCooldown = 5;

    [Header("Forces / Angles / Times")]
    [SerializeField] private float _CameraLockAngle = 10f;
    [SerializeField] private float _LockOnSpeed = 20.0f;

    [Header("Debug")]
    [SerializeField] private MeshRenderer _Sword;
    [SerializeField] private GameObject _StealthSword;

    private List<Transform> _VisibleEnemies = new List<Transform>();
    private Rigidbody _RigidBody = null;
    private Animator _PlayerAnimator;

    //Abilities
    [System.NonSerialized] public FightingSystem Fighting;
    private Possesion _Possesion;
    private Interactables _Interactables;
    private InputSwitchCombo _SwitchRot;
    private FieldOfView _FoV;
    [SerializeField] private float _PossesionCd;

    //Input variables
    private float _XInput = 0f;
    private float _YInput = 0f;
    private bool _LockOn;
    private float _RXInput;
    private float _RYInput;

    //LockOn
    private Transform _RightEnemy;
    private Transform _LeftEnemy;
    private Transform _LockOnEnemy;
    private GameObject _EnemyLockOn;
    private GameObject _EnemyCanvas;

    private bool _Moving;
    private bool IsLockOn;
    private bool _IsDodging;
    private float _LockedTime;
    private float _CameraTime;
    public bool Dodging = false;
    private bool _LerpRot = true;
    private bool _UpdatePlayerRot;
    private float _ChangeLockOnCd;
    private bool _UpdateCameraRot;

    [NonSerialized] public bool IsOnFight;

    [Header("On Draw Gizmos")]
    [SerializeField] private float _InnerRadius = 1;
    [SerializeField] private float _MidRadius = 3;
    [SerializeField] private float _OutRadius = 5;


    public void CantCancelAnimation()
    {
        Fighting.CanCacelAnimation = false;
    }
    public void CanCancelAnimation()
    {
        Fighting.CanCacelAnimation = true;
    }

    public void SetLockOn(bool lockState)
    {
        IsLockOn = lockState;
        _CameraBase.GetComponent<CameraController>().PlayerLockOn = IsLockOn;

        if (IsLockOn == false)
        {
            _EnemyLockOn.SetActive(false);
            _EnemyCanvas.SetActive(false);
            _PlayerAnimator.SetBool("LockOn", false);
        }
    }

    public bool GetLockOn()
    {
        return IsLockOn;
    }

    public void LockTo(Transform enemyHit)
    {
        //Set the enemyLockOn variables and set the lockOn to true
        _LockedTime = 0f;
        IsLockOn = true;
        _CameraBase.GetComponent<CameraController>().PlayerLockOn = IsLockOn;
        _LockOnEnemy = enemyHit;
        _EnemyLockOn = _LockOnEnemy.GetComponentInChildren<LockOnIcon>().transform.GetChild(0).gameObject;
        _EnemyCanvas = _LockOnEnemy.GetComponentInChildren<EnemyHealthBar>().transform.GetChild(0).gameObject;
        _UpdateCameraRot = false;
        _PlayerAnimator.SetBool("LockOn", true);
        GetSideEnemies();
    }

    private void OnEnable()
    {
        _SwitchRot = GetComponentInChildren<InputSwitchCombo>();
        _PlayerAnimator = GetComponentInChildren<Animator>();
    }

    private void Awake()
    {
        if (_MainCharacter) _PlayerResources.RestartResources();
    }

    /// <summary>
    /// Unity's Start method. Called before the first frame
    /// </summary>
    private void Start()
    {
        if (_PlayerValue != null)
        {
            if (_MainCharacter)
            {
                _PlayerValue.value = gameObject.transform;
                if (!_PlayerValue.UseCheckpoint)
                {
                    _PlayerValue.UseCheckpoint = true;
                    _PlayerValue.checkpoint = transform.position;
                }
                else
                {
                    transform.position = _PlayerValue.checkpoint;
                }
            }
        }

        Fighting = GetComponentInChildren<FightingSystem>();
        _RigidBody = GetComponent<Rigidbody>();
        _Possesion = GetComponent<Possesion>();
        _Interactables = GetComponent<Interactables>();
        _FoV = _CameraBase.GetComponentInChildren<FieldOfView>();
        
        if (_Sword != null && _StealthSword != null)
        {
            _Sword.enabled = false;
            _StealthSword.SetActive(false);
        }
    }


    /// <summary>
    /// Unity's Update method. Called once per frame
    /// </summary>
    private void Update()
    {
        if (_GameState.GamePaused) return;

        _PassiveEssence = true;        

        ReadInputs();

        if (_PlayerValue != null)
            _PlayerValue.value.position = gameObject.transform.position;    //Update the player position scriptable object

        if (!_MainCharacter)
            _CameraBase.GetComponent<CameraController>().SetEnemyPos(transform.position);

        CheckLockOn();

        UpdateRotation();

        SetAnimValues();

        CheckCanMove();
    }

    private void CheckCanMove()
    {        
        int _ActualAnimationHash = _PlayerAnimator.GetCurrentAnimatorStateInfo(2).shortNameHash; //getting the animation hash
        
        if (_AnimHash.PlaceHolder == _ActualAnimationHash)
        {
            _CanMove = true;
            _IsDodging = false;
        }
        
        if(_AnimHash.Attack_01 == _ActualAnimationHash || _AnimHash.Attack_02 == _ActualAnimationHash || 
           _AnimHash.Attack_03 == _ActualAnimationHash || _AnimHash.Attack_04 == _ActualAnimationHash)     //If the player is in idle on the attack layer
        {
            _CanMove = false;
        }
    }


    /// <summary>
    /// Restore the player essence
    /// </summary>
    private void RestoreEssence()
    {
        //Check if the gameObject with this component is the player
        if (_MainCharacter)
        {
            //If the essence is not full and the player is not using it, restore it
            if (_PassiveEssence && _PlayerResources.CurrentEssence < _PlayerResources.MaxEssence)
                _PlayerResources.CurrentEssence += _EssenceRecoverAmount;
        }
    }

    public void AddEssence(int amount)
    {
        if (_PlayerResources.CurrentEssence + amount <= _PlayerResources.MaxEssence)
            _PlayerResources.CurrentEssence += amount;
        else
            _PlayerResources.CurrentEssence = _PlayerResources.MaxEssence;

        _EssenceBar.UpdateEssence();
    }


    /// <summary>
    /// Tries to lock on to an enemy or rotate the camera
    /// </summary>
    private void CheckLockOn()
    {
        //Update the enemies insie the field of view
        _FoV.UpdateList();

        //Order the enemy list by distance
        _VisibleEnemies = _FoV.VisibleTargets.OrderBy(
            x =>
                Vector3.Distance(this.transform.position, x.transform.position)
        ).ToList();

        //If the player pressed the lockOn input
        if (_LockOn)
        {
            _UpdatePlayerRot = true;

            //If I am not locked on to anyone
            if (IsLockOn == false)
            {
                //Check if there's and enemy inside my field of view
                if (_VisibleEnemies.Count > 0)
                {
                    //Set the enemyLockOn variables and set the lockOn to true
                    _LockedTime = 0f;
                    IsLockOn = true;
                    _CameraBase.GetComponent<CameraController>().PlayerLockOn = IsLockOn;
                    _LockOnEnemy = _VisibleEnemies[0];
                    _EnemyLockOn = _LockOnEnemy.GetComponentInChildren<LockOnIcon>().transform.GetChild(0).gameObject;
                    _EnemyCanvas = _LockOnEnemy.GetComponentInChildren<EnemyHealthBar>().transform.GetChild(0).gameObject;
                    _UpdateCameraRot = false;
                    _PlayerAnimator.SetBool("LockOn", true);
                    GetSideEnemies();
                }
                else //If there's no enemy inside my field of view
                {
                    //Tell the camera to rotate
                    _UpdateCameraRot = true;
                    _CameraTime = 0f;
                }

                if (_VisibleEnemies.Count <= 0)
                {
                    
                }
            }
            else //If the player is already locked on to an enemy
            {
                //Stop the lockOn
                IsLockOn = false;
                _PlayerAnimator.SetBool("LockOn", false);
                _CameraBase.GetComponent<CameraController>().PlayerLockOn = IsLockOn;
                _LerpRot = true;
                _EnemyLockOn.SetActive(false);
                _EnemyCanvas.SetActive(false);
            }
        }

        if (IsLockOn) //If the player is lockedOn
        {
            //Get the distance from the enemy
            var distToEnemy = Vector3.Distance(transform.position, _LockOnEnemy.position);

            //Check if the distance to the enemy exits the lockOn distance
            if (distToEnemy >= _DistanceToBreakLockOn)
            {
                //Stop the lockOn
                IsLockOn = false;
                _PlayerAnimator.SetBool("LockOn", false);
                _CameraBase.GetComponent<CameraController>().PlayerLockOn = IsLockOn;
                _LerpRot = true;
                _EnemyCanvas.SetActive(false);
                _EnemyLockOn.SetActive(false);
            }
        }

        if (IsLockOn) //If the player is still in lockOn
        {
            if (_RXInput != 0 && _ChangeLockOnCd <= 0)
            {
                GetSideEnemies();

                if (_RXInput == 1) //Left
                {
                    if (_LeftEnemy != null)
                    {
                        _LockedTime = 0f;
                        _LerpRot = true;
                        _EnemyLockOn.SetActive(false);
                        _EnemyCanvas.SetActive(false);
                        _LockOnEnemy = _LeftEnemy;
                        _EnemyLockOn = _LockOnEnemy.GetComponentInChildren<LockOnIcon>().transform.GetChild(0).gameObject;
                        _EnemyCanvas = _LockOnEnemy.GetComponentInChildren<EnemyHealthBar>().transform.GetChild(0).gameObject;
                        _ChangeLockOnCd = .25f;
                    }
                }
                else
                {
                    if (_RightEnemy != null)
                    {
                        _LockedTime = 0f;
                        _LerpRot = true;
                        _EnemyLockOn.SetActive(false);
                        _EnemyCanvas.SetActive(false);
                        _LockOnEnemy = _RightEnemy;
                        _EnemyLockOn = _LockOnEnemy.GetComponentInChildren<LockOnIcon>().transform.GetChild(0).gameObject;
                        _EnemyCanvas = _LockOnEnemy.GetComponentInChildren<EnemyHealthBar>().transform.GetChild(0).gameObject;
                        _ChangeLockOnCd = .25f;
                    }
                }
            }

            if (_ChangeLockOnCd > 0)
            {
                _ChangeLockOnCd -= Time.deltaTime;
                if (_ChangeLockOnCd < 0)
                    _ChangeLockOnCd = 0;
            }

            //If the camera is already looking at the enemy don't do anything
            if (!_LerpRot) return;

            //Get the old camera rotation
            var oldRot = _CameraBase.rotation;

            //Make the camera look at the enemy
            _CameraBase.LookAt(_LockOnEnemy);

            //Set the camera x rotation to a certain rotation so that the lockOn is just on the Y axis
            _CameraBase.localEulerAngles = new Vector3(_CameraLockAngle, _CameraBase.localEulerAngles.y, _CameraBase.localEulerAngles.z);

            _EnemyLockOn.SetActive(true);
            _EnemyCanvas.SetActive(true);

            //Get the new camera rotation
            var newRot = _CameraBase.rotation;

            //Lerp the two rotations during .25 seconds
            if (_LockedTime < .25f)
            {
                _CameraBase.rotation = Quaternion.Lerp(oldRot, newRot, Time.deltaTime * _LockOnSpeed);
                _LockedTime += Time.deltaTime;
                _UpdatePlayerRot = true;
            }
            else
            {
                _LerpRot = false;
            }
        }
    }

    private void LateUpdate()
    {
        if (IsLockOn && !_LerpRot)
        {
            //Make the camera look at the enemy
            _CameraBase.LookAt(_LockOnEnemy);

            //Set the camera x rotation to a certain rotation so that the lockOn is just on the Y axis
            _CameraBase.localEulerAngles = new Vector3(_CameraLockAngle, _CameraBase.localEulerAngles.y, _CameraBase.localEulerAngles.z);
        }
    }

    private void GetSideEnemies()
    {
        //get vector between 2 points
        Vector3 playToEnemy = _LockOnEnemy.position - transform.position;

        //Get the distance from the player to the locked on enemy
        float distCenter = Vector3.Distance(transform.position, _LockOnEnemy.position);

        _LeftEnemy = null;
        _RightEnemy = null;
        float closeNeg = -179, closePos = 179;


        for (int i = 0; i < _VisibleEnemies.Count; i++)
        {
            if (_VisibleEnemies[i] == _LockOnEnemy) continue;

            Vector3 playToEnemyTemp = _VisibleEnemies[i].position - transform.position;

            float angleBetEnemies = Vector3.SignedAngle(playToEnemy, playToEnemyTemp, Vector3.up);

            if (Vector3.Distance(transform.position, _VisibleEnemies[i].position) > distCenter + 1)
            {
                if (angleBetEnemies > 0 && _RightEnemy != null) continue;
                if (angleBetEnemies < 0 && _LeftEnemy != null) continue;
            }

            if (angleBetEnemies > 0)
            {
                if (angleBetEnemies < closePos)
                {
                    closePos = angleBetEnemies;
                    _RightEnemy = _VisibleEnemies[i];
                }
            }
            else
            {
                if (angleBetEnemies > closeNeg)
                {
                    closeNeg = angleBetEnemies;
                    _LeftEnemy = _VisibleEnemies[i];
                }
            }
        }
    }




    /// <summary>
    /// Reads the inputs of the player and checks if his resources let him execute some actions
    /// </summary>    
    private bool _PossesionReady;
    private float _InputVelocity;
    private bool _CanMove;

    private void ReadInputs()
    {
        ReadPossession();
        if (_Possesion != null && _Possesion.possesing && !_Possesion.canGetOut) return;

        ReadMoveInputs();

        ReadDodge();

        ReadInteractables();
    }

    private void ReadMoveInputs()
    {
        if (_SwitchRot.CanRotate)
        {
            _XInput = Input.GetAxisRaw("Horizontal");
            _YInput = Input.GetAxisRaw("Vertical");
        }
        _LockOn = Input.GetButtonDown("LockOn");
        _RXInput = Input.GetAxisRaw("RotHorizontal");
        _RYInput = Input.GetAxisRaw("RotVertical");

        //Check if the player is moving
        Vector2 inputs = new Vector2(_XInput, _YInput);
        _Moving = inputs.magnitude >= _DeadRadius;
        _InputVelocity = inputs.magnitude;

        if (_InputVelocity > 1)
            _InputVelocity = 1;
    }

    private void ReadDodge()
    {
        //If the player presses the dodge input
        if (Input.GetButtonDown("Dodge"))
        {
            if (Fighting.CanCacelAnimation)
            {
                Fighting.CanCacelAnimation = false;
                Dodging = true;
                _IsDodging = true;

                //Dodge blend
                _PlayerAnimator.SetFloat("Horizontal", 0);
                _PlayerAnimator.SetFloat("Vertical", 0);

                if (IsLockOn)
                {
                    _PlayerAnimator.SetFloat("HorizontalDodge", _XInput);
                    _PlayerAnimator.SetFloat("VerticalDodge", _YInput);
                }
                else
                {
                    _PlayerAnimator.SetFloat("HorizontalDodge", 0);
                    _PlayerAnimator.SetFloat("VerticalDodge", 0);
                }

                _PlayerAnimator.CrossFade("Dodge", 0.1f, 2);

                if(_PlayerAnimator.GetCurrentAnimatorStateInfo(2).shortNameHash == _AnimHash.Dodge)
                {
                    _PlayerAnimator.Play("Dodge", 2, .18f);
                }
            }
        }
    }

    private void ReadInteractables()
    {
        if (Input.GetButtonDown("Interactables"))
            _Interactables.InteractablePressed();
    }

    private void ReadPossession()
    {
        _PossesionImage.fillAmount = 1 - (_PossesionCd / _PossesionDefaultCooldown);
        
        if (!_MainCharacter || _Possesion == null) return;

        _CameraBase.GetComponent<CameraController>().PlayerPossesing = _Possesion.possesing;
        _CameraBase.GetComponent<CameraController>().MoveCamera = !(_Possesion.possesing && !_Possesion.canGetOut);

        if (_PossesionCd > 0 && !_Possesion.possesing)
        {
            _PossesionCd -= Time.deltaTime;
            if (_PossesionCd < 0) _PossesionCd = 0;
            return;
        }
        else
        {
            _PossesionReady = true;
        }

        if (Input.GetButtonDown("Possession"))
        {
            //If I am inside somebody, get out
            if (_Possesion.possesing)
            {
                if(_Possesion.canGetOut)
                    _Possesion.SetInput(true);

                return;
            }

            if (!IsLockOn || !_PossesionReady) return;

            if (_PlayerResources.CurrentEssence > _PossesionCost)
            {
                _PossesionReady = false;
                _PossesionCd = _PossesionDefaultCooldown;
                _PassiveEssence = false;
                _Possesion.SetInput(true);
                _PlayerResources.CurrentEssence -= _PossesionCost;
            }
            else
            {
                //Not enough essence
                print("Not enough essence for possesion");
            }
        }

    }

    /// <summary>
    /// Updates the rotation of the camera / the rotation of the player based on the camera
    /// </summary>
    private void UpdateRotation()
    {
        //If the camera doesn't need to rotate and the player is locked on to an enemy
        if (!_UpdatePlayerRot && IsLockOn)
        {
            //Make the camera and the player look at the enemy
            transform.LookAt(_LockOnEnemy);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
            return;
        }

        //Rotate the camera to where the player is looking
        if (_UpdateCameraRot)
        {
            //Rotate the camera in .25 seconds
            if (_CameraTime < .25f)
            {
                //Get the old rotation of the camera
                var oldRot = _CameraBase.rotation;

                //Make the camera rotate to the same angle of the character
                _CameraBase.localEulerAngles = new Vector3(_CameraBase.localEulerAngles.x, transform.localEulerAngles.y, _CameraBase.localEulerAngles.z);

                //Get the new rotation
                var newRot = _CameraBase.rotation;

                //Lerp between both rotations
                _CameraBase.rotation = Quaternion.Lerp(oldRot, newRot, Time.deltaTime * _LockOnSpeed);

                //Update the camera controller with the new rotations
                var _RotY = _CameraBase.localEulerAngles.x;
                var _RotX = _CameraBase.localEulerAngles.y;

                _CameraBase.GetComponent<CameraController>().SetRotations(new Vector2(_RotX, _RotY));

                _CameraTime += Time.deltaTime;
            }
            else
            {
                _UpdateCameraRot = false;
            }
        }
        else
        {
            //If the player starts moving and the camera should not rotate
            if (_Moving)
            {
                //Get the degrees of the player based on his input
                float degrees = Mathf.Rad2Deg * Mathf.Atan2(_XInput, _YInput);

                //Make the player look at where the camera is facing + the deegrees obtained
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, degrees + _CameraBase.eulerAngles.y, transform.eulerAngles.z);
            }
        }
        _UpdatePlayerRot = false;
    }

    private void SetAnimValues()
    {
        if (_PlayerAnimator == null) return;

        if (Fighting.IsPlayerAbleToMove)
        {
            _PlayerAnimator.SetFloat("Velocity", _InputVelocity);
            _PlayerAnimator.SetFloat("Horizontal", _XInput);
            _PlayerAnimator.SetFloat("Vertical", _YInput);
        }
        else
        {
            _PlayerAnimator.SetFloat("Velocity", 0);
            _PlayerAnimator.SetFloat("Horizontal", 0);
            _PlayerAnimator.SetFloat("Vertical", 0);
        }
    }

    public void FinishDodge()
    {
        _IsDodging = false;
    }

    private void FixedUpdate()
    {
        //If the player is not dashing
        if (_IsDodging || !Fighting.IsPlayerAbleToMove)
        {
            _Moving = false;
            return;
        }

        if(_CanMove)
            ApplyMovement();
    }
    
    /// <summary>
    /// Moves the player by changing the velocity of his rigidbody
    /// </summary>
    private void ApplyMovement()
    {
        Vector3 velocity;

        if (!_Moving) return;

        //Get the velocity of the player
        velocity = new Vector3(_XInput, 0f, _YInput).normalized * _InputVelocity;

        if (IsLockOn)
        {
            if (Mathf.Abs(_XInput) > Mathf.Abs(_YInput))
            {
                velocity *= _LockSidePlayerSpeed;
            }
            else
            {
                if (_YInput > 0)
                    velocity *= _FightModeSpeed;
                else
                    velocity *= _LockBackPlayerSpeed;
            }
        }
        else
        {
            if(IsOnFight)
                velocity *= _FightModeSpeed;
            else                
                velocity *= _PlayerSpeed;
        }

        //Get the rotation of the camera
        Transform cameraT = _CameraBase.GetChild(1);
        cameraT.rotation = _CameraBase.GetChild(0).rotation;
        Vector3 originalRot = cameraT.rotation.eulerAngles;
        cameraT.rotation = Quaternion.Euler(0f, originalRot.y, originalRot.z);

        //Set the velocity to a global value based on the cameraT rotation
        velocity = cameraT.TransformVector(velocity);

        //Set the y velocity to stay the same
        velocity.y = _RigidBody.velocity.y;

        //Apply the velocity to the rigidbody
        _RigidBody.velocity = velocity;

    }

    public void StartAttackAnim()
    {
        Fighting.StartingAttack();
    }

    private void OnDrawGizmos()
    {
        if (gameObject.layer == 10)
        {
            /// Transform transform = GetComponent<Transform>();
            Gizmos.color = Color.white;
            float theta = 0;

            float innerCirclex = _InnerRadius * Mathf.Cos(theta);
            float innerCircley = _InnerRadius * Mathf.Sin(theta);

            float midCirclex = _MidRadius * Mathf.Cos(theta);
            float midCircley = _MidRadius * Mathf.Sin(theta);

            float outCirclex = _OutRadius * Mathf.Cos(theta);
            float outCircley = _OutRadius * Mathf.Sin(theta);

            Vector3 Shortpos = transform.position + new Vector3(innerCirclex, 0, innerCircley);
            Vector3 Medpos = transform.position + new Vector3(midCirclex, 0, midCircley);
            Vector3 Outpos = transform.position + new Vector3(outCirclex, 0, outCircley);

            Vector3 newShortPos = Shortpos;
            Vector3 lastShortPos = Shortpos;

            Vector3 newMedPos = Medpos;
            Vector3 lastMedPos = Medpos;

            Vector3 newOutPos = Outpos;
            Vector3 lastOutPos = Outpos;

            for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
            {
                innerCirclex = _InnerRadius * Mathf.Cos(theta);
                innerCircley = _InnerRadius * Mathf.Sin(theta);

                midCirclex = _MidRadius * Mathf.Cos(theta);
                midCircley = _MidRadius * Mathf.Sin(theta);

                outCirclex = _OutRadius * Mathf.Cos(theta);
                outCircley = _OutRadius * Mathf.Sin(theta);


                newShortPos = transform.position + new Vector3(innerCirclex, 0, innerCircley);
                newMedPos = transform.position + new Vector3(midCirclex, 0, midCircley);
                newOutPos = transform.position + new Vector3(outCirclex, 0, outCircley);

                Gizmos.DrawLine(Shortpos, newShortPos);
                Gizmos.DrawLine(Medpos, newMedPos);
                Gizmos.DrawLine(Outpos, newOutPos);

                Shortpos = newShortPos;
                Medpos = newMedPos;
                Outpos = newOutPos;
            }
            Gizmos.DrawLine(Shortpos, lastShortPos);
            Gizmos.DrawLine(Medpos, lastMedPos);
            Gizmos.DrawLine(Outpos, lastOutPos);
        }
    }
}

