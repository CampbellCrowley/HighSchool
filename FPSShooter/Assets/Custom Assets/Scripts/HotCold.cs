using UnityEngine;
using System.Collections;

public
class HotCold : MonoBehaviour {
 public
  GameObject target;
 public
  GameObject player;
 public
  GUIText signal;

 private
  float lastDistance = 0f;
 private
  float time_still = 0f;

 public
  void Start() {
    if (target == null) {
      target = GameObject.FindWithTag("Target");
    }
    if(player == null) {
      player = GameObject.FindWithTag("Player");
    }
    if (signal == null) {
      GUIText[] texts = FindObjectsOfType<GUIText>();
      foreach (GUIText t in texts) {
        if (t.name == "HotCold") {
          signal = t;
          break;
        }
      }
    } else {
      signal = Instantiate(signal);
    }
  }

 public
  void FixedUpdate() {
    if (target == null) {
      target = GameObject.FindWithTag("Target");
      if (signal != null) signal.text = "";
      return;
    }
    if (player == null) {
      player = GameObject.FindWithTag("Player");
      if (signal != null) signal.text = "";
      return;
    }
    if (signal == null) {
      GUIText[] texts = FindObjectsOfType<GUIText>();
      foreach (GUIText t in texts) {
        if (t.name == "HotCold") {
          signal = t;
          break;
        }
      }
      return;
    }
    if(GameData.isPaused) {
      signal.text = "";
      return;
    }
    float distance =
        Vector3.Distance(target.transform.position, player.transform.position);
    if (Mathf.Abs(distance - lastDistance) < 0.1) {
      if (time_still > 1f) {
        signal.color = Color.black;
        signal.text = "O";
      }
      time_still += Time.deltaTime;
      return;
    } else if (distance < lastDistance) {
      signal.color = Color.red;
      signal.text = "Hotter";
    } else if (distance > lastDistance) {
      signal.color = Color.blue;
      signal.text = "Colder";
    }
    time_still = 0;
    lastDistance = distance;
  }
}
