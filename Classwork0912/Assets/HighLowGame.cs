using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class HighLowGame : MonoBehaviour {

  private int goal;
  private int tries;
  public int initialTries = 5;
  public InputField input;
  public Text output;

  public void Start() {
    goal = Random.Range(1,101);
    output.text = "Guess a number between 1 and 100!";
    tries=initialTries;
    input.ActivateInputField();
  }

  public void Guess() {
    int guess = int.Parse(input.text);
    if(tries<=-100) {
      toMenu();
    } else if(guess > goal) {
      tries--;
      output.text = "Too High!\n" + tries + " left.";
      if(tries<=0) {
        output.text= "You Lose... The answer was " + goal + "\nPress Button to return to Menu.";
        tries=-100;
      } else {
        input.ActivateInputField();
      }
    } else if(guess < goal) {
      tries--;
      output.text = "Too Low!\n" + tries + " left.";
      if(tries<=0) {
        output.text= "You Lose... The answer was " + goal + "\nPress Button to return to Menu.";
        tries=-100;
      } else {
        input.ActivateInputField();
      }
    } else if (guess == goal) {
      output.text = "Congrats! You've Won!\nPress Button to return to Menu.";
      tries=-100;
    }
  }

  public void toMenu() {
    SceneManager.LoadScene("Menu");
  }
  public void toGame() {
    SceneManager.LoadScene("HighLow");
  }
  public void toInstructions() {
    SceneManager.LoadScene("Help");
  }

}
