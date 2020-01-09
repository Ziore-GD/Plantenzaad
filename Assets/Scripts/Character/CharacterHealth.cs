using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterHealth : MonoBehaviour {
    private int _currentHealth, _maxHealth = 3;
    private bool _realPlayer = false;

    void Awake()
    {
        if(GetComponent<CharacterMovement>() != null){
            _realPlayer = true;
        }
    }
    public void DeltaHealth (int delta) {
        _currentHealth -= delta;
        if (_currentHealth <= 0) {
            Dead ();
        }
    }

    private void Dead () {
        if(!_realPlayer){
            gameObject.SetActive(false);
            return;
        }
        Announcer.Instance.Log ("You have died.....");
        Invoke ("Restart", 3);
    }
    private void Restart () {
        SceneManager.LoadScene (0);
    }
}