using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
  public
   TerrainGenerator Terrain;

   [Header("Sudo Player")]
  public
   InitPlayer SudoPlayer;
  public
   GameObject Camera1;

   [Header("Real Player")]
  public
   InitPlayer RealPlayer;
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
     initialMoveSpeed = SudoPlayer.moveSpeed;
     initalSudoPosition = Camera1.transform.localPosition;
     initalSudoRotation = Camera1.transform.localRotation;
     GameData.showCursor = true;
   }

  public
   void PlayGame() {
     GameData.nextLevel();
     GameData.showCursor = true;
   }
  public
   void Settings() {
     GameData.Settings();
     GameData.showCursor = true;
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
         Camera2.transform.position = Camera1.transform.position;
         Camera2.transform.rotation = Camera1.transform.rotation;
         Camera1.SetActive(false);
         Camera2.SetActive(true);
         SudoPlayer.moveSpeed = 0f;
         RealPlayer.transform.position =
             Camera1.transform.position + Vector3.forward * 1000f;
         RealPlayer.transform.rotation = SudoPlayer.transform.rotation;
         Terrain.player = RealPlayer.transform.gameObject;
         Terrain.movePlayerToTop();
         inTransition = false;
       } else {
         Camera1.transform.position = Camera2.transform.position;
         Camera1.transform.rotation = Camera2.transform.rotation;
         Camera1.SetActive(true);
         Camera2.SetActive(false);
         Terrain.player = SudoPlayer.transform.gameObject;
         inTransition = false;
       }
     } else {
       bool escape = Input.GetAxis("Cancel") > 0.5f;
       if (escape && inTutorial) {
         ExitTutorial();
       } else if(!inTutorial) {
         Camera1.transform.localPosition =
             Vector3.Lerp(Camera1.transform.localPosition, initalSudoPosition,
                          0.15f * Time.deltaTime);
         Camera1.transform.localRotation =
             Quaternion.Lerp(Camera1.transform.localRotation, initalSudoRotation,
                          0.15f * Time.deltaTime);
         SudoPlayer.moveSpeed = Mathf.Lerp(
             SudoPlayer.moveSpeed, initialMoveSpeed, 0.3f * Time.deltaTime);
       }
     }
   }
 }
