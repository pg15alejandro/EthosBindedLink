using UnityEngine;

public class AnimationHashGetter : MonoBehaviour
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------
    [SerializeField] private SOAnimationHashes _AnimationHash;

    
    /// <summary>
    /// Unity's Start method. Called before the first frame
    /// </summary>
    private void Start()
    {
        _AnimationHash.Stun                     = Animator.StringToHash("Stun");
        _AnimationHash.WalkAnimationHash        = Animator.StringToHash("Walk");
        _AnimationHash.IdleAnimationHash        = Animator.StringToHash("Idle");
        _AnimationHash.HitAnimationHash         = Animator.StringToHash("Hit");
        _AnimationHash.BackHitAnimationHash     = Animator.StringToHash("Back_Hit");
        _AnimationHash.ChestHitAnimationHash    = Animator.StringToHash("Chest_Hit");
        _AnimationHash.LeftHitAnimationHash     = Animator.StringToHash("Left_Hit");
        _AnimationHash.Attack_01                = Animator.StringToHash("Attack_01");
        _AnimationHash.Attack_02                = Animator.StringToHash("Attack_02");
        _AnimationHash.Attack_03                = Animator.StringToHash("Attack_03");
        _AnimationHash.Attack_04                = Animator.StringToHash("Attack_04");
        _AnimationHash.KnockbackAnimationHash   = Animator.StringToHash("Knockback");
        _AnimationHash.BlockAnimation           = Animator.StringToHash("Block");
        _AnimationHash.RunAnimationHash         = Animator.StringToHash("Run");
        _AnimationHash.FightingIdleHash         = Animator.StringToHash("FightingIdle");
        _AnimationHash.SheathSword             = Animator.StringToHash("SheathSword");
        _AnimationHash.UnsheatheSword           = Animator.StringToHash("UnsheatheSword");

        _AnimationHash.Block                    = Animator.StringToHash("Block");
        _AnimationHash.BlockHit                 = Animator.StringToHash("BlockHit");
        _AnimationHash.FrontHit                 = Animator.StringToHash("FrontHit");
        _AnimationHash.BackHit                  = Animator.StringToHash("BackHit");
        _AnimationHash.HighAttack_L             = Animator.StringToHash("HighAttack_L");
        _AnimationHash.HighAttack_R             = Animator.StringToHash("HighAttack_R");
        _AnimationHash.SpinAttack_R             = Animator.StringToHash("SpinAttack_R");
        _AnimationHash.MidAttack_R              = Animator.StringToHash("MidAttack_R");
        _AnimationHash.StraightAttack           = Animator.StringToHash("StraightAttack");
        _AnimationHash.PlaceHolder              = Animator.StringToHash("PlaceHolder"); 
        _AnimationHash.Dodge                    = Animator.StringToHash("Dodge");
    }
}
