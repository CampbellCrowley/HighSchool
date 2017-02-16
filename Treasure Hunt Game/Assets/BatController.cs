using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public
class BatController : MonoBehaviour {
 private
  float forward = 0f;
 private
  Animator anim;
 public
  Transform player;
 public
  Terrain ground;
 public
  float moveSpeed = 0.5f;
 public
  float lookSpeed = 1.0f;

  void Start() {
    anim = GetComponent<Animator>();
  }

  void FixedUpdate() {
    transform.rotation = Quaternion.identity;

    float TerrainHeight = ground.SampleHeight(transform.position);

    Vector3 movement = Vector3.MoveTowards(transform.position,
                                           player.position + Vector3.up * 1.5f,
                                           moveSpeed * Time.deltaTime);
    forward = (transform.position - movement).magnitude /
              (moveSpeed * Time.deltaTime);
    transform.position = movement;
    if (transform.position.y < TerrainHeight) {
      transform.position = new Vector3(transform.position.x, TerrainHeight,
                                       transform.position.z);
    }
    Quaternion rotation = Quaternion.LookRotation(
        (player.position - transform.position).normalized);
    transform.rotation = rotation;
  }

  void OnAnimatorIK() { anim.SetFloat("Speed", forward); }
}
