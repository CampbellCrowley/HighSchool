using UnityEngine;

public class PlayerShooter : MonoBehaviour {
  public Projectile projectilePlaceholder;
  public float projectile_speed;
  private float lastShotTime = 0;
  private LineRenderer line;

  public void Start() {
    projectilePlaceholder.placeholder = true;
    lastShotTime = Time.time;
    LineRenderer = GetComponent<LineRenderer>();
  }

  public void Update() {
    float shoot = Input.GetAxis("Fire1");
    float shoot2 = Input.GetAxis("Fire2");
    if(shoot > 0.5f && Time.time - lastShotTime > 0.2f && GameData.collectedCollectibles > 0) {
      GameData.collectedCollectibles --;
      lastShotTime = Time.time;
      Projectile projectile = Instantiate(
          projectilePlaceholder, projectilePlaceholder.transform.position,
          projectilePlaceholder.transform.rotation, transform);
      projectile.placeholder = false;
      projectile.GetComponent<Rigidbody>().velocity = transform.rotation*(Vector3.forward *
                                                    projectile_speed);
      Physics.IgnoreCollision(GetComponent<CapsuleCollider>(),
                              projectile.GetComponent<CapsuleCollider>());
      projectile.transform.parent = null;
    } else if(shoot2 > 0.5f && Time.time - lastShotTime > 0.225f && GameData.collectedCollectibles > 0) {
      GameData.collectedCollectibles -= 2;
      lastShotTime = Time.time;
      RayCast raycast;
      Physics.Raycast(projectilePlaceholder.transform.position, Vector3.forward, out raycast);

      if(line!=null && ) {
        line.SetPosition (0, projectileHolder.transform.position);
      }

    }
  }
}
