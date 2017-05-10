using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public
class MoveDestination : MonoBehaviour {
  public GameObject[] Destinations;
  public GUIText goalGui;
  GameObject player;
  UnityEngine.AI.NavMeshAgent agent;
  int goal = 2;
  int goal_goal = 2;
  float lastSpawnTime = 0f;

  void Start() {
    agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    player = GameObject.FindGameObjectWithTag("Player");
    lastSpawnTime = Time.time;
  }

  void Update() {
    if (Vector3.Distance(player.transform.position, transform.position) < 10f) {
      goal = Destinations.Length;
    } else {
      goal = goal_goal;
    }
    // goalGui.text =
    //     "Goal: " + (goal == Destinations.Length ? "Player" : goal.ToString());
    if (goal == Destinations.Length) {
      agent.destination = player.transform.position;
    } else {
      agent.destination = Destinations[goal].transform.position;
      if (agent.remainingDistance < 1f) {
        goal_goal = UnityEngine.Random.Range(0, Destinations.Length);
      }
    }
    if (Time.time - lastSpawnTime > 0.5f &&
        GameObject.FindGameObjectsWithTag("Enemy").Length <= 100) {
      lastSpawnTime = Time.time;
      Instantiate(gameObject);
    }
  }
}
