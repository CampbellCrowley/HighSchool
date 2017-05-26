using UnityEngine;
using System.Collections;

public
class PortalController : MonoBehaviour {
  TerrainGenerator terrain;
  Vector3 startPosition;

 public
  void Start() {
    terrain = FindObjectOfType<TerrainGenerator>();
    startPosition = transform.position;
  }

 public
  void FixedUpdate() {
    if (terrain == null) {
      terrain = FindObjectOfType<TerrainGenerator>();
      return;
    }
    transform.position = new Vector3(
        startPosition.x,
        terrain.GetTerrainHeight(startPosition.x, startPosition.z) + 7f,
        startPosition.z);
  }
}
