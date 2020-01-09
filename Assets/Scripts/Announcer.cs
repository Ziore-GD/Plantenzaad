using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Announcer : MonoBehaviour {
    private Text _announceTxt;
    private static Announcer _announcer;
    public static Announcer Instance {
        get {
            if (_announcer == null) _announcer = FindObjectOfType<Announcer> ();
            return _announcer;
        }
    }

    void Awake () {
        _announceTxt = GetComponent<Text> ();
    }    
    private IEnumerator TextVisibility(){
		yield return new WaitForSecondsRealtime (5);
        _announceTxt.text = "";
        yield return null;
    }
    public void Log (string txt) {
        _announceTxt.text = txt;
		StopAllCoroutines ();
		StartCoroutine (TextVisibility ());
    }
}