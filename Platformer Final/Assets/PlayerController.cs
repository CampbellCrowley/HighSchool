using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
#pragma warning disable 0168

public
class PlayerController : MonoBehaviour {
 public
  static Rigidbody2D rbody;
 public
  float Scale = 0.25f, moveForce = 10, jumpVelocity = 10;
 public
  GameObject MainCamera, background, jumpParticles;
 public
  GUIText text, lives;
 public
  float xCamOffset = 8f, yCamOffset = 1.5f, yCamTolerance = 1f,
        backgroundSpeed = 0.8f, deadTime = 3f;
 public
  GameObject jumpSound, collectSoundObject, deathSound, collectLifeSoundObject;

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
    rbody = gameObject.GetComponent<Rigidbody2D>();
    startPos = transform.position;
    startRot = transform.rotation;
  }

 public
  void Start() { ResetPlayer(); }

 public
  void Update() {
    text.text =
        GameData.collectibles + "/" + GameData.getNeededCollectibles() +
          " Collectibles Collected\n" +
          (10-GameObject.FindGameObjectsWithTag("Projectile").Length) + " Projectiles Remaining";
        // GameData.collectibles + " Collectibles Collected";
    if(GameData.lives > 0) {
      lives.text = "Level " + GameData.GetLevel() + " || " + ((GameData.lives == 1)
          ? "LAST LIFE" : (GameData.lives + " Lives Remaining"));
    } else {
      lives.text = "GAME OVER";
    }
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
    touchingFloor = false;

    string floortag1 = "Floor";
    string floortag2 = "ClimbableWall";
    GameObject[] floor1 = GameObject.FindGameObjectsWithTag(floortag1);
    GameObject[] floor2 = GameObject.FindGameObjectsWithTag(floortag2);
    GameObject[] floors = new GameObject[floor1.Length + floor2.Length];
    floor1.CopyTo(floors, 0);
    floor2.CopyTo(floors, floor1.Length);

    foreach (GameObject each in floors) {
      foreach (Collider2D each2 in each.GetComponents<Collider2D>()) {
        touchingFloor = rbody.IsTouching(each2);
        if (touchingFloor) break;
      }
      if (touchingFloor) break;
    }

    if (transform.position.y < -10) {
      ResetPlayer(true);
    }
    if (!dead) {
      if (jump && touchingFloor) {
        rbody.velocity = new Vector2(rbody.velocity.x, jumpVelocity);
        try{Instantiate(jumpSound);} catch (UnassignedReferenceException e) {}
        try{Instantiate(jumpParticles, transform.position + Vector3.down/2,
                    Quaternion.identity);} catch (UnassignedReferenceException e) {}

        jump = false;
      } else {
        if (touchingFloor && !lastTouchingFloor) {
          try{Instantiate(jumpParticles, transform.position + Vector3.down/2,
                      Quaternion.identity);} catch (UnassignedReferenceException e) {}

        }
        rbody.AddForce(new Vector2(hmovements * moveForce, 0));
      }

      if (rbody.velocity.x * 0.02f + transform.position.x < 0) {
        transform.position = new Vector2(0, transform.position.y);
        rbody.velocity = new Vector2(0, rbody.velocity.y);
      }
    } else {
      GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
      foreach (GameObject each in projectiles) {
        Destroy(each);
      }
      if (Time.timeSinceLevelLoad - DeathTime >= deadTime) {
          if(GameData.lives<=0) {
            GameData.GameOver();
          } else {
            ResetPlayer();
          }
      } else if (newDeath) {
        rbody.velocity = new Vector2(0, 0);
        rbody.AddForceAtPosition(
            new Vector2(-150, 100),
            (Vector2)transform.position + 1.5f * Vector2.up);
        rbody.freezeRotation = false;
        GameData.lives--;
        newDeath = false;
        Instantiate(deathSound);
      }
    }
  }

 public
  void ResetPlayer(bool reload = false) {
    if (reload) {
      GameData.RestartLevel();
      //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
      return;
    }
    transform.localScale = new Vector3(Scale, Scale, Scale);
    transform.position = startPos;
    transform.rotation = startRot;
    rbody.freezeRotation = true;
    dead = false;
    newDeath = true;
    rbody.velocity = new Vector2(0f, 0f);
  }

 public
  void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.CompareTag("Collectible")) {
      Instantiate(collectSoundObject);
      Destroy(other.gameObject);
      GameData.IncrementCollectibles();
    } else if (other.gameObject.CompareTag("Exit")) {
      if(GameData.collectibles >= GameData.getNeededCollectibles())
        GameData.NextLevel();
    } else if (!dead && other.gameObject.CompareTag("Enemy")) {
      dead = true;
      DeathTime = Time.timeSinceLevelLoad;
    } else if (other.gameObject.CompareTag("Health")) {
      Destroy(other.gameObject);
      Instantiate(collectLifeSoundObject);
    }
  }
}
