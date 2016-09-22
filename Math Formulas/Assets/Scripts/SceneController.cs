using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public
class SceneController : MonoBehaviour {
 public
  static string PreviousScene;
 public
  static QuestionData data;

 public
  static SceneController Instance;
 public
  void Awake() {
    if (Instance == null) {
      DontDestroyOnLoad(gameObject);
      Instance = this;
      // Debug.Log("1");
    } else if (Instance != this) {
      // Debug.Log("2");
      Destroy(gameObject);
    }
  }
 public
  void Start() {
    data = new QuestionData();
    data.processValues();
  }

 public static void resetGameData() {
    data = new QuestionData();
    data.processValues();
 }
 public
  static void toInstructions() {
    PreviousScene = SceneManager.GetActiveScene().name;
    SceneManager.LoadScene("Help");
  }
 public
  static void toMenu() {
    PreviousScene = SceneManager.GetActiveScene().name;
    SceneManager.LoadScene("Menu");
  }
 public
  static void toGame() {
    PreviousScene = SceneManager.GetActiveScene().name;
    SceneManager.LoadScene("Game");
  }
 public
  static void toScore() {
    PreviousScene = SceneManager.GetActiveScene().name;
    SceneManager.LoadScene("Score");
  }
 public
  static void toPrevious() {
    string next = PreviousScene;
    PreviousScene = SceneManager.GetActiveScene().name;
    SceneManager.LoadScene(next);
    toMenu();
  }

}

public class QuestionData {
 private
  const float pi = 3.14f;
 public
  const int TEXT_FIELD = 0, SLIDER = 1;

 public
  int number = 0;
 public
  int correct = 0;
 public
  int incorrect = 0;
 public
  string currentInput = "";

 public
  string[] Questions = {
      "What is the area of a circle with a radius of " + getNum(0) + "?",
      "What is the area of a triangle with a base of " + getNum(1, 0) +
          " and a height of " + getNum(1, 1) + "?",
      "What is the area of a trapezoid with bases of " + getNum(2, 0) +
          " and " + getNum(2, 1) + " and a height of " + getNum(2, 2) + "?",
      "What is the volume of a sphere with a radius of " + getNum(3) + "?",
      "What is the distance between the two poins: (" + getNum(4, 0) + ", " +
          getNum(4, 1) + ") (" + getNum(4, 2) + ", " + getNum(4, 3) + ")?",
      "Where is " + getNum(5,0) + " on the number line?"};

 // Ensure the first value equals the number of Questions
 public
  static float[, ] values = new float[ 6, 5 ];

 // Ensure the size equals the number of Questions
 public
  string[] Answers = new string[6];

 public
  static int[] Types = {TEXT_FIELD, TEXT_FIELD, TEXT_FIELD,
                        TEXT_FIELD, TEXT_FIELD, SLIDER};

 private
  static float getNum(int question, int part = 0) {
    if (Types[question] == TEXT_FIELD) {
      int number = Random.Range(0, 11);
      values[ question, part ] = number;
      return number;
    } else if(Types[question] == SLIDER) {
      int number = Random.Range(-10, 11);
      values[ question, part ] = number;
      return number;
    } else {
      Debug.LogError("Invalid type of question");
      return 0;
    }
  }

 public
  bool processValues() {
    // Area of Circle
    Answers[0] = (values[ 0, 0 ] * values[ 0, 0 ] * pi) + "";
    // Area of Triangle
    Answers[1] = (values[ 1, 0 ] * values[ 1, 1 ] * 0.5f) + "";
    // Area of Trapezoid
    Answers[2] = ((values[ 2, 0 ] + values[ 2, 1 ]) / 2 * values[ 2, 2 ]) + "";
    // Area of Sphere
    Answers[3] =
        ((4f / 3f) * pi * values[ 3, 0 ] * values[ 3, 0 ] * values[ 3, 0 ]) +
        "";
    // Distance between two points
    Answers[4] = Mathf.Sqrt(((values[ 4, 2 ] - values[ 4, 0 ]) *
                             (values[ 4, 2 ] - values[ 4, 0 ])) +
                            ((values[ 4, 3 ] - values[ 4, 1 ]) *
                             (values[ 4, 3 ] - values[ 4, 1 ]))) +
                 "";
    // Number Line
    Answers[5] = values[ 5, 0 ] + "";

    Debug.Log(string.Join("\n", Answers));
    return true;
  }
}
