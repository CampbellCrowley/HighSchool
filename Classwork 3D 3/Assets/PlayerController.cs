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

	void Update () {
    Rigidbody rbody = GetComponent<Rigidbody>();
    float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
    float lookHorizontal = Input.GetAxis ("Mouse X");
    float lookVertical = Input.GetAxis ("Mouse Y");
    bool isGrounded = Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                ((m_Capsule.height/2f) - m_Capsule.radius) + advancedSettings.groundCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore);
    bool jump = Input.GetAxis("Jump") > 0.5 && isGrounded;
    
    if(debug != null) {
      debug.text = "Horizontal: " + moveHorizontal
                    + "\nVertical: " + moveVertical
                    + "\nMouse X: " + lookHorizontal
                    + "\nMouseY: " + lookVertical
                    + "\nTime: " + Time.time;
    }

    if(Time.time<2) {
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

		rbody.AddForce (movement);

    Camera.transform.position = ((5f*Camera.transform.position + (transform.position + Vector3.back*distance + Vector3.up*distance))/6f) ;
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
}
