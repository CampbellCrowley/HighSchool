using UnityEngine;

public class PlayerShooter : MonoBehaviour {
  public Projectile projectilePlaceholder;
  public float projectile_speed;
  private float lastShotTime = 0;

  public void Start() {
    projectilePlaceholder.placeholder = true;
    lastShotTime = Time.time;
  }

  public void Update() {
    float shoot = Input.GetAxis("Fire1");
    if(shoot > 0.5f && Time.time - lastShotTime > 0.0f) {
      lastShotTime = Time.time;
      Projectile projectile = Instantiate(
          projectilePlaceholder, projectilePlaceholder.transform.position,
          projectilePlaceholder.transform.rotation, null);
      projectile.GetComponent<Rigidbody>().AddForce(Vector3.forward *
                                                    projectile_speed);
      projectile.lifespan = 3.0f;
      projectile.placeholder = false;
      Physics.IgnoreCollision(GetComponent<CapsuleCollider>(),
                              projectile.GetComponent<CapsuleCollider>());
    }
  }
}
