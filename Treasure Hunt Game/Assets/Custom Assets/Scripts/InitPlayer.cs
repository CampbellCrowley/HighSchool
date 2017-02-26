using UnityEngine;
using System.Collections;

public
class InitPlayer : MonoBehaviour {
  // Height off the ground to spawn
  [SerializeField] public float spawnHeight = 2;
  [SerializeField] public float moveSpeed = 2;
  [SerializeField] public TerrainGenerator Ground;

 public
  void Start() { GameData.showCursor = true; }

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
   if(Ground != null) {
    transform.position += Vector3.forward * Time.deltaTime * moveSpeed;
    transform.position =
        Mathf.Lerp(transform.position.y, Ground.GetTerrainHeight() + 50f,
                   3.0f * Time.deltaTime) *
            Vector3.up +
        transform.position.x * Vector3.right +
        transform.position.z * Vector3.forward;
   } else {
    transform.position += Vector3.forward * Time.deltaTime * moveSpeed;
   }
 }
}
