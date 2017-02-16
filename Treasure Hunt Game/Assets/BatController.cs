using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public
class BatController : MonoBehaviour {
 private
  float forward = 0f;
 private
  float moveAngle = 0f;
 private
  Animator anim;
 public
  Transform player;
 private
  GameObject Camera;
 public
  float moveSpeed = 0.5f;
 public
  float lookSpeed = 1.0f;
 private
  Color startColor;

  void Start() {
    anim = GetComponent<Animator>();
    Camera = GameObject.Find("Main Camera");
    startColor = RenderSettings.fogColor;
  }

  void FixedUpdate() {
    transform.rotation = Quaternion.identity;

    Vector3 movement = Vector3.MoveTowards(transform.position,
                                           player.position + Vector3.up * 1.5f,
                                           moveSpeed * Time.deltaTime);
    forward = (transform.position - movement).magnitude /
              (moveSpeed * Time.deltaTime);
    transform.position = movement;
    float vignette = 0.0f;
    vignette = 0.45f - ((transform.position - player.position).magnitude / 50f);
    if (vignette >= 0) {
      Camera.GetComponent<VignetteAndChromaticAberration>().intensity =
          vignette;
      RenderSettings.fogStartDistance = 200 * (1 - (vignette / 0.45f));
      RenderSettings.fogEndDistance = 300 * (1 - (vignette / 0.45f));
      RenderSettings.fogColor =
          Color.Lerp(startColor, Color.red, (vignette / 0.45f));
    }

    Quaternion rotation = Quaternion.LookRotation(
        (player.position - transform.position).normalized);
    transform.rotation = rotation;
  }

  void OnAnimatorIK() { anim.SetFloat("Speed", forward); }
}
