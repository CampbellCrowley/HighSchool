using UnityEngine;
using System.Collections;

public class CollectibleController : MonoBehaviour {
  public AudioSource Sound;
  void OnDestroy() {
    Sound.Play();
  }
}
