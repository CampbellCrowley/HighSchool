using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level1Controller : MonoBehaviour {
 public
  Text livesText;
 private
	GameObject gameManager;
 private
	GameData gameDataScript;
 public
  void Start () {
    gameManager = GameObject.Find("GameManager");
    gameDataScript = gameManager.GetComponent<GameData>();
	}
 public
  void Update () {
    livesText.text = "Lives = " + gameDataScript.GetLives();
  }
 public
  void AddLife()
  {
    gameDataScript.IncrementLife();
  }
 public
  void SubtractLife()
  {
    gameDataScript.DecrementLife();
  }
 public
  void Level2()
  {
    SceneManager.LoadScene("Level2");
  }
}
