using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Reward : Interactable {
    [SerializeField] protected ParticleSystem _effect;
    private bool _taken = false;

    public override void Entered (GameObject player) {
        CharacterInteracter interacter = player.GetComponent<CharacterInteracter> ();
        interacter.SetInteractable (this);

        if (!_taken) {
            Announcer.Instance.Log ("Press E to save your firefly-friend.");
        }
    }

    public override void Interact (GameObject player) {
        CharacterInteracter interacter = player.GetComponent<CharacterInteracter> ();
        if (!_taken) {
            _taken = true;
            _effect.Stop ();
            interacter.GetComponent<CharacterAttack> ().Upgrade ();
            Invoke ("Dissapear", 2);
        }
    }
    private void Dissapear () {
        gameObject.SetActive (false);
    }
    public override void LeftArea (GameObject player) { }
    protected virtual void OnPressure () { }
}