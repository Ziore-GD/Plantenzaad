using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    private CharacterMovement _character;
    // private float _depth = -10;

    private Vector3 _add = new Vector3 ();
    [SerializeField] private float _speed = 5;
    private Vector3 _originalPos;
    private float _shakeAmount = .25f;
    private static CameraFollow cam;
    public static CameraFollow Instance {
        get {
            if(cam == null) cam = Camera.main.GetComponent<CameraFollow>();
            return cam;
        }
    }

    private void TryGetCharacter () {
        _character = FindObjectOfType<CharacterMovement> ();
    }

    void Awake () {
        _originalPos = transform.localPosition;
        _originalPos.z = -10;
    }

    private void LateUpdate () {
        if (_character == null) {
            TryGetCharacter ();
            return;
        }
        // Vector3 newPos = _character.transform.position;
        // newPos.z = YPos;
        // transform.position = Vector3.Slerp (transform.position, newPos, Time.fixedDeltaTime * _speed);
        // if (Vector2.Distance (transform.position, newPos) < 0.5f) {
        //     transform.position = newPos;
        // }

        NativeArray<float3> pos = new NativeArray<float3> (1, Allocator.TempJob);
        MoveCam moveCam = new MoveCam {
            speed = _speed * Time.fixedDeltaTime,
            newPos = _character.transform.position + _add,
            pos = transform.position,
            posOut = pos
        };

        JobHandle jobHandle = moveCam.Schedule ();
        JobHandle.ScheduleBatchedJobs ();
        jobHandle.Complete ();
        transform.position = moveCam.posOut[0];
        pos.Dispose ();
    }
    public IEnumerator Shake () {
        _originalPos = transform.localPosition;
        float shakeTime = 0.1f;
        while (shakeTime > 0) {
            shakeTime -= Time.fixedDeltaTime;
            transform.localPosition = _originalPos + (Vector3) UnityEngine.Random.insideUnitCircle * _shakeAmount;
            yield return null;
        }

    }
    public struct MoveCam : IJob {
        public float speed;
        public float3 pos;
        public float3 newPos;
        public NativeArray<float3> posOut;
        public void Execute () {
            newPos.z = -10;
            posOut[0] = Vector3.Slerp (pos, newPos, speed);
            if (Vector3.Distance (posOut[0], newPos) < 0.01f) {
                posOut[0] = newPos;
            }
        }
    }
}