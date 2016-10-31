using UnityEngine;
using System.Collections;

public class WinScript : MonoBehaviour {

  public GUIText text;
	void Start () {
	  text.text = "You won " + GameData.dirts + " dirts!";
	}
}
