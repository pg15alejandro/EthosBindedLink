//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using UnityEngine;
using UnityEngine.Experimental.VFX;

public class FightingSystem : MonoBehaviour
{
    [SerializeField] private SOAnimationHashes _AnimationHashes;
    [SerializeField] private int _BaseDamage;
    [SerializeField] private int _EnhancedDamage;
    [SerializeField] private LayerMask _EnemyLayer; //tag of the enenmy layer
    [SerializeField] private Vector3 _OverlapBoxOffset;
    [SerializeField] private Vector3 _OverlapBoxHalfSize;
    [SerializeField] private MoveOrbsToPlayer _Orbs;

    [SerializeField] private VisualEffect _ArmorClashParticle;
    [SerializeField] private VisualEffect _ArmorSparkParticle;

    [Header("Fighting Loop")]
    [SerializeField] private int _EnhancedKillHealthRestore;
    [SerializeField] private int _SwordKillHealthRestore;
    [SerializeField] private int _EnhancedKillManaRestore;
    [SerializeField] private int _SwordKillManaRestore;

    [System.NonSerialized] public bool EnhancedAttack;
    [System.NonSerialized] public bool Blocking;
    [System.NonSerialized] public bool IsPlayerAbleToMove = true;

    [SerializeField] private string _AnimationToPlayName;
    public bool CanAttack = false;

    [SerializeField] private SOResources _PlayerResources;
    private PlayerController _PlayController;
    private InputSwitchCombo _InputCombo;
    private Animator _Animator;
    private bool _BackToIdle;
    private bool _ButtonPressed;
    private float _PutSwordIn;

    private bool _CollisionDetected = false;
    private bool _BodyPartDetected;
    private bool _Attack;
    private int _FightModeLayer;
    private float _Block;
    private bool _SwordOut = false;
    private int _TempAnim = -1;
    private int _AnimationToPlay = -2;

    [System.NonSerialized] public bool CanCacelAnimation = false;

    private void OnEnable()
    {
        _InputCombo = GetComponentInParent<InputSwitchCombo>();
        _Animator = GetComponentInParent<Animator>();
        _PlayController = GetComponentInParent<PlayerController>();

    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void Update()
    {
        if (!CanAttack) return;

        InputReader();
        SheathUnsheathe();
        AttackChecker();
        if (_InputCombo.DetectingCollisions)
        {
            OverlapBoxCollisionChecker();
        }
        AnimatorChecker();

        if (CanCacelAnimation)
        {
            AnimationCancel();
            ////print("@@@#You can cancel the animation");
        }
        else
        {
            ////print("@@@#You cant cancel the animation");
        }
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void AnimatorChecker()
    {
        if (_Animator.runtimeAnimatorController.name == "Anim_Arcadia_AS")
        {
            _FightModeLayer = _Animator.GetLayerIndex("Override");
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void InputReader()
    {
        _Attack = Input.GetButtonDown("Attack");
        _Block = Input.GetAxisRaw("Block");
        _PutSwordIn = Input.GetAxisRaw("PutSwordIn");

    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void SheathUnsheathe()
    {
        if (_Attack || _Block > 0)
        {
            if (_Animator.GetBool("FightMode") == false && _SwordOut == false)
            {
                _SwordOut = true;
                _Animator.SetBool("FightMode", true);
                GetComponentInParent<PlayerController>().IsOnFight = true;
                _Animator.SetTrigger("Unsheathe");
                _Attack = false;
                _Block = 0;
            }
        }

        if (_PutSwordIn > 0 && _Animator.GetCurrentAnimatorStateInfo(1).shortNameHash != _AnimationHashes.SheathSword && _SwordOut == true)
        {
            _SwordOut = false;
            _Animator.SetBool("FightMode", false);
            GetComponentInParent<PlayerController>().IsOnFight = false;
            _Animator.SetTrigger("Sheath");
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void AttackChecker()
    {

        int _ActualAnimationHash = _Animator.GetCurrentAnimatorStateInfo(_FightModeLayer).shortNameHash; //getting the animation hash
        if (_AnimationHashes.PlaceHolder != _ActualAnimationHash && _AnimationHashes.Attack_01 != _ActualAnimationHash
        && _AnimationHashes.Attack_02 != _ActualAnimationHash && _AnimationHashes.Attack_03 != _ActualAnimationHash
        && _AnimationHashes.Attack_04 != _ActualAnimationHash)     //If the player is in idle on the attack layer
        {
            //need to add line to cut attack animation
            return;//Do I need to do something here?
        }

        if (_AnimationHashes.PlaceHolder == _ActualAnimationHash && !_Animator.IsInTransition(_FightModeLayer))     //If the player is in idle on the attack layer
        {
            IsPlayerAbleToMove = true;
            _PlayController.Dodging = false;
        }


        //If I am readingInputs and I press the button
        if (_InputCombo.IsInputReading && _Attack && _InputCombo.CanReadInput && _Block == 0)
        {
            _CollisionDetected = false;
            _ButtonPressed = true;
            _InputCombo.CanReadInput = false;
            ////print("Calling attack");
            AttackAssigner();
        }
        //If the player is not attacking and presses block
        if (_AnimationHashes.Attack_01 != _ActualAnimationHash && _AnimationHashes.Attack_02 != _ActualAnimationHash
         && _AnimationHashes.Attack_03 != _ActualAnimationHash && _AnimationHashes.Attack_04 != _ActualAnimationHash)
        {
            Block();
        }

        //If the current animation is an attack animation
        if (_AnimationHashes.Attack_01 == _ActualAnimationHash || _AnimationHashes.Attack_02 == _ActualAnimationHash
        || _AnimationHashes.Attack_03 == _ActualAnimationHash || _AnimationHashes.Attack_04 == _ActualAnimationHash)
        {
            if (!CanCacelAnimation)
            {
                IsPlayerAbleToMove = false;
            }
            else
            {
                IsPlayerAbleToMove = true;
            }
        }
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void AttackAssigner()
    {
        DifferentNumberChecker();
        _TempAnim = _AnimationToPlay;
        switch (_AnimationToPlay)
        {
            case 1:
                _AnimationToPlayName = "Attack_01";
                //_Animator.CrossFade("Attack_01", .25f, 2);
                break;
            case 2:
                _AnimationToPlayName = "Attack_02";
                //_Animator.CrossFade("Attack_02", .25f, 2);
                break;
            case 3:
                _AnimationToPlayName = "Attack_03";
                //_Animator.CrossFade("Attack_03", .25f, 2);
                break;
            case 4:
                _AnimationToPlayName = "Attack_04";
                //_Animator.CrossFade("Attack_04", .25f, 2);
                break;
        }

        if (_Animator.GetCurrentAnimatorStateInfo(2).shortNameHash == _AnimationHashes.PlaceHolder)
        {
            _Animator.CrossFade(_AnimationToPlayName, 0.1f, 2);
            _ButtonPressed = false;
        }
    }

    public void AnimationCancel()
    {
        if (_ButtonPressed)
        {
            AnimationPlayer(_AnimationToPlayName);
            CanCacelAnimation = false;
            //print("@@@#You canceled the animation");
            _ButtonPressed = false;
        }
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void AnimationPlayer(string animationtoplay)
    {
        _Animator.CrossFade(animationtoplay, 0.3f, 2);
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void DifferentNumberChecker()
    {
        _AnimationToPlay = UnityEngine.Random.Range(1, 5);

        if (_AnimationToPlay == _TempAnim)
        {
            DifferentNumberChecker();
        }
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Block()
    {
        if (_Block >= .1f)
        {
            Blocking = true;
            _Animator.SetBool("Block", true);
        }
        else
        {
            Blocking = false;
            _Animator.SetBool("Block", false);
        }
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void StartingAttack()
    {
        _CollisionDetected = false;
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void OverlapBoxCollisionChecker()
    {
        Collider[] hitColliders = new Collider[10];
        int x = Physics.OverlapBoxNonAlloc(transform.position + _OverlapBoxOffset, _OverlapBoxHalfSize, hitColliders, transform.rotation, _EnemyLayer, QueryTriggerInteraction.Collide); //Should have distance

        if (_InputCombo.DetectingCollisions && !_CollisionDetected)
        {
            foreach (var item in hitColliders)
            {
                if (item == null) continue;

                //If I hit an enemy, or one of their weapons
                if (item.gameObject.layer == LayerMask.NameToLayer("Enemy") || item.gameObject.layer == LayerMask.NameToLayer("EliteEnemy")
                || item.gameObject.layer == LayerMask.NameToLayer("ExecutionerBoss") || item.gameObject.layer == LayerMask.NameToLayer("King"))
                {
                    _CollisionDetected = true;
                    CollisionManagerOverlapBox(item.gameObject);
                    EnemyHitAnimationChooser(item.gameObject);
                }
            }
        }

#if UNITY_EDITOR
        DrawBoxAt(_OverlapBoxOffset, _OverlapBoxHalfSize, Color.red);
#endif
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void EnemyHitAnimationChooser(GameObject other)
    {
        if(other.layer == LayerMask.NameToLayer("King"))
        {
            var particles = other.GetComponentInChildren<ParticleSystem>();

            if(particles != null)
                particles.Play();
        }

        var animatorScr = other.GetComponentInParent<Animator>();
        var movementScr = other.GetComponentInParent<MovementAI>();

        if (movementScr != null)
        {
            movementScr.PlayerDetected();
        }
        var randomHit = UnityEngine.Random.Range(0, 2);

        if (other.layer == 17 || other.layer == 27)
        {
            if (other.layer == 17)
            {
                other.GetComponentInChildren<ExecutionerFightingSystem>().WasHit = true;
            }
            randomHit = 0;
        }


        if (randomHit == 0)
        {
            //print("@@@Enemy chest collider");
            animatorScr.CrossFade("ChestHit", .1f, 2);
            return;
        }
        else if (randomHit == 1)
        {
            //print("@@@Enemy left arm collider");
            animatorScr.CrossFade("LeftHit", .1f, 2);
            return;
        }
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void CollisionManagerOverlapBox(GameObject other)
    {
        var myhealth = gameObject.GetComponentInParent<AHealth>();
        var healthScr = other.GetComponent<AHealth>();
        var fightAiScr = other.GetComponentInChildren<FightSystemAI>();

        if (EnhancedAttack == true)
        {
            //Return resources
            if (healthScr.GetCurrentHealth() <= _EnhancedDamage)
            {
                _Orbs.MoveHealthOrbToPlayer(other.transform, _EnhancedKillHealthRestore);
                _Orbs.MoveEssenceOrbToPlayer(other.transform, _EnhancedKillManaRestore);
                _PlayController.SetLockOn(false);
            }
            else
            {
                if (!_PlayController.GetLockOn())
                    _PlayController.LockTo(other.gameObject.transform);
            }



            healthScr.Damage(_EnhancedDamage);

            EnhancedAttack = false;
        }
        else
        {
            if (healthScr.GetCurrentHealth() <= _BaseDamage)
            {
                _Orbs.MoveHealthOrbToPlayer(other.transform, _SwordKillHealthRestore);
                _Orbs.MoveEssenceOrbToPlayer(other.transform, _SwordKillManaRestore);

                _PlayController.SetLockOn(false);
            }
            else
            {
                if (!_PlayController.GetLockOn())
                    _PlayController.LockTo(other.gameObject.transform);
            }

            healthScr.Damage(_BaseDamage);
        }

        if (fightAiScr != null)
            fightAiScr.Engaging = true;

    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void DrawBoxAt(Vector3 offset, Vector3 scale, Color lineColor)
    {
#if UNITY_EDITOR
        //Debug.Log();"Drawing box");
        var from = transform.GetOffsetVector(offset, scale, new Vector3(-1, -1, -1));
        var to = transform.GetOffsetVector(offset, scale, new Vector3(-1, -1, 1));
        Debug.DrawLine(from, to, lineColor);

        from = transform.GetOffsetVector(offset, scale, new Vector3(-1, 1, -1));
        to = transform.GetOffsetVector(offset, scale, new Vector3(-1, 1, 1));
        Debug.DrawLine(from, to, lineColor);

        from = transform.GetOffsetVector(offset, scale, new Vector3(1, -1, -1));
        to = transform.GetOffsetVector(offset, scale, new Vector3(1, -1, 1));
        Debug.DrawLine(from, to, lineColor);

        from = transform.GetOffsetVector(offset, scale, new Vector3(1, 1, -1));
        to = transform.GetOffsetVector(offset, scale, new Vector3(1, 1, 1));
        Debug.DrawLine(from, to, lineColor);
#endif
    }
}

