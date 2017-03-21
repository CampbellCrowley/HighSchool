using UnityEngine;

public
class EnemyController : MonoBehaviour {
 public
  Projectile projectilePlaceholder;
 public
  GameObject projectileGun;
 public
  bool isRaycasting = true;
 public
  float projectile_speed;
 public
  int health = 5;
 public
  bool spawnChildren = true;
 private
  GameObject player;
 private
  LineRenderer line;
 private
  bool shoot = false;

 private
  Vector3 lastTargetPosition;
 private
  Vector3 lastPosition;
 private
  Vector3 startPos;
 private
  float lastSpawnTime;
 private
  float lastShotTime;
 public
  void Start() {
    GameData.numEnemies++;
    player = GameObject.Find("Ethan");
    startPos = transform.position;
    lastTargetPosition = projectilePlaceholder.transform.position;
    lastPosition = transform.position;
    lastSpawnTime = Time.time;
    lastShotTime = Time.time;
    line = GetComponent<LineRenderer>();
    line.startColor = Color.white;
    line.endColor = Color.red;
    line.startWidth = 0.1f;
    line.endWidth = 0.01f;
  }
 public
  void kill() { Destroy(gameObject); }
 public
  void Update() {
    transform.position = Vector3.Lerp(startPos, startPos + Vector3.forward * 3,
                                      Mathf.Abs(Time.time % 3 - 1));
    transform.position =
        Vector3.Lerp(transform.position, transform.position + Vector3.left * 3,
                     Mathf.Abs(Time.time % 3 - 2));
    if (health == 0) {
      GetComponent<MeshRenderer>().material.color = Color.red;
    } else if (health == 1) {
      GetComponent<MeshRenderer>().material.color =
          (Color.red + Color.yellow) / 2;
    } else if (health == 2) {
      GetComponent<MeshRenderer>().material.color = Color.yellow;
    } else {
      GetComponent<MeshRenderer>().material.color = Color.white;
    }

    Vector3 target = Vector3.Lerp(player.transform.position + Vector3.up * 1.2f,
                                  lastTargetPosition, 55.0f * Time.deltaTime);
    projectilePlaceholder.transform.LookAt(target);
    if ((player.transform.position - projectilePlaceholder.transform.position)
            .magnitude <= 10f) {
      lastTargetPosition = target;
    } else {
      lastTargetPosition = Quaternion.Euler(0,1f,0) * (lastTargetPosition + transform.position-lastPosition);
    }
    lastPosition = transform.position;

    if (isRaycasting) {
      RaycastHit raycast;
      Physics.Raycast(projectilePlaceholder.transform.position,
                      projectilePlaceholder.transform.forward, out raycast, 10f,
                      ~(1 << 2));

      shoot = false;
      // line.enabled = false;
      line.SetPosition(0, projectilePlaceholder.transform.position);
      line.SetPosition(1, projectilePlaceholder.transform.position +
                              projectilePlaceholder.transform.forward * 10f);
      if (line != null && raycast.transform != null) {
        if (raycast.transform.gameObject.GetComponent<PlayerController>()) {
          // line.enabled = true;
          shoot = true;
          lastShotTime -= (10 - raycast.distance) / 20f;
        }
      }
    }
    projectileGun.transform.rotation = projectilePlaceholder.transform.rotation * Quaternion.Euler(90f,0,0);
    if (Time.time - lastShotTime > 1.75f && (!isRaycasting || shoot)) {
      lastShotTime = Time.time;
      Projectile projectile = Instantiate(
          projectilePlaceholder, projectilePlaceholder.transform.position,
          projectilePlaceholder.transform.rotation, transform);

      projectile.placeholder = false;

      projectile.GetComponent<Rigidbody>().velocity =
          projectile.transform.forward * projectile_speed;

      projectile.transform.Rotate(new Vector3(90f, 0, 0));

      Physics.IgnoreCollision(GetComponent<BoxCollider>(),
                              projectile.GetComponent<CapsuleCollider>());
      projectile.transform.parent = null;
    }
    if(Time.time - lastSpawnTime > 2.1f && GameData.numEnemies < 10) {
      lastSpawnTime = Time.time;
      Instantiate(gameObject);
    }
  }

 public
  void OnDestroy() {
    GameData.numEnemies--;
  }

 public
  void OnCollisionEnter(Collision other) {
    if (other.gameObject.GetComponent<Projectile>() != null) {
      health--;
      if(health<0) {
        kill();
      }
    }
  }
}
