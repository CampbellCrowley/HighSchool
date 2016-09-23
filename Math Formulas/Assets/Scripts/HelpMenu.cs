using UnityEngine;
using System.Collections;

public class HelpMenu : MonoBehaviour {

  public void toMenu() {
    SceneController.toPrevious();
  }
  public void Quit() {
    SceneController.QuitGame();
  }
}
