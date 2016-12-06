using UnityEngine;
using System.Collections;
#pragma warning disable 0108

public
class PlaySoundOnLoad : MonoBehaviour {
 private
  AudioSource audio;
  void Start() {
    audio = gameObject.GetComponent(typeof(AudioSource)) as AudioSource;
    audio.Play();
  }

  void Update() {
    if (!audio.isPlaying) Destroy(gameObject);
  }
}
