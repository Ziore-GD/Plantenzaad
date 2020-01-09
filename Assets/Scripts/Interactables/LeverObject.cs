using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeverObject : Lever {
    [SerializeField] private GameObject _destroyableObject;

    protected override void OnInteract () {
        Destroy(_destroyableObject);
    }
}