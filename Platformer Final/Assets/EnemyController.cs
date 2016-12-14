using UnityEngine;
using System.Collections;
#pragma warning disable 0168

public class EnemyController : MonoBehaviour {

  public float speed = 1f;
  public float time = 5f;
  public float timeoffset = 2.5f;
  public int hp = 1;
  public GameObject deathSound;
  private Rigidbody2D rbody;
  private Vector2 startPos;
  void Awake() {
    startPos = transform.position;
  }
  void Start() {
    rbody = GetComponent<Rigidbody2D>();
  }
	void FixedUpdate () {
    if(speed==0) {
      GameObject[] Projectiles = GameObject.FindGameObjectsWithTag("Projectile");
      for(int i=0; i<Projectiles.Length; i++) {
        if(Vector2.Distance(Projectiles[i].transform.position, transform.position) < 5) {
          rbody.velocity = new Vector2(0, 3f);
          break;
        }
      }
    } else {
      if(Time.realtimeSinceStartup % (time*2) < time) {
        rbody.position =
          new Vector2(
            startPos.x + (speed * (Time.realtimeSinceStartup % (time*2))),
            rbody.position.y
          );
      } else {
        rbody.position =
          new Vector2(
            startPos.x + ((time*2*speed) - speed *
                  (Time.realtimeSinceStartup % (time*2))),
            rbody.position.y
          );
      }
    }
    try {
      transform.rotation = Quaternion.AngleAxis((transform.position.x-startPos.x) /
                (GetComponent<CircleCollider2D>().radius * Mathf.PI / 180f * transform.localScale.x), -Vector3.forward);
    } catch (MissingComponentException e) {}
	}

  void OnCollisionEnter2D(Collision2D other) {
    if(other.gameObject.CompareTag("Projectile")) {
      Debug.Log("Hit!");
      hp--;
      Destroy(other.gameObject);
      if(hp<=0) {
        Debug.Log("Kill!");
        Destroy(gameObject);
      }
    }
  }
  void OnDestroy() {
    Instantiate(deathSound);
  }
}
