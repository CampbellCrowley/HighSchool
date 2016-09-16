using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

  public Text number1;
  public Text number2;
  public Text number3;
  public void RandomNumber(int num) {
    switch(num) {
      case 1:
        number1.text = Random.Range(1,101)+"";
        break;
      case 2:
        number2.text = Random.Range(5,26)+"";
        break;
      case 3:
        number3.text = Random.Range(111,133)+"";
        break;
      default:
        Debug.Log("Invalid");
        break;
    }
  }
}
