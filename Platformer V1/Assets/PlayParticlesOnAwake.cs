using UnityEngine;
using System.Collections;

public class PlayParticlesOnAwake : MonoBehaviour {
  private ParticleSystem particles;
	void Start () {
          particles =
              gameObject.GetComponent(typeof(ParticleSystem)) as ParticleSystem;
          particles.Play();
	}
	
	void Update () {
	  if(!particles.isPlaying) Destroy(gameObject);
	}
}
