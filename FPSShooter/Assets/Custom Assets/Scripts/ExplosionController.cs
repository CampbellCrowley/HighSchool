using UnityEngine;

public
class ExplosionController : MonoBehaviour {
 private
  float birth;
 public
  float lifespan = 0.2f;  // Time in seconds

  void Start() { birth = Time.time; }

  void Update() {
    if (Time.time - birth > lifespan) {
      if (GetComponent<ParticleSystem>() != null) {
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        if (Time.time - birth > GetComponent<ParticleSystem>().main.duration) {
          Destroy(gameObject);
          Debug.Log("Explostion Destroyed");
        }
      } else {
        Destroy(gameObject);
      }
    }
  }
}
