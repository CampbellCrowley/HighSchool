using UnityEngine;

public class Projectile : MonoBehaviour {
  public float lifespan = 10000.0f;
  private float birth;

  public void Start() {
    birth = Time.time;
  }
  public void Update() {
    if(Time.time - birth > lifespan) {
      Destroy(gameObject);
    }
  }
}
