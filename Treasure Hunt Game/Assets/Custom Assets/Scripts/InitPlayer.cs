using UnityEngine;
using System.Collections;

public
class InitPlayer : MonoBehaviour {
  // Height off the ground to spawn
  [SerializeField] public float spawnHeight = 2;
  [SerializeField] public float moveSpeed = 2;

 public
  void go(float x, float y, float z) {
    transform.position = new Vector3(x, y + spawnHeight, z);
    Debug.Log("Player Spawned\n" + transform.position);
  }
 public
  void updatePosition(float x, float y, float z) {
    transform.position = new Vector3(x, y, z);
    Debug.Log("Player moved to " + transform.position);
  }
 public Transform getTransform() {
  return transform;
 }

 public void Update() {
    transform.position += Vector3.forward * Time.deltaTime * moveSpeed;
 }
}
