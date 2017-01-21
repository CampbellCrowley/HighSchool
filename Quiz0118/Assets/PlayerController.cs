using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

  public float moveSpeed = 5f;
  public float jumpMultiplier = 5f;
  public float distance = 5f;
  public GameObject Camera;
  private bool isGrounded = false;

	void Update () {
    Rigidbody rbody = GetComponent<Rigidbody>();
    float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
    bool jump = Input.GetAxis("Jump") > 0.5 && isGrounded;

    if(Time.time<2) {
      moveHorizontal=0;
      moveVertical=0;
      jump=false;
    }

		Vector3 movement = new Vector3 (moveHorizontal * moveSpeed, jump ? moveSpeed*jumpMultiplier : 0.0f, moveVertical * moveSpeed);

		rbody.AddForce (movement);

    Camera.transform.position = ((5f*Camera.transform.position + (transform.position + Vector3.back*distance + Vector3.up*distance))/6f) ;
	}

  void OnCollisionEnter(Collision other) {
    if(other.gameObject.CompareTag("Floor")) {
      isGrounded = true;
    }
  }
  void OnCollisionExit(Collision other) {
    if(other.gameObject.CompareTag("Floor")) {
      isGrounded = false;
    }
  }
}
