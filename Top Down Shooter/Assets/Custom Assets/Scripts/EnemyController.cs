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
  int StartHealth = 5;
 public
  int health = 5;
 public
  bool spawnChildren = true;

 private
  GameObject player;
 private TerrainGenerator ground;
 private
  LineRenderer line;
 private
  bool shoot = false;
 private
  Vector3 lastTargetPosition;
 private
  Vector3 lastPosition;
 private
  Quaternion lastRotation;
 private
  Vector3 startPos;
 private
  float lastSpawnTime;
 private
  float lastShotTime;
 public
  void Awake() { GameData.numEnemies++; }
 public
  void Start() {
    health = StartHealth;
    player = GameObject.FindGameObjectsWithTag("Player")[0];
    ground = GameObject.FindObjectOfType<TerrainGenerator>();
    startPos = transform.position;
    lastTargetPosition = projectilePlaceholder.transform.position;
    lastPosition = transform.position;
    lastRotation = transform.rotation;
    lastSpawnTime = Time.time;
    lastShotTime = Time.time;
    line = GetComponent<LineRenderer>();
    line.startColor = Color.white;
    line.endColor = Color.red;
    line.startWidth = 0.1f;
    line.endWidth = 0.01f;
  }
 public
  void kill() {
    if(GameData.numEnemies-1 <= 0) {
      GameData.nextLevel();
    }
    Destroy(gameObject);
  }
 public
  void Update() {
    // The enemy moves in a triangle pattern in respect to time and it's
    // starting position.
    transform.position = Vector3.Lerp(startPos, startPos + Vector3.forward * 3,
                                      Mathf.Abs(Time.time % 3 - 1));
    transform.position =
        Vector3.Lerp(transform.position, transform.position + Vector3.left * 3,
                     Mathf.Abs(Time.time % 3 - 2));

    // Keep the enemy a constant distance off the ground.
    if(ground!=null) {
      transform.position = new Vector3(transform.position.x,
                                       ground.GetTerrainHeight(gameObject) + 1f,
                                       transform.position.z);
    }

    // Change the enemy's color to show remaining health.
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

    // Move the point at which the enemy is aiming towards the player with
    // damping.
    Vector3 target = Vector3.Lerp(player.transform.position + Vector3.up * 1.2f,
                                  lastTargetPosition, 55.0f * Time.deltaTime);
    projectilePlaceholder.transform.LookAt(target);
    // If the player is close enough, slowly move the enemy towards the player.
    if ((player.transform.position - projectilePlaceholder.transform.position)
            .magnitude <= 20f) {
      lastTargetPosition = target;
      startPos = Vector3.MoveTowards(
          startPos, player.transform.position + Vector3.up * 1.2f,
          1.0f * Time.deltaTime);
    } else {
      lastTargetPosition =
          Quaternion.Euler(0, 1f, 0) *
          (lastTargetPosition + transform.position - lastPosition);
    }
    lastPosition = transform.position;

    // Raycasting means the enemy is checking to make sure the player is close,
    // and there are no obstacles between the player and the enemy, before
    // shooting.
    if (isRaycasting) {
      RaycastHit raycast;
      Physics.Raycast(projectilePlaceholder.transform.position,
                      projectilePlaceholder.transform.forward, out raycast, 20f,
                      ~(1 << 2));

      shoot = false;
      line.SetPosition(0, projectilePlaceholder.transform.position);
      line.SetPosition(1, projectilePlaceholder.transform.position +
                              projectilePlaceholder.transform.forward * 10f);
      if (line != null && raycast.transform != null) {
        if (raycast.transform.gameObject == player && raycast.distance <= 10f) {
          shoot = true;
          lastShotTime -= (10 - raycast.distance) / 20f;
        }
      }
    }

    // Keep the gun and gameObject pointed in the same direction as the
    // projectilePlaceholder with some damping.
    projectileGun.transform.rotation =
        projectilePlaceholder.transform.rotation * Quaternion.Euler(180f, 0, 0);
    Quaternion rotationOffset = Quaternion.Euler(0f, 0f, 90f);
    transform.rotation =
        Quaternion.Lerp(lastRotation, projectileGun.transform.rotation, 0.1f) *
        rotationOffset;
    lastRotation = transform.rotation * Quaternion.Inverse(rotationOffset);

    // Shoot a projectile towards the player at an interval and if raycasting,
    // then only when the player is close enough.
    if (Time.time - lastShotTime > 1.75f && (!isRaycasting || shoot)) {
      lastShotTime = Time.time;
      Projectile projectile = Instantiate(
          projectilePlaceholder, projectilePlaceholder.transform.position,
          projectilePlaceholder.transform.rotation, transform);

      projectile.placeholder = false;

      projectile.GetComponent<Rigidbody>().velocity =
          projectile.transform.forward * projectile_speed;

      projectile.transform.Rotate(new Vector3(90f, 0, 0));

      Physics.IgnoreCollision(GetComponent<Collider>(),
                              projectile.GetComponent<Collider>());
      projectile.transform.parent = null;
    }

    // Spawn a copy at a random location around the player.
    if (Time.time - lastSpawnTime > 5.0f && GameData.numEnemies < 50 &&
        spawnChildren) {
      lastSpawnTime = Time.time;
      Vector3 spawnPosition =
          player.transform.position + new Vector3(Random.Range(-100f, 100f),
                                                  transform.position.y,
                                                  Random.Range(-100f, 100f));
      Instantiate(gameObject, spawnPosition, Quaternion.identity);
    }
    if (spawnChildren &&
        Vector3.Distance(transform.position, player.transform.position) >
            150f) {
      Destroy(gameObject);
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
