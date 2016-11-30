using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour {
  public float speed = 1f;
  public float lifespan = 2f;
  public int AmmoCount = 10;
  private float birthTime;
  void Awake() {
    if(GameObject.FindGameObjectsWithTag("Projectile").Length > AmmoCount) {
      Destroy(gameObject);
    }
  }
  void Start() {
    birthTime = Time.timeSinceLevelLoad;
    GetComponent<Rigidbody2D>().velocity = new Vector2(speed / transform.localScale.x, 0f);
    Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(),
        GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>());
    foreach (GameObject each in GameObject.FindGameObjectsWithTag("Projectile")) {
      Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(),
        each.GetComponent<Collider2D>());
    }
  }
  void Update() {
    if(Time.timeSinceLevelLoad - birthTime > lifespan) {
      Destroy(gameObject);
    }
  }
}
