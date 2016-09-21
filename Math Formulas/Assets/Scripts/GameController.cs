using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

  public string[] Questions;
  public InputField input;
  public Text question;
  public Text output;

  public void Start() {
    question.text = SceneController.data.Questions[SceneController.data.number];
    output.text = "";
    input.text = SceneController.data.currentInput;
  }
  public void Update() {
    SceneController.data.currentInput = input.text;
  }
  public void toInstructions() {
    SceneController.toInstructions();
  }
  public void checkAnswer() {
    if(input.text.Equals(SceneController.data.Answers[SceneController.data.number])) {
      output.text = "Correct!";
      SceneController.data.correct++;
      SceneController.data.number++;
      question.text = SceneController.data.Questions[SceneController.data.number];
      input.text = "";
    } else {
      output.text = "Incorrect";
      SceneController.data.incorrect++;
      SceneController.data.number++;
      question.text = SceneController.data.Questions[SceneController.data.number];
      input.text = "";
    }
  }
}
