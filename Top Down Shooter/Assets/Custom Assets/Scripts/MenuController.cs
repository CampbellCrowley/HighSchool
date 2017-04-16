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
  Vector3 initialSudoPosition;
 private
  Quaternion initialSudoRotation;
 private
  bool overrideToggle = false;

 public
  void Awake() {
    if (SudoPlayer_ != null) {
      initialMoveSpeed = SudoPlayer_.moveSpeed;
      initialSudoPosition = Camera1.transform.localPosition;
      initialSudoRotation = Camera1.transform.localRotation;
    }
    foreach (GUIText OSD in FindObjectsOfType<GUIText>()) {
      OSD.enabled = false;
    }
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
    overrideToggle = true;
    Toggle temp = GameObject.Find("Toggle Vignette").GetComponent<Toggle>();
    if (temp != null) temp.isOn = GameData.vignette;
    temp = GameObject.Find("Toggle DOF").GetComponent<Toggle>();
    if (temp != null) temp.isOn = GameData.dof;
    temp = GameObject.Find("Toggle Motion Blur").GetComponent<Toggle>();
    if (temp != null) temp.isOn = GameData.motionBlur;
    temp = GameObject.Find("Toggle Bloom and Flare").GetComponent<Toggle>();
    if (temp != null) temp.isOn = GameData.bloomAndFlares;
    temp = GameObject.Find("Toggle Fullscreen").GetComponent<Toggle>();
    if (temp != null) temp.isOn = GameData.fullscreen;
    temp = GameObject.Find("Toggle Sound Effects").GetComponent<Toggle>();
    if (temp != null) temp.isOn = GameData.soundEffects;
    temp = GameObject.Find("Toggle Music").GetComponent<Toggle>();
    if (temp != null) temp.isOn = GameData.music;
    temp = GameObject.Find("Toggle Camera Damping").GetComponent<Toggle>();
    if (temp != null) temp.isOn = GameData.cameraDamping;
    overrideToggle = false;

    GameData.showCursor = true;
    Camera1.SetActive(false);
    Camera2.SetActive(false);
    SettingsCamera.SetActive(true);
  }
 public
  void CloseSettings() {
    GameData.showCursor = true;
    GameData.SaveSettings();
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
    } else if (SudoPlayer_ != null) {
      bool escape = Input.GetAxis("Cancel") > 0.5f;
      if (escape && inTutorial) {
        ExitTutorial();
      } else if (!inTutorial) {
        Camera1.transform.localPosition =
            Vector3.Lerp(Camera1.transform.localPosition, initialSudoPosition,
                         0.15f * Time.deltaTime);
        Camera1.transform.localRotation =
            Quaternion.Lerp(Camera1.transform.localRotation, initialSudoRotation,
                            0.15f * Time.deltaTime);
        SudoPlayer_.moveSpeed = Mathf.Lerp(
            SudoPlayer_.moveSpeed, initialMoveSpeed, 0.3f * Time.deltaTime);
      }
    }
  }

 public
  void ToggleVignette() {
    if (overrideToggle) return;
    GameData.vignette = !GameData.vignette;
  }
 public
  void ToggleDOF() {
    if (overrideToggle) return;
    GameData.dof = !GameData.dof;
  }
 public
  void ToggleMotionBlur() {
    if (overrideToggle) return;
    GameData.motionBlur = !GameData.motionBlur;
  }
 public
  void ToggleBloomAndFlare() {
    if (overrideToggle) return;
    GameData.bloomAndFlares = !GameData.bloomAndFlares;
  }
 public
  void ToggleFullscreen() {
    if (overrideToggle) return;
    GameData.fullscreen = !GameData.fullscreen;
    Screen.fullScreen = GameData.fullscreen;
  }
 public
  void ToggleSoundEffects() {
    if (overrideToggle) return;
    GameData.soundEffects = !GameData.soundEffects;
  }
 public
  void ToggleMusic() {
    if (overrideToggle) return;
    GameData.music = !GameData.music;
  }
 public
  void ToggleCameraDamping() {
    if (overrideToggle) return;
    GameData.cameraDamping = !GameData.cameraDamping;
  }
}
