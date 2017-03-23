using UnityEngine;

public
class Projectile : MonoBehaviour {
 public
  float lifespan = 3.0f;
 public
  bool placeholder = false;
 private
  float birth;

 public
  void Start() {
    birth = Time.time;
    if (placeholder) {
      GetComponent<Rigidbody>().isKinematic = true;
      GetComponent<MeshRenderer>().enabled = false;
      GetComponent<CapsuleCollider>().enabled = false;
    } else {
      GetComponent<Rigidbody>().isKinematic = false;
      GetComponent<MeshRenderer>().enabled = true;
      GetComponent<CapsuleCollider>().enabled = true;
    }
  }
 public
  void Update() {
    GameObject player = GameObject.Find("Ethan");
    if (Time.time - birth > lifespan && !placeholder) {
      Destroy(gameObject);
    } else {
      Transform currentTransform = transform;
      transform.LookAt(player.transform);
      transform.rotation =
          Quaternion.Lerp(currentTransform.rotation, transform.rotation, 0.1f);
    }
  }
 public
  void OnCollisionEnter(Collision other) {
    if (other.gameObject.CompareTag("Player")) {
      GameData.health--;
    }
  }
}
