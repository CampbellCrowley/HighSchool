using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// This class stores all of the data that may be persistent across multiple
// scenes. It also controlls scene switching because different data may need to
// be saved across scenes that other scripts do not have access to.
public class GameData : MonoBehaviour {
  // Current number of collectibles.
 public
  static int dirts = 0;
  // Numbers of lives remaining
 public
  static int lives = 3;
  // Needed number of collected collectibles to continue to next level.
 public
  int[] NeededDirts = {0, 0, 0};
  // If this key is pressed the level will be skipped.
 public
  KeyCode SkipLevel = KeyCode.P;
  // Number of collected collectibles that the level started with already
  // collected.
 private
  int InitialDirts = 0;
  // Instance of this script to ensure there is only one running at any time.
 public
  static GameData Instance;

 public
  void Awake() {
    if (Instance == null) {
      // If this is the first GameData script, ensure it is persistent and
      // solitary.
      DontDestroyOnLoad(gameObject);
      Instance = this;
    } else if (Instance != this) {
      // Destroy this script if there is already a GameData instantiated.
      Destroy(gameObject);
    }
  }

  // Increase the current number of collected collectibles by 1.
 public
  static void IncrementDirts() {
    dirts++;
  }
  // Decrease the current number of collected collectibles by 1.
 public
  static void DecrementDirts() {
    dirts--;
  }
  // Returns the number of currently collected collectibles.
 public
  static int GetLives() {
    return dirts;
  }
  // Returns the needed number of collected collectibles for the current level.
 public
  static int getNeededDirts() {
    return Instance.NeededDirts[SceneManager.GetActiveScene().buildIndex];
  }
  // Reload the current scene and reset collectibles to initial amount at the
  // first time the level was loaded.
 public
  static void RestartLevel() {
    dirts = Instance.InitialDirts;
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
  // Load the next scene based on the build index and set the number of collected
  // collectibles to start the next level with.
 public
  static void NextLevel() {
    Instance.InitialDirts = dirts;
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
  }
 public
  static void Update() {
    // If the key becomes pressed, the current level will be skipped.
    if(Input.GetKeyDown(Instance.SkipLevel)) NextLevel();
  }
}
