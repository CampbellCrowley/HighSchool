using UnityEngine;

public
class PlayerShooter : MonoBehaviour {
 public
  Projectile projectilePlaceholder;
 public
  float projectile_speed;
 private
  float lastShotTime = 0;
 private
  LineRenderer line;
 private
  GameObject player;

 public
  void Start() {
    if (projectilePlaceholder == null) {
      Debug.LogWarning(
          "PlayerShooter does not have a projectile placeholder and " +
          "therefore will not work");
    } else {
      projectilePlaceholder.placeholder = true;
      player = GameObject.FindGameObjectsWithTag("Player")[0];
      lastShotTime = Time.time;
      line = GetComponent<LineRenderer>();
      if (line == null) {
        Debug.LogWarning("PlayerShooter could not find LineRenderer");
      } else {
        line.startColor = Color.white;
        line.endColor = Color.red;
        line.startWidth = 0.1f;
        line.endWidth = 0.01f;
        Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
        line.material = whiteDiffuseMat;
      }
    }
  }

 public
  void Update() {
    if (projectilePlaceholder == null) return;
    if (player.GetComponent<PlayerController>().isDead) return;
    float shoot = Input.GetAxis("Fire1");
    float shoot2 = Input.GetAxis("Fire2");
    if (shoot > 0.5f && Time.time - lastShotTime > 0.2f &&
        GameData.collectedCollectibles > 0) {
      GameData.collectedCollectibles--;
      lastShotTime = Time.time;
      Projectile projectile = Instantiate(
          projectilePlaceholder, projectilePlaceholder.transform.position,
          projectilePlaceholder.transform.rotation, transform);
      projectile.placeholder = false;
      projectile.GetComponent<Rigidbody>().velocity =
          transform.rotation * (Vector3.forward * projectile_speed);
      Physics.IgnoreCollision(player.GetComponent<CapsuleCollider>(),
                              projectile.GetComponent<CapsuleCollider>());
      projectile.transform.parent = null;
    } else if (shoot2 > 0.5f && Time.time - lastShotTime > 0.225f &&
               GameData.collectedCollectibles > 0) {
      GameData.collectedCollectibles -= 2;
      lastShotTime = Time.time;
      RaycastHit raycast;
      Physics.Raycast(projectilePlaceholder.transform.position,
                      transform.rotation * Vector3.forward, out raycast, 15f,
                      ~(1 << 8));

      if (line != null && raycast.transform != null) {
        line.enabled = true;
        line.SetPosition(0, projectilePlaceholder.transform.position);
        EnemyController enemy =
            raycast.transform.gameObject.GetComponent<EnemyController>();
        if (enemy != null && (enemy.health -= 2) <= 0) {
          enemy.kill();
          line.SetPosition(1, raycast.transform.position);
        } else {
          line.SetPosition(1, projectilePlaceholder.transform.position +
                                  transform.rotation * (Vector3.forward * 10f));
        }
      } else {
        line.enabled = true;
        line.SetPosition(0, projectilePlaceholder.transform.position);
        line.SetPosition(1, projectilePlaceholder.transform.position +
                                transform.rotation * (Vector3.forward * 10f));
      }
    }
    if (line != null && Time.time - lastShotTime > 0.1f) {
      line.enabled = false;
    }
  }
}
