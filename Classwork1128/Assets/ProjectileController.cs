using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour {
  public float speed = 1f;
  public float lifespan = 2f;
  private float birthTime;
  void Start() {
    birthTime = Time.timeSinceLevelLoad;
    GetComponent<Rigidbody2D>().velocity = new Vector2(speed / transform.localScale.x, 0f);
  }
  void Update() {
    if(Time.timeSinceLevelLoad - birthTime > lifespan) {
      Destroy(gameObject);
    }
  }
}
