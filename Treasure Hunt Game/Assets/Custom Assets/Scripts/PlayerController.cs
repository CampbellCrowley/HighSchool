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
    AudioClip CollectibleSound;
   public
    AudioClip Pain;
   public
    AudioClip LevelFail;
   public
    AudioClip[] FootSteps;
  }

 [Header("Movement")]
 public
  float moveSpeed = 5f;
 public
  float jumpMultiplier = 5f;
 public
  float staminaDepletionRate = 0.1f;  // Percent per second
 public
  float staminaRechargeDelay = 3.0f;  // Seconds
 public
  float staminaRechargeMultiplier = 1.5f;
 [Header("Camera")]
 public
  GameObject Camera;
 public
  bool CameraDamping = false;
 public
  bool CameraObjectAvoidance = true;
 public
  bool rotateWithCamera = false;
 public
  float MaxCameraDistance = 3f;
 [Header("OSDs/HUD")]
 public
  GUIText collectedCounter;
 public
  GUIText lifeCounter;
 public
  GUIText timer;
 public
  GUIText stamina;
 public
  float staminaCountBars = 20f;
 public
  GUIText debug;
  [Header("Look and Sound")]
 public
  float footstepSize = 0.5f;
 public
  float footstepSizeCrouched = 0.7f;
 public
  float footstepSizeSprinting = 0.3f;
 public
  Sounds sounds;
 [Header("Misc.")]
 public
  float GameTime = 10f;
 public
  GameObject Ragdoll;

 private
  Rigidbody rbody;
 private
  Animator anim;
 private
  Color startColor;
 private
  float turn = 0f;
 private
  float forward = 0f;
 private
  float moveAngle = 0f;
 private
  float CurrentCameraDistance = 3f;
 private
  bool isGrounded = true;
 private
  bool wasGrounded = true;
 private
  bool isCrouched = false;
 private
  bool isSprinting = false;
 private
  bool isDead = false;
 private
  float endTime = 0f;
 private
  float levelStartTime = 0f;
 private
  float lastGroundedTime = 0f;
 private
  float lastSprintTime = 0f;
 private
  float staminaRemaining = 1.0f;
 private
  float lastJumpSoundTimejump = 0.0f;
 private
  float lastFootstepTime = 0.0f;
 private
  float deathTime = 0.0f;

  void Awake() {
    if (sounds.LandSound != null) sounds.LandSound.LoadAudioData();
    if (sounds.JumpSound != null) sounds.JumpSound.LoadAudioData();
    foreach (AudioClip step in sounds.FootSteps) {
      if (step != null) step.LoadAudioData();
    }
    GameData.showCursor = false;
  }

  void Start() {
    UnDead();

    anim = GetComponent<Animator>();
    rbody = GetComponent<Rigidbody>();

    startColor = RenderSettings.fogColor;

    levelStartTime = Time.time;
    lastGroundedTime = Time.time;
    lastSprintTime = Time.time;
    lastJumpSoundTimejump = Time.time;
    lastFootstepTime = Time.time;
  }

  void Update() {
    if (isDead && Time.realtimeSinceStartup - deathTime >= 5f) {
      if (GameData.health > 0)
        GameData.restartLevel();
      else
        GameData.MainMenu();
    }
  }

  void FixedUpdate() {
    // Inputs
    float moveHorizontal = Input.GetAxis("Horizontal");
    float moveVertical = Input.GetAxis("Vertical");
    float lookHorizontal = Input.GetAxis("Mouse X");
    float lookVertical = Input.GetAxis("Mouse Y");
    RaycastHit hitinfo;
    wasGrounded = isGrounded;
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

    // Prevent movement in first 1.5 seconds of the level.
    if (Time.time - levelStartTime < 1.5 || isDead) {
      moveHorizontal = 0;
      moveVertical = 0;
      lookHorizontal = 0;
      lookVertical = 0;
      jump = false;
    }

    // Stamina
    if (isSprinting) lastSprintTime = Time.time;
    if (Time.time - lastSprintTime >= staminaRechargeDelay) {
      staminaRemaining +=
          staminaDepletionRate * Time.deltaTime * staminaRechargeMultiplier;
      if (staminaRemaining > 1.0) staminaRemaining = 1.0f;
    }
    if (staminaRemaining <= 0) {
      isSprinting = false;
    } else if (isSprinting && (moveHorizontal != 0 || moveVertical != 0)) {
      staminaRemaining -= staminaDepletionRate * Time.deltaTime;
    }

    // Start countdown once player moves.
    if (!(moveHorizontal == 0 && moveVertical == 0 && !jump) && endTime == 0f) {
      endTime = Time.time + GameTime;
    }

    // HUD
    if (collectedCounter != null) {
      collectedCounter.text =
          GameData.collectedCollectibles + " Items";
    }
    if (lifeCounter != null) {
      lifeCounter.text = GameData.health + " Health";
    }
    if(stamina != null) {
      stamina.text = "Stamina: ";
      for (int i = 0; i < (int)(staminaRemaining * staminaCountBars); i++) {
        stamina.text += "|";
      }
      for (int i = (int)(staminaCountBars * staminaRemaining);
           i < staminaCountBars; i++) {
        stamina.text += "!";
      }
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

    // Movement
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

    // Rotation
    if (rotateWithCamera) {
      transform.rotation = Quaternion.Euler(
          transform.eulerAngles.x, transform.eulerAngles.y + lookHorizontal,
          transform.eulerAngles.z);
    } else {
      moveAngle = Mathf.Atan(moveHorizontal / moveVertical) * 180f / Mathf.PI;
      if (!(moveAngle >= 0 || moveAngle < 0)) {
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

    // Camera
    if (CameraObjectAvoidance) {
      RaycastHit hit;
      Physics.Linecast(transform.position + Vector3.up * 2f,
                       Camera.transform.position, out hit,
                       ~LayerMask.GetMask("Enemy"));
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

    // VFX
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

    // Sound
    if (isGrounded && Time.time - lastGroundedTime >= 0.05f &&
        sounds.Player != null) {
      AudioPlayer player = Instantiate(sounds.Player) as AudioPlayer;
      if (sounds.LandSound != null) {
        player.clip = sounds.LandSound;
      } else {
        Destroy(player.gameObject);
      }
    }
    if (jump && Time.time - lastJumpSoundTimejump >= 0.5f &&
        sounds.Player != null) {
      AudioPlayer player = Instantiate(sounds.Player) as AudioPlayer;
      if (sounds.JumpSound != null) {
        player.clip = sounds.JumpSound;
      } else {
        Destroy(player.gameObject);
      }
      lastJumpSoundTimejump = Time.time;
    }
    if (isGrounded && (moveVertical != 0 || moveHorizontal != 0) &&
        sounds.Player != null) {
      if ((isSprinting &&
           Time.time - lastFootstepTime >= footstepSizeSprinting) ||
          (isCrouched &&
           Time.time - lastFootstepTime >= footstepSizeCrouched) ||
          (!isSprinting && !isCrouched &&
           Time.time - lastFootstepTime >= footstepSize)) {
        lastFootstepTime = Time.time;
        AudioPlayer player = Instantiate(sounds.Player) as AudioPlayer;
        AudioClip footstepSound =
            sounds.FootSteps[(int)Random.Range(0, sounds.FootSteps.Length)];
        if (footstepSound != null) {
          player.clip = footstepSound;
          player.volume = 0.3f;
        } else {
          Destroy(player.gameObject);
        }
      }
    }

    if (isGrounded) lastGroundedTime = Time.time;
  }

  void Dead() {
    isDead = true;
    if (GameData.health <= 0) {
      if (sounds.Player != null) {
        AudioPlayer player = Instantiate(sounds.Player) as AudioPlayer;
        if (sounds.LevelFail != null)
          player.clip = sounds.LevelFail;
        else
          Destroy(player.gameObject);
      }
    } else {
      if (sounds.Player != null) {
        AudioPlayer player = Instantiate(sounds.Player) as AudioPlayer;
        if (sounds.Pain != null)
          player.clip = sounds.Pain;
        else
          Destroy(player.gameObject);
      }
    }
    deathTime = Time.realtimeSinceStartup;
    Time.timeScale = 0.3f;
    Time.fixedDeltaTime = 0.02f * Time.timeScale;
    GetComponent<Rigidbody>().isKinematic = true;
    if(Ragdoll != null) {
      Ragdoll.SetActive(true);
      Ragdoll.transform.position = transform.position;
      Ragdoll.transform.rotation = transform.rotation;
      foreach (SkinnedMeshRenderer renderer in
                   GetComponentsInChildren<SkinnedMeshRenderer>()) {
        renderer.enabled = false;
      }
      GetComponent<CapsuleCollider>().enabled = false;
      GetComponent<Animator>().enabled = false;
      // Camera.transform.eulerAngles = new Vector3(85f, 0, 0);
    }
  }
  void UnDead() {
    Time.timeScale = 1.0f;
    Time.fixedDeltaTime = 0.02f * Time.timeScale;
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
      if (sounds.Player != null) {
        AudioPlayer player = Instantiate(sounds.Player) as AudioPlayer;
        if (sounds.CollectibleSound != null)
          player.clip = sounds.CollectibleSound;
        else
          Destroy(player.gameObject);
      }
    } else if (other.gameObject.CompareTag("Enemy")) {
      GameData.health--;
      Dead();
      // other.gameObject.transform.position += Vector3.up * 2f;
    } else if (other.gameObject.CompareTag("Portal")) {
      if(GameData.levelComplete()) {
        GameData.nextLevel();
      }
    }
  }
}
