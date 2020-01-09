using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShowObjPlate : PressurePlate {
    [SerializeField] GameObject _activateObj;
    protected override void OnPressure () {
        _activateObj.SetActive (true);
    }
}