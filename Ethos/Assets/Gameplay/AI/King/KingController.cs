//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum KingStage { FIRST, SECOND, THIRD }
public enum KingState { IDLE, DEFEND, ATTACK }
public enum KingBasicAttacks { HIGH_ATTACK_LEFT = 1, MED_ATTACK_LEFT, HIGH_ATTACK_RIGHT, MED_ATTACK_RIGHT }
public enum KingSpecialAttacks { PAN_SWING = 1, STOMP, THRUST }
//SpawnExecutioners, Walk, WalkBackwards, Run

public class KingController : MonoBehaviour
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------
    public KingStage _KingStage;
    public KingState _KingState;
    private KingBasicAttacks _KingBasicAttack;
    private KingSpecialAttacks _KingSpecialAttack;
    private KingHealth _Health;
    private KingFightingSystem _KingFighting;
    private KingMovement _KingMovement;
    public bool AttackIsSpecial;

    float _TempTimer = 5f;
    bool _StartStageOne = true;
    bool _StartStageTwo = true;
    bool _StartStageThree = true;
    private bool _CanDoAction;
    private bool _LeftAttack;
    private bool _SpecialAttack;

    [SerializeField] private SOResources _PlayerResources;
    [SerializeField] private float _SpecialAttackCooldown = 5f;
    [SerializeField] private List<GameObject> _Executioners;
    [SerializeField] private KingExecutionerList _ExecList;
    [SerializeField] private WinLose _WinLose;
    [SerializeField] private GameObject _MusGameObj;
    [SerializeField] private GameObject _Shield;

    private int _AttackCount;

    private Animator _Anim;
    private bool _SummonExe;
    private int _AttackLimit;
    private int _AttackDamage;
    private bool _WaitForNextStage;
    [SerializeField] private int _FirstStageAttackLimit = 3;
    [SerializeField] private int _SecondStageAttackLimit = 5;

    /// <summary>
    /// Unity's Start method. Called before the first frame
    /// </summary>    
    void Start()
    {
        _Anim = GetComponentInChildren<Animator>();
        _Health = GetComponent<KingHealth>();
        _KingFighting = GetComponentInChildren<KingFightingSystem>();
        _KingMovement = GetComponent<KingMovement>();

        _KingStage = KingStage.FIRST;
        _KingState = KingState.IDLE;
        _CanDoAction = true;

        SetExecutionersInitial();
    }

    private void SetExecutionersInitial()
    {
        for (int i = 0; i < _Executioners.Count; i++)
        {
            _Executioners[i].GetComponent<Animator>().speed = 0f;
            _Executioners[i].GetComponent<ExecutionerHealthKing>().Invulnerable = true;
        }
    }


    /// <summary>
    /// Unity's Update method. Called once per frame
    /// </summary>    
    private void Update()
    {
        switch (_KingStage)
        {
            case KingStage.FIRST:
                StageOne();
                break;

            case KingStage.SECOND:
                StageTwo();
                break;

            case KingStage.THIRD:
                StageThree();
                break;

            default:
                break;
        }
    }


    /// <summary>
    /// Set all the stuff for the stage one of the boss (only called once)
    /// </summary>
    private void SetStageOne()
    {
        _StartStageOne = false;
        _AttackLimit = _FirstStageAttackLimit;
        AkSoundEngine.SetSwitch("king_phase", "phase_1", _MusGameObj);
        //_KingUI.SetActive(true);
    }


    /// <summary>
    /// Set all the stuff for the stage two of the boss (only called once)
    /// </summary>
    private void SetStageTwo()
    {
        _KingMovement.SetDoMovement();
        _StartStageTwo = false;
        _Health.invulnerable = true;
        _TempTimer = 5;
        AkSoundEngine.SetSwitch("king_phase", "phase_2", _MusGameObj);                
        AkSoundEngine.PostEvent("Play_King_Summon", this.gameObject);
    }


    /// <summary>
    /// Set all the stuff for the stage three of the boss (only called once)
    /// </summary>
    private void SetStageThree()
    {
        //Set the postProcessing stuff
        _Health.invulnerable = false;
        _AttackLimit = _SecondStageAttackLimit;
        AkSoundEngine.SetSwitch("king_phase", "phase_3", _MusGameObj);
        AkSoundEngine.PostEvent("CS_VO_011_Alexeon", this.gameObject);        
        _Shield.SetActive(false);     
        _StartStageThree = false;   
    }


    /// <summary>
    /// Checks if the stage needs to change and calls DoAction
    /// </summary>
    private void StageOne()
    {
        if (_StartStageOne)
            SetStageOne();

        if (_Health.healthPercentage <= .45f)
            _KingStage = KingStage.SECOND;

        if (_CanDoAction)
            DoAction();
    }


    /// <summary>
    /// Moves the king to his throne, calls TakesEssence and summons the executioners
    /// </summary>
    private void StageTwo()
    {

        if (_StartStageTwo)
        {
            SetStageTwo();
            return;
        }

        if (_WaitForNextStage)
        {
            if(_ExecList.ExecutionersAlive)
                return;

            _KingStage = KingStage.THIRD;
        }

        if (!_KingMovement.ReachedThrone) return;

        /*//Takes essence from you
        if (!_SummonExe)
        {
            TakeEssence();
            return;
        }*/

        //Summon the executioners
        //_Anim.SetTrigger("SpawnExecutioners");
        SummonExecutioners();        
        _Shield.SetActive(true);

        _WaitForNextStage = true;
    }

    public void SummonExecutioners()
    {
        for (int i = 0; i < _Executioners.Count; i++)
        {
            _Executioners[i].GetComponent<ExecutionersKing>().enabled = true;
            _Executioners[i].GetComponent<NavMeshAgent>().enabled = true;
            var anim = _Executioners[i].GetComponent<Animator>();
            _Executioners[i].GetComponent<ExecutionerHealthKing>().Invulnerable = false;
            anim.speed = 1f;
            anim.SetFloat("Velocity", 1);

            _Executioners[i].layer = LayerMask.NameToLayer("ExecutionerBoss");
        }
    }


    /// <summary>
    /// Checks if the king is defeated and calls DoAction
    /// </summary>
    private void StageThree()
    {
        if (_StartStageThree) SetStageThree();

        if (_CanDoAction && _Health.healthPercentage > 0)
            DoAction();
    }


    /// <summary>
    /// Checks the state of the king and performs an action depending on it
    /// </summary>
    private void DoAction()
    {
        //Block, dodgeLeft, dodgeRight
        //HighAttackLeft, HighAttackRight, MidLeft, MidRight
        //panSwing, stomp, thrust
        //Run, walk, walkBackwards        

        float randomAttack;

        //Check if the player needs to keep attacking
        if (_KingState != KingState.ATTACK)
        {
            if (_KingStage == KingStage.FIRST && _KingState == KingState.IDLE)
            {
                _KingState = KingState.DEFEND;
            }
            else
            {
                _KingState = KingState.ATTACK;
                _KingMovement.SetDoMovement();
            }

            /*if (_KingState == KingState.DEFEND) _KingState = KingState.IDLE;
            else _KingState = KingState.ATTACK;*/
        }
        else
        {
            if (_AttackCount >= _AttackLimit)
                _KingState = KingState.IDLE;
        }

        switch (_KingState)
        {
            case KingState.IDLE:
                {
                    _AttackCount = 0;
                    _LeftAttack = true;
                    _Health.invulnerable = false;
                    print($"+++++++++ IDLE");
                }
                break;

            case KingState.DEFEND:
                {
                    _KingMovement.SetDoMovement();
                    _Health.invulnerable = true;
                    print($"+++++++++ DEFEND");
                }
                break;

            case KingState.ATTACK:
                {
                    _Health.invulnerable = false;

                    _TempTimer -= Time.deltaTime;

                    if (_KingMovement.DoMovement) return;

                    if (_TempTimer <= 0 && !_SpecialAttack)
                        _SpecialAttack = true;



                    if (_SpecialAttack)
                    {
                        randomAttack = UnityEngine.Random.Range(1, Enum.GetNames(typeof(KingSpecialAttacks)).Length + 1);
                        _KingSpecialAttack = (KingSpecialAttacks)randomAttack;
                        _TempTimer = _SpecialAttackCooldown;
                        _SpecialAttack = false;
                        _LeftAttack = false;
                        AttackIsSpecial = true;
                        print($"+++++++++ Special Attack:    {_KingSpecialAttack}");
                    }
                    else
                    {
                        if (_LeftAttack)
                        {
                            randomAttack = UnityEngine.Random.Range(1, (Enum.GetNames(typeof(KingBasicAttacks)).Length / 2) + 1);
                            _LeftAttack = false;
                        }
                        else
                        {
                            randomAttack = UnityEngine.Random.Range((Enum.GetNames(typeof(KingBasicAttacks)).Length / 2) + 1, Enum.GetNames(typeof(KingBasicAttacks)).Length + 1);
                            _LeftAttack = true;
                        }

                        _KingBasicAttack = (KingBasicAttacks)randomAttack;
                        AttackIsSpecial = false;
                        print($"+++++++++ Basic Attack:    {_KingBasicAttack}");
                    }

                    _AttackCount++;
                }
                break;

            default:
                Debug.LogError("No KingState value");
                break;
        }

        //StartCoroutine(ActionTimer(3.0f));
        SetAnimValues();
        _CanDoAction = false;
    }


    /// <summary>
    /// Button smash event that takes your essence away
    /// </summary>
    private void TakeEssence()
    {
        print("++++++GLU GLU GLU");
        if (_TempTimer <= 0)
        {
            _SummonExe = true;
            return;
        }

        _TempTimer -= Time.deltaTime;

        if (_PlayerResources.essencePercentage >= .2)
        {
            _PlayerResources.CurrentEssence -= 1;

            //Need to change this button
            if (Input.GetKeyDown(KeyCode.Z))
                _PlayerResources.CurrentEssence += 5;
        }
    }


    /// <summary>
    /// Sets _CanAttack to true. Called at the end of the attacking animations
    /// </summary>    
    public void SetCanAttack()
    {
        _CanDoAction = true;
    }


    /// <summary>
    /// Sets _CanAttack to true after time seconds. Temporal thing before implementing animations.
    /// </summary>
    private IEnumerator ActionTimer(float time)
    {
        yield return new WaitForSeconds(time);

        _CanDoAction = true;

        StopAllCoroutines();
    }


    /// <summary>
    /// Sets the corresponding animation to the king, depending on his state/stage
    /// </summary>
    private void SetAnimValues()
    {
        if (_Anim == null) return;

        /*_Anim.SetTrigger("HighLeft");
        return;*/

        switch (_KingState)
        {
            case KingState.IDLE:
                _Anim.SetTrigger("Idle");
                break;

            case KingState.DEFEND:
                //_Anim.SetTrigger("Block");
                break;

            case KingState.ATTACK:
                {
                    if (AttackIsSpecial)
                    {
                        switch (_KingSpecialAttack)
                        {
                            case KingSpecialAttacks.PAN_SWING:
                                _Anim.SetTrigger("PanSwing");
                                break;
                            case KingSpecialAttacks.STOMP:
                                _Anim.SetTrigger("Stomp");
                                break;
                            case KingSpecialAttacks.THRUST:
                                _Anim.SetTrigger("Thrust");
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (_KingBasicAttack)
                        {
                            case KingBasicAttacks.HIGH_ATTACK_LEFT:
                                _Anim.SetTrigger("HighLeft");
                                break;
                            case KingBasicAttacks.HIGH_ATTACK_RIGHT:
                                _Anim.SetTrigger("HighRight");
                                break;
                            case KingBasicAttacks.MED_ATTACK_LEFT:
                                _Anim.SetTrigger("MidLeft");
                                break;
                            case KingBasicAttacks.MED_ATTACK_RIGHT:
                                _Anim.SetTrigger("MidRight");
                                break;
                            default:
                                break;
                        }
                    }
                }
                break;

            default:
                Debug.LogError("No KingState value");
                break;
        }
    }

    public void SetAnimStart()
    {
        _KingFighting.AnimStart();
    }

    public void SetAnimStop()
    {
        _KingFighting.AnimStop();
    }

    public void WinState()
    {
        _WinLose.Win();
    }
}
