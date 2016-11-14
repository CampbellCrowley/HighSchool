using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour {

  public void toInstructions() {
    GameData.toInstructions();
  }
  public void toGame() {
    GameData.NextLevel();
  }
  public void toMenu() {
    GameData.GameOver();
  }
  public void Quit() {
    Application.Quit();
  }

}
