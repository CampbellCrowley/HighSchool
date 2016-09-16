using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
	public InputField userInput1,userInput2;
	public Text displayText, operationText;
	private int ActiveInput;
	private bool lastPressed = false;
	public void Start() {
		displayText.text = "Enter Something";
		operationText.text = "+";
		userInput1.ActivateInputField ();
		ActiveInput = 1;
	}
	public void Update() {
		if (Input.GetButtonDown ("Tab") && !lastPressed) {
			lastPressed = true;
			SwapActive ();
		} else {
			lastPressed = false;
		}
		if (Input.GetButtonDown ("Add")) {
			userInput1.DeactivateInputField ();
			userInput2.DeactivateInputField ();
			Add ();
		}
		if (Input.GetButtonDown ("Subtract")) {
			userInput1.DeactivateInputField ();
			userInput2.DeactivateInputField ();
			Subtract ();
		}
		if (Input.GetButtonDown ("Multiply")) {
			userInput1.DeactivateInputField ();
			userInput2.DeactivateInputField ();
			Multiply ();
		}
		if (Input.GetButtonDown ("Divide")) {
			userInput1.DeactivateInputField ();
			userInput2.DeactivateInputField ();
			Divide ();
		}
	}
	public void SwapActive() {
		if (ActiveInput == 1) {
			userInput2.ActivateInputField ();
			ActiveInput = 2;
		} else {
			userInput1.ActivateInputField ();
			ActiveInput = 1;
		}
	}
	public void OnUserInput(){
		Debug.Log ("User Input: " + userInput1.text + " + " + userInput2.text);
	}
	public void OnUserInputFinish() {
		Debug.Log ("User Finished: " + userInput1.text + " + " + userInput2.text);
		Add ();
	}
	public bool CheckValid() {
		bool one, two;
		float One, Two;
		one = float.TryParse (userInput1.text, out One);
		two = float.TryParse (userInput2.text, out Two);
		if (one && two)
			return true;
		else if (one) {
			displayText.text = "Input 2 is Invalid";
			userInput2.ActivateInputField ();
			ActiveInput = 2;
		} else if (two) {
			displayText.text = "Input 1 is Invalid";
			userInput1.ActivateInputField ();
			ActiveInput = 1;
		} else {
			displayText.text = "Please enter valid numbers";
			SwapActive ();
		}
		return false;
	}
	public void Add() {
		if (CheckValid())
			displayText.text = float.Parse (userInput1.text) + float.Parse (userInput2.text) + "";
		operationText.text = "+";
	}
	public void Subtract() {
		if (CheckValid ())
			displayText.text = float.Parse (userInput1.text) - float.Parse (userInput2.text) + "";
		operationText.text = "-";
	}
	public void Multiply() {
		if (CheckValid())
			displayText.text = float.Parse (userInput1.text) * float.Parse (userInput2.text) + "";
		operationText.text = "x";
	}
	public void Divide() {
		if (CheckValid ())
			displayText.text = float.Parse (userInput1.text) / float.Parse (userInput2.text) + "";
		operationText.text = "/";
	}
}
