using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public
class SceneController : MonoBehaviour {
 public
  static string PreviousScene;
 public
  static QuestionData data;
 private
  static bool changeScene = false;
 private
  static string nextScene = "Menu";
 private
  Image overlayImage;
 public
  GameObject overlayPrefab;

 public
  static SceneController Instance;
 public
  void Awake() {
    AudioListener.volume = 1.0f;
    if (Instance == null) {
      DontDestroyOnLoad(gameObject);
      Instance = this;
    } else if (Instance != this) {
      Destroy(gameObject);
    }
  }
 public
  void Start() {
    data = new QuestionData();
    data.processValues();
  }
 public
  void Update() {
    if (overlayImage == null) {
      newOverlay();
    }
    if (changeScene) {
      AudioListener.volume -= 0.01f * Time.deltaTime * 100f;
      overlayImage.color +=
          new Color(0f, 0f, 0f, 0.01f) * Time.deltaTime * 100f;
      if (AudioListener.volume <= 0 && overlayImage.color.a >= 1) {
        changeScene = false;
        gotoScene(nextScene);
      }
    } else {
      if (overlayImage.color.a <= 0)
        overlayImage.color =
            new Color(overlayImage.color.r, overlayImage.color.g,
                      overlayImage.color.b, 0f);
      else
        overlayImage.color -= new Color(0f, 0f, 0f, 0.01f);
    }
  }

 private
  void newOverlay() {
    GameObject overlay = Instantiate(overlayPrefab) as GameObject;
    overlay.transform.SetParent(GameObject.Find("Canvas").transform, true);
    overlay.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    overlayImage = overlay.GetComponent<Image>();
  }
 public
  static void resetGameData() {
    data = new QuestionData();
    data.processValues();
  }
 private
  void gotoScene(string scenename) {
    PreviousScene = SceneManager.GetActiveScene().name;
    SceneManager.LoadScene(scenename);
  }
 public
  static void toInstructions() {
    changeScene = true;
    nextScene = "Help";
  }
 public
  static void toMenu() {
    changeScene = true;
    nextScene = "Menu";
  }
 public
  static void toGame() {
    changeScene = true;
    nextScene = "Game";
  }
 public
  static void toScore() {
    changeScene = true;
    nextScene = "Score";
  }
 public
  static void toPrevious() {
    changeScene = true;
    nextScene = PreviousScene;
  }

}

public class QuestionData {
 private
  const float pi = 3.14f;
 public
  const int TEXT_FIELD = 0, NUMBER_FIELD = 1, SLIDER = 2;

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
      "Where is " + getNum(5, 0) + " on the number line?",
      "What is the derivative of " + getNum(6, 0) + "X^" + getNum(6, 1) +
          "?\n(Answer should be given in the same format as the question)",
      "What is " + getNum(7, 0) + " + " + getNum(7, 1) +
          " if we are moving south at 20 miles per hour when the plane turns " +
          "left around Jupiter and my favourite color is capricorn?",
      "What is " + (getNum(8, 0) * getNum(8, 1)) + " degrees F in C?",
      "What is the slope of a line with the function " + getNum(9, 0) + "Y + " +
          getNum(9, 1) + "X = " + getNum(9, 2) + "?"};

  // Ensure the first value equals the number of Questions
 public
  static float[, ] values = new float[ 10, 5 ];

  // Ensure the size equals the number of Questions
 public
  string[] Answers = new string[10];

 public
  static int[] Types = {NUMBER_FIELD, NUMBER_FIELD, NUMBER_FIELD, NUMBER_FIELD,
                        NUMBER_FIELD, SLIDER,       TEXT_FIELD,   NUMBER_FIELD,
                        NUMBER_FIELD, NUMBER_FIELD};

 private
  static float getNum(int question, int part = 0) {
    if (Types[question] == TEXT_FIELD || Types[question] == NUMBER_FIELD) {
      int number = Random.Range(1, 16);
      values[ question, part ] = number;
      return number;
    } else if (Types[question] == SLIDER) {
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
    // Derivative
    Answers[6] =
        (values[ 6, 0 ] * values[ 6, 1 ]) + "X^" + (values[ 6, 1 ] - 1);
    // Simple addition
    Answers[7] = (values[ 7, 0 ] + values[ 7, 1 ]) + "";
    // Temperature conversion
    Answers[8] = (((values[ 8, 0 ] * values[ 8, 1 ]) - 32f) * 5f / 9f) + "";
    // Slope of line
    Answers[9] = ((values[ 9, 2 ] / values[ 9, 0 ]) /
                  (values[ 9, 2 ] / values[ 9, 1 ])) +
                 "";

    Debug.Log(string.Join("\n", Answers));
    return true;
  }
}
