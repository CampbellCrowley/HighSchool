using UnityEngine;
using System.Collections;

public
class MovingEnemy : MonoBehaviour {
 public
  bool movingRight = true;
 public
  float movespeed = 3f;

 private
  Rigidbody2D rbody;
 public
  void Awake() { rbody = GetComponent<Rigidbody2D>(); }
 public
  void FixedUpdate() {
    if (rbody.velocity.x == 0) {
      movingRight = !movingRight;
    }
    if (movingRight) {
      transform.localScale = new Vector3(1f, 1f, 1f);
      rbody.velocity = new Vector2(movespeed, rbody.velocity.y);
    } else {
      transform.localScale = new Vector3(-1f, 1f, 1f);
      rbody.velocity = new Vector2(-movespeed, rbody.velocity.y);
    }
  }
 public void OnTriggerEnter2D(Collider2D other) {
  if(!other.gameObject.CompareTag("Collectible")) {
    movingRight = !movingRight;
  }
 }
}
