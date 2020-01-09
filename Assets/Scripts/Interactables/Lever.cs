using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lever : Interactable {
    private Animator _anim;
    private bool _switched = false;

    private SpriteRenderer _renderer;
    void Start () {
        _anim = GetComponent<Animator>();
    }
    public override void Entered (GameObject player) {
        Announcer.Instance.Log ("Press E to pull the lever.");
    }

    public override void Interact (GameObject player) {
        _switched = !_switched;
        if (!_switched) {
            _anim.SetTrigger("Disable");
        } else {
            _anim.SetTrigger("Enable");
            OnInteract();
        }
    }
    public override void LeftArea (GameObject player) { }
    protected virtual void OnInteract(){
        
    }
}