using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour {
  [System.Serializable] public class Sounds {
   public
    AudioPlayer Player;
   public
    AudioClip ShootSound;
   public
    AudioClip BombDrop;
  }
  public int health = 10;
  public Projectile projectilePlaceholder;
  public GameObject projectileGun;
  public float projectile_speed;
  public Sounds sounds;

  private GameObject player;
  private Animation anim;
  private float startTime;
  private float deathTime;
  private bool firstRoar = false;
  private bool wasFirstRoar = false;
  private bool dying = false;
  private Vector3 lastTargetPosition;
  private float lastShotTime;

	void Start () {
    anim = GetComponent<Animation>();
    startTime = Time.time;
    anim.Play();
  }

	void Update () {
    if(Time.time-deathTime > 5f && dying) {
      GameData.nextLevel();
    }
    if(Time.time- startTime > 5 && Time.time - startTime - Time.deltaTime < 5) {
      anim.CrossFade("rage", 0.1f);
      firstRoar = true;
    } else if (Mathf.RoundToInt(Time.time - startTime) % 10 == 0 &&
               Time.time - startTime > 20 && !dying) {
      anim.CrossFade("rage", 0.1f);
    } else if(!anim.isPlaying) {
      firstRoar = false;
      if(!dying) {
        anim.CrossFade("idle");
      } else {
        anim.CrossFade("sleep");
      }
    }
    if(firstRoar && !wasFirstRoar) {
      wasFirstRoar = true;
      Time.timeScale = 0.2f;
      Time.fixedDeltaTime = 0.02f * Time.timeScale;
    } else if(!firstRoar && wasFirstRoar) {
      wasFirstRoar = false;
      Time.timeScale = 1.0f;
      Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }




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
    // Move the point at which the enemy is aiming, towards the player with
    // damping.
    Vector3 target = Vector3.Lerp(lastTargetPosition,
                                  player.transform.position + Vector3.up * 1.2f,
                                  53.0f * Time.deltaTime);
    if (projectilePlaceholder != null &&
        (player.transform.position - projectilePlaceholder.transform.position)
                .magnitude <= 100) {
      lastTargetPosition = target;

    }

    if (projectilePlaceholder != null) {
      projectilePlaceholder.transform.LookAt(lastTargetPosition);
    }

    if (projectilePlaceholder != null && Time.time - lastShotTime > 1.75f) {
      lastShotTime = Time.time;
      Projectile projectile = Instantiate(
          projectilePlaceholder, projectilePlaceholder.transform.position,
          projectilePlaceholder.transform.rotation, transform);

      projectile.placeholder = false;

      projectile.GetComponent<Rigidbody>().velocity =
          projectile.transform.forward * projectile_speed;

      projectile.transform.Rotate(new Vector3(90f, 0, 0));

      Physics.IgnoreCollision(GetComponentInChildren<Collider>(),
                              projectile.GetComponent<Collider>());
      projectile.transform.parent = null;
      PlaySound(sounds.ShootSound, 1f);
    }
    // Keep the gun and gameObject pointed in the same direction as the
    // projectilePlaceholder with some damping.
    if (projectileGun != null) {
      projectileGun.transform.rotation =
          projectilePlaceholder.transform.rotation *
          Quaternion.Euler(180f, 0, 0);
    }
	}

  public void Damage() {
    if (!dying) {
      anim.CrossFade("damage", 0.1f);
      health--;
      if (health == 0) {
        anim.Stop();
        anim.Play("sleep_start");
        dying = true;
        deathTime = Time.time;
      }
    }
  }
  void PlaySound(AudioClip clip, float volume = -1f) {
    if (sounds.Player != null && clip != null && GameData.soundEffects) {
      AudioPlayer player = Instantiate(sounds.Player, transform.position,
                                       Quaternion.identity) as AudioPlayer;
      player.clip = clip;
      if (volume >= 0f && volume <= 1f) {
        player.volume = volume;
      }
    }
  }
}
