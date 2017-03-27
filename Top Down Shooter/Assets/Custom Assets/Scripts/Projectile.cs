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
      MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
      foreach (MeshRenderer m in meshRenderers)
        m.enabled = false;
      GetComponent<Collider>().enabled = false;
    } else {
      GetComponent<Rigidbody>().isKinematic = false;
      GetComponent<MeshRenderer>().enabled = true;
      MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
      foreach (MeshRenderer m in meshRenderers)
        m.enabled = true;
      GetComponent<Collider>().enabled = true;
    }
  }
 public
  void Update() {
    if (Time.time - birth > lifespan && !placeholder) {
      Destroy(gameObject);
    }
  }
 public
  void OnCollisionEnter(Collision other) {
    if (other.gameObject.CompareTag("Player")) {
      GameData.health--;
    }
  }
}
