//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MovementAI : MonoBehaviour
{

    [Header("ENEMIES DISTANCES")]
    // [SerializeField] private float _DistanceInnerEnemies;
    // [SerializeField] private float _DistanceToMoveBack;
    // [SerializeField] private float _DistanceOutterEnemies;

    [Header("DEBUG")]
    [SerializeField] private float _DistanceFromPlayer;
    [SerializeField] private int _MaxiumEnemiesInside;
    [SerializeField] private float _DistanceNavMesh;
    [SerializeField] private float _DistanceToSwitch;
    [SerializeField] private float _MaxStoppingTime;
    [SerializeField] private int _PointsIndex = 0;
    [Header("PATROLING")]
    [SerializeField] private float _StoppingTimePatrolPoints;
    [SerializeField] public bool OnlyPatrolEnemy;
    [SerializeField] private GameObject _PatrolToFollow;
    [Header("AI LOGIC")]
    [SerializeField] private SOEnemyRegister _EnemyList;
    [SerializeField] private GameObject _StaticForward;
    // [SerializeField] private float _OutsideMaxAngleEnemies;
    // [SerializeField] private float _InsideMaxAngleEnemies; 
    [SerializeField] private float _RotateSpeed;
    [Header("EXTRAS")]
    [SerializeField] private SOTransform _Player;
    [SerializeField] private SOSoundState _SoundState;
    [SerializeField] private SOAnimationHashes _AnimHashes;
    [Header("SPEED SWITCHER")]
    [SerializeField] private float _WalkingSpeed;
    [SerializeField] private float _RunningSpeed;
    // [SerializeField] private float _DistanceToRunInnerEnemies;
    // [SerializeField] private float _DistanceToRunOuterEnemies;
    [SerializeField] private float _LerpT = .1f;
    private float _TLerp;
    public bool _Approach = false;
    private bool _PlayerDetected;
    private float _Distance;
    private float _DegreesOfSeparationOutside;
    private float _DegreesOfSeparationInside;
    private int _AmmountOfPointsPatrol;

    private InputSwitchCombo _InpSw;
    private NavMeshAgent _NavMesh;
    private FieldOfView _FoV;
    private FightSystemAI _Fight;
    private Animator _Anim;
    private Quaternion _LookRotation;
    private Vector3 _Direction;


    [System.NonSerialized] public bool _CanMove = true;
    public float _MaxTimeBetweenEnemySwitch;
    public float _TimeBetweenEnemySwitch;

    public bool InPosition = false;
    [SerializeField] private float _Velocity;
    [SerializeField] private float _TimeBetweenPoints;
    [SerializeField] private float _MinVelocity;
    [SerializeField] private float _MaxVelocity;
    private float _AnimVal;
    private bool _GetValue = true;
    private float _PrevTo = -2;
    private bool _FirstTime = true;

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void OnEnable()
    {
        if (_PatrolToFollow != null)
        {
            _AmmountOfPointsPatrol = _PatrolToFollow.transform.childCount;
        }
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void Start()
    {
        _Anim = GetComponentInChildren<Animator>();
        _FoV = GetComponentInChildren<FieldOfView>();
        _Fight = GetComponentInChildren<FightSystemAI>();
        _NavMesh = GetComponent<NavMeshAgent>();
        _InpSw = GetComponent<InputSwitchCombo>();
        _NavMesh.SetDestination(transform.position);
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void Update()
    {
        if (!_NavMesh.enabled) return;
        //print($"Is NavMeshAgent stopped? {_NavMesh.isStopped}");
        if (_EnemyList.EnemiesAlerted.Count > 0) _DegreesOfSeparationOutside = _EnemyList.OutsideMaxAngleEnemies / _EnemyList.EnemiesAlerted.Count;       //getting degrees of separation
        if (_EnemyList.EnemiesInside.Count > 0) _DegreesOfSeparationInside = _EnemyList.InsideMaxAngleEnemies / _EnemyList.EnemiesInside.Count;
        _DistanceNavMesh = _NavMesh.remainingDistance;
        _Distance = Vector3.Distance(transform.position, _Player.value.position); //getting distance from player
        _Anim.SetFloat("Distance", _Distance);
        PlayerDetection();
        PatrolSystem();
        var currentAnim = _Anim.GetCurrentAnimatorStateInfo(2).shortNameHash;
        var backhitanim = _AnimHashes.BackHitAnimationHash;
        var lefthitanim = _AnimHashes.LeftHitAnimationHash;
        var chesthitanim = _AnimHashes.ChestHitAnimationHash;

        if (currentAnim == _AnimHashes.Stun) return;
        if (_Approach == true && _CanMove && currentAnim != backhitanim && currentAnim != lefthitanim && currentAnim != chesthitanim)
        {
            if (currentAnim == _AnimHashes.Attack_01 || currentAnim == _AnimHashes.Attack_02 || currentAnim == _AnimHashes.Attack_03 || currentAnim == _AnimHashes.Attack_04)
            {
                _NavMesh.isStopped = true;
            }
            if (currentAnim != _AnimHashes.Attack_01 && currentAnim != _AnimHashes.Attack_02 && currentAnim != _AnimHashes.Attack_03 && currentAnim != _AnimHashes.Attack_04)
            {
                RotateTowardsPlayer();
                RunToPlayer();
                MoveToPlayer();
                MoveBackFromPlayer();
            }

        }
        FightModeAnimatorSetter();
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void PlayerDetection()
    {
        if (_FoV.VisibleTargets.Count >= 1 && OnlyPatrolEnemy == false)     //if the enemy sees the player
        {
            if (_SoundState._CurrentIntensity == GameIntensity.EXPLORE)
            {
                // print("--------------------------------------------");
                // print(this.gameObject);
                // print("--------------------------------------------");
                AkSoundEngine.SetState("game_intensity", "battle");
                _SoundState._CurrentIntensity = GameIntensity.BATTLE;
            }
            PlayerDetected();
        }
        else
        {
            //AkSoundEngine.PostEvent("Play_Enemies_Idle", this.gameObject);
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void PlayerDetected()
    {
        _Approach = true;
        _PlayerDetected = true;
        _Anim.SetBool("FightMode", true);
        _NavMesh.isStopped = false;
        if (!gameObject.GetComponent<AIHealth>().IsAlive) return;
        if (!_EnemyList.EnemiesAlerted.Contains(gameObject) && !_EnemyList.EnemiesInside.Contains(gameObject))
        {
            _EnemyList.EnemiesAlerted.Add(gameObject);              //adding the enemies to alerted list
        }
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void PatrolSystem()
    {
        if (_PatrolToFollow == null) return;
        if (_PlayerDetected == false || OnlyPatrolEnemy == true)
        {
            _DistanceNavMesh = _NavMesh.remainingDistance;

            if (_StoppingTimePatrolPoints > 0f && _NavMesh.remainingDistance <= .01f)
            {
                _Anim.SetFloat("Velocity", 0);
                _NavMesh.isStopped = true;
                _StoppingTimePatrolPoints -= Time.deltaTime;
                var temp = _PointsIndex;
                var temp2 = temp++;
                var point = _PatrolToFollow.transform.GetChild(temp2);
                RotateTowardsPoint(point);
            }

            if (_PointsIndex < _AmmountOfPointsPatrol && _StoppingTimePatrolPoints <= 0f && _NavMesh.remainingDistance <= .01f)
            {
                _NavMesh.isStopped = false;
                var point = _PatrolToFollow.transform.GetChild(_PointsIndex);
                _NavMesh.SetDestination(point.position);
                _PointsIndex++;
                _StoppingTimePatrolPoints = _MaxStoppingTime;
            }
            else if (_PointsIndex >= _AmmountOfPointsPatrol)
            {
                _NavMesh.isStopped = false;
                _PointsIndex = 0;
            }
        }

        if (_NavMesh.remainingDistance > .1f)
        {
            _Anim.SetFloat("Velocity", 1);
        }
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void FightModeAnimatorSetter()
    {
        if (_EnemyList.EnemiesInside.Contains(gameObject))
        {
            if (_Distance <= _EnemyList.DistanceToMoveBackInnerEnemies || _Distance <= _EnemyList.DistanceToMoveBackOuterEnemies)        //player is to close to the enemy
            {
                if (_NavMesh.remainingDistance > .1f)
                {
                    _Anim.SetFloat("Velocity", LerpAnimator(-1));
                    _Anim.ResetTrigger("Attack_01");
                    _Anim.ResetTrigger("Attack_02");
                    _Anim.ResetTrigger("Block");
                }
            }
            else
            {
                if (_Distance > _EnemyList.DistanceInnerEnemies && _Distance < _EnemyList.DistanceToRunInnerEnemies && _NavMesh.remainingDistance > .1f)
                {
                    _Anim.SetFloat("Velocity", LerpAnimator(1));
                    _Anim.ResetTrigger("Attack_01");
                    _Anim.ResetTrigger("Attack_02");
                    _Anim.ResetTrigger("Block");

                }
            }
        }
        else if (_EnemyList.EnemiesAlerted.Contains(gameObject))
        {

            if (_Distance > _EnemyList.DistanceOutterEnemies && _Distance < _EnemyList.DistanceToRunOuterEnemies && _NavMesh.remainingDistance > .1f)
            {
                _Anim.SetFloat("Velocity", LerpAnimator(1));
                _Anim.ResetTrigger("Attack_01");
                _Anim.ResetTrigger("Attack_02");
                _Anim.ResetTrigger("Block");
            }
            else if (_Distance < _EnemyList.DistanceOutterEnemies && _NavMesh.remainingDistance > .1f)
            {
                _Anim.SetFloat("Velocity", LerpAnimator(-1));
                _Anim.ResetTrigger("Attack_01");
                _Anim.ResetTrigger("Attack_02");
                _Anim.ResetTrigger("Block");

            }
        }

        if (_NavMesh.remainingDistance < .1f)
        {
            _Anim.SetFloat("Velocity", LerpAnimator(0));
        }
    }

    private void MoveBackFromPlayer()
    {
        if (_EnemyList.EnemiesInside.Contains(gameObject))
        {
            if (_Distance <= _EnemyList.DistanceToMoveBackInnerEnemies)        //player is to close to the enemy
            {
                _NavMesh.isStopped = false;
                Vector3 _TargetPosition = transform.position + -transform.forward * 3f;     //getting the backwards vector plus a distance
                _NavMesh.SetDestination(_TargetPosition);
                InPosition = false;
                _NavMesh.isStopped = false;
                _Fight.StartAttack = false;
            }
        }

        if (_EnemyList.EnemiesAlerted.Contains(gameObject))
        {
            if (_Distance <= _EnemyList.DistanceOutterEnemies)        //player is to close to the enemy
            {
                _NavMesh.isStopped = false;
                Vector3 _TargetPosition = transform.position + -transform.forward * 3f;     //getting the backwards vector plus a distance
                _NavMesh.SetDestination(_TargetPosition);
                InPosition = false;
                _NavMesh.isStopped = false;
                _Fight.StartAttack = false;
            }
        }



    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void MoveToPlayer()
    {
        if (_EnemyList.EnemiesInside.Contains(gameObject) && _Distance > _EnemyList.DistanceInnerEnemies)
        {
            InPosition = false;
            _NavMesh.isStopped = false;
            _Fight.StartAttack = false;
            // Vector3 DistDifference = (transform.position - _Player.value.position).normalized;
            // _NavMesh.SetDestination(_Player.value.position + (DistDifference * _DistanceInnerEnemies));

            int index = _EnemyList.EnemiesInside.IndexOf(gameObject);
            Quaternion rotation = Quaternion.AngleAxis(_DegreesOfSeparationInside * index, Vector3.up);       //getting the distance to add with the correspondent degrees of separation
            Vector3 addDistanceToDirection = rotation * _StaticForward.transform.forward * _EnemyList.DistanceInnerEnemies;
            _NavMesh.SetDestination(_Player.value.position + addDistanceToDirection);
        }

        //Outter enemies
        if (_EnemyList.EnemiesAlerted.Contains(gameObject) && _Distance > _EnemyList.DistanceOutterEnemies)
        {
            _Anim.ResetTrigger("Attack_01");
            _Anim.ResetTrigger("Attack_02");
            _Anim.ResetTrigger("Block");
            _Fight.RandomNumber = 0;
            _Fight.StartAttack = false;

            int index = _EnemyList.EnemiesAlerted.IndexOf(gameObject);
            Quaternion rotation = Quaternion.AngleAxis(_DegreesOfSeparationOutside * index, Vector3.up);       //getting the distance to add with the correspondent degrees of separation
            Vector3 addDistanceToDirection = rotation * _StaticForward.transform.forward * _EnemyList.DistanceOutterEnemies;
            _NavMesh.SetDestination(_Player.value.position + addDistanceToDirection);
        }
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void RotateTowardsPlayer()
    {
        //transform.LookAt(_Player.value.position);
        // if (_InpSw.CanRotate)
        // {

        // find the vector pointing from our position to the target
        _Direction = (_Player.value.position - transform.position).normalized;

        //    create the rotation we need to be in to look at the target
        _LookRotation = Quaternion.LookRotation(_Direction);

        //   rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, _LookRotation, Time.deltaTime * _RotateSpeed);
        //   }

    }

    private void RotateTowardsPoint(Transform point)
    {
        _Direction = (point.transform.position - transform.position).normalized;

        //    create the rotation we need to be in to look at the target
        _LookRotation = Quaternion.LookRotation(_Direction);

        //   rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, _LookRotation, Time.deltaTime * _RotateSpeed);
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void RunToPlayer()
    {
        if (_EnemyList.EnemiesInside.Contains(gameObject) && _Distance > _EnemyList.DistanceInnerEnemies)
        {
            if (_Distance > _EnemyList.DistanceToRunInnerEnemies)
            {
                _Anim.SetFloat("Velocity", LerpAnimator(2));
                _NavMesh.speed = _RunningSpeed;
            }
            else
            {
                _Anim.SetFloat("Velocity", LerpAnimator(1));
                _NavMesh.speed = _WalkingSpeed;
            }
        }

        //Outter enemies
        if (!_EnemyList.EnemiesInside.Contains(gameObject) && _Distance > _EnemyList.DistanceOutterEnemies)
        {
            if (_Distance > _EnemyList.DistanceToRunOuterEnemies)
            {
                _Anim.SetFloat("Velocity", LerpAnimator(2));
                _NavMesh.speed = _RunningSpeed;
            }
            else
            {
                _Anim.SetFloat("Velocity", LerpAnimator(1));
                _NavMesh.speed = _WalkingSpeed;
            }
        }
    }


    private float LerpAnimator(float to)
    {
        if (to != _PrevTo)
        {
            _GetValue = true;
            _TLerp = 0;
        }
        _PrevTo = to;

        if (_GetValue)
        {
            _AnimVal = _Anim.GetFloat("Velocity");
            _GetValue = false;
        }

        var newAnimVal = Mathf.Lerp(_AnimVal, to, _TLerp);
        _TLerp += _LerpT * Time.deltaTime;

        return newAnimVal;
    }
}
