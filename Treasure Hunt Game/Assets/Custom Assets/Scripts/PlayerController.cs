using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public
 class PlayerController : MonoBehaviour {
  [System.Serializable] public class Sounds {
    public
     AudioPlayer Player;
    public
     AudioClip JumpSound;
    public
     AudioClip LandSound;
    public
     AudioClip[] FootSteps;
   }

   public float moveSpeed = 5f;
  public
   float jumpMultiplier = 5f;
  public
   float MaxCameraDistance = 3f;
  private
   float CurrentCameraDistance = 3f;
  public
   GameObject Camera;
  public
   bool CameraDamping = false;
  public
   bool CameraObjectAvoidance = true;
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
  public
   GUIText debug;
  public
   Sounds sounds;

 private
  float endTime = 0f;
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
 private
  bool isSprinting = false;
 private
  Color startColor;
 private
  float lastGroundedTime = 0f;

  void Awake() {
    sounds.LandSound.LoadAudioData();
    sounds.JumpSound.LoadAudioData();
    foreach (AudioClip step in sounds.FootSteps) {
      step.LoadAudioData();
    }
  }

  void Start() {
    Cursor.visible = false;
    anim = GetComponent<Animator>();
    rbody = GetComponent<Rigidbody>();
    startColor = RenderSettings.fogColor;
    lastGroundedTime = Time.time;
  }

  void FixedUpdate() {
    float moveHorizontal = Input.GetAxis("Horizontal");
    float moveVertical = Input.GetAxis("Vertical");
    float lookHorizontal = Input.GetAxis("Mouse X");
    float lookVertical = Input.GetAxis("Mouse Y");
    RaycastHit hitinfo;
    isGrounded = Physics.SphereCast(transform.position + Vector3.up * 0.01f,
                                    0.0f, Vector3.down, out hitinfo, 0.22f);
    isCrouched = Input.GetAxis("Crouch") > 0.5;
    bool jump = Input.GetAxis("Jump") > 0.5 && isGrounded && !isCrouched;
    isSprinting = (Input.GetAxis("Sprint") > 0.5 && !isCrouched) ||
                  (isSprinting && !isGrounded);

    if (debug != null) {
      debug.text = "Horizontal: " + moveHorizontal + "\nVertical: " +
                   moveVertical + "\nMouse X: " + lookHorizontal +
                   "\nMouseY: " + lookVertical + "\nTime: " + Time.time;
    }

    if (Time.time < 1.5) {
      moveHorizontal = 0;
      moveVertical = 0;
      lookHorizontal = 0;
      lookVertical = 0;
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
    } else if(isSprinting) {
      movement *= moveSpeed * 2.5f;
      forward = movement.magnitude / 5f;
    } else {
      movement *= moveSpeed * 1.0f;
      forward = movement.magnitude / 6f;
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

    if (CameraObjectAvoidance) {
      RaycastHit hit;
      Physics.Linecast(transform.position + Vector3.up * 2f,
                       Camera.transform.position, out hit);
      if (hit.transform != Camera.transform && hit.transform != transform &&
          hit.transform != null) {
        CurrentCameraDistance = hit.distance;
      } else {
        CurrentCameraDistance += 1.0f * Time.deltaTime;
        if (CurrentCameraDistance > MaxCameraDistance) {
          CurrentCameraDistance = MaxCameraDistance;
        }
      }
    }

    Vector3 newCameraPos =
        transform.position + Vector3.up * 2f +
        Vector3.ClampMagnitude(
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
                 Mathf.Sin(Camera.transform.eulerAngles.x / 180f * Mathf.PI)),
            1.0f) *
            CurrentCameraDistance;
    if (CameraDamping) {
      newCameraPos =
          Vector3.Lerp(Camera.transform.position, newCameraPos, 0.15f);
    }
    Camera.transform.position = newCameraPos;
    Camera.transform.rotation =
        Quaternion.Euler(Camera.transform.eulerAngles.x - lookVertical,
                         Camera.transform.eulerAngles.y + lookHorizontal, 0);

    if (Camera.transform.eulerAngles.x > 75.0f &&
        Camera.transform.eulerAngles.x < 90.0f) {
      Camera.transform.rotation =
          Quaternion.Euler(75.0f, Camera.transform.eulerAngles.y, 0f);
    } else if (Camera.transform.eulerAngles.x < 360f - 75.0f &&
               Camera.transform.eulerAngles.x > 90.0f) {
      Camera.transform.rotation =
          Quaternion.Euler(-75.0f, Camera.transform.eulerAngles.y, 0f);
    }

    float vignette = 0.0f;
    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    float enemydistance = 100f;
    for (int i = 0; i < enemies.Length; i++) {
      float tempdist =
          (enemies[i].transform.position - transform.position).magnitude;
      if (tempdist < enemydistance) {
        enemydistance = tempdist;
      }
    }
    vignette = 0.45f - (enemydistance / 50f);
    if (vignette >= 0) {
      Camera.GetComponent<VignetteAndChromaticAberration>().intensity =
          vignette;
      RenderSettings.fogStartDistance = 200 * (1 - (vignette / 0.45f));
      RenderSettings.fogEndDistance = 300 * (1 - (vignette / 0.45f));
      RenderSettings.fogColor =
          Color.Lerp(startColor, Color.red, (vignette / 0.45f));
    }

    if (isGrounded && Time.time - lastGroundedTime >= 0.05f) {
      AudioPlayer player = Instantiate(sounds.Player) as AudioPlayer;
      player.clip = sounds.LandSound;
    }

    if (isGrounded) lastGroundedTime = Time.time;
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
    anim.SetFloat("Jump", -9 + (Time.time - lastGroundedTime) * 9f);
    anim.SetFloat("JumpLeg", -1 + (Time.time - lastGroundedTime) * 4f);
    anim.SetBool("OnGround", (Time.time - lastGroundedTime <= 0.05f));
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
    } else if (other.gameObject.CompareTag("Portal")) {
      if(GameData.levelComplete()) {
        GameData.nextLevel();
      }
    }
  }
}
