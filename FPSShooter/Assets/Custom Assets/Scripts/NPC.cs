using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {
  [System.Serializable]
  public class Sounds {
    public AudioPlayer Player;
    public AudioClip Sound;
  }
  public MeshRenderer TextBox;
  public float showTextDistance = 10f;
  public Sounds sounds;

  public
   void Start() { PlaySound(sounds.Sound, .5f, true); }

  public void FixedUpdate() {
    if (TextBox == null) return;
    PlayerController[] players = FindObjectsOfType<PlayerController>();
    TextBox.enabled = false;
    foreach (PlayerController p in players) {
      if (Vector3.Distance(p.transform.position, transform.position) <
          showTextDistance) {
        TextBox.enabled = true;
      }
    }
  }
  void PlaySound(AudioClip clip, float volume = -1f, bool loop = false) {
    if (sounds.Player != null && clip != null && GameData.soundEffects) {
       AudioPlayer player = Instantiate(sounds.Player, transform.position,
                                        Quaternion.identity) as AudioPlayer;
      player.clip = clip;
      player.loop = loop;
      if (volume >= 0f && volume <= 1f) {
        player.volume = volume;
      }
    }
  }
}
