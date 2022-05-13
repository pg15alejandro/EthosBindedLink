using System;
using System.Collections;
using UnityEngine;

public class FightSystemAI : MonoBehaviour
{

    [SerializeField] private int _MaxNumber = 3;
    [SerializeField] private int _MinNumber = 1;
    [SerializeField] private int _Damage;
    [SerializeField] private LayerMask _PlayerLayer;
    [SerializeField] private LayerMask _PlayerSword;
    [SerializeField] private Vector3 _OverlapBoxOffset;
    [SerializeField] private Vector3 _OverlapBoxHalfSize;
    [SerializeField] private AIAttackManager _AttackManager;
    [SerializeField] private SOAnimationHashes _AnimHashes;
    private InputSwitchCombo _InputSw;
    [System.NonSerialized] public bool StartAttack;
    [System.NonSerialized] public int RandomNumber;
    [System.NonSerialized] public int SwordHits;
    [System.NonSerialized] public bool Blocking;
    [System.NonSerialized] public bool AmAttacking;
    [System.NonSerialized] public bool IsAttacking = false;
    [System.NonSerialized] public bool CanSwitch = false;
    [SerializeField] private SOResources _PlayerResources;
    public bool Engaging = false;


    public float swordPercentage => (float)(3 - SwordHits) / 3;
    public event Action<float> OnSwordChanged = delegate { };


    private Animator _Anim;
    private bool _AnimEnded;
    private bool _NotAttack;
    private bool _hasntBeenDetected = true;
    private bool _HitAnimbeenDetected = true;
    private bool _StopDetectingCollisions = false;
    [SerializeField] private float _DotPro;
    [SerializeField] private float _MaxTimeBetweenAttacks;
    [SerializeField] public float TimeBetweenAttacks;
    [System.NonSerialized] public bool AlreadyAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        _Anim = GetComponentInParent<Animator>();
        _InputSw = GetComponentInParent<InputSwitchCombo>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_PlayerResources.CurrentHealth <= 0)
            return;

        int _ActualOverrideAnimation = _Anim.GetCurrentAnimatorStateInfo(2).shortNameHash; //getting the animation hash

        RandomNumber = UnityEngine.Random.Range(_MinNumber, _MaxNumber);    //gets a random number each frame to choose the attack

        if (AmAttacking == true && _NotAttack == false && TimeBetweenAttacks <= 0)
        {
            TimeBetweenAttacks = _MaxTimeBetweenAttacks;
            Attack();  //if the enemy is close enough starts attacking
        }
        TimeBetweenAttacks -= Time.deltaTime;
        if (_Anim.IsInTransition(2))
        {
            _AnimEnded = true;
            Blocking = false;
            _StopDetectingCollisions = false;
            _hasntBeenDetected = true;
            _HitAnimbeenDetected = true;
        }
        if (_Anim.GetCurrentAnimatorStateInfo(2).shortNameHash == _AnimHashes.PlaceHolder)
        {
            IsAttacking = false;
            AmAttacking = false;
            CanSwitch = true;
            AlreadyAttacking = false;
        }
        if (_ActualOverrideAnimation == _AnimHashes.Attack_01 || _ActualOverrideAnimation == _AnimHashes.Attack_02)
        {
            CanSwitch = false;
            if (_Anim.IsInTransition(2))
            {

                _NotAttack = false;
            }
            else
            {
                _NotAttack = true;
            }
        }
        // if (_InputSw.DetectingCollisions)
        // {
        OverlapBoxCollisionChecker();
        //}
    }

    private void Attack()
    {
        switch (RandomNumber)
        {
            case 1:
                _Anim.CrossFade("Attack_01", .1f, 2); //starts highattack animation
                break;
            case 2:
                _Anim.CrossFade("Attack_02", .1f, 2);  //starts midattack animation
                break;
            default:
                break;
        }
    }


    public virtual void OverlapBoxCollisionChecker()
    {
        Collider[] hitColliders = new Collider[10];
        int x = Physics.OverlapBoxNonAlloc(transform.position + _OverlapBoxOffset, _OverlapBoxHalfSize, hitColliders, transform.rotation, _PlayerLayer, QueryTriggerInteraction.Collide); //Should have distance

        if (_InputSw.DetectingCollisions)
        {
            if (!_StopDetectingCollisions)
            {
                for (int i = 0; i < x; i++)
                {
                    _StopDetectingCollisions = true;
                    foreach (var item in hitColliders)
                    {
                        if (item == null) continue;
                        if (item.gameObject.layer == 10 || item.gameObject.layer == 12)
                        {
                            if (_hasntBeenDetected)
                            {
                                _hasntBeenDetected = false;
                                if (item.GetComponent<Animator>().GetCurrentAnimatorStateInfo(2).shortNameHash == _AnimHashes.PlaceHolder)
                                {
                                    CollisionManagerOverlapBox(item.gameObject);
                                    PlayerHitAnimationChooser(item.gameObject);
                                }
                            }
                        }

                    }
                }
            }
        }

        // if (_InputSw.DetectingCollisions)
        // {
#if UNITY_EDITOR
        DrawBoxAt(_OverlapBoxOffset, _OverlapBoxHalfSize, Color.red);
#endif
        //  }

    }

    public void CollisionManagerOverlapBox(GameObject other)
    {
        float _PlayerL = Mathf.Log(_PlayerLayer.value, 2);
        float _PlayerSwordL = Mathf.Log(_PlayerSword.value, 2);

        if (other.transform.gameObject.layer == 10 && gameObject.layer == 13) //checks if the weapon collided with the player
        {
            if (other.transform.gameObject.GetComponentInChildren<FightingSystem>().Blocking)
            {
                AkSoundEngine.PostEvent("Play_Arcadia_Block", this.gameObject);
                other.transform.gameObject.GetComponent<Animator>().CrossFade("PlaceHolder", .1f, 1); //????
                _Anim.CrossFade("Stun", .1f, 2);
            }
            else
            {
                other.transform.GetComponent<PlayerHealth>().Damage(_Damage);
            }
        }

        if (other.transform.gameObject.layer == 10 && gameObject.layer == 14) //checks if the weapon collided with the player
        {
            if (other.transform.gameObject.GetComponentInChildren<FightingSystem>().Blocking)
            {
                AkSoundEngine.PostEvent("Play_Arcadia_Block", this.gameObject);
                other.transform.GetComponent<PlayerHealth>().Damage(_Damage / 2);
                other.transform.GetComponent<Animator>().CrossFade("BlockHit", .1f, 2);
            }
            else
            {
                other.transform.GetComponent<PlayerHealth>().Damage(_Damage);
            }
        }

    }

    private void PlayerHitAnimationChooser(GameObject other)
    {

        if (other.GetComponent<AHealth>()._CurrentHealth <= 0)
            return;
        var animatorScr = other.GetComponentInParent<Animator>();
        var isblocking = other.GetComponentInParent<PlayerController>().Fighting.Blocking;
        var RandomNumber = UnityEngine.Random.Range(0, 2);
        if (isblocking) return;

        if (_PlayerResources.CurrentHealth <= 0)
            return;

        switch (RandomNumber)
        {
            case 0:
                animatorScr.CrossFade("ChestHit", 0);
                break;
            case 1:
                animatorScr.CrossFade("LeftHit", 0);
                break;
        }
    }

    private void DrawBoxAt(Vector3 offset, Vector3 scale, Color lineColor)
    {
#if UNITY_EDITOR
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



//not using


// if (other.transform.gameObject.layer == 12 && gameObject.layer == 13) //normal enemy sword w player sword
// {
//     SwordHits++;
//     switch (SwordHits)
//     {
//         case 1:
//             AkSoundEngine.PostEvent("Play_Arcadia_Parry_1st", this.gameObject);
//             break;

//         case 2:
//             AkSoundEngine.PostEvent("Play_Arcadia_Parry_2nd", this.gameObject);
//             break;

//         case 3:
//             AkSoundEngine.PostEvent("Play_Arcadia_Parry_3rd", this.gameObject);
//             break;

//         default:
//             break;
//     }

//     OnSwordChanged(swordPercentage);
//     if (SwordHits < 3)
//     {
//         _Anim.SetTrigger("Hit");
//     }
//     else
//     {
//         Destroy(gameObject.transform.parent);
//     }
//}
