using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 0168

public
class MenuController : MonoBehaviour {
 public
  TerrainGenerator terrain;
 public
  GameObject SettingsCamera;

  [Header("Sudo Player")]
 public
  InitPlayer SudoPlayer_;
 public
  GameObject Camera1;

  [Header("Real Player")]
 public
  InitPlayer RealPlayer_;
 public
  GameObject Camera2;

 private
  bool inTutorial = false;
 private
  bool inTransition = false;
 private
  float initialMoveSpeed;
 private
  Vector3 initalSudoPosition;
 private
  Quaternion initalSudoRotation;

 public
  void Awake() {
    initialMoveSpeed = SudoPlayer_.moveSpeed;
    initalSudoPosition = Camera1.transform.localPosition;
    initalSudoRotation = Camera1.transform.localRotation;
    foreach (GUIText OSD in FindObjectsOfType<GUIText>()) {
      OSD.enabled = false;
    }

    try {
      GameObject.Find("Toggle Vignette").GetComponent<Toggle>().isOn =
          GameData.vignette;
      GameObject.Find("Toggle DOF").GetComponent<Toggle>().isOn = GameData.dof;
      GameObject.Find("Toggle Motion Blur").GetComponent<Toggle>().isOn =
          GameData.motionBlur;
      GameObject.Find("Toggle Bloom and Flare").GetComponent<Toggle>().isOn =
          GameData.bloomAndFlares;
      GameObject.Find("Toggle Fullscreen").GetComponent<Toggle>().isOn =
          GameData.fullscreen;
      GameObject.Find("Toggle Sound Effects").GetComponent<Toggle>().isOn =
          GameData.soundEffects;
      GameObject.Find("Toggle Music").GetComponent<Toggle>().isOn =
          GameData.music;
      GameObject.Find("Toggle Camera Damping").GetComponent<Toggle>().isOn =
          GameData.cameraDamping;
    } catch (NullReferenceException e) {}
    GameData.fullscreen = Screen.fullScreen;
  }
 public
  void Start() { GameData.showCursor = true; }

 public
  void MainMenu() { GameData.MainMenu(); }
 public
  void PlayGame() {
    GameData.showCursor = true;
    GameData.nextLevel();
  }
 public
  void OpenSettings() {
    GameData.showCursor = true;
    Camera1.SetActive(false);
    Camera2.SetActive(false);
    SettingsCamera.SetActive(true);
  }
 public
  void CloseSettings() {
    GameData.showCursor = true;
    Camera1.SetActive(true);
    Camera2.SetActive(false);
    SettingsCamera.SetActive(false);
  }
 public
  void PlayTutorial() {
    inTutorial = true;
    inTransition = true;
    GameData.showCursor = false;
  }
 public
  void ExitTutorial() {
    inTutorial = false;
    inTransition = true;
    GameData.showCursor = true;
  }
 public
  void quitGame() { GameData.quit(); }

 public
  void Update() {
    if (inTransition) {
      Button[] buttons =
          GameObject.FindObjectsOfType(typeof(Button)) as Button[];
      for (int i = 0; i < buttons.Length; i++) {
        buttons[i].interactable = !buttons[i].interactable;
      }
      if (inTutorial) {
        foreach (GUIText OSD in FindObjectsOfType<GUIText>()) {
          OSD.enabled = true;
        }
        Camera2.transform.position = Camera1.transform.position;
        Camera2.transform.rotation = Camera1.transform.rotation;
        Camera1.SetActive(false);
        Camera2.SetActive(true);
        SudoPlayer_.moveSpeed = 0f;
        RealPlayer_.transform.position =
            Camera1.transform.position + Vector3.forward * 1000f;
        RealPlayer_.transform.rotation = SudoPlayer_.transform.rotation;
        terrain.player = RealPlayer_.transform.gameObject;
        terrain.movePlayerToTop();
        inTransition = false;
      } else {
        foreach (GUIText OSD in FindObjectsOfType<GUIText>()) {
          OSD.enabled = false;
        }
        Camera1.transform.position = Camera2.transform.position;
        Camera1.transform.rotation = Camera2.transform.rotation;
        Camera1.SetActive(true);
        Camera2.SetActive(false);
        terrain.player = SudoPlayer_.transform.gameObject;
        inTransition = false;
      }
    } else {
      bool escape = Input.GetAxis("Cancel") > 0.5f;
      if (escape && inTutorial) {
        ExitTutorial();
      } else if (!inTutorial) {
        Camera1.transform.localPosition =
            Vector3.Lerp(Camera1.transform.localPosition, initalSudoPosition,
                         0.15f * Time.deltaTime);
        Camera1.transform.localRotation =
            Quaternion.Lerp(Camera1.transform.localRotation, initalSudoRotation,
                            0.15f * Time.deltaTime);
        SudoPlayer_.moveSpeed = Mathf.Lerp(
            SudoPlayer_.moveSpeed, initialMoveSpeed, 0.3f * Time.deltaTime);
      }
    }
  }

 public
  void ToggleVignette() { GameData.vignette = !GameData.vignette; }
 public
  void ToggleDOF() { GameData.dof = !GameData.dof; }
 public
  void ToggleMotionBlur() { GameData.motionBlur = !GameData.motionBlur; }
 public
  void ToggleBloomAndFlare() {
    GameData.bloomAndFlares = !GameData.bloomAndFlares;
  }
 public
  void ToggleFullscreen() {
    GameData.fullscreen = !GameData.fullscreen;
    Screen.fullScreen = GameData.fullscreen;
  }
 public
  void ToggleSoundEffects() { GameData.soundEffects = !GameData.soundEffects; }
 public
  void ToggleMusic() {
    GameData.music = !GameData.music;
  }
 public
  void ToggleCameraDamping() {
    GameData.cameraDamping = !GameData.cameraDamping;
  }
}
