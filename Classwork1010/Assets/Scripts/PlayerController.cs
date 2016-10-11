using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour {

  public Rigidbody2D rbody;
  public float Scale = 0.25f;
  public float moveForce = 10;
  public float jumpVelocity = 10;
  public GameObject MainCamera;
  public float xCamOffset = 8f;
  public float yCamOffset = 1.5f;
  public float yCamTolerance = 1f;
  private bool touchingFloor = false;
  private bool jump = false;
  private float hmovements, vmovements;
  private Vector2 startPos;

  void Awake() {
    startPos = transform.position;
  }
  void Start() {
    ResetPlayer();
  }
  void Update() {
    jump = Input.GetButtonDown("Jump");
    hmovements = Input.GetAxis ("Horizontal");
    vmovements = Input.GetAxis("Vertical");
    if(hmovements < 0) {
      transform.localScale = new Vector3(-Scale, Scale, Scale);
    } else if(hmovements > 0) {
      transform.localScale = new Vector3(Scale, Scale, Scale);
    }
    float camx = transform.position.x + xCamOffset;
    float camy = MainCamera.transform.position.y;
    if(camy < transform.position.y + yCamOffset - yCamTolerance) {
      camy = transform.position.y + yCamOffset - yCamTolerance;
    } else if(camy > transform.position.y + yCamOffset + yCamTolerance) {
      camy = transform.position.y + yCamOffset + yCamTolerance;
    }
    MainCamera.transform.position = new Vector3(camx, camy, -10f);
  }
  void FixedUpdate() {
    foreach(GameObject each in GameObject.FindGameObjectsWithTag("Floor")) {
      touchingFloor = rbody.IsTouching(each.GetComponent<Collider2D>());
      if(touchingFloor) break;
    }

    if(transform.position.y < -10)
      ResetPlayer();

    if(jump && touchingFloor) {
      rbody.velocity = new Vector2(rbody.velocity.x, jumpVelocity);
    } else {
      rbody.AddForce(new Vector2(hmovements*moveForce, 0));
    }

    if(rbody.velocity.x > 7) {
      rbody.velocity = new Vector2(7, rbody.velocity.y);
    }
    if(false && rbody.velocity.y > 10) {
      rbody.velocity = new Vector2(rbody.velocity.x, 10);
    }
  }

  void ResetPlayer() {
    transform.localScale = new Vector3(Scale, Scale, Scale);
    // transform.position = new Vector2(-715f, -293f);
    transform.position = startPos;
    rbody.velocity = new Vector2(0f,0f);
  }
}
