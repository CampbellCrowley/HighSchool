﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public
class GameData : MonoBehaviour {
 public
  static GameData Instance;
 public
  static AudioSource MusicPlayer;
 public
  void Awake() {
    if (Instance == null) {
      DontDestroyOnLoad(gameObject);
      Instance = this;
    } else if (Instance != this) {
      Destroy(gameObject);
    }
    MusicPlayer = GetComponent<AudioSource>();
  }
 public
  static int collectedCollectibles = 100;
 public
  static int health = 10;
 public
  static bool showCursor = true;
 public
  static bool isPaused = false;
 public
  static int numEnemies = 0;

 private
  static int neededCollectibles = 3;

 public
  static int getLevel() { return SceneManager.GetActiveScene().buildIndex; }
 public
  static bool levelComplete() {
    return collectedCollectibles >= neededCollectibles;
  }
 public
  void NextLevel() {
    collectedCollectibles = 0;
    Debug.Log("Next Level!");
    SceneManager.LoadScene(getLevel() + 1);
  }
 public
  static void nextLevel() {
    collectedCollectibles = 0;
    Debug.Log("Next Level!");
    SceneManager.LoadScene(getLevel() + 1);
  }
 public
  static void restartLevel() {
    Debug.Log("Restarting Level!");
    collectedCollectibles = 0;
    SceneManager.LoadScene(getLevel());
  }
 public
  static void MainMenu() {
    Debug.Log("Menu!");
    SceneManager.LoadScene(0);
  }
 public
  static void quit() {
    Debug.Log("Exiting Game");
    Application.Quit();
  }

 public
  void Update() {
    Cursor.visible = showCursor;
    if (Input.GetAxis("Skip") > 0.5f) {
      Debug.Log("Skip Button Pressed.");
      nextLevel();
    } else if (Input.GetAxis("MainMenu") > 0.5f) {
      Debug.Log("Main Menu Button Pressed.");
      MainMenu();
    }
    float goalVol = music ? 0.5f : 0.0f;
    if (MusicPlayer != null)
      MusicPlayer.volume = Mathf.Lerp(MusicPlayer.volume, goalVol, 0.1f);
  }

  public static bool vignette = true;
  public static bool dof = true;
  public static bool motionBlur = true;
  public static bool bloomAndFlares = false;
  public static bool fullscreen = true;
  public static bool soundEffects = true;
  public static bool music = true;
  public static bool cameraDamping = true;
}
