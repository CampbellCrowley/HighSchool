using UnityEngine;

public
class Projectile : MonoBehaviour {
 public
  float lifespan = 3.0f;
 public
  bool bomb = false;
 public
  GameObject Explosion;
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
      // GetComponent<MeshRenderer>().enabled = true;
      GetComponent<MeshRenderer>().enabled = false;
      MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
      foreach (MeshRenderer m in meshRenderers)
        m.enabled = true;
      GetComponent<Collider>().enabled = true;
    }
  }
 private
  void Explode() {
    if (Explosion == null) return;
    GameObject explosion =
        Instantiate(Explosion, transform.position, Quaternion.identity);
    explosion.transform.parent = null;
  }
 public
  void Update() {
    if (Time.time - birth > lifespan && !placeholder) {
      if (bomb) Explode();
      Destroy(gameObject);
    } else if(Time.time - birth > 0.5 && !placeholder) {
      GetComponent<MeshRenderer>().enabled = true;
    }
  }
 public
  void OnCollisionEnter(Collision other) {
    if (other.gameObject.CompareTag("Player")) {
      if(GameData.getLevel() == 4) {
        GameData.health = 0;
      } else {
        GameData.health--;
      }
    } else if(other.gameObject.CompareTag("Boss")) {
      FindObjectOfType<BossController>().Damage();
      Destroy(gameObject);
    }
  }
}
