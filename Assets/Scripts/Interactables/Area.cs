using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Area : Interactable {
    [SerializeField] private string _name;
    [SerializeField] private bool _useNameOnly = false;

    public override void Interact (GameObject player) { }

    public override void Entered (GameObject player) {
        if (_useNameOnly) {
            Announcer.Instance.Log (_name);
        } else {
            Announcer.Instance.Log ("You have entered " + _name + " area");
        }
    }
    public override void LeftArea (GameObject player) {
        if (!_useNameOnly) {
            Announcer.Instance.Log ("You have left " + _name + " area");
        }
    }
}