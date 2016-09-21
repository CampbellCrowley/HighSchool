using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

  public static string PreviousScene;
  public static QuestionData data = new QuestionData();

  public SceneController Instance;
  public void Awake() {
    if (Instance == null) {
      DontDestroyOnLoad(gameObject);
      Instance = this;
      data.processValues();
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

public class QuestionData {
  private float pi = 3.14f;
  public int number = 0;
  public int correct = 0;
  public int incorrect = 0;
  public string currentInput = "";
  public string[] Questions = {
      "What is the area of a circle with a radius of " + getNum(0)
                + " using 3.14 as pi?",
      "What is the area of a triangle with a base of " + getNum(1,0)
                + " and a height of " + getNum(1,1) + "?",
      "What is the area of a trapezoid with a bases of " + getNum(2,0) + " and "
                + getNum(2,1) + " and a height of " + getNum(2,2) + "?",
      "What is the area of a sphere with a radius of " + getNum(3)
                + " using 3.14 as pi.",
      "What is the distance between the two poins: (" + getNum(4,0) + ", "
                + getNum(4,1) + "), (" + getNum(4,2) + ", " + getNum(4,3)
                + ")?"
    };
  public static float[,] values = new float[5,5];
  public string[] Answers = new string[5];


  private static float getNum(int question, int part = 0) {
    int number = Random.Range(0,10);
    values[question,part] = number; 
    return number;
  }
  public bool processValues() {
    Answers[0] = values[0,0] * values[0,0] * pi + "";
    Answers[1] = values[1,0] * values[1,1] * 0.5f + "";
    Answers[2] = 0f + "";
    Answers[3] = 4f/3f * pi * values[2,0] * values[2,0] * values[2,0] + "";
    Answers[4] = Mathf.Sqrt(
                  (values[3,0] - values[3,2]) * (values[3,0] - values[3,2]) +
                  (values[3,1] - values[3,3]) * (values[3,1] - values[3,3])
                 ) + "";

    Debug.Log(string.Join("\n", Answers));
    return true;
  }
}
