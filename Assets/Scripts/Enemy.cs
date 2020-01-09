using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Physics2D;

public class Enemy : MonoBehaviour {
    private Rigidbody2D rb;
    private PathWalker _walker;
    private Pathfinding _pather;
    private Animator _anim;
    public Transform _targetPos;
    private bool canAttack = true;
    private Transform checkPlayerDist;
    [SerializeField] private int _currentHealth = 2;
    [SerializeField] private GameObject _fireFly;

    // Start is called before the first frame update
    void Awake () {
        checkPlayerDist = FindObjectOfType<CharacterMovement> ().transform;
        _anim = GetComponent<Animator> ();
        _walker = GetComponent<PathWalker> ();
        _pather = Pathfinding.instance;
        _walker.MoveSpeed = 100;
        _anim.SetFloat ("Speed", 1);
        _walker.StoppingDistance = 1;
        _walker.PathFinishedDelegate = FinishedPath;
        if (_targetPos != null)
            _pather.FindPath (Vector3Int.RoundToInt (transform.position), Vector3Int.RoundToInt (_targetPos.position), out _walker.Path);
    }

    public virtual void FinishedPath () {
        if (_targetPos != null) {
            if (!_pather.FindPath (Vector3Int.RoundToInt (transform.position), Vector3Int.RoundToInt (_targetPos.position + (Vector3) (Random.insideUnitSphere * 2)), out _walker.Path)) {
                FinishedPath ();
            }
        } else {
            if (!_pather.FindPath (Vector3Int.RoundToInt (transform.position), Vector3Int.RoundToInt (transform.position + (Vector3) (Random.insideUnitCircle * 4)), out _walker.Path)) {
                FinishedPath ();
            }
        }
    }

    public void DeltaHealth (int delta) {
        _currentHealth -= delta;
        if (_currentHealth <= 0) {
            if (_fireFly != null) {
                _fireFly.SetActive (true);
                _fireFly.transform.position = transform.position;
            }
            gameObject.SetActive (false);
        }
    }
    void OnTriggerEnter2D (Collider2D other) {
        CharacterHealth h = other.GetComponent<CharacterHealth> ();
        if (h != null) {
            _targetPos = h.transform;
            FinishedPath ();
        }
    }
    void OnTriggerExit2D (Collider2D other) {
        CharacterHealth h = other.GetComponent<CharacterHealth> ();
        if (h != null) {
            _targetPos = null;
        }
    }

    // Update is called once per frame
    void Update () {
        if (Vector3.Distance (transform.position, checkPlayerDist.position) > 10) return;
        
        if (Statics.GetTurn (transform.position.x - _walker.Destination.x)) {
            transform.rotation = Quaternion.Euler (new Vector3 (0, 180, 0));
        } else {
            transform.rotation = new Quaternion (0, 0, 0, 0);
        }
        if (_targetPos == null) return;

        if (Vector3.Distance (transform.position, _targetPos.position) <= 1.5f) {
            Attack ();
        }
    }

    void Attack () {
        if (!canAttack) {
            return;
        }
        StartCoroutine (AttackRoutine ());
    }

    IEnumerator AttackRoutine () {
        canAttack = false;
        _walker.Stop ();
        _anim.SetTrigger ("Attack");
        yield return new WaitForSeconds (1);

        Hit ();
        yield return new WaitForSeconds (0.5f);

        canAttack = true;
        _walker.Stop ();

        yield return null;
    }

    void Hit () {
        Vector3 dir = transform.position - _targetPos.position;
        Collider2D[] col2D = Physics2D.OverlapCircleAll (transform.position + dir, 3f);
        foreach (Collider2D h in col2D) {
            CharacterHealth player = h.transform.GetComponent<CharacterHealth> ();
            if (player != null) {
                player.DeltaHealth (1);
            }
        }

    }
}