using UnityEngine;

public class PlayerShooter : MonoBehaviour {
  public Projectile projectilePlaceholder;
  public float projectile_speed;

  public void Update() {
    float shoot = Input.GetAxis("Fire1");
    projectilePlaceholder.lifespan = 60.0f;
    if(shoot > 0.5f) {
      Projectile projectile = Instantiate(projectilePlaceholder, projectilePlaceholder.transform, true);
      projectile.GetComponent<Rigidbody> ().AddRelativeForce (Vector3.forward * projectile_speed);
      projectile.lifespan = 3.0f;
    }
  }
}
