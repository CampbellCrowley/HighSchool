using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

  public float moveSpeed = 5f;
  public float jumpMultiplier = 5f;
  public float distance = 5f;
  public GameObject Camera;

	void Update () {
    Rigidbody rbody = GetComponent<Rigidbody>();
    float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
    bool jump = Input.GetAxis("Jump") > 0.5 && (rbody.velocity.y == 0f);

		Vector3 movement = new Vector3 (moveHorizontal * moveSpeed, jump ? moveSpeed*jumpMultiplier : 0.0f, moveVertical * moveSpeed);

		rbody.AddForce (movement);

    Camera.transform.position = ((2f*Camera.transform.position + (transform.position + Vector3.back*distance + Vector3.up*distance))/3f) ;

    if (transform.position.y<0) {
      transform.position = new Vector3(1f, 1f, 1f);
    }
	}
}
