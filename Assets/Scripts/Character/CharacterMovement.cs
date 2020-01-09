using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour {
    private Animator _anim;
    private Rigidbody2D _rb;
    private Vector2 _turnDir;
    [SerializeField] private float _movementSpeed = 300f;
    private float _addedSpeed = 1;
    private bool _canDodge = true;

    private void Start () {
        _anim = GetComponent<Animator> ();
        _rb = GetComponent<Rigidbody2D> ();
    }
    public void Pause () {
        _movementSpeed = 0;
        SetRBVelocity (new Vector2 (0, 0));
    }
    public void Resume () {
        _movementSpeed = 300;
    }
    public void SetDash (float dur, float power) {
        StartCoroutine (AddSpeed (dur, power));
    }
    public void DoDodge () {
        if (_canDodge) {
            StartCoroutine (AddSpeed (1, 2));
            StartCoroutine (DodgeCD (3));
        }
    }
    public void FixedUpdate () {
        _turnDir = Statics.GetMouseDir (transform.position);
        if (_turnDir.x >= -0.3f && _turnDir.x <= 0.3f)
            return;

        if (Statics.GetTurn (_turnDir.x)) {
            transform.rotation = Quaternion.Euler (new Vector3 (0, 180, 0));
        } else {
            transform.rotation = new Quaternion (0, 0, 0, 0);
        }
    }

    public void Move (Vector2 dir) {
        // Vector2 dir = value.Get<Vector2> ();
        float dt = Time.deltaTime;
        SetRBVelocity ((dir * _movementSpeed * dt) * _addedSpeed);
    }

    private void SetRBVelocity (Vector2 velo) {
        float speed = 0;
        if (velo.x != 0) {
            speed = GetAnimSpeed (velo.x, _turnDir.x);
        } else if (velo.y != 0) {
            speed = GetAnimSpeed (velo.y, _turnDir.x);
        }

        _anim.SetFloat ("Speed", speed);
        _rb.velocity = velo;
    }
    float GetAnimSpeed (float x1, float x2) => (x1 > 0f && x2 < 0) || (x1 < 0 && x2 > 0) ? -x1 : x1;

    private IEnumerator AddSpeed (float duration, float amount) {
        _addedSpeed += amount;
        Vector2 preVal = _rb.velocity;
        SetRBVelocity (_rb.velocity * _addedSpeed);
        yield return new WaitForSeconds (duration);
        SetRBVelocity (preVal);
        _addedSpeed -= amount;
        yield return null;
    }
    private IEnumerator DodgeCD (float cd) {
        _canDodge = false;
        // ActionBar.Instance.SetBasicCD (2, 3);
        _anim.SetTrigger ("Dodge");
        yield return new WaitForSeconds (cd);
        _canDodge = true;
        yield return null;
    }
    internal void Teleport (float dist, float ChannelTime) {
        Invoke ("DoTeleport", ChannelTime);
    }
    internal void DoTeleport () {
        // transform.position += (_dir * _tpDistance);
    }
    internal void OnMovementSpeedChanged (float speed) {
        _movementSpeed = speed;
    }
}