using UnityEngine;
using System.Collections;

public
class PortalController : MonoBehaviour {
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
    BlueCollider = BluePortal.GetComponent<Collider2D>();
    OrangeCollider = OrangePortal.GetComponent<Collider2D>();
    PurpleCollider = PurplePortal.GetComponent<Collider2D>();
    GreenCollider = GreenPortal.GetComponent<Collider2D>();
  }

 public
  void OnTriggerEnter2D(Collider2D other) { trigger(other); }
 public
  void OnTriggerStay2D(Collider2D other) { trigger(other); }
 public
  void OnTriggerExit2D(Collider2D other) {
    if (other == BlueCollider) {
      justTeleportedToBlue = false;
    } else if (other == OrangeCollider) {
      justTeleportedToOrange = false;
    } else if (other == GreenCollider) {
      justTeleportedToGreen = false;
    } else if (other == PurpleCollider) {
      justTeleportedToPurple = false;
    }
  }

 private
  void trigger(Collider2D other) {
    if (other == BlueCollider && !justTeleportedToBlue) {
      gotoOrange();
    } else if (other == OrangeCollider && !justTeleportedToOrange) {
      gotoBlue();
    } else if (other == PurpleCollider && !justTeleportedToPurple) {
      gotoGreen();
    } else if (other == GreenCollider && !justTeleportedToGreen) {
      gotoPurple();
    }
  }
 private
  void gotoOrange() {
    justTeleportedToOrange = true;
    transform.position = OrangePortal.transform.position;
    rotateVelocity(Quaternion.Angle(OrangePortal.transform.rotation,
                                    BluePortal.transform.rotation));
  }

 private
  void gotoBlue() {
    justTeleportedToBlue = true;
    transform.position = BluePortal.transform.position;
    rotateVelocity(Quaternion.Angle(BluePortal.transform.rotation,
                                    OrangePortal.transform.rotation));
  }

 private
  void gotoPurple() {
    justTeleportedToPurple = true;
    transform.position = PurplePortal.transform.position;
    rotateVelocity(Quaternion.Angle(PurplePortal.transform.rotation,
                                    GreenPortal.transform.rotation));
  }

 private
  void gotoGreen() {
    justTeleportedToGreen = true;
    transform.position = GreenPortal.transform.position;
    rotateVelocity(Quaternion.Angle(GreenPortal.transform.rotation,
                                    PurplePortal.transform.rotation));
  }

 private
  void rotateVelocity(float Angle) {
    float mag = PlayerController.rbody.velocity.magnitude;
    float angle = Mathf.Atan(PlayerController.rbody.velocity.y /
                                    PlayerController.rbody.velocity.x);
    angle *= 180f / Mathf.PI;
    if(transform.localScale.x < 0) angle+=180f;
    for(;Angle < 0; Angle+=360f);
    for(;angle < 0; angle+=360f);
    Debug.Log("Angle: " + angle +
              " --> " + (angle+Angle));
    angle += Angle;
    Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    Vector2 direction = rotation * new Vector2(mag, 0f);
    Vector2 velocity = direction;
    PlayerController.rbody.velocity = velocity;
 }
}
