using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

  public float speed = 1f;
  public float time = 5f;
  public float timeoffset = 2.5f;
  private Rigidbody2D rbody;
  private Vector2 startPos;
  void Awake() {
    startPos = transform.position;
  }
  void Start() {
    rbody = GetComponent<Rigidbody2D>();
  }
	void FixedUpdate () {
    if(Time.realtimeSinceStartup % (time*2) < time) {
      rbody.position = new Vector2(startPos.x + (speed * (Time.realtimeSinceStartup % (time*2))), rbody.position.y);
    } else {
      rbody.position = new Vector2(startPos.x + ((time*2*speed) - speed * (Time.realtimeSinceStartup % (time*2))), rbody.position.y);
    }
	}
}
