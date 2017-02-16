using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public
class PlayerController : MonoBehaviour {
 public
  float moveSpeed = 5f;
 public
  float jumpMultiplier = 5f;
 public
  float distance = 5f;
 public
  GameObject Camera;
 public
  bool CameraDamping = false;
 public
  bool rotateWithCamera = false;
 public
  GUIText collectedCounter;
 public
  GUIText lifeCounter;
 public
  GUIText timer;
 public
  float GameTime = 10f;
 private
  float endTime = 0f;
 public
  GUIText debug;
 private
  Rigidbody rbody;
 private
  Animator anim;
 private
  float turn = 0f;
 private
  float forward = 0f;
 private
  float moveAngle = 0f;
 private
  bool isGrounded = false;
 private
  bool isCrouched = false;

  void Awake() { Cursor.visible = false; }

  void Start() {
    anim = GetComponent<Animator>();
    rbody = GetComponent<Rigidbody>();
  }

  void FixedUpdate() {
    float moveHorizontal = Input.GetAxis("Horizontal");
    float moveVertical = Input.GetAxis("Vertical");
    float lookHorizontal = Input.GetAxis("Mouse X");
    float lookVertical = Input.GetAxis("Mouse Y");
    RaycastHit hitinfo;
    Physics.SphereCast(transform.position, 0.1f, Vector3.down, out hitinfo);
    isGrounded = hitinfo.distance < 0.05f;
    isCrouched = Input.GetAxis("Crouch") > 0.5;
    bool jump = Input.GetAxis("Jump") > 0.5 && isGrounded && !isCrouched;
    bool sprint = Input.GetAxis("Sprint") > 0.5 && !isCrouched && isGrounded;

    if (debug != null) {
      debug.text = "Horizontal: " + moveHorizontal + "\nVertical: " +
                   moveVertical + "\nMouse X: " + lookHorizontal +
                   "\nMouseY: " + lookVertical + "\nTime: " + Time.time;
    }

    if (Time.time < 1.5) {
      moveHorizontal = 0;
      moveVertical = 0;
      jump = false;
    }
    if (!(moveHorizontal == 0 && moveVertical == 0 && !jump) && endTime == 0f) {
      endTime = Time.time + GameTime;
    }
    if (collectedCounter != null) {
      collectedCounter.text =
          GameData.collectedCollectibles + " Items";
    }
    if (lifeCounter != null) {
      lifeCounter.text = GameData.health + " Health";
    }
    if (timer != null) {
      float timeRemaining = Mathf.Round((endTime - Time.time) * 10f) / 10f;
      if (endTime == 0f) timeRemaining = GameTime;
      string timeRemaining_ = "Time Remaining: ";
      if (timeRemaining > 0) {
        timeRemaining_ += timeRemaining;
      } else {
        timeRemaining_ += "0.0";
      }
      if (timeRemaining % 1 == 0 && timeRemaining > 0) timeRemaining_ += ".0";
      timer.text = timeRemaining_;
    }

    Vector3 movement =
        moveHorizontal * Vector3.right + moveVertical * Vector3.forward;

    Vector3.ClampMagnitude(movement, 1.0f);
    if (isCrouched) {
      movement *= moveSpeed * 0.5f;
      forward = movement.magnitude / 3.2f;
    } else if(sprint) {
      movement *= moveSpeed * 2.5f;
      forward = movement.magnitude / 5f;
    } else {
      movement *= moveSpeed * 1.0f;
      forward = movement.magnitude / 5f;
    }

    movement += ((jump ? (moveSpeed * jumpMultiplier) : 0.0f) +
                 (rbody.velocity.y - 9.81f * Time.deltaTime)) *
                Vector3.up;
    movement =
        Quaternion.Euler(0, Camera.transform.eulerAngles.y, 0) * movement;

    rbody.velocity = Vector3.Lerp(movement, rbody.velocity, 0.5f);
    if (rotateWithCamera) {
      transform.rotation = Quaternion.Euler(
          transform.eulerAngles.x, transform.eulerAngles.y + lookHorizontal,
          transform.eulerAngles.z);
    } else {
      moveAngle = Mathf.Atan(moveHorizontal / moveVertical) * 180f / Mathf.PI;
      if (moveAngle != moveAngle) {
        moveAngle = 0f;
      }
      if (moveVertical < 0) moveAngle += 180f;
      if (Mathf.Abs(rbody.velocity.x) > 0.01f ||
          Mathf.Abs(rbody.velocity.z) > 0.01f) {
        moveAngle += Camera.transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(
            0f,
            Mathf.LerpAngle(rbody.transform.eulerAngles.y, moveAngle, 0.07f),
            0f);

        rbody.transform.rotation = Quaternion.identity;
        transform.rotation = rotation;
      }
    }

    Vector3 newCameraPos =
        transform.position + Vector3.up * 3f +
        (Vector3.left *
             (Mathf.Sin(Camera.transform.eulerAngles.y / 180f * Mathf.PI) -
              Mathf.Sin(Camera.transform.eulerAngles.y / 180f * Mathf.PI) *
                  Mathf.Sin((-45f + Camera.transform.eulerAngles.x) / 90f *
                            Mathf.PI)) +
         Vector3.back *
             (Mathf.Cos(Camera.transform.eulerAngles.y / 180f * Mathf.PI) -
              Mathf.Cos(Camera.transform.eulerAngles.y / 180f * Mathf.PI) *
                  Mathf.Sin((-45f + Camera.transform.eulerAngles.x) / 90f *
                            Mathf.PI)) +
         Vector3.up *
             Mathf.Sin(Camera.transform.eulerAngles.x / 180f * Mathf.PI)) *
            distance;
    if (CameraDamping) {
      newCameraPos =
          Vector3.Lerp(Camera.transform.position, newCameraPos, 0.15f);
    }
    Camera.transform.position = newCameraPos;
    Camera.transform.rotation =
        Quaternion.Euler(Camera.transform.eulerAngles.x - lookVertical,
                         Camera.transform.eulerAngles.y + lookHorizontal,
                         Camera.transform.eulerAngles.z);
  }
  void OnAnimatorIK() {
    if (Mathf.Abs(rbody.velocity.x) > 0.01f ||
        Mathf.Abs(rbody.velocity.z) > 0.01f) {
      turn = (moveAngle - anim.bodyRotation.eulerAngles.y) / 180f;
      while (turn < -1) turn += 2;
      while (turn > 1) turn -= 2;
    }
    anim.SetFloat("Forward", forward * 1.1f);
    anim.SetFloat("Turn", turn);
    anim.SetBool("OnGround", isGrounded);
    anim.SetBool("Crouch", isCrouched);
  }

  void OnTriggerEnter(Collider other) {
    Debug.Log(other);
    if (other.gameObject.CompareTag("Collectible") &&
        (endTime > Time.time || timer == null)) {
      Destroy(other.gameObject);
      GameData.collectedCollectibles++;
    } else if (other.gameObject.CompareTag("Enemy")) {
      GameData.health--;
      other.gameObject.transform.position += Vector3.up * 5f;
    }
  }
}
