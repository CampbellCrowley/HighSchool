using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
  public int radius;
  public InputField input;
  public Text output;
  public float acceptableMargin = 1.0f;

  public void Start() {
    radius = Random.Range(1,11);
    output.text = "What is the area of a circle with a radius of " + radius;
  }

  public void CheckAnswer() {
    if (float.Parse(input.text) <= radius*radius*3.14f + acceptableMargin
        && float.Parse(input.text) >= radius*radius*3.14f - acceptableMargin) {
      output.text = "Correct!";
    } else {
      output.text = "WRONG! The answer was " + radius*radius*3.14;
    }
  }
}
