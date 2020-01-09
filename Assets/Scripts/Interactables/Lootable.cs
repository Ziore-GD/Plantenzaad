using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lootable : Interactable {
    private bool _holding = false;
    public override void Entered (GameObject player) {
        Announcer.Instance.Log ("Press E to pickup this item.");
    }

    public override void Interact (GameObject player) {
        CharacterInteracter interacter = player.GetComponent<CharacterInteracter> ();
        if (!_holding) {
            interacter.SetHolding (this);
        }
        _holding = !_holding;
    }

    public override void LeftArea (GameObject player) {
        _holding = false;
    }
}