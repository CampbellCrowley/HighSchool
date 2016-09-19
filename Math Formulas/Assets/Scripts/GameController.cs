using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

  public string[] Questions;
  private QuestionData data = new QuestionData();
  public InputField input;
  public Text question;
  public Text output;

  public void Start() {
    data = SceneController.getQuestionData();
  }
  public void Awake() {
  }
  public void Update() {
    data.currenttext = input.text;
  }
  public void toInstructions() {
    SceneController.data = data;
    SceneController.toInstructions();
  }
  public void checkAnswer(){
    input.transform.position = new Vector3(100,100,100);
  }
}
