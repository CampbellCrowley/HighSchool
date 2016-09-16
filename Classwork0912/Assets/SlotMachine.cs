using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlotMachine : MonoBehaviour {

  public Text text1, text2, text3, output;

  public void Start() {
    text1.text = Random.Range(1,10)+"";
    text2.text = Random.Range(1,10)+"";
    text3.text = Random.Range(1,10)+"";
  }
  public void Go() {
    int num1 = Random.Range(1,10);
    int num2 = Random.Range(1,10);
    int num3 = Random.Range(1,10);

    text1.text=num1+"";
    text2.text=num2+"";
    text3.text=num3+"";

    if (num1 == num2 && num2 == num3) {
      output.text = "Triple";
    } else if (num1 == num2 || num2 == num3 || num3 == num1) {
      output.text = "Pair";
    } else {
      output.text = "_";
    }
  }
}
