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
  GameObject MainCamera;
 public
  GUIText text;
 public
  float xCamOffset = 8f, yCamOffset = 1.5f, yCamTolerance = 1f;
 public
  GameObject exit;
 public
  AudioSource jumpSound;
 public
  GameObject collectSoundObject;
 private
  bool touchingFloor = false, jump = false;
 private
  float hmovements, vmovements;
 private
  Vector2 startPos;

  void Awake() {
    rbody = rbody_;
    text.text = GameData.dirts + " Dirts Collected";
    startPos = transform.position;
  }
  void Start() {
    ResetPlayer();
  }
  void Update() {
    hmovements = Input.GetAxis("Horizontal");
    vmovements = Input.GetAxis("Vertical");
    jump = Input.GetButton("Jump") || vmovements > 0.5;
    if (hmovements < 0) {
      transform.localScale = new Vector3(-Scale, Scale, Scale);
    } else if (hmovements > 0) {
      transform.localScale = new Vector3(Scale, Scale, Scale);
    }
    float camx = transform.position.x + xCamOffset;
    float camy = MainCamera.transform.position.y;
    if (camy < transform.position.y + yCamOffset - yCamTolerance) {
      camy = transform.position.y + yCamOffset - yCamTolerance;
    } else if (camy > transform.position.y + yCamOffset + yCamTolerance) {
      camy = transform.position.y + yCamOffset + yCamTolerance;
    }
    MainCamera.transform.position = new Vector3(camx, camy, -10f);
  }
  void FixedUpdate() {
    foreach (GameObject each in GameObject.FindGameObjectsWithTag("Floor")) {
      touchingFloor = rbody.IsTouching(each.GetComponent<Collider2D>());
      if (touchingFloor) break;
    }

    if (transform.position.y < -10) ResetPlayer(true);

    if (jump && touchingFloor) {
      rbody.velocity = new Vector2(rbody.velocity.x, jumpVelocity);
      jumpSound.Play();
      jump = false;
    } else {
      rbody.AddForce(new Vector2(hmovements * moveForce, 0));
    }

    if (rbody.velocity.x*0.02 < 0) {
      transform.position = new Vector2(0, transform.position.y);
      rbody.velocity = new Vector2(0, rbody.velocity.y);
    }
  }

  void ResetPlayer(bool reload = false) {
    transform.localScale = new Vector3(Scale, Scale, Scale);
    transform.position = startPos;
    rbody.velocity = new Vector2(0f, 0f);
    if(reload) GameData.RestartLevel();
  }

  void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.CompareTag("Collectible")) {
      GameData.dirts++;
      text.text = GameData.dirts + " Dirts Collected of " + GameData.getNeededDirts();
      Instantiate(collectSoundObject);
      Destroy(other.gameObject);
      // if(collectedItems>=neededItems) nextLevel();
    } else if (other.gameObject.CompareTag("Portal")) {
      if(GameData.dirts >= GameData.getNeededDirts()) {
        GameData.NextLevel();
      }
    }
  }
}
