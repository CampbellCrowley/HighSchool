using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HelloButtons : MonoBehaviour {
	[SerializeField] Text helloText;
	public void ChangeText(string text) {
		helloText.text = text;
	}
}
