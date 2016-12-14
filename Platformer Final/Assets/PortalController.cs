using UnityEngine;
using System.Collections;
#pragma warning disable 0168

public
class PortalController : MonoBehaviour {
  public bool DEBUG = false;
 public
  GameObject BluePortal, OrangePortal;
 public
  GameObject PurplePortal, GreenPortal;
 private
  Collider2D BlueCollider, OrangeCollider, PurpleCollider, GreenCollider;
 private
  bool justTeleportedToBlue = false;
 private
  bool justTeleportedToOrange = false;
 private
  bool justTeleportedToGreen = false;
 private
  bool justTeleportedToPurple = false;

 public
  void Awake() {
    if (BluePortal != null)
      BlueCollider = BluePortal.GetComponent<Collider2D>();
    if (OrangePortal != null)
      OrangeCollider = OrangePortal.GetComponent<Collider2D>();
    if (PurplePortal != null)
      PurpleCollider = PurplePortal.GetComponent<Collider2D>();
    if (GreenPortal != null)
      GreenCollider = GreenPortal.GetComponent<Collider2D>();
  }

 public
  void OnTriggerEnter2D(Collider2D other) { trigger(other); }
 public
  void OnTriggerStay2D(Collider2D other) { trigger(other); }
 public
  void OnTriggerExit2D(Collider2D other) {
    if (BlueCollider != null && other == BlueCollider) {
      justTeleportedToBlue = false;
    } else if (OrangeCollider != null && other == OrangeCollider) {
      justTeleportedToOrange = false;
    } else if (GreenCollider != null && other == GreenCollider) {
      justTeleportedToGreen = false;
    } else if (PurpleCollider != null && other == PurpleCollider) {
      justTeleportedToPurple = false;
    }
  }

 private
  void trigger(Collider2D other) {
    if (BlueCollider != null && other == BlueCollider &&
        !justTeleportedToBlue) {
      gotoOrange();
    } else if (OrangeCollider != null && other == OrangeCollider &&
               !justTeleportedToOrange) {
      gotoBlue();
    } else if (PurpleCollider != null && other == PurpleCollider &&
               !justTeleportedToPurple) {
      gotoGreen();
    } else if (GreenCollider != null && other == GreenCollider &&
               !justTeleportedToGreen) {
      gotoPurple();
    }
  }
 private
  void gotoOrange() {
    justTeleportedToOrange = true;
    transform.position = OrangePortal.transform.position;
    rotateVelocity(OrangePortal.transform.rotation.eulerAngles.z -
                   BluePortal.transform.rotation.eulerAngles.z + 180f);
    if(DEBUG)
      PlayerController.rbody.velocity = new Vector2(10f, 30f);
  }

 private
  void gotoBlue() {
    justTeleportedToBlue = true;
    transform.position = BluePortal.transform.position;
    rotateVelocity(BluePortal.transform.rotation.eulerAngles.z -
                   OrangePortal.transform.rotation.eulerAngles.z + 180f);
  }

 private
  void gotoPurple() {
    justTeleportedToPurple = true;
    transform.position = PurplePortal.transform.position;
    rotateVelocity(PurplePortal.transform.rotation.eulerAngles.z -
                   GreenPortal.transform.rotation.eulerAngles.z + 180f);
  }

 private
  void gotoGreen() {
    justTeleportedToGreen = true;
    transform.position = GreenPortal.transform.position;
    rotateVelocity(GreenPortal.transform.rotation.eulerAngles.z -
                   PurplePortal.transform.rotation.eulerAngles.z + 180f);
  }

 private
  void rotateVelocity(float Angle) {
    float mag = PlayerController.rbody.velocity.magnitude;
    float velAngle = Mathf.Atan(PlayerController.rbody.velocity.y /
                                PlayerController.rbody.velocity.x);
    float newAngle = velAngle + (Angle * Mathf.PI / 180f);

    newAngle *= 180f / Mathf.PI;

    Debug.Log("Angle: " + velAngle * 180f / Mathf.PI + " --> " + newAngle);
    Quaternion rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);
    Vector2 direction = rotation * new Vector2(mag, 0f);
    Vector2 velocity = direction;
    PlayerController.rbody.velocity = velocity;
  }
}
