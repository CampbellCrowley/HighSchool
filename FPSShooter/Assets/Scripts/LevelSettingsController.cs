using System;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
#pragma warning disable 0168

public
class LevelSettingsController : MonoBehaviour {
 public
  void Awake() {
    Debug.Log("Vignette: " + GameData.vignette + ", DOF: " + GameData.dof +
              ", Blur: " + GameData.motionBlur + ", Bloom: " +
              GameData.bloomAndFlares);
    try {
      GameObject.FindObjectsOfType<VignetteAndChromaticAberration>()[0]
          .enabled = GameData.vignette;
    } catch (IndexOutOfRangeException e) {
    }
    try {
      GameObject.FindObjectsOfType<DepthOfField>()[0].enabled = GameData.dof;
    } catch (IndexOutOfRangeException e) {
    }
    try {
      GameObject.FindObjectsOfType<CameraMotionBlur>()[0].enabled =
          GameData.motionBlur;
    } catch (IndexOutOfRangeException e) {
    }
    try {
      GameObject.FindObjectsOfType<BloomAndFlares>()[0].enabled =
          GameData.bloomAndFlares;
    } catch (IndexOutOfRangeException e) {
    }
  }
  void Update() {
    try {
      GameObject.FindObjectsOfType<VignetteAndChromaticAberration>()[0]
          .enabled = GameData.vignette;
    } catch (IndexOutOfRangeException e) {
    }
    try {
      GameObject.FindObjectsOfType<DepthOfField>()[0].enabled = GameData.dof;
    } catch (IndexOutOfRangeException e) {
    }
    try {
      GameObject.FindObjectsOfType<CameraMotionBlur>()[0].enabled =
          GameData.motionBlur;
    } catch (IndexOutOfRangeException e) {
    }
    try {
      GameObject.FindObjectsOfType<BloomAndFlares>()[0].enabled =
          GameData.bloomAndFlares;
    } catch (IndexOutOfRangeException e) {
    }
  }
}
