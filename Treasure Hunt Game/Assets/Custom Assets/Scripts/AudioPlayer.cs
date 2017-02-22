using System.Collections;
using UnityEngine;

public
class AudioPlayer : MonoBehaviour {
 private
  AudioSource source;
 public
  AudioClip clip;
 private
  bool started = false;

 public
  void Update() {
    if (source == null || !source.isPlaying) {
      if (started) {
        Destroy(gameObject);
      } else {
        source = gameObject.AddComponent<AudioSource>() as AudioSource;
        source.clip = clip;
        source.Play();
        started = true;
      }
    }
  }
}
