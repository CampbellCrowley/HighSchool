using UnityEngine;
#pragma warning disable 0168

public
class EnemyController : MonoBehaviour {
  [System.Serializable] public class Sounds {
   public
    AudioPlayer Player;
   public
    AudioClip ShootSound;
   public
    AudioClip BombDrop;
  }
 public
  Projectile projectilePlaceholder;
 public
  GameObject projectileGun;
 public
  bool isRaycasting = true;
 public
  Projectile Bomb;
 public
  float bombDropInterval = 2f;
 public
  float projectile_speed;
 public
  int StartHealth = 5;
 public
  int health = 5;
 public
  bool spawnChildren = true;
 public
  int maxTotalEnemies = 5;
 public
  bool useNavMesh = false;
 public
  bool isFlying = false;
 public
  bool moveInTriangles = true;
 public
  Vector3 moveVector;
 public
  Sounds sounds;

 private
  GameObject player;
 private
  TerrainGenerator ground;
 private
  LineRenderer line;
 private
  bool shoot = false;
 private
  float spawnTime;
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
 private
  float lastBombDropTime;
 private
  float lastMoveTime;
 public
  void Awake() { GameData.numEnemies++; }
 public
  void Start() {
    health = StartHealth;
    ground = FindObjectOfType<TerrainGenerator>();
    if (ground != null) {
      Debug.Log(gameObject.name + " found TerrainGenerator!");
    } else {
      Debug.LogWarning(gameObject.name + " did not find TerrainGenerator.");
    }
    startPos = transform.position;
    if (projectilePlaceholder != null) {
      lastTargetPosition = projectilePlaceholder.transform.position;
    } else {
      lastTargetPosition = Vector3.zero;
    }
    lastPosition = transform.position;
    lastRotation = transform.rotation;
    spawnTime = Time.time;
    lastSpawnTime = Time.time;
    lastShotTime = Time.time;
    lastBombDropTime = Time.time;
    lastMoveTime = Time.time;
    line = GetComponent<LineRenderer>();
    if (line != null) {
      line.startColor = Color.white;
      line.endColor = Color.red;
      line.startWidth = 0.1f;
      line.endWidth = 0.01f;
    }
  }
 public
  void kill() {
    // if(GameData.numEnemies-1 <= 0) {
    //   GameData.nextLevel();
    // }
    Destroy(gameObject);
  }
 public
  void Update() {
    if (player == null) {
      PlayerController[] players =
          GameObject.FindObjectsOfType<PlayerController>();
      if (players.Length >= 1) {
        player = players[0].gameObject;
      } else
        return;
    }
    if (!player.GetComponent<PlayerController>().spawned) {
      return;
    }
    if (ground == null) {
      ground = FindObjectOfType<TerrainGenerator>();
      if (ground != null) {
        Debug.Log(gameObject.name + " found TerrainGenerator!");
      }
    }
    if (Time.time - spawnTime >= 1f) {
      UnityEngine.AI.NavMeshAgent agent =
          GetComponent<UnityEngine.AI.NavMeshAgent>();
      if (agent != null) agent.enabled = true;
    }

    // Change the enemy's color to show remaining health.
    try {
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
    } catch (MissingComponentException e) {
    }

    if (GameData.isPaused) return;

    // Move the point at which the enemy is aiming, towards the player with
    // damping.
    Vector3 target = Vector3.Lerp(lastTargetPosition,
                                  player.transform.position + Vector3.up * 1.2f,
                                  53.0f * Time.deltaTime);
    if (useNavMesh && Time.time - spawnTime >= 10f) {
      UnityEngine.AI.NavMeshAgent agent =
          GetComponent<UnityEngine.AI.NavMeshAgent>();
      if (agent != null && agent.isOnNavMesh) {
        if (Vector3.Distance(player.transform.position,
                             projectilePlaceholder.transform.position) <= 20f &&
            Vector3.Distance(agent.destination, player.transform.position) >
                0.5f) {
          lastTargetPosition = target;
          agent.destination = player.transform.position + Vector3.up * 0.5f;
        } else if (agent.remainingDistance < 0.5 ||
                   agent.destination == Vector3.zero) {
          agent.destination =
              new Vector3(transform.position.x + Random.Range(-15f, 15f),
                          transform.position.y,
                          transform.position.z + Random.Range(-15f, 15f));
          lastTargetPosition = agent.destination;
        }
      }
    } else if(useNavMesh) {
      GetComponent<MeshRenderer>().material.color =
          Mathf.RoundToInt(Time.time) % 2 == 0 ? Color.red : Color.green;
    }
    // If the player is close enough, slowly move the enemy towards the player.
    if (!useNavMesh && projectilePlaceholder != null &&
        (player.transform.position - projectilePlaceholder.transform.position)
                .magnitude <= 20f) {
      lastTargetPosition = target;
      startPos = transform.position;
      lastMoveTime = Time.time;
      startPos = Vector3.MoveTowards(
          startPos, player.transform.position + Vector3.up * 1.2f,
          2.0f * Time.deltaTime);
      transform.position = startPos;
    } else if (!useNavMesh) {
      lastTargetPosition =
          Quaternion.Euler(0, 2.0f, 0) *
          (lastTargetPosition + transform.position - lastPosition);
      // The enemy moves in a pattern in respect to time and it's starting
      // position.
      if (moveInTriangles) {
        transform.position =
            Vector3.Lerp(startPos, startPos + Vector3.forward * 3,
                         Mathf.Abs((1 + Time.time - lastMoveTime) % 3 - 1));
        transform.position = Vector3.Lerp(
            transform.position, transform.position + Vector3.left * 3,
            Mathf.Abs((2 + Time.time - lastMoveTime) % 4 - 2));
      } else {
        transform.position = transform.position + moveVector * Time.deltaTime;
      }
    }
    // Keep the enemy a constant distance off the ground.
    if (!useNavMesh && ground != null && !isFlying) {
      transform.position = new Vector3(transform.position.x,
                                       ground.GetTerrainHeight(gameObject) + 1f,
                                       transform.position.z);
    } else if (!useNavMesh && ground != null && isFlying) {
      transform.position = new Vector3(
          transform.position.x, ground.GetTerrainHeight(gameObject) + 20f,
          transform.position.z);
    }
    lastPosition = transform.position;

    if (projectilePlaceholder != null) {
      projectilePlaceholder.transform.LookAt(lastTargetPosition);
    }

    // Raycasting means the enemy is checking to make sure the player is close,
    // and there are no obstacles between the player and the enemy, before
    // shooting.
    if (isRaycasting) {
      RaycastHit raycast;
      Physics.Raycast(projectilePlaceholder.transform.position,
                      projectilePlaceholder.transform.forward, out raycast, 20f,
                      ~(1 << 2));

      shoot = false;
      if (line != null) {
        line.SetPosition(0, projectilePlaceholder.transform.position);
        line.SetPosition(1, projectilePlaceholder.transform.position +
                                projectilePlaceholder.transform.forward * 10f);
        if (raycast.transform != null) {
          if (raycast.transform.gameObject == player &&
              raycast.distance <= 10f) {
            shoot = true;
            lastShotTime -= (10 - raycast.distance) / 20f;
          }
        }
      }
    }

    // Keep the gun and gameObject pointed in the same direction as the
    // projectilePlaceholder with some damping.
    if (projectileGun != null) {
      projectileGun.transform.rotation =
          projectilePlaceholder.transform.rotation *
          Quaternion.Euler(180f, 0, 0);
    }
    if (!useNavMesh && projectileGun != null) {
      Quaternion rotationOffset = Quaternion.Euler(0f, 0f, 90f);
      transform.rotation =
          Quaternion.Lerp(lastRotation, projectileGun.transform.rotation,
                          0.1f) *
          rotationOffset;
      lastRotation = transform.rotation * Quaternion.Inverse(rotationOffset);
    }

    // Shoot a projectile towards the player at an interval and if raycasting,
    // then only when the player is close enough.
    if (projectilePlaceholder != null && Time.time - lastShotTime > 1.75f &&
        (!isRaycasting || shoot)) {
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
      PlaySound(sounds.ShootSound);
    }

    if(Bomb != null && Time.time - bombDropInterval > lastBombDropTime) {
      lastBombDropTime = Time.time;
      Projectile bomb =
          Instantiate(Bomb, transform.position, Quaternion.identity);
      bomb.transform.localScale *= 2f;
      bomb.placeholder = false;
      Physics.IgnoreCollision(GetComponent<Collider>(),
                              bomb.GetComponent<Collider>());
      bomb.transform.parent = null;
      bomb.GetComponent<Rigidbody>().useGravity = true;
      PlaySound(sounds.BombDrop);
    }

    // Spawn a copy at a random location around the player.
    PlayerController pc = player.GetComponent<PlayerController>();
    if (((pc != null && !pc.spawned) || Time.time - lastSpawnTime > 5.0f) &&
        GameData.numEnemies < maxTotalEnemies && spawnChildren) {
      lastSpawnTime = Time.time;
      //float X = Random.Range(0f, 350f);
      //float Z = Random.Range(0f, 350f);
      float X = Random.Range(-100f, 100f);
      float Z = Random.Range(-100f, 100f);
      Vector3 spawnPosition =
          new Vector3(player.transform.position.x + X,
                      ground.GetTerrainHeight(X + player.transform.position.x,
                                              Z + player.transform.position.z),
                      player.transform.position.z + Z);
          //new Vector3(X, ground.GetTerrainHeight(X, Z), Z);
      UnityEngine.AI.NavMeshAgent nma =
          GetComponent<UnityEngine.AI.NavMeshAgent>();
      if (nma != null) nma.enabled = false;
      Instantiate(gameObject, spawnPosition, Quaternion.identity);
      if (nma != null) nma.enabled = true;
      Debug.Log("Enemy Spawned");
    }
    if (spawnChildren && GameData.numEnemies > 1 &&
        Vector2.Distance(
            new Vector2(transform.position.x, transform.position.z),
            new Vector2(player.transform.position.x,
                        player.transform.position.z)) > 150f) {
      Debug.Log("Enemy Despawning " + transform.position + " - " +
                player.transform.position + " = " +
                Vector2.Distance(
                    new Vector2(transform.position.x, transform.position.z),
                    new Vector2(player.transform.position.x,
                                player.transform.position.z)));
      Destroy(gameObject);
    }
  }

  void PlaySound(AudioClip clip) {
    if (sounds.Player != null && clip != null && GameData.soundEffects) {
      AudioPlayer player = Instantiate(sounds.Player) as AudioPlayer;
      player.clip = clip;
    }
  }

 public
  void OnDestroy() {
    GameData.numEnemies--;
  }

 public
  void OnTriggerEnter(Collider other) {
    if (other.gameObject.CompareTag("Explosion")) {
      Debug.Log("Enemy was exploded.");
      kill();
    }
  }
 public
  void OnCollisionEnter(Collision other) {
    if (other.gameObject.GetComponent<Projectile>() != null) {
      Destroy(other.gameObject);
      health--;
      if(health<0) {
        Debug.Log("Enemy has no health: Dying.");
        kill();
      }
    }
  }
}
