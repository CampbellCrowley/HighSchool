using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public
class GameData : MonoBehaviour {
 public
  static GameData Instance;
 public
  AudioSource MusicPlayer;
 public
  AudioClip QueuedMusic;
 public
  GameObject PauseMenu;
 private
  GameObject PauseMenu_;
 public
  void Awake() {
    if (Instance == null) {
      MusicPlayer = GetComponent<AudioSource>();
      //QueuedMusic = MusicPlayer.clip;
      DontDestroyOnLoad(gameObject);
      Instance = this;
    } else if (Instance != this) {
      if (GetComponent<AudioSource>().clip != null) {
        Instance.QueuedMusic = GetComponent<AudioSource>().clip;
      } else if (QueuedMusic != null) {
        Instance.QueuedMusic = QueuedMusic;
      }
      Destroy(gameObject);
    }
  }
 public
  void Start() {
    LoadSettings();
    if (MusicPlayer != null && !music) {
      MusicPlayer.volume = 0.0f;
    }
  }
 public
  static int health = 5;
 public
  static int tries = 3;
 public
  static int collectedCollectibles = 10000;
 public
  static bool showCursor = true;
 public
  static bool isPaused = false;
 public
  static VehicleController Vehicle;
 public
  static string username = "Username";
 public
  static int numEnemies = 0;
 public
  static int numVehicles = 0;
 public
  static bool levelComplete() {
    return true;
  }
 public
  static int getLevel() {
    // return SceneManager
    //     .GetSceneByName(UnityEngine.Networking.MyNetworkManagerHUD.getLevel())
    //     .buildIndex;
    return SceneManager.GetActiveScene().buildIndex;
  }
 public
  static void gotoLevel(int level) {
    int nextIndex = level;
    Debug.Log("Goto Level! (" + nextIndex + ")");
    GameData.Vehicle = null;
    GameData.isPaused = false;
    SceneManager.LoadScene(nextIndex);
    FindObjectOfType<UnityEngine.Networking.NetworkManager>()
        .ServerChangeScene(SceneManager.GetSceneByBuildIndex(nextIndex).name);
  }
 public
  static void nextLevel() {
    int nextIndex = getLevel() + 1;
    Debug.Log("Next Level! (" + nextIndex + ")");
    GameData.Vehicle = null;
    GameData.isPaused = false;
    // UnityEngine.Networking.MyNetworkManagerHUD.changeLevel(
    //    SceneManager.GetSceneByBuildIndex(getLevel() + 1));
    SceneManager.LoadScene(nextIndex);
    FindObjectOfType<UnityEngine.Networking.NetworkManager>()
        .ServerChangeScene(SceneManager.GetSceneByBuildIndex(nextIndex).name);
  }
 public
  static void restartLevel() {
    Debug.Log("Restarting Level!");
    GameData.Vehicle = null;
    GameData.isPaused = false;
    GameData.health = 5;
    FindObjectOfType<UnityEngine.Networking.NetworkManager>()
        .ServerChangeScene(SceneManager.GetSceneByBuildIndex(getLevel()).name);
  }
 public
  static void MainMenu() {
    Debug.Log("Menu!");
    GameData.Vehicle = null;
    GameData.isPaused = false;
    // SceneManager.UnloadSceneAsync("DontDestroyOnLoad");
    // SceneManager.LoadScene(0);
    // UnityEngine.Networking.MyNetworkManagerHUD.manager.ServerChangeScene(
    //     SceneManager.GetSceneByBuildIndex(0).name);
    // UnityEngine.Networking.NetworkManager.Shutdown();
    // UnityEngine.Networking.MyNetworkManagerHUD.manager.StopClient();
    // UnityEngine.Networking.ClientScene.DestroyAllClientObjects();
    FindObjectOfType<UnityEngine.Networking.NetworkManager>().StopHost();
    // UnityEngine.Networking.NetworkClient.ShutdownAll();
    GameData.health = 5;
    GameData.tries = 3;
  }
 public
  static void quit() {
    Debug.Log("Exiting Game");
    Application.Quit();
  }

 public
  void Update() {
    if (Vehicle != null && Vehicle.name.Contains("boat")) {
      if (getLevel() == 1) {
        nextLevel();
      } else if (getLevel() == 2) {
        GameObject Target = GameObject.FindWithTag("Target");
        GameObject Player = GameObject.FindWithTag("Player");
        Target.transform.localPosition = new Vector3(0, 0, 0);
        Target.transform.position = new Vector3(
            Target.transform.position.x, Target.transform.position.y, 10000);
        if (Vector3.Distance(Target.transform.position,
                             Player.transform.position) < 8000) {
          nextLevel();
        }
      }
    }

    if(Input.GetKeyDown("b") && getLevel() != 0) {
      gotoLevel(4);
    }
    if(Input.GetButtonDown("Pause") && getLevel() != 0) {
      GameData.isPaused = !GameData.isPaused;
      GameData.showCursor = isPaused;
      if(GameData.isPaused) {
        PauseMenu_ = Instantiate(PauseMenu);
        PauseMenu_.GetComponent<Canvas>().worldCamera = Camera.main;
      } else {
        Destroy(PauseMenu_);
      }
    } else if (Input.GetButtonDown("Menu") && getLevel() != 0) {
      MainMenu();
    }
    Cursor.visible = showCursor;
    Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Locked;
    if (getLevel() > 0 && Input.GetAxis("Skip") > 0.5f) {
      nextLevel();
    }
    if (MusicPlayer != null) {
      float goalVol = music ? 0.5f : 0.0f;
      if(QueuedMusic != null && QueuedMusic != MusicPlayer.clip) {
        goalVol = 0.0f;
        if(MusicPlayer.volume <= 0.001f) {
          Debug.Log("Playing Music: " + QueuedMusic.name);
          MusicPlayer.clip = QueuedMusic;
          MusicPlayer.Play();
        }
      }
      MusicPlayer.volume = Mathf.Lerp(MusicPlayer.volume, goalVol, 0.1f);
    }
  }

 public
  static void LoadSettings() {
    string debug = "Settings Loaded: [\n";
    if (PlayerPrefs.HasKey("Vignette")) {
      vignette = PlayerPrefs.GetInt("Vignette") == 1 ? true : false;
      debug += "Vignette: " + vignette + ",\n";
    }

    if (PlayerPrefs.HasKey("DOF")) {
      dof = PlayerPrefs.GetInt("DOF") == 1 ? true : false;
      debug += "DOF: " + dof + ",\n";
    }

    if (PlayerPrefs.HasKey("Motion Blur")) {
      motionBlur = PlayerPrefs.GetInt("Motion Blur") == 1 ? true : false;
      debug += "Motion Blur: " + motionBlur + ",\n";
    }

    if (PlayerPrefs.HasKey("Bloom and Flare")) {
      bloomAndFlares =
          PlayerPrefs.GetInt("Bloom and Flare") == 1 ? true : false;
      debug += "Bloom and Flare: " + bloomAndFlares + ",\n";
    }

    if (PlayerPrefs.HasKey("Fullscreen")) {
      fullscreen = PlayerPrefs.GetInt("Fullscreen") == 1 ? true : false;
      debug += "Fullscreen: " + fullscreen + ",\n";
    }

    if (PlayerPrefs.HasKey("Sound Effects")) {
      soundEffects = PlayerPrefs.GetInt("Sound Effects") == 1 ? true : false;
      debug += "Sound Effects: " + soundEffects + ",\n";
    }

    if (PlayerPrefs.HasKey("Music")) {
      music = PlayerPrefs.GetInt("Music") == 1 ? true : false;
      debug += "Music: " + music + ",\n";
    }

    if (PlayerPrefs.HasKey("Camera Damping")) {
      cameraDamping = PlayerPrefs.GetInt("Camera Damping") == 1 ? true : false;
      debug += "Camera Damping: " + cameraDamping + ",\n";
    }

    debug += "]";
    Debug.Log(debug);

    Screen.fullScreen = fullscreen;
  }

 public
  static void SaveSettings() {
    PlayerPrefs.SetInt("Vignette", vignette ? 1 : 0);
    PlayerPrefs.SetInt("DOF", dof ? 1 : 0);
    PlayerPrefs.SetInt("Motion Blur", motionBlur ? 1 : 0);
    PlayerPrefs.SetInt("Bloom and Flare", bloomAndFlares ? 1 : 0);
    PlayerPrefs.SetInt("Fullscreen", fullscreen ? 1 : 0);
    PlayerPrefs.SetInt("Sound Effects", soundEffects ? 1 : 0);
    PlayerPrefs.SetInt("Music", music ? 1 : 0);
    PlayerPrefs.SetInt("Camera Damping", cameraDamping ? 1 : 0);

    PlayerPrefs.Save();
  }

 public
  static bool vignette = true;
 public
  static bool dof = true;
 public
  static bool motionBlur = true;
 public
  static bool bloomAndFlares = false;
 public
  static bool fullscreen = true;
 public
  static bool soundEffects = true;
 public
  static bool music = true;
 public
  static bool cameraDamping = true;
}
