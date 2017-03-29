using UnityEngine;
using System.Collections;

public
class InitPlayer : MonoBehaviour {
  // Height off the ground to spawn
  [SerializeField] public float spawnHeight = 2;
  [SerializeField] public float moveSpeed = 2;
  [SerializeField] public TerrainGenerator ground;
  [SerializeField] public bool isMainMenu = false;

 private
  bool spawned = false;

 public
  void go(float x, float y, float z) {
    if (!spawned) {
      transform.position = new Vector3(x, y + spawnHeight, z);
      Debug.Log("Player Spawned\n" + transform.position);
      spawned = true;
    }
  }
 public
  void updatePosition(float x, float y, float z) {
    transform.position = new Vector3(x, y, z);
    Debug.Log("Player moved to " + transform.position);
  }
 public
  Transform getTransform() { return transform; }

 public
  void Update() {
    if (!isMainMenu) return;
    if (ground != null) {
      transform.position += Vector3.forward * Time.deltaTime * moveSpeed;
      transform.position =
          Mathf.Lerp(transform.position.y, ground.GetTerrainHeight() + 50f,
                     3.0f * Time.deltaTime) *
              Vector3.up +
          transform.position.x * Vector3.right +
          transform.position.z * Vector3.forward;
    } else {
      transform.position += Vector3.forward * Time.deltaTime * moveSpeed;
    }
  }
}
