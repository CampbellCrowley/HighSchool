using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public
class MoveDestination : MonoBehaviour {
  public GameObject[] Destinations;
  public GUIText goalGui;
  UnityEngine.AI.NavMeshAgent agent;
  int goal = 2;

  void Start() { agent = GetComponent<UnityEngine.AI.NavMeshAgent>(); }

  void Update() {
    goalGui.text = "Goal: " + goal;
    agent.destination = Destinations[goal].transform.position;
    if (agent.remainingDistance < 1f) {
      goal = UnityEngine.Random.Range(0, Destinations.Length);
    }
  }
}
