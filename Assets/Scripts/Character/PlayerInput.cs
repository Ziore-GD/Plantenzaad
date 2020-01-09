using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (CharacterMovement))]
public class PlayerInput : MonoBehaviour {
    private InputMaster _controls;
    private CharacterMovement _movement;
    private CharacterInteracter _interacter;
    private CharacterAttack _attack;
    void Awake () {
        _movement = GetComponent<CharacterMovement> ();
        _interacter = GetComponent<CharacterInteracter> ();
        _attack = GetComponent<CharacterAttack> ();
        _controls = new InputMaster ();

        _controls.Player.Movement.performed += ctx => Move (ctx.ReadValue<Vector2> ());
        _controls.Player.Movement.canceled += ctx => Move (ctx.ReadValue<Vector2> ());
        // _controls.Player.BasicAttack.performed += __ => Attack (__.ReadValue<float> ());
        _controls.Player.Interact.performed += __ => Interact ();
        _controls.Player.Attack.performed += __ => _attack.StartAttack();
        _controls.Player.Attack.canceled += __ => _attack.StopAttack();
        _controls.Player.SpecialAttack.performed += __ => _attack.ShootSpecial();

        // _controls.UserInterface.CharacterStats.performed += __ => StatsPanel.Instance.UIToggle ();
        // _controls.UserInterface.QuestLog.performed += __ => QuestHandler.Instance.UIToggle ();
        // _controls.UserInterface.Backpack.performed += __ => Inventory.Instance.UIToggle ();
    }

    void OnEnable () {
        _controls.Enable ();
    }

    void OnDisable () {
        _controls.Disable ();
    }

    void Move (Vector2 dir) {
        _movement.Move (dir);
    }

    void Interact () {
        _interacter.Interact ();
    }
}