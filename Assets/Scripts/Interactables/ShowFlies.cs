using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShowFlies : Interactable {
    [SerializeField] private Animator _anim;
    private bool _entered = false;

    public override void Entered (GameObject player) {
        if (!_entered) {
            _entered = true;
            _anim.enabled = true;
            _anim.SetTrigger ("StartAnim");
            FindObjectOfType<CharacterMovement> ().Pause ();
            Invoke ("ResumeGame", 52);
        }
    }
    private void ResumeGame () {
        _anim.enabled = false;
        FindObjectOfType<CharacterMovement> ().Resume ();
    }

    public override void Interact (GameObject player) {

    }
    public override void LeftArea (GameObject player) { }
    protected virtual void OnInteract () {

    }
}