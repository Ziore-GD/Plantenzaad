using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
public class PathWalker : MonoBehaviour {
    private Node _currentNode = null;
    private float _remainingDistance;
    private Rigidbody2D rb;

    public List<Node> Path = new List<Node> ();
    internal float MoveSpeed = 1;
    internal float StoppingDistance;
    internal bool Stopped = false;
    public delegate void PathFinished ();
    public PathFinished PathFinishedDelegate;

    public float RemainingDistance {
        get {
            if (Path.Count == 0 || Path == null)
                return 20;

            return Vector3.Distance (Path[Path.Count - 1].vectorPos, transform.position);
        }
    }
    public Vector3 Destination {
        get {
            if (_currentNode != null)
                return _currentNode.vectorPos;

            return transform.position;
        }
    }

    void Awake () {
        rb = GetComponent<Rigidbody2D> ();
    }

    public void Stop () {
        Stopped = !Stopped;

        if (Stopped) {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            this.enabled = false;
        } else {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            this.enabled = true;
        }
    }
    void Update () {
        // if (_currentNode != null && Path.Count > 0) {
        //     rb.velocity = (_currentNode.vectorPos - transform.position).normalized * MoveSpeed;
        //     _remainingDistance = Vector3.Distance (_currentNode.vectorPos, transform.position);
        //     if (_remainingDistance <= StoppingDistance) {
        //         _currentNode = Path[0];
        //         Path.RemoveAt (0);
        //     }
        // } else if (Path.Count <= 0) {
        //     PathFinishedDelegate ();
        //     rb.velocity = new Vector2 ();
        // } else if (Path.Count > 0) {
        //     _currentNode = Path[0];
        //     Path.RemoveAt (0);
        //     rb.velocity = new Vector2 ();
        // }
        // return;

        if (_currentNode == null) {
            if (Path.Count > 0) {
                _currentNode = Path[0];
                Path.RemoveAt (0);
            } else {
                rb.velocity = new Vector2 ();
                PathFinishedDelegate ();
                return;
            }
        } else if (Path.Count <= 0) PathFinishedDelegate ();

        NativeArray<bool> results = new NativeArray<bool> (1, Allocator.TempJob);
        NativeArray<float2> velo = new NativeArray<float2> (1, Allocator.TempJob);

        MoveWalkers moveWalkers = new MoveWalkers {
            speed = MoveSpeed * Time.fixedDeltaTime,
            stoppingDist = StoppingDistance,
            nextPos = _currentNode.vectorPos,
            mobPos = transform.position,
            velocity = velo,
            newPath = results
        };

        JobHandle jobHandle = moveWalkers.Schedule ();
        JobHandle.ScheduleBatchedJobs();
        jobHandle.Complete ();

        if (results[0]) {
            _currentNode = Path[0];
            Path.RemoveAt (0);
        }
        rb.velocity = velo[0];
        
        results.Dispose ();
        velo.Dispose ();
    }

    [BurstCompile]
    public struct MoveWalkers : IJob {
        public float speed;
        public float stoppingDist;
        public float3 nextPos;
        public float3 mobPos;
        // public PathFinished pathFinished;
        public NativeArray<float2> velocity;
        public NativeArray<bool> newPath;

        public void Execute () {
            velocity[0] = (Vector2) Vector3.Normalize (((float3) nextPos - mobPos)) * speed;
            float remainingDistance = Vector3.Distance (nextPos, mobPos);
            if (remainingDistance <= stoppingDist) {
                newPath[0] = true;
            }
        }
    }
    // public struct MoveWalkers : IJobParallelFor {
    //     int dt;
    //     public float[] speed;
    //     public float[] stoppingDist;
    //     public List<Node> path;
    //     public Node[] node;
    //     public float3[] mobPos;
    //     public float[] count;
    //     public PathFinished[] pathFinished;
    //     public NativeArray<float2> velocity;
    //     public NativeArray<bool> newPath;

    //     public void Execute (int i) {
    //         if (node[i] != null && path.Count > 0) {
    //             velocity[i]  = (Vector2)Vector3.Normalize(((float3)node[i].vectorPos - mobPos[i]))* speed[i]*dt ;
    //             float remainingDistance = Vector3.Distance (node[i].vectorPos, mobPos[i]);
    //             if (remainingDistance <= stoppingDist[i]) {
    //                 newPath[i]= true;
    //                 // node[0] = path[0];
    //                 // path.RemoveAt (0);
    //             }
    //         } else if (path.Count <= 0) {
    //             pathFinished[i] ();
    //             velocity[i]  = new Vector2 ();
    //         } else if (path.Count > 0) {
    //             newPath[i]= true;
    //             // node[0] = path[0];
    //             // path.RemoveAt (0);
    //             velocity[i] = new Vector2 ();
    //         }
    //     }
    // }
}