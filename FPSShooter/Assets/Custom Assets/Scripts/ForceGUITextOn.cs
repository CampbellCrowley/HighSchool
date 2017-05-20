using UnityEngine;
using System.Collections;

public
class ForceGUITextOn : MonoBehaviour {
  public GUIText Text;
  public void LateUpdate() { Text.enabled = true; }
}
