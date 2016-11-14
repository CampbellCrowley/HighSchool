using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public
class PlayerController : MonoBehaviour {
 public
  Rigidbody2D rbody_;
 public
  static Rigidbody2D rbody;
 public
  float Scale = 0.25f, moveForce = 10, jumpVelocity = 10;
 public
  GameObject MainCamera, background, exit, jumpParticles;
 public
  GUIText text, lives;
 public
  float xCamOffset = 8f, yCamOffset = 1.5f, yCamTolerance = 1f,
        backgroundSpeed = 0.8f, deadTime = 3f;
 public
  AudioSource jumpSound;
 public
  GameObject collectSoundObject, deathSound, collectLifeSoundObject;

 private
  bool touchingFloor = false, jump = false, lastTouchingFloor = false,
       dead = false, newDeath = true;
 private
  float hmovements, vmovements;
 private
  Vector2 startPos;
 private
  Quaternion startRot;
 private
  float DeathTime;

 public
  void Awake() {
    rbody = rbody_;
    startPos = transform.position;
    startRot = transform.rotation;
  }

 public
  void Start() { ResetPlayer(); }

 public
  void Update() {
    text.text =
        GameData.dirts + " Dirts Collected of " + GameData.getNeededDirts();
    lives.text = "Level " + GameData.GetLevel() + " (" + ((GameData.lives == 1)
        ? "LAST LIFE)" : (GameData.lives + " Lives Remaining"));

    hmovements = Input.GetAxis("Horizontal");
    vmovements = Input.GetAxis("Vertical");
    jump = Input.GetButton("Jump") || vmovements > 0.5;

    if (!dead) {
      if (hmovements < 0) {
        transform.localScale = new Vector3(-Scale, Scale, Scale);
      } else if (hmovements > 0) {
        transform.localScale = new Vector3(Scale, Scale, Scale);
      }
    }

    float camy = MainCamera.transform.position.y;
    float camw =
        Vector2.Distance(MainCamera.GetComponent<Camera>().ScreenToWorldPoint(
                             new Vector2(Screen.width, 0)),
                         MainCamera.GetComponent<Camera>().ScreenToWorldPoint(
                             new Vector2(0, 0)));
    float camx = transform.position.x + xCamOffset * camw;

    if (camy < transform.position.y + yCamOffset - yCamTolerance) {
      camy = transform.position.y + yCamOffset - yCamTolerance;
    } else if (camy > transform.position.y + yCamOffset + yCamTolerance) {
      camy = transform.position.y + yCamOffset + yCamTolerance;
    }
    if (camx - camw / 2 < 0) {
      camx = camw / 2;
    }
    MainCamera.transform.position = new Vector3(camx, camy, -10f);
    background.transform.position =
        new Vector2((camx + camw / 2) * backgroundSpeed, camy);
  }

 public
  void FixedUpdate() {
    lastTouchingFloor = touchingFloor;
    foreach (GameObject each in GameObject.FindGameObjectsWithTag("Floor")) {
      touchingFloor = rbody.IsTouching(each.GetComponent<Collider2D>());
      if (touchingFloor) break;
    }

    if (transform.position.y < -10) {
      GameData.DecrementLives();
      ResetPlayer(false);
    }
    if (!dead) {
      if (jump && touchingFloor) {
        rbody.velocity = new Vector2(rbody.velocity.x, jumpVelocity);
        jumpSound.Play();
        Instantiate(jumpParticles, transform.position + Vector3.down,
                    Quaternion.identity);
        jump = false;
      } else {
        if (touchingFloor && !lastTouchingFloor) {
          Instantiate(jumpParticles, transform.position + Vector3.down,
                      Quaternion.identity);
        }
        rbody.AddForce(new Vector2(hmovements * moveForce, 0));
      }

      if (rbody.velocity.x * 0.02f + transform.position.x < 0) {
        transform.position = new Vector2(0, transform.position.y);
        rbody.velocity = new Vector2(0, rbody.velocity.y);
      }
    } else {
      if (Time.timeSinceLevelLoad - DeathTime >= deadTime) {
        ResetPlayer();
      } else if (newDeath) {
        rbody.velocity = new Vector2(0, 0);
        rbody.AddForceAtPosition(
            new Vector2(-100, 5),
            (Vector2)transform.position + 1.25f * Vector2.up);
        rbody.freezeRotation = false;
        newDeath = false;
        Instantiate(deathSound);
      }
    }
  }

 public
  void ResetPlayer(bool reload = false) {
    transform.localScale = new Vector3(Scale, Scale, Scale);
    transform.position = startPos;
    transform.rotation = startRot;
    rbody.freezeRotation = true;
    dead = false;
    newDeath = true;
    rbody.velocity = new Vector2(0f, 0f);
    if (reload) GameData.RestartLevel();
  }

 public
  void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.CompareTag("Collectible")) {
      GameData.dirts++;
      Instantiate(collectSoundObject);
      Destroy(other.gameObject);
    } else if (other.gameObject.CompareTag("Exit")) {
      if (GameData.dirts >= GameData.getNeededDirts()) {
        GameData.NextLevel();
      }
    } else if (!dead && other.gameObject.CompareTag("Enemy")) {
      GameData.DecrementLives();
      dead = true;
      DeathTime = Time.timeSinceLevelLoad;
      if (GameData.lives <= 0) {
        GameData.GameOver();
      } else {
        // ResetPlayer();
      }
    } else if (other.gameObject.CompareTag("Health")) {
      Destroy(other.gameObject);
      GameData.IncrementLives();
      Instantiate(collectLifeSoundObject);
    }
  }
}
