using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour {
 public
  static int dirts = 0;
 public int[] NeededDirts = {100, 200, 400};

 public
  static GameData Instance;
 public
  void Awake() {
    if (Instance == null) {
      DontDestroyOnLoad(gameObject);
      Instance = this;
    } else if (Instance != this) {
      Destroy(gameObject);
    }
  }

 public
  static void IncrementDirts() {
    dirts++;
  }
 public
  static void DecrementDirts() {
    dirts--;
  }
 public
  static int GetLives() {
    return dirts;
  }
 public
  static int getNeededDirts() {
    return Instance.NeededDirts[SceneManager.GetActiveScene().buildIndex];
  }
}
