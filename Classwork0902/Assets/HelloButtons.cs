using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HelloButtons : MonoBehaviour {
	[SerializeField] Text helloText;
	public void ChangeText(string text) {
		helloText.text = text;
	}
	public void NextScene() {
		SceneManager.LoadScene (1);
	}
}
