using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

  public Rigidbody2D rbody;
  public float moveForce = 10;

  void FixedUpdate() {
    float hmovements = Input.GetAxis ("Horizontal");
    float vmovements = Input.GetAxis("Vertical");
    if((vmovements > 0.5f || Input.GetButtonDown("Jump")) && touchGround &&
          rbody.IsTouching(
                      GameObject.FindWithTag("Floor").GetComponent<Collider2D>()
                          )) {
      rbody.AddForce(new Vector2(hmovements*moveForce, 20*moveForce));
    } else {
      rbody.AddForce(new Vector2(hmovements*moveForce, 0));
    }
  }
}
