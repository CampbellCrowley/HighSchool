using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
  public static SceneController Instance;
  public void Awake() {
    if (Instance == null) {
      DontDestroyOnLoad(gameObject);
      Instance = this;
      Debug.Log("1");
    } else if (Instance != this) Destroy (gameObject);
  }

  public string PreviousScene;

  public void toInstructions() {
    PreviousScene = SceneManager.GetActiveScene().name;
    SceneManager.LoadScene("Help");
  }
  public void toMenu() {
    PreviousScene = SceneManager.GetActiveScene().name;
    SceneManager.LoadScene("Menu");
  }
  public void toGame() {
    PreviousScene = SceneManager.GetActiveScene().name;
    SceneManager.LoadScene("Game");
  }
}
