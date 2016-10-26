using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour {
 private
  int lives = 3;

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
  void IncrementLife() {
    lives++;
  }
 public
  void DecrementLife() {
    lives--;
  }
 public
  int GetLives() {
    return lives;
  }
}
