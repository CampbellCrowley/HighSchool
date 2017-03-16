using UnityEngine;

public
class EnemyController : MonoBehaviour {
 public
  Projectile projectilePlaceholder;
 public
  float projectile_speed;
 public
  int health = 5;
 private Vector3 player;

 private
  Vector3 startPos;
 public
  void Start() {
    startPos = transform.position;
    player = GameObject.Find("Ethan").transform.position;
    Debug.Log(player);
  }
 public
  void kill() { Destroy(gameObject); }
 public
  void Update() {
    // transform.position = Vector3.Lerp(startPos, startPos + Vector3.forward * 3,
    //                                   Mathf.Abs(Time.time % 2 - 1));
    // if (health == 0) {
    //   GetComponent<MeshRenderer>().material.color = Color.red;
    // } else if (health == 1) {
    //   GetComponent<MeshRenderer>().material.color =
    //       (Color.red + Color.yellow) / 2;
    // } else if (health == 2) {
    //   GetComponent<MeshRenderer>().material.color = Color.yellow;
    // } else {
    //   GetComponent<MeshRenderer>().material.color = Color.white;
    // }

    // if ((int)Time.time % 1 == 0) {
    //   Projectile projectile = Instantiate(
    //       projectilePlaceholder, projectilePlaceholder.transform.position,
    //       projectilePlaceholder.transform.rotation, transform);

    //   projectile.placeholder = false;

    //   player = GameObject.Find("Ethan").transform.position;
    //   projectile.transform.LookAt(player + Vector3.up*1.2f);

    //   projectile.GetComponent<Rigidbody>().velocity =
    //       projectile.transform.forward * projectile_speed;

    //   projectile.transform.Rotate(new Vector3(90f, 0, 0));

    //   Physics.IgnoreCollision(GetComponent<BoxCollider>(),
    //                           projectile.GetComponent<CapsuleCollider>());
    //   projectile.transform.parent = null;
    // }
  }
  void OnTriggerEnter(Collider other) {
    Destroy(gameObject);
  }
}
