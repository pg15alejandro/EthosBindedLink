//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Interactables : MonoBehaviour
{
    /// <summary>
    /// Enumerator of the different types of interactables
    /// </summary>
    private enum InteractablesType
    {
        NONE,
        POTION,
        SCROLL,
        LEFT_DOOR,
        DOUBLE_DOOR,
        SWORD,
        GALLOW
    }


    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------
    InteractablesType _InteractableType;
    bool _UseInteractable;
    Animator _Anim;
    private Transform _Model;
    private FightingSystem _FS;
    [SerializeField] private GameObject _DungeonSword;
    [SerializeField] private RuntimeAnimatorController _AnimToSwitch;
    [SerializeField] private MeshRenderer _Sword;
    [SerializeField] private GameplayLogic _gmLogic;
    [SerializeField] private TextBoxEnablersDisablers _Ui;
    [SerializeField] private PlayableDirector _Pb;
    private bool _HasPlayed;
    GameObject _Gallow;

    /// <summary>
    /// Unity's Start method. Called before the first frame
    /// </summary>
    private void Start()
    {
        _Anim = GetComponentInChildren<Animator>();
        _Model = transform.GetChild(0);
        _FS = GetComponentInChildren<FightingSystem>();
    }


    /// <summary>
    /// Unity's Update method. Called once per frame
    /// </summary>
    private void Update()
    {
        Debug.DrawRay(_Model.transform.position, _Model.transform.forward, Color.magenta);
        // if (!_UseInteractable) return;

        // _UseInteractable = false;

        // switch (_InteractableType)
        // {
        //     case InteractablesType.POTION:
        //         PickPotion();
        //         break;

        //     case InteractablesType.SCROLL:
        //         PickScroll();
        //         break;

        //     case InteractablesType.LEFT_DOOR:
        //         OpenLeftDoor();
        //         break;

        //     case InteractablesType.DOUBLE_DOOR:
        //         OpenDoubleDoor();
        //         break;

        //     case InteractablesType.SWORD:
        //         PickSword();
        //         break;

        //     default:
        //         break;
        // }
    }

    private void PickPotion()
    {
        throw new NotImplementedException();
    }

    private void PickScroll()
    {
        throw new NotImplementedException();
    }

    private void PickSword()
    {
        transform.LookAt(new Vector3(_DungeonSword.transform.position.x, transform.position.y, transform.position.z));
        _DungeonSword.SetActive(false);
        _Anim.runtimeAnimatorController = _AnimToSwitch as RuntimeAnimatorController;
        _Sword.enabled = true;
        _gmLogic.HasSword = true;
        _FS.CanAttack = true;
        _Anim.SetBool("FightMode", true);
        GetComponent<PlayerController>().IsOnFight = true;

        AkSoundEngine.SetSwitch("item_type", "sword", this.gameObject);
        AkSoundEngine.PostEvent("Play_Pick_up", this.gameObject);

        _Ui.PickUpSwordBox();
        _InteractableType = InteractablesType.NONE;
    }

    private void PlayCinematic()
    {
        if (!_HasPlayed)
        {
            var interUI = _Gallow.GetComponentInChildren<InteractablesCanvas>();
            if (interUI != null)
                interUI.gameObject.SetActive(false);

            _HasPlayed = true;
            Time.timeScale = 1f;
            _Pb.Play();
        }
    }


    /// <summary>
    /// Checks if the model is looking at the door and opens it
    /// </summary>
    private void OpenLeftDoor()
    {
        RaycastHit hit;
        if (Physics.Raycast(_Model.transform.position, _Model.transform.forward, out hit, 1.5f))
        {
            if (!_gmLogic.CanOpenDoor) return;

            var doorAnim = hit.transform.gameObject.GetComponentInParent<Animator>();
            if (doorAnim == null) return;
            doorAnim.SetTrigger("OpenDoor");

            var interUI = doorAnim.gameObject.GetComponentInChildren<InteractablesCanvas>();
            if (interUI != null)
                interUI.gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// Checks if the model is looking at the door and opens it
    /// </summary>
    private void OpenDoubleDoor()
    {
        RaycastHit hit;
        if (Physics.Raycast(_Model.transform.position, _Model.transform.forward, out hit, 1.5f))
        {

            hit.transform.gameObject.GetComponentInParent<Animator>().SetTrigger("OpenDoor");
        }
    }


    /// <summary>
    /// Called when the collider other enters the trigger
    /// </summary>
    /// <param name="other">The collision data associated with this collision</param>    
    private void OnTriggerEnter(Collider other)
    {
        /*if(other.gameObject.layer != LayerMask.NameToLayer("Interactables")){
            _InteractableType = InteractablesType.NONE;
            return;
        } */

        switch (other.tag)
        {
            case "Potion":
                _InteractableType = InteractablesType.POTION;
                break;

            case "Scroll":
                _InteractableType = InteractablesType.SCROLL;
                break;

            case "LeftDoor":
                _InteractableType = InteractablesType.LEFT_DOOR;
                break;

            case "DoubleDoor":
                _InteractableType = InteractablesType.DOUBLE_DOOR;
                break;

            case "SwordDungeon":
                if (!_gmLogic.HasSword) _InteractableType = InteractablesType.SWORD;
                break;

            case "Gallow":
                    if (_gmLogic.CinematicReady) _InteractableType = InteractablesType.GALLOW;
                    _Gallow = other.gameObject;
                break;

            default:
                //_InteractableType = InteractablesType.NONE;
                break;
        }
    }

    /// <summary>
    /// Called when the collider other has stopped touching the trigger
    /// </summary>

    private void OnTriggerStay(Collider other)
    {
        if (!_UseInteractable) return;
        print("In trigger");

        _UseInteractable = false;

        switch (_InteractableType)
        {
            case InteractablesType.POTION:
                PickPotion();
                break;

            case InteractablesType.SCROLL:
                PickScroll();
                break;

            case InteractablesType.LEFT_DOOR:
                OpenLeftDoor();
                break;

            case InteractablesType.DOUBLE_DOOR:
                OpenDoubleDoor();
                break;

            case InteractablesType.SWORD:
                PickSword();
                break;

            case InteractablesType.GALLOW:
                PlayCinematic();
                break;

            default:
                break;
        }
    }



    /// <param name="other">The collision data associated with this collision</param>    
    private void OnTriggerExit(Collider other)
    {
        /*if(_InteractableType == InteractablesType.SWORD)
            _InteractableType = InteractablesType.NONE;*/
    }


    /// <summary>
    /// Sets the interactable input to true
    /// </summary>
    public void InteractablePressed()
    {
        _UseInteractable = true;
    }
}
