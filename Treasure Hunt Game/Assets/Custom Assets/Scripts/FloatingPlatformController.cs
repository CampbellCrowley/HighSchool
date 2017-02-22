using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public
class FloatingPlatformController : MonoBehaviour {
 public
  Vector3 moveGoal;
 public
  float moveSpeed = 5f;
 private
  Vector3 startPos;
 private
  float percent;
 private
  bool towardsGoal = true;

  void Start() {
    startPos = transform.position;
    moveGoal += startPos;
  }

  void Update() {
    if (towardsGoal) {
      percent += moveSpeed * Time.deltaTime;
    } else {
      percent -= moveSpeed * Time.deltaTime;
    }
    float actualPercent = percent;
    if (actualPercent > 1) {
      if (actualPercent > 1.5) {
        towardsGoal = false;
      }
      actualPercent = 1f;
    } else if (actualPercent < 0) {
      if (actualPercent < -0.5f) {
        towardsGoal = true;
      }
      actualPercent = 0f;
    }

    transform.position = Vector3.Lerp(startPos, moveGoal, actualPercent);
  }
}
