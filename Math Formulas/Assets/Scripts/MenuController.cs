using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour {

  public void toInstructions() {
    SceneController.toInstructions();
  }
  public void toGame() {
    SceneController.toGame();
  }

}
