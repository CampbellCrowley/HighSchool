using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour {
 public
  static int collectibles = 0;
 public
  static int lives = 3;
 public
  int[] NeededCollectibles = {100, 200, 400};
 private
  static int InitialCollectibles = 0;
  // If this key is pressed the level will be skipped.
 public
  KeyCode SkipLevel = KeyCode.P;

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
  void Start() { ResetData(); }

 private
  static void ResetData() {
    Debug.Log("Resetting GameData");
    collectibles = 0;
    lives = 3;
    InitialCollectibles = 0;
  }

 public
  static void IncrementCollectibles() {
    collectibles++;
  }
 public
  static void DecrementCollectibles() {
    collectibles--;
  }
 public
  static int GetCollectibles() {
    return collectibles;
  }
 public
  static int getNeededCollectibles() {
    return Instance.NeededCollectibles[SceneManager.GetActiveScene().buildIndex];
  }


  // Reload the current scene and reset collectibles to initial amount at the
  // first time the level was loaded.
 public
  static void RestartLevel() {
    collectibles = InitialCollectibles;
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  // Load the next scene based on the build index and set the number of
  // collected collectibles to start the next level with.
 public
  static void NextLevel() {
    Debug.Log("Next Level");
    InitialCollectibles = collectibles;
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  }

 public
  static void PreviousLevel() {
    Debug.Log("Previous Level");
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
  }

 public
  static void toInstructions() {
    Debug.Log("To Help");
    SceneManager.LoadScene("Help");
  }

 public
  static int GetLevel() {
    return SceneManager.GetActiveScene().buildIndex;
  }

  // Load the first level and reset all data to uninitialized state.
 public
  static void GameOver() {
    Debug.Log("Game Over");
    ResetData();
    SceneManager.LoadScene(0);
  }

 public
  void Update() {
    // If the key becomes pressed, the current level will be skipped.
    if (Input.GetKeyDown(Instance.SkipLevel)) NextLevel();
  }
}
