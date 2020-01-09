using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PressurePlate : Interactable {
    [SerializeField] protected ParticleSystem _effect;
    protected bool _pressure = false;
    protected Lootable _obj;

    public override void Entered (GameObject player) {
        CharacterInteracter interacter = player.GetComponent<CharacterInteracter> ();
        interacter.SetInteractable (this);
        if (interacter.IsHolding) {
            Announcer.Instance.Log ("Press E to place object.");
        } else {
            Announcer.Instance.Log ("Press E to pickup object.");
        }
    }

    public override void Interact (GameObject player) {
        CharacterInteracter interacter = player.GetComponent<CharacterInteracter> ();
        if (interacter.IsHolding) {
            _pressure = true;
            _obj = interacter.ReleaseOnPlate ();
            _effect.Play ();
            OnPressure ();
        } else {
            if (_obj != null) {
                interacter.SetHolding (_obj);
                _effect.Stop ();
                _pressure = false;
            }
        }
    }
    public override void LeftArea (GameObject player) { }
    protected virtual void OnPressure () { }
}