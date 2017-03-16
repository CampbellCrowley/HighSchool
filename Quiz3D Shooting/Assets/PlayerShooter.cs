using UnityEngine;

public class PlayerShooter : MonoBehaviour {
  public Projectile projectilePlaceholder;
  public float projectile_speed;
  private float lastShotTime = 0;
  private LineRenderer line;

  public void Start() {
    projectilePlaceholder.placeholder = true;
    lastShotTime = Time.time;
    line = GetComponent<LineRenderer>();
    if(line !=null){
    line.startColor = Color.white;
    line.endColor = Color.red;
    line.startWidth = 0.1f;
    line.endWidth = 0.01f;
    }
  }

  public void Update() {
    float shoot = Input.GetAxis("Fire1");
    float shoot2 = Input.GetAxis("Fire2");
    if (shoot > 0.5f && Time.time - lastShotTime > 0.2f &&
        GameData.collectedCollectibles > 0) {
      GameData.collectedCollectibles --;
      lastShotTime = Time.time;
      Projectile projectile = Instantiate(
          projectilePlaceholder, projectilePlaceholder.transform.position,
          projectilePlaceholder.transform.rotation, transform);
      projectile.placeholder = false;
      projectile.GetComponent<Rigidbody>().velocity =
          transform.rotation * (Vector3.forward * projectile_speed);
      Physics.IgnoreCollision(GetComponent<CapsuleCollider>(),
                              projectile.GetComponent<CapsuleCollider>());
      projectile.transform.parent = null;
    } else if (shoot2 > 0.5f && Time.time - lastShotTime > 0.225f &&
               GameData.collectedCollectibles > 0) {
      GameData.collectedCollectibles -= 2;
      lastShotTime = Time.time;
      RaycastHit raycast;
      Physics.Raycast(projectilePlaceholder.transform.position,
                      transform.rotation * Vector3.forward * 10f, out raycast);

      if(line!=null && raycast.transform != null) {
        line.enabled = true;
        line.SetPosition (0, projectilePlaceholder.transform.position);
        line.SetPosition (1, raycast.transform.position);
        if (raycast.transform.gameObject.GetComponent<EnemyController>()
                .health-- <= 0) {
          raycast.transform.gameObject.GetComponent<EnemyController>().kill();
        }
      } else if(line!=null) {
        line.enabled = true;
        line.SetPosition (0, projectilePlaceholder.transform.position);
        line.SetPosition(1, projectilePlaceholder.transform.position +
                                transform.rotation * (Vector3.forward * 10f));
      }

    }
    if(line!=null && Time.time - lastShotTime > 0.1f) {
      line.enabled = false;
    }
  }
}
