using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInteracter : MonoBehaviour {
    private Interactable _current;
    [SerializeField] private Lootable _holdingObj;

    public bool IsHolding {
        get {
            return _holdingObj != null;
        }
    }

    public void Interact () {
        if (_holdingObj != null && _current.GetComponent<PressurePlate> () == null) {
            _holdingObj.LeftArea (gameObject);
            Release ();
            return;
        }

        if (_current != null) {
            _current.Interact (gameObject);
        }
    }

    public void SetInteractable (Interactable interactable) {
        _current = interactable;
    }
    public void SetHolding (Lootable lootable) {
        _holdingObj = lootable;
        _holdingObj.transform.parent = transform;
    }
    public void Release () {
        _holdingObj.transform.parent = null;
        _holdingObj = null;
    }
    public Lootable ReleaseOnPlate () {
        Lootable obj = _holdingObj;
        _holdingObj.transform.parent = null;
        _holdingObj = null;
        return obj;
    }
}