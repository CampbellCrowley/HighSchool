using UnityEngine;
using System.Collections;

public class PortalController : MonoBehaviour {

  public GameObject BluePortal, OrangePortal;
  public GameObject PurplePortal, GreenPortal;
  private Collider2D BlueCollider, OrangeCollider, PurpleCollider, GreenCollider;

  public void Awake() {
     BlueCollider = BluePortal.GetComponent<Collider2D>();
     OrangeCollider = OrangePortal.GetComponent<Collider2D>();
     PurpleCollider = PurplePortal.GetComponent<Collider2D>();
     GreenCollider = GreenPortal.GetComponent<Collider2D>();
  }

  public void OnTriggerEnter2D(Collider2D other) {
    if(other == BlueCollider) {
      gotoOrange();
    } else if(other == OrangeCollider) {
      gotoBlue();
    }
  }

  private void gotoOrange() {
    transform.position = OrangePortal.transform.position;
    // transform.rotation = new Vector2(OrangePortal.transform.rotation.x - BluePortal.transform.rotation.x, transform.rotation.y);
  }

  private void gotoBlue() {
    transform.position = BluePortal.transform.position;
    Debug.Log("Angle: " + Quaternion.Angle(BluePortal.transform.rotation, OrangePortal.transform.rotation));
  }
}
