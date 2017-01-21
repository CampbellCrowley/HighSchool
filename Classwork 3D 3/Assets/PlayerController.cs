using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

  public float moveSpeed = 5f;
  public float jumpMultiplier = 5f;
  public float distance = 5f;
  public GameObject Camera;
  public GUIText timer;
  public float GameTime = 10f;
  private float endTime = 0f;
  public GUIText debug;

  private bool isGrounded = false;

  void Awake() {
    Cursor.visible=false;
  }

	void Update () {
    Rigidbody rbody = GetComponent<Rigidbody>();
    float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
    float lookHorizontal = Input.GetAxis ("Mouse X");
    float lookVertical = Input.GetAxis ("Mouse Y");
    bool jump = Input.GetAxis("Jump") > 0.5 && isGrounded;

    if(debug != null) {
      debug.text = "Horizontal: " + moveHorizontal
                    + "\nVertical: " + moveVertical
                    + "\nMouse X: " + lookHorizontal
                    + "\nMouseY: " + lookVertical
                    + "\nTime: " + Time.time;
    }

    if(Time.time<1.5) {
      moveHorizontal=0;
      moveVertical=0;
      jump=false;
    }
    if(!(moveHorizontal==0 && moveVertical==0 && !jump) && endTime == 0f) {
      endTime = Time.time + GameTime;
    }
    float timeRemaining = Mathf.Round((endTime - Time.time)*10f)/10f;
    if(endTime==0f) timeRemaining = GameTime;
    string timeRemaining_ = "Time Remaining: ";
    if(timeRemaining>0) {
      timeRemaining_+=timeRemaining;
    } else {
      timeRemaining_+="0.0";
    }
    if(timeRemaining%1==0 && timeRemaining>0) timeRemaining_ += ".0";
    timer.text = timeRemaining_;

		Vector3 movement = new Vector3 (moveHorizontal * moveSpeed, jump ? moveSpeed*jumpMultiplier : 0.0f, moveVertical * moveSpeed);
    movement = Quaternion.Euler(0,Camera.transform.eulerAngles.y,0) * movement;

		rbody.AddForce (movement);
    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y+lookHorizontal, transform.eulerAngles.z);

    Camera.transform.position = ((5f*Camera.transform.position + (transform.position + Vector3.left*distance*Mathf.Sin(Camera.transform.eulerAngles.y/180f*Mathf.PI) + Vector3.back*distance*Mathf.Cos(Camera.transform.eulerAngles.y/180f*Mathf.PI) + Vector3.up*distance*Mathf.Cos((Camera.transform.eulerAngles.x-45f)/180f*Mathf.PI)))/6f) ;
    Camera.transform.rotation = Quaternion.Euler(Camera.transform.eulerAngles.x-lookVertical, Camera.transform.eulerAngles.y+lookHorizontal, Camera.transform.eulerAngles.z);

    if (transform.position.y<0 && false) {
      transform.position = new Vector3(1f, 1f, 1f);
      rbody.velocity = new Vector3(0,0,0);
    }
	}

  void OnTriggerEnter(Collider other) {
    Debug.Log(other);
    if(other.gameObject.CompareTag("Collectible") && endTime > Time.time) {
      Destroy(other.gameObject);
      GameData.collectedCollectibles++;
    }
  }
  void OnCollisionEnter(Collision other) {
    isGrounded=true;
  }
  void OnCollisionExit(Collision other) {
    isGrounded=false;
  }
}
