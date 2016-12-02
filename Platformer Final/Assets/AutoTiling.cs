using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class AutoTiling : MonoBehaviour {
  public GameObject[] tiles = new GameObject[0];
  public void OnGUI() {
    Event e = Event.current;
    Debug.Log(e.control + " " + e.keyCode);
    if(e.control && e.keyCode == KeyCode.T) {
      Debug.Log("Tiling!");
      for(int i=0; i<tiles.Length; i++) {
        tiles[i].transform.position = tiles[i].transform.localScale.x*Vector2.right*i;
      }
    }
  }
}
