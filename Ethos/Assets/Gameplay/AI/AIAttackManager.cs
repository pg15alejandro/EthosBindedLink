//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Linq;
using UnityEngine;

public class AIAttackManager : MonoBehaviour
{
    [SerializeField] private SOEnemyRegister _EnemyList;
    [SerializeField] private SOTransform _Player;
    [SerializeField] private SOAnimationHashes _AnimHashes;

    private Animator _EnemyAnimator;
    private int _EnemyIndex = -1;

    private void Update()
    {
        foreach (var item in _EnemyList.EnemiesInside.ToList())
        {
            var movai = item.GetComponent<MovementAI>();
            if (movai._TimeBetweenEnemySwitch > 0 && !item.GetComponentInChildren<FightSystemAI>().Engaging)
            {
                movai._TimeBetweenEnemySwitch -= Time.deltaTime;  //decreasing the timer
            }
        }


        foreach (var item in _EnemyList.EnemiesAlerted.ToList())
        {
            if (!_EnemyList.EnemiesInside.Contains(item.gameObject) && _EnemyList.EnemiesInside.Count < _EnemyList.MaxiumEnemiesInside)     //makes sure that there is always an enemy inside
            {
                if (_EnemyList.Executioners.Count <= 0)
                {
                    _EnemyList.MoveEnemyFromAlertedToInside(item.gameObject);
                }
            }
        }

        if (_EnemyList.Executioners.Count > 0) _EnemyList.ExecutionerInside(gameObject);

        if (_EnemyList.EnemiesInside.Count <= 0 || _EnemyList.Executioners.Count > 0) return; //if there are enemies around the player

        foreach (var item in _EnemyList.EnemiesInside.ToList())
        {
            var fighai = item.GetComponentInChildren<FightSystemAI>();
            var movai = item.GetComponentInChildren<MovementAI>();
            if (!fighai.IsAttacking && movai._TimeBetweenEnemySwitch <= 0 && fighai.CanSwitch)
            {
                _EnemyList.TryMoveAIFromInsideToAlerted(item);    //moves the enemy back
                movai._TimeBetweenEnemySwitch = movai._MaxTimeBetweenEnemySwitch;   //reset the timer
                item.GetComponentInChildren<FightSystemAI>().AlreadyAttacking = false;
                _EnemyIndex = Random.Range(0, _EnemyList.EnemiesAlerted.Count); //gets another enemy
                if (_EnemyList.EnemiesAlerted.Count > 0)
                {
                    _EnemyList.MoveEnemyFromAlertedToInside(_EnemyList.EnemiesAlerted[_EnemyIndex].gameObject); //moves the enemy in
                }
            }
        }

        foreach (var item in _EnemyList.EnemiesInside.ToList())
        {
            if (Vector3.Distance(item.transform.position, _Player.value.position) < 3)
            {
                if (!item.GetComponentInChildren<FightSystemAI>().AlreadyAttacking)   //getting the distance so they dont attack far
                {
                    _EnemyAnimator = item.GetComponentInChildren<Animator>();
                    if (_EnemyAnimator.GetCurrentAnimatorStateInfo(2).shortNameHash == _AnimHashes.PlaceHolder)   //if its on idle
                    {
                        var fightAi = item.GetComponentInChildren<FightSystemAI>();
                        fightAi.IsAttacking = true;
                        fightAi.AmAttacking = true;  //tell the AI to attack
                        item.GetComponentInChildren<FightSystemAI>().AlreadyAttacking = true;
                    }
                }
            }
        }
    }
}
