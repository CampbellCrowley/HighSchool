using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour {
  public static GameData Instance;
  public void Awake() {
    if (Instance == null) {
      DontDestroyOnLoad(gameObject);
      Instance = this;
    } else if (Instance != this) {
      Destroy(gameObject);
    }
  }
  public static int collectedCollectibles = 0;
  public static int health = 5;
  public static bool showCursor = false;
  private static int neededCollectibles = 3;

  public
   static bool levelComplete() {
     return collectedCollectibles >= neededCollectibles;
   }
  public static void nextLevel() {
    Debug.Log("Next Level!");
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  }
  public static void Settings() {
    Debug.Log("Settings");
  }
  public static void quit() {
    Debug.Log("Exiting Game");
    Application.Quit();
   }

  public
   void Update() {
     Cursor.visible = showCursor;
     if (Input.GetAxis("Skip") > 0.5f) {
       nextLevel();
     }
   }
 }
