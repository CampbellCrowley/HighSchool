using UnityEngine;

public
class ExplosionController : MonoBehaviour {
 private
  float birth;
 public
  float lifespan = 0.2f;  // Time in seconds

  void Start() { birth = Time.time; }

  void Update() {
    if (Time.time - birth > lifespan) Destroy(gameObject);
  }
}
