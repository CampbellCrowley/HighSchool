using UnityEngine;
using System.Collections;

public
class InitPlayer : MonoBehaviour {
  // Height off the ground to spawn
  [SerializeField] public float spawnHeight = 2;
  [SerializeField] public float moveSpeed = 2;
  [SerializeField] public bool isMainMenu = false;
  [SerializeField] public TerrainGenerator ground;

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
      float groundHeight = ground.GetTerrainHeight(this) + 50f;
      if(groundHeight == 50f) groundHeight = transform.position.y;
      transform.position += Vector3.forward * Time.deltaTime * moveSpeed;
      transform.position =
          Mathf.Lerp(transform.position.y, groundHeight,
                     1.0f * Time.deltaTime) *
              Vector3.up +
          transform.position.x * Vector3.right +
          transform.position.z * Vector3.forward;
    } else {
      transform.position += Vector3.forward * Time.deltaTime * moveSpeed;
    }
  }
}
