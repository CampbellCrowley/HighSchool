using UnityEngine;

public
class PlayerShooter : MonoBehaviour {
  [System.Serializable] public class Sounds {
   public
    AudioPlayer Player;
   public
    AudioClip Sound1;
   public
    AudioClip Sound2;
   public
    AudioClip Sound3;
   public
    AudioClip BombDrop;
  }

 public
  Projectile projectilePlaceholder;
 public
  Projectile bombPlaceholder;
 public
  float projectile_speed;
 public
  float weapon1ShotDelay = 0.2f;
 public
  float weapon2ShotDelay = 5.0f;
 public
  float weapon3ShotDelay = 0.5f;

 public
  static float reloadTime = 0.0f;

 private
  float lastShotTime = 0;
 private
  float lastBombTime = 0;
 private
  Vector3 localPosition;
 public
  GUIText weaponDisplay;
 public
  GUIText Crosshair;
 private
  LineRenderer line;
 private
  GameObject player;
 private
  int weapon = 1;
 public
  Sounds sounds;

 public
  void Start() {
    if (projectilePlaceholder == null) {
      Debug.LogError(
          "PlayerShooter does not have a projectile placeholder and " +
          "therefore will not work");
    } else {
      projectilePlaceholder.placeholder = true;
      GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
      if(players.Length == 0) {
        Debug.LogError(
            "There must be at least one player with the tag of \"Player\"!");
        return;
      }
      weaponDisplay = Instantiate(weaponDisplay);
      if (Crosshair != null) Crosshair = Instantiate(Crosshair);
      player = players[0];
      localPosition = projectilePlaceholder.transform.localPosition;
      lastShotTime = Time.time;
      lastBombTime = Time.time;
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
    if (player == null) return;
    if (player.GetComponent<PlayerController>() == null) return;
    if (player.GetComponent<PlayerController>().isDead) return;
    if (GameData.isPaused) return;
    if (GameData.inVehicle) return;
    projectilePlaceholder.transform.localPosition = localPosition;

    float shoot = Input.GetAxis("Fire1");
    float shoot2 = Input.GetAxis("Fire2");
    float ScrollWheel = Input.GetAxis("Mouse ScrollWheel");
    if (ScrollWheel < 0) weapon--;
    if (ScrollWheel > 0) weapon++;
    if (Input.GetKey("1") || weapon < 1) weapon = 1;
    if (Input.GetKey("2")) weapon = 2;
    if (Input.GetKey("3") || weapon > 3) weapon = 3;

    if (weaponDisplay != null) {
      weaponDisplay.text = "Weapon: ";
      switch (weapon) {
        case 1:
          reloadTime = Time.time - lastShotTime - weapon1ShotDelay;
          if (reloadTime > 0) reloadTime = 0;
          else reloadTime = Mathf.Abs(reloadTime);
          weaponDisplay.text += "Primary";
          for (int i = 0; i < reloadTime; i++) {
            weaponDisplay.text += ".";
          }
          break;
        case 2:
          reloadTime = Time.time - lastShotTime - weapon2ShotDelay;
          if (reloadTime > 0) reloadTime = 0;
          else reloadTime = Mathf.Abs(reloadTime);
          weaponDisplay.text += "Secondary";
          for (int i = 0; i < reloadTime; i++) {
            weaponDisplay.text += ".";
          }
          break;
        case 3:
          reloadTime = Time.time - lastShotTime - weapon3ShotDelay;
          if (reloadTime > 0) reloadTime = 0;
          else reloadTime = Mathf.Abs(reloadTime);
          weaponDisplay.text += "Tertiary";
          for (int i = 0; i < reloadTime; i++) {
            weaponDisplay.text += ".";
          }
          break;
        default:
          weaponDisplay.text += "Something broke...";
          break;
      }
    }

    if (Crosshair != null) {
      Crosshair.text = "+";
      if (reloadTime > 0) {
        Crosshair.text += "\nReloading";
        for (int i = 0; i < reloadTime; i++) {
          Crosshair.text += ".";
        }
      }
    }

    if (!player.GetComponent<PlayerController>().spawned) return;

    if (weapon == 1 && shoot > 0.5f &&
        Time.time - lastShotTime > weapon1ShotDelay) {
      //GameData.collectedCollectibles--;
      lastShotTime = Time.time;
      PlaySound(sounds.Sound1);
      Projectile projectile = Instantiate(
          projectilePlaceholder, projectilePlaceholder.transform.position,
          projectilePlaceholder.transform.rotation, transform);
      projectile.placeholder = false;
      projectile.GetComponent<Rigidbody>().velocity =
          transform.rotation * (Vector3.forward * projectile_speed);
      Physics.IgnoreCollision(player.GetComponent<Collider>(),
                              projectile.GetComponent<Collider>());
      projectile.GetComponent<Rigidbody>().useGravity = true;
      projectile.transform.parent = null;
    } else if (weapon == 2 && shoot > 0.0f &&
               Time.time - lastShotTime > weapon2ShotDelay) {
      //GameData.collectedCollectibles -= 2;
      PlaySound(sounds.Sound2);
      lastShotTime = Time.time;
      RaycastHit raycast;
      Physics.Raycast(
          projectilePlaceholder.transform.position - Vector3.up * 0.01f,
          transform.rotation * Vector3.forward, out raycast, 100f, ~(1 << 8));

      if (line != null && raycast.transform != null) {
        line.enabled = true;
        line.SetPosition(
            0, projectilePlaceholder.transform.position - Vector3.up * 0.1f);
        EnemyController enemy =
            raycast.transform.gameObject.GetComponent<EnemyController>();
        if (enemy != null && (enemy.health -= 2) <= 0) {
          enemy.kill();
          line.SetPosition(1, raycast.transform.position);
        } else {
          line.SetPosition(1,
                           projectilePlaceholder.transform.position +
                               transform.rotation * (Vector3.forward * 100f));
        }
      } else {
        line.enabled = true;
        line.SetPosition(0, projectilePlaceholder.transform.position);
        line.SetPosition(1, projectilePlaceholder.transform.position +
                                transform.rotation * (Vector3.forward * 100f));
      }
    } else if (weapon == 3 && shoot > 0.5f &&
               Time.time - lastShotTime > weapon3ShotDelay) {
      //GameData.collectedCollectibles--;
      PlaySound(sounds.Sound3);
      lastShotTime = Time.time;
      Projectile projectile = Instantiate(
          projectilePlaceholder, projectilePlaceholder.transform.position,
          projectilePlaceholder.transform.rotation, transform);
      projectile.placeholder = false;
      projectile.GetComponent<Rigidbody>().velocity =
          transform.rotation * (Vector3.forward * projectile_speed);
      Physics.IgnoreCollision(player.GetComponent<Collider>(),
                              projectile.GetComponent<Collider>());
      projectile.GetComponent<Rigidbody>().useGravity = false;
      projectile.transform.parent = null;
    }

    // Drop bomb
    if (bombPlaceholder != null && shoot2 > 0.5f &&
        Time.time - lastBombTime > 1.0f && GameData.collectedCollectibles > 0) {
      GameData.collectedCollectibles--;
      PlaySound(sounds.BombDrop);
      lastBombTime = Time.time;
      Projectile projectile = Instantiate(
          bombPlaceholder, bombPlaceholder.transform.position,
          bombPlaceholder.transform.rotation, transform);
      projectile.placeholder = false;
      projectile.GetComponent<Rigidbody>().velocity =
          transform.rotation * (Vector3.forward * projectile_speed / 2f);
      Physics.IgnoreCollision(player.GetComponent<Collider>(),
                              projectile.GetComponent<Collider>());
      projectile.GetComponent<Rigidbody>().useGravity = true;
      projectile.transform.parent = null;
    }

    if (line != null && Time.time - lastShotTime > 0.25f) {
      line.enabled = false;
    }
  }
  void PlaySound(AudioClip clip) {
    if (sounds.Player != null && clip != null && GameData.soundEffects) {
      AudioPlayer player = Instantiate(sounds.Player) as AudioPlayer;
      player.clip = clip;
    }
  }
}
