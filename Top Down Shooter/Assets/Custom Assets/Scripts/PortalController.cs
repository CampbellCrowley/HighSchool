using UnityEngine;

public class PortalController : MonoBehaviour {
  public void Update() {
    if(GameData.levelComplete()) {
      GetComponent<Renderer>().enabled = true;
      GetComponent<Collider>().enabled = true;
    } else {
      GetComponent<Renderer>().enabled = false;
      GetComponent<Collider>().enabled = false;
    }
  }
}
