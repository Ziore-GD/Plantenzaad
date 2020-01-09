using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (CircleCollider2D))]
public abstract class Interactable : MonoBehaviour {
    void Awake () {
        GetComponent<CircleCollider2D> ().isTrigger = true;
    }
    void OnTriggerEnter2D (Collider2D other) {
        CharacterInteracter interacter = other.GetComponent<CharacterInteracter> ();
        if (interacter != null) {
            interacter.SetInteractable (this);
            Entered(other.gameObject);
        }
    }
    void OnTriggerExit2D (Collider2D other) {
        CharacterInteracter interacter = other.GetComponent<CharacterInteracter> ();
        if (interacter != null) {
            interacter.SetInteractable (null);
            LeftArea (other.gameObject);
        }
    }

    public abstract void Interact (GameObject player);
    public abstract void Entered (GameObject player);
    public abstract void LeftArea (GameObject player);
}