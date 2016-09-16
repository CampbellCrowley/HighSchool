using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
	public Text text;
	public InputField input;
	public void toYards() {
		text.text = float.Parse(input.text) / 3f + "yd";
	}
	public void toInches() {
		text.text = float.Parse(input.text) * 12f + "in";
	}
	public void toMeters() {
		text.text = float.Parse(input.text) * 0.3048f + "m";
	}

}
