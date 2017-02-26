using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour {
  private bool open = false;

  public void Start() {
      GetComponentsInChildren<ParticleSystem>()[0].Stop();
      GetComponent<MeshRenderer>().enabled = false;
      GetComponent<AudioSource>().enabled = false;
      if (GameData.soundEffects) {
        GetComponent<AudioSource>().enabled = false;
      }
  }
  public void Update() {
    if(GameData.levelComplete() && !open) {
      GetComponentsInChildren<ParticleSystem>()[0].Play();
      GetComponent<MeshRenderer>().enabled = true;
      GetComponent<AudioSource>().enabled = true;
      open = true;
    }
  }
}
