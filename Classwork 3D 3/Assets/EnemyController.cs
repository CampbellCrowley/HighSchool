using UnityEngine;

public
class EnemyController : MonoBehaviour {
 public
  int health = 5;
 private Vector3 startPos;
 public
  void Start() { startPos = transform.position; }
 public
  void kill() { Destroy(gameObject); }
 public
  void Update() {
    transform.position = Vector3.Lerp(startPos, startPos + Vector3.forward * 3,
                                      Mathf.Abs(Time.time % 2 - 1));
    if(health==3) {
      GetComponent<Material>().color = Color.red;
    }
 }
}
