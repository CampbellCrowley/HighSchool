using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

  public Rigidbody2D rbody;
  public GameObject MainCamera;
  public GUIText text;
  public string collectibleFlag = "Collectible";
  public string endPortalFlag = "Portal";
  public string floorFlag = "Floor";
  public bool isTouchingFloor = false;
  public float jumpVelocity = 10f;
  public float moveForce = 10f;

  private float hmovements, vmovements;
  private int collectedItems = 0;

  void Update() {
    hmovements = Input.GetAxis("Horizontal");
    vmovements = Input.GetAxis("Vertical");
    MainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
  }

  void FixedUpdate() {
    foreach(GameObject each in GameObject.FindGameObjectsWithTag(floorFlag)) {
      isTouchingFloor = rbody.IsTouching(each.GetComponent<Collider2D>());
      if(isTouchingFloor) break;
    }

    if(isTouchingFloor && vmovements>0.5)
       rbody.velocity = new Vector2(rbody.velocity.x, jumpVelocity);
    else
       rbody.AddForce(new Vector2(hmovements * moveForce, 0));
  }

  void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.CompareTag(collectibleFlag)) {
      collectedItems++;
      text.text = collectedItems + " Items Collected";
      Destroy(other.gameObject);
    } else if (other.gameObject.CompareTag(endPortalFlag)) {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
  }
}
