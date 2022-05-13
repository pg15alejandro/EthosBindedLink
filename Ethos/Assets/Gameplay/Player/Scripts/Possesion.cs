using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using System;

public enum EnemyPosType { REGULAR = 1, ELITE, EXECUTIONERBOSS, KING }

public class Possesion : MonoBehaviour
{
    private Animator _PlayerAnim;
    [SerializeField] private SOTransform _PlayerTransform;
    [SerializeField] private CinemachineVirtualCamera _VirtualCamera;
    [SerializeField] private CinemachineBrain _CinemachineBrain;
    [SerializeField] private CameraController _EnemyCamController;
    [SerializeField] private CameraController _CamController;
    [SerializeField] private float _SlowTime;
    [SerializeField] private TextBoxEnablersDisablers _UI;
    [SerializeField] private HealthBar _HealthBar;
    [SerializeField] private SOEnemyRegister _EnemyList;
    [SerializeField] private MoveOrbsToPlayer _Orbs;

    [SerializeField] private ParticleSystem _PossesionTrail;
    [SerializeField] private ParticleSystem _PossesionHit;
    [SerializeField] private SkinnedMeshRenderer[] _MeshRenderMat;
    [SerializeField] private SkinnedMeshRenderer[] _MeshRenderORIGINAL;
    [SerializeField] private EnemyPlacement _EnemiesInZone;
    [SerializeField] private GameplayLogic _GmLogic;

    [SerializeField] private RuntimeAnimatorController _ArcadiaAsGuardAnimator;
    [SerializeField] private RuntimeAnimatorController _ArcadiaAsExecutionerAnimator;
    private RuntimeAnimatorController _EnemyAnimator;
    private bool _PossesionInput;

    private Rigidbody _RigidBody;
    private PlayerController _PlayController;
    private GameObject _EnemyHit;
    private Transform _Model;
    private Transform _ModelPivotTransform;
    Volume _PossesionVolume;
    private int _EnemyHitLayer = -1;

    public bool possesing { get; private set; }
    public bool canGetOut { get; private set; }
    private bool _BlendingCam;
    private bool _SkipFrame;
    private float _LerpT = .1f;
    private bool _LerpTime;
    private float _TScale;
    [Header("FightingLoop")]
    [SerializeField] private int _EssenceToRestore;
    [SerializeField] private int _AmmountOfHealthToGain;
    [SerializeField] private float _PossesionDamage;
    [SerializeField] private bool _firstTime = true;
    private EnemyPosType _EnemyHitType;
    [SerializeField] private Material _DisolveMaterial;
    private int _IndexMaterialSwap = 0;
    [SerializeField] private float _TimeToDisolveArcadia;
    private bool _UpdatePosition;

    /// <summary>
    /// Sets the possesion input to given value
    /// </summary>
    /// <param name="keyPressed">Value of the input</param>
    public void SetInput(bool keyPressed)
    {
        _PossesionInput = keyPressed;
    }


    /// <summary>
    /// Unity's Start method. Called before the first frame
    /// </summary>
    private void Start()
    {
        _RigidBody = GetComponent<Rigidbody>();
        _PlayController = GetComponent<PlayerController>();
        _PossesionVolume = GetComponent<Volume>();
        _PlayerAnim = GetComponent<Animator>();
        _PossesionTrail.Stop();
        _PossesionHit.Stop();
        _Model = transform.GetChild(0);
        _ModelPivotTransform = _Model.GetChild(2);
        _MeshRenderORIGINAL = new SkinnedMeshRenderer[_MeshRenderMat.Length];
        for (int i = 0; i < _MeshRenderMat.Length; i++)
        {
            _MeshRenderORIGINAL[i] = _MeshRenderMat[i];
        }
    }


    /// <summary>
    /// Unity's Update method. Called once per frame
    /// </summary>
    private void Update()
    {
        SetPostProcessing();


        if (_UpdatePosition) UpdatingPlayerPosition();
        //Update the player position if he is possesing somebody
        if (possesing && _EnemyHit != null)
        {
            if (_EnemyList != null)
            {
                if (_EnemyList.EnemiesInside.Contains(_EnemyHit))
                {
                    _EnemyList.EnemiesInside.Remove(_EnemyHit);
                }

                if (_EnemyList.Executioners.Contains(_EnemyHit))
                {
                    _EnemyList.Executioners.Remove(_EnemyHit);
                }
            }
        }

        CheckBlendingCamera();

        //Read for possesion input
        if (!_PossesionInput) return;

        //Set the input to false
        _PossesionInput = false;

        //If I am already possesing and the input is pressed
        if (possesing)
        {
            StopAllCoroutines();
            Deposses();
            return;
        }
        else
        {

            int enemyL = LayerMask.NameToLayer("Enemy");
            int eliteL = LayerMask.NameToLayer("EliteEnemy");
            int kingexecutioner = LayerMask.NameToLayer("ExecutionerBoss");

            //Check if nothing is between the enemy and the player
            RaycastHit hit;
            bool enemy = Physics.Raycast(_ModelPivotTransform.position, _ModelPivotTransform.forward * 10, out hit);

            var t = hit.transform.gameObject;

            if (!enemy) return;

            if (hit.transform.gameObject.layer == enemyL)
            {
                _EnemyHitType = EnemyPosType.REGULAR;
            }
            else if (hit.transform.gameObject.layer == eliteL)
            {
                _EnemyHitType = EnemyPosType.ELITE;
            }
            else if (hit.transform.gameObject.layer == kingexecutioner)
            {
                _EnemyHitType = EnemyPosType.EXECUTIONERBOSS;
            }
            else return;

            //Check if nothing is between the enemy and the player
            _EnemyHit = hit.transform.gameObject;

            //Set possesing to true
            possesing = true;
            canGetOut = false;

            //Stop the lockOn

            //Set the enemy camera base to the enemy position
            _EnemyCamController.SetEnemyPos(hit.transform.position);

            _PlayerAnim.CrossFade("Possesion", .1f, 2);

            //Disable the character scripts and enable the character scripts inside the possesed enemy
            PreBlendingActions();

            //Start the Depossess coroutine
            StartCoroutine(Depossesing());
        }
    }

    private void UpdatingPlayerPosition()
    {
        _PlayerTransform.value.position = _EnemyHit.transform.position;
    }

    private void CheckBlendingCamera()
    {
        if (_LerpTime)
        {
            //  Time.timeScale = Mathf.Lerp(1f, _SlowTime, _TScale);
            //_TScale += _LerpT;
        }

        if (!_BlendingCam) return;

        _LerpTime = false;
        _TScale = 0f;

        if (possesing)
        {
            if (_CinemachineBrain.ActiveBlend != null)
            {
                if(_EnemiesInZone != null)
                    StopAnimators();

                print($"QQQQQQQ DURATION: {_CinemachineBrain.ActiveBlend.Duration}");
                //Time.timeScale = _SlowTime;
                var from = new Vector3(_PlayerTransform.Hand.position.x, _PlayerTransform.Hand.position.y, _PlayerTransform.Hand.position.z);
                var to = new Vector3(_EnemyHit.transform.position.x, _EnemyHit.transform.position.y + 1.5f, _EnemyHit.transform.position.z);
                var actualPosition = Vector3.Lerp(from, to, _CinemachineBrain.ActiveBlend.TimeInBlend / _CinemachineBrain.ActiveBlend.Duration);
                var ammount = Mathf.Lerp(-1, 1, _TimeToDisolveArcadia * Time.deltaTime);
                _MeshRenderMat[0].material.SetFloat("DissolveLevel", ammount);

                _PossesionTrail.transform.position = actualPosition;
                _PossesionTrail.transform.rotation = _PlayerTransform.value.rotation;
            }
            else
            {
                if (_SkipFrame)
                {
                    _SkipFrame = false;
                    return;
                }
                print("Playing hit particle");
                _PlayController.SetLockOn(false);
                _EnemyHit.GetComponent<Animator>().speed = 1;
                ZoneEnemiesReseter();
                _UpdatePosition = true;

                _PossesionHit.transform.position = _EnemyHit.transform.position;
                _PossesionHit.transform.rotation = _EnemyHit.transform.rotation;
                _PossesionTrail.Stop();
                _PossesionHit.Play();
                PostBlendingActions();
                _BlendingCam = false;
                _EnemyCamController.IsPlayerCamera = true;
                canGetOut = true;
                Time.timeScale = 1f;
            }
        }
        else
        {
            _BlendingCam = false;
        }

    }

    private void ZoneEnemiesReseter()
    {
        if (_EnemiesInZone != null && _GmLogic != null)
        {
            foreach (var enemy in _EnemiesInZone.Areas[_GmLogic.ZoneIndex].Enemies)
            {
                if (enemy.name != _EnemyHit.name)
                {
                    enemy.GetComponent<Animator>().speed = 1;
                    var normalEnemy = enemy.GetComponent<MovementAI>();
                    var eliteEnemy = enemy.GetComponent<ExecutionerMovement>();
                    var navmeshEnemy = enemy.GetComponent<NavMeshAgent>();
                    var normalEnemyFight = enemy.GetComponent<FightSystemAI>();
                    var eliteEnemyFight = enemy.GetComponent<ExecutionerFightingSystem>();
                    if (normalEnemy != null) normalEnemy.enabled = true;
                    if (eliteEnemy != null) eliteEnemy.enabled = true;
                    if (navmeshEnemy != null) navmeshEnemy.enabled = true;
                    if (normalEnemyFight != null) normalEnemyFight.enabled = true;
                    if (eliteEnemyFight != null) eliteEnemyFight.enabled = true;
                }
            }
        }
    }

    private void PreBlendingActions()
    {
        if (possesing)
        {
            //Character actions            
            GetComponent<CapsuleCollider>().enabled = false;
            _RigidBody.isKinematic = true;
            _EnemyAnimator = _EnemyHit.GetComponent<Animator>().runtimeAnimatorController;
            //RegularEnemy
            FightSystemAI regAIFightingSystem = _EnemyHit.GetComponentInChildren<FightSystemAI>();
            MovementAI regEnemyMovement = _EnemyHit.GetComponentInChildren<MovementAI>();

            //EliteEnemy
            ExecutionerFightingSystem eliteAIFightingSystem = _EnemyHit.GetComponentInChildren<ExecutionerFightingSystem>(); ;
            ExecutionerMovement eliteEnemyMovement = _EnemyHit.GetComponent<ExecutionerMovement>(); ;

            //Execuioner Boss
            KingExecutionerFightingSystem executionerBossFightingSystem = _EnemyHit.GetComponentInChildren<KingExecutionerFightingSystem>(); ;
            ExecutionersKing executionerBossMovement = _EnemyHit.GetComponent<ExecutionersKing>(); ;

            if (_EnemyHitType == EnemyPosType.REGULAR)
            {
                if (regAIFightingSystem != null) regAIFightingSystem.enabled = false;
                if (regEnemyMovement != null) regEnemyMovement.enabled = false;
            }
            if (_EnemyHitType == EnemyPosType.ELITE)
            {
                if (eliteAIFightingSystem != null) eliteAIFightingSystem.enabled = false;
                if (eliteEnemyMovement != null) eliteEnemyMovement.enabled = false;
            }
            if (_EnemyHitType == EnemyPosType.EXECUTIONERBOSS)
            {
                if (executionerBossFightingSystem != null) executionerBossFightingSystem.enabled = false;
                if (executionerBossMovement != null) executionerBossMovement.enabled = false;
            }

            var enemyNavMesh = _EnemyHit.GetComponent<NavMeshAgent>();
            var enemyRB = _EnemyHit.GetComponent<Rigidbody>();
            var enemyAnimator = _EnemyHit.GetComponent<Animator>();

            if (enemyNavMesh != null) enemyNavMesh.enabled = false;
            if (enemyRB != null) enemyRB.freezeRotation = true;
            _EnemyHit.GetComponent<Rigidbody>().isKinematic = false;

            enemyAnimator.CrossFade("PlaceHolder", 0, 0);
            enemyAnimator.CrossFade("PlaceHolder", 0, 1);
            enemyAnimator.CrossFade("PlaceHolder", 0, 2);

            //Set camera rotations
            _EnemyCamController.SetRotations(_CamController.GetRotations());
            _EnemyCamController.gameObject.transform.eulerAngles = _CamController.gameObject.transform.eulerAngles;
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            _VirtualCamera.Priority = 15;
            GetComponent<CapsuleCollider>().enabled = true;
            _RigidBody.isKinematic = false;

            //Set camera rotations
            // _EnemyCamController.SetRotations(_CamController.GetRotations());
            // _EnemyCamController.gameObject.transform.eulerAngles = _CamController.gameObject.transform.eulerAngles;
        }
    }

    private void PostBlendingActions()
    {
        //If there's no enemy do nothing
        if (_EnemyHit == null) return;

        //RegularEnemy
        MovementAI regEnemyMovement = _EnemyHit.GetComponentInChildren<MovementAI>();
        FightSystemAI regAIFightingSystem = _EnemyHit.GetComponentInChildren<FightSystemAI>();
        FightingSystemRegularEnemyArcadia regEnemyPlayerFightingSystem = _EnemyHit.GetComponentInChildren<FightingSystemRegularEnemyArcadia>();
        RegularEnemyController regController = _EnemyHit.GetComponent<RegularEnemyController>();

        //EliteEnemy
        ExecutionerMovement eliteEnemyMovement = _EnemyHit.GetComponent<ExecutionerMovement>(); ;
        ExecutionerFightingSystem eliteAIFightingSystem = _EnemyHit.GetComponentInChildren<ExecutionerFightingSystem>(); ;
        ExecutionerArcadiaFightingSystem eliteEnemyPlayerFightSystem = _EnemyHit.GetComponentInChildren<ExecutionerArcadiaFightingSystem>();

        //ExecutionerBoss
        ExecutionersKing executionerBossEnemyMovement = _EnemyHit.GetComponent<ExecutionersKing>(); ;
        KingExecutionerFightingSystem executionerBossFightingSystem = _EnemyHit.GetComponentInChildren<KingExecutionerFightingSystem>(); ;
        ExecutionerArcadiaFightingSystem executionerBossEnemyPlayerFightSystem = _EnemyHit.GetComponentInChildren<ExecutionerArcadiaFightingSystem>();

        //PENDING TO GET ARCADIA CONTROLING THE KING EXECUTIONER MOVEMENT AND FIGHTING

        var enemyNavMesh = _EnemyHit.GetComponent<NavMeshAgent>();
        var enemyRB = _EnemyHit.GetComponent<Rigidbody>();
        var enemyAnimator = _EnemyHit.GetComponent<Animator>();

        if (possesing)
        {
            //Character actions
            transform.GetChild(0).gameObject.SetActive(false);

            //Enemy actions
            if (_EnemyHitType == EnemyPosType.REGULAR)
            {
                if (regController != null) regController.enabled = true;
                if (regEnemyPlayerFightingSystem != null) regEnemyPlayerFightingSystem.enabled = true;
                _EnemyHit.GetComponent<Animator>().runtimeAnimatorController = _ArcadiaAsGuardAnimator;     ///////
            }

            if (_EnemyHitType == EnemyPosType.ELITE)
            {
                if (regController != null) regController.enabled = true;
                if (eliteEnemyPlayerFightSystem != null) eliteEnemyPlayerFightSystem.enabled = true;
                _EnemyHit.GetComponent<Animator>().runtimeAnimatorController = _ArcadiaAsExecutionerAnimator; ///////
            }

            if (_EnemyHitType == EnemyPosType.EXECUTIONERBOSS)
            {
                if (regController != null) regController.enabled = true;
                if (executionerBossEnemyPlayerFightSystem != null) executionerBossEnemyPlayerFightSystem.enabled = true;
                //DOUBLE CHECK ANIMATOR AND MISSING ENABLE THE FIGHTING AS ARCADIA AND CHECK THE MOVEMENT
                _EnemyHit.GetComponent<Animator>().runtimeAnimatorController = _ArcadiaAsExecutionerAnimator; ///////
            }

            _EnemyHit.gameObject.layer = LayerMask.NameToLayer("Player");

            //Set camera rotations
            _CamController.SetRotations(_EnemyCamController.GetRotations());
            _CamController.gameObject.transform.eulerAngles = _EnemyCamController.gameObject.transform.eulerAngles;

        }
        else
        {
            if (_EnemyHitType == EnemyPosType.REGULAR)
            {
                if (regController != null) regController.enabled = false;
                if (regEnemyPlayerFightingSystem != null) regEnemyPlayerFightingSystem.enabled = false;
                if (regAIFightingSystem != null) regAIFightingSystem.enabled = true;
                if (regEnemyMovement != null)
                {
                    regEnemyMovement.enabled = true;
                    regEnemyMovement.OnlyPatrolEnemy = false;
                    regEnemyMovement.PlayerDetected();
                }
                _EnemyHit.GetComponent<Animator>().runtimeAnimatorController = _EnemyAnimator; ///////
            }

            if (_EnemyHitType == EnemyPosType.ELITE)
            {
                if (regController != null) regController.enabled = false;
                if (eliteEnemyPlayerFightSystem != null) eliteEnemyPlayerFightSystem.enabled = false;
                if (eliteAIFightingSystem != null) eliteAIFightingSystem.enabled = true;
                if (eliteEnemyMovement != null)
                {
                    eliteEnemyMovement.enabled = true;
                    eliteEnemyMovement.PlayerDetected();
                }
                _EnemyHit.GetComponent<Animator>().runtimeAnimatorController = _EnemyAnimator; ///////
            }

            if (_EnemyHitType == EnemyPosType.EXECUTIONERBOSS)
            {
                if (regController != null) regController.enabled = false;
                if (executionerBossEnemyPlayerFightSystem != null) executionerBossEnemyPlayerFightSystem.enabled = false;
                if (executionerBossFightingSystem != null) executionerBossFightingSystem.enabled = true;
                if (executionerBossEnemyMovement != null)
                {
                    executionerBossEnemyMovement.enabled = true;
                }

                //CHECK THE ANIMATOR AND MISSING DISABLE FIGHTING AS ARCADIA CHECK MOVEMENT AS ARCADIA
                _EnemyHit.GetComponent<Animator>().runtimeAnimatorController = _EnemyAnimator; ///////
            }

            if (enemyNavMesh != null) enemyNavMesh.enabled = true;
            if (enemyRB != null) enemyRB.freezeRotation = false;
            _EnemyHit.GetComponent<Rigidbody>().isKinematic = true;

            if (_EnemyHitType == EnemyPosType.REGULAR) _EnemyHit.gameObject.layer = LayerMask.NameToLayer("Enemy");
            if (_EnemyHitType == EnemyPosType.ELITE) _EnemyHit.gameObject.layer = LayerMask.NameToLayer("EliteEnemy");
            if (_EnemyHitType == EnemyPosType.EXECUTIONERBOSS) _EnemyHit.gameObject.layer = LayerMask.NameToLayer("ExecutionerBoss");
            _EnemyCamController.IsPlayerCamera = false;
            enemyAnimator.CrossFade("Stun", .1f, 2);
            _UpdatePosition = false;
            if (_firstTime)
            {
                AkSoundEngine.PostEvent("Dungeon_Arcadia_005", this.gameObject);
                StartCoroutine(AfterPosBox());
                _firstTime = false;
            }
        }
    }    

    private IEnumerator AfterPosBox()
    {
        yield return new WaitForSeconds(.5f);

        _UI.AfterPossesionBox();

        StopAllCoroutines();
    }

    public void ChangeCamPriority()
    {
        _VirtualCamera.Priority = 5;
        _PossesionTrail.transform.position = _PlayerTransform.Hand.position;
        _PossesionTrail.transform.rotation = _PlayerTransform.value.rotation;
        _PossesionTrail.Play();
        _BlendingCam = true;
        _SkipFrame = true;

        //Play the possesing audio
        AkSoundEngine.PostEvent("Play_Arcadia_Possession_Initial", this.gameObject);
    }

    public void StartTimeLerp()
    {
        AnimationClip animClip;
        var animClipInfo = _PlayerAnim.GetCurrentAnimatorClipInfo(2);


        animClip = animClipInfo[0].clip;

        var lerpFrames = (animClip.events[5].time) * 30 - (animClip.events[4].time * 30);
        _LerpT = 1 / lerpFrames;

        _LerpTime = true;
    }

    private IEnumerator Depossesing()
    {
        yield return new WaitForSeconds(10);

        Deposses();
    }

    private void Deposses()
    {
        transform.position = _EnemyHit.transform.position;
        transform.rotation = _EnemyHit.transform.rotation;

        possesing = false;
        _BlendingCam = true;

        AkSoundEngine.PostEvent("Play_Arcadia_Possession_Getout", this.gameObject);

        PreBlendingActions();
        PostBlendingActions();

        var enemyHealth = _EnemyHit.GetComponent<AHealth>();
        if (enemyHealth.GetCurrentHealth() <= _PossesionDamage)
        {
            _Orbs.MoveEssenceOrbToPlayer(_EnemyHit.transform, _EssenceToRestore);
            _Orbs.MoveHealthOrbToPlayer(_EnemyHit.transform, _AmmountOfHealthToGain);
        }
    }

    private void SetPostProcessing()
    {
        //Check for null
        if (_PossesionVolume == null) return;

        //If I'm possesing, increase the postprocessing
        if (possesing && _PossesionVolume.weight < 1)
            _PossesionVolume.weight += .01f;

        //If I'm not possesing, decrease the postprocessing
        if (!possesing && _PossesionVolume.weight > 0)
            _PossesionVolume.weight -= .01f;

    }

    private void StopAnimators()
    {        
        if(_EnemiesInZone == null) return;

        foreach (var enemy in _EnemiesInZone.Areas[_GmLogic.ZoneIndex].Enemies)
        {
            enemy.GetComponent<Animator>().speed = 0;
            var normalEnemy = enemy.GetComponent<MovementAI>();
            var eliteEnemy = enemy.GetComponent<ExecutionerMovement>();
            var navmeshEnemy = enemy.GetComponent<NavMeshAgent>();
            var normalEnemyFight = enemy.GetComponent<FightSystemAI>();
            var eliteEnemyFight = enemy.GetComponent<ExecutionerFightingSystem>();
            if (normalEnemy != null) normalEnemy.enabled = false;
            if (eliteEnemy != null) eliteEnemy.enabled = false;
            if (navmeshEnemy != null) navmeshEnemy.enabled = false;
            if (normalEnemyFight != null) normalEnemyFight.enabled = false;
            if (eliteEnemyFight != null) eliteEnemyFight.enabled = false;
        }
    }
}

