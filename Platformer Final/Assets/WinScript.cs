using UnityEngine;
using System.Collections;

public class WinScript : MonoBehaviour {

  public GUIText text;
	void Start () {
	  text.text = "You collected " + GameData.collectibles + " poptarts of the 9 total!";
	}
  public void Quit() {
    Application.Quit();
  }
}
