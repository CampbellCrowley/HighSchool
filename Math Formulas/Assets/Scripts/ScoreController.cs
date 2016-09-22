using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreController : MonoBehaviour {
  public
   Text score, grade;

  public void Awake() {
    score.text = "E = Error\nI broke something... Sorry!";
    grade.text = "E";
  }
  public void Start() {
     float percent = (float)SceneController.data.correct /
                     ((float)SceneController.data.correct +
                      (float)SceneController.data.incorrect) *
                     100f;

     score.text = "Correct: " + SceneController.data.correct + "\nIncorrect: " +
                  SceneController.data.incorrect + "\nPercent: " + percent +
                  "%";
     if(percent>=90) {
        grade.text = "A";
        grade.color = Color.green;
     } else if (percent >= 80) {
        grade.text = "B";
        grade.color = Color.yellow;
     } else if (percent >= 70) {
        grade.text = "C";
        grade.color = (Color.red + Color.yellow)/2;
     } else if (percent >= 60) {
        grade.text = "D";
        grade.color = Color.red;
     } else if (percent >= 50) {
        grade.text = "F";
        grade.color = Color.magenta;
     } else {
        grade.text = "Epic Fail";
        grade.color = Color.gray;
     }
  }
  public
   void toMenu() {
     SceneController.resetGameData();
     SceneController.toMenu();
   }
}
