using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CharacterAttack : MonoBehaviour {
    private Animator _anim;
    [SerializeField] private ParticleSystem basicfx;
    [SerializeField] private ParticleSystem specialfx;
    [SerializeField] private GameObject _ded;
    private bool _canAttack = true;
    private bool _canShootSpecial = true;
    private bool _keyPressed = false;
    private float _basicCooldown = 2;
    private float _specialCooldown = 10;
    private int upgradeAmount = 0;

    void Awake () {
        _anim = GetComponent<Animator> ();
        basicfx.GetComponent<ParticleOnHit> ().OnHitDelegate = AttackOnHit;
        specialfx.GetComponent<ParticleOnHit> ().OnHitDelegate = SpecialOnHit;
    }

    public void Upgrade () {
        _basicCooldown -= .5f;
        _specialCooldown -= 2f;
        upgradeAmount++;
        if (upgradeAmount >= 4) {
            Announcer.Instance.Log ("You have saved all of your firefly-friends!");
            Invoke ("ToStartScreen", 4);
        } else {
            Announcer.Instance.Log ("You have saved a firefly-friend!");
        }
    }

    public void ToStartScreen () {
        SceneManager.LoadScene (0);
    }

    public void ShootSpecial () {
        if (!_canShootSpecial) return;
        StartCoroutine (this.SpecialCD ());
        var emitParams = new ParticleSystem.EmitParams ();
        emitParams.velocity = Statics.GetAttackDir (transform.position) * 10;

        emitParams.position = transform.position + new Vector3 (0, 1f);
        specialfx.Emit (emitParams, 1);
    }
    private void Attack () {
        if (!_canAttack) return;
        StartCoroutine (this.BasicCD ());

        var emitParams = new ParticleSystem.EmitParams ();
        emitParams.velocity = Statics.GetAttackDir (transform.position) * 10;

        emitParams.position = transform.position + new Vector3 (0, 1f);
        basicfx.Emit (emitParams, 1);
    }

    private IEnumerator BasicCD () {
        _canAttack = false;
        yield return new WaitForSeconds (_basicCooldown);
        _canAttack = true;
        if (_keyPressed) {
            Attack ();
        }
    }
    private IEnumerator SpecialCD () {
        _canShootSpecial = false;
        yield return new WaitForSeconds (_specialCooldown);
        _canShootSpecial = true;
    }
    public void StartAttack () {
        _anim.SetBool ("Attacking", true);
        _keyPressed = true;
        Attack ();
    }
    public void StopAttack () {
        _anim.SetBool ("Attacking", false);
        _keyPressed = false;
    }
    protected void AttackOnHit (Transform other) {
        Enemy e = other.GetComponent<Enemy> ();
        if (e != null) {
            e.DeltaHealth (1);
        }
    }
    protected void SpecialOnHit (Transform other) {
        Pot p = other.GetComponent<Pot> ();
        if (p != null) {
            Instantiate (_ded, transform.position, Quaternion.identity);
            transform.position = other.position;
            _anim.SetTrigger ("Reincarnate");
        }
    }
}