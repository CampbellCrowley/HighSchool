using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Level2Controller : MonoBehaviour {

 public
  Text livesText;
 private
	GameObject gameManager;
 private
	GameData gameDataScript;

  void Start () {
    gameManager = GameObject.Find("GameManager");
    gameDataScript = gameManager.GetComponent<GameData>();
    livesText.text = "Lives = " + gameDataScript.GetLives();
	}
}
