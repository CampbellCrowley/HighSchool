using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

  public Rigidbody2D rbody;
  public float moveForce = 10;
  private bool touchingFloor = false;

  void FixedUpdate() {
    float hmovements = Input.GetAxis ("Horizontal");
    float vmovements = Input.GetAxis("Vertical");

    foreach(GameObject each in GameObject.FindGameObjectsWithTag("Floor")) {
      touchingFloor = rbody.IsTouching(each.GetComponent<Collider2D>());
      if(touchingFloor) break;
    }

    if((vmovements > 0.5f || Input.GetButtonDown("Jump")) && touchingFloor) {
      rbody.AddForce(new Vector2(hmovements*moveForce, 20*moveForce));
    } else {
      rbody.AddForce(new Vector2(hmovements*moveForce, 0));
    }
  }
}
