using UnityEngine;
using System.Collections;

public class PlaySoundOnLoad : MonoBehaviour {

  private AudioSource audio;
	// Use this for initialization
	void Start () {
	  audio = gameObject.GetComponent(typeof(AudioSource)) as AudioSource;
    audio.Play();
	}
	
	// Update is called once per frame
	void Update () {
	  if(!audio.isPlaying) Destroy(gameObject);
	}
}
