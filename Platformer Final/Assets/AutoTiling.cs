using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class AutoTiling : MonoBehaviour {
  public GameObject[] tiles = new GameObject[0];
  public float multiplier = 1f;
  public bool updateList = false;
  public void UpdateList() {
    tiles = GameObject.FindGameObjectsWithTag("FloorTile");
  }
  public void OnGUI() {
    Event e = Event.current;
    if(updateList) {
      updateList=false;
      UpdateList();
    }
    if (e.ToString() != "Repaint" && e.ToString() != "Layout") {
      // Debug.Log("START");
      // Debug.Log("Char " + e.character);
      // Debug.Log("isKey " + e.isKey);
      // Debug.Log("keyCode " + e.keyCode);
      // Debug.Log("Mbutton " + e.button);
      // Debug.Log("E " + e);
      // Debug.Log("END");
      if(e.keyCode == KeyCode.T) {
        Debug.Log("Tiling!");
        for(int i=0; i<tiles.Length; i++) {
          tiles[i].transform.position = Vector2.right*i*multiplier;
        }
      }
    }
  }
}
