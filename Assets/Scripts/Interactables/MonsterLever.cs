using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MonsterLever : Lever {
    [SerializeField] private GameObject _mob;
    [SerializeField] private int _amount;
    private bool _pulled = false;

    protected override void OnInteract () {
        if(_pulled) return;
        for (int i = 0; i < _amount; i++)
            Instantiate (_mob, transform.position + (Vector3) (Random.insideUnitCircle * 2), Quaternion.identity);
    }
}