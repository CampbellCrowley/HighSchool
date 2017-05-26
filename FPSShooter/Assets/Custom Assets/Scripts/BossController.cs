using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour {
  private Animation anim;

	void Start () {
    anim = GetComponent<Animation>();
	}

	void Update () {
    anim.Play();
	}
}
