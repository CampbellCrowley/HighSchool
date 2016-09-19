using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
  public SceneController Instance;
  public static string PreviousScene;

  public void Awake() {
    if (Instance == null) {
      DontDestroyOnLoad(gameObject);
      Instance = this;
      Debug.Log("1");
    } else if (Instance != this) {
      Debug.Log("2");
      Destroy (gameObject);
    }
  }

  public static void toInstructions() {
    PreviousScene = SceneManager.GetActiveScene().name;
    SceneManager.LoadScene("Help");
  }
  public static void toMenu() {
    PreviousScene = SceneManager.GetActiveScene().name;
    SceneManager.LoadScene("Menu");
  }
  public static void toGame() {
    PreviousScene = SceneManager.GetActiveScene().name;
    SceneManager.LoadScene("Game");
  }
  public static void toPrevious() {
    string next = PreviousScene;
    PreviousScene = SceneManager.GetActiveScene().name;
    SceneManager.LoadScene(next);
    toMenu();
  }

}
