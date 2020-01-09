using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StartGame : MonoBehaviour {
    void Awake () {
        GetComponent<Button> ().onClick.AddListener (() => Play ());
    }

    private void Play () {
        SceneManager.LoadScene (1);
    }
}