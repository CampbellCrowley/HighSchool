using UnityEngine;
using System.Collections;

public class VehicleController : MonoBehaviour {
  public bool canFly = false;
  public bool bankTurn = true;
  public bool isChildScript = true;
  public float camDistance = 4f;
  public float moveSpeed = 3f;
  public float sprintMultiplier = 3f;

  private float forward = 0;
  private float turn = 0;
  private float lookH = 0;
  private float lookV = 0;
  private float sprint = 0;
  private Camera cam;

  private Rigidbody rbody;

  public
   void Start() { rbody = GetComponent<Rigidbody>(); }

  public
   void UpdateInputs(float forward, float turn, float lookH, float lookV,
                     float sprint, Camera cam) {
     this.forward = forward;
     this.turn = turn;
     this.lookH = lookH;
     this.lookV = lookV;
     this.sprint = sprint;
     this.cam = cam;
   }

  public void FixedUpdate() {
    transform.position = rbody.position;
    transform.rotation = rbody.rotation;
  }

  public
   void Update() {
     rbody.rotation = Quaternion.Euler(0, rbody.rotation.eulerAngles.y, 0);
     if (!GameData.inVehicle) return;
     if (!isChildScript) {
       forward = Input.GetAxis("Vertical");
       turn = Input.GetAxis("Horizontal");
       sprint = Input.GetAxis("Sprint");
       lookH = Input.GetAxis("Mouse X");
       lookV = Input.GetAxis("Mouse Y");
     }

     if (Mathf.Abs(forward) > 0.2) {
       rbody.rotation = Quaternion.Euler(0, rbody.rotation.eulerAngles.y + turn,
                                         turn * -10f);
     }
     rbody.velocity =
         (forward > 0
              ? (moveSpeed + moveSpeed * sprint * (sprintMultiplier - 1)) *
                    (Mathf.Abs(forward))
              : -0.5f) *
         (Quaternion.Euler(0, rbody.rotation.eulerAngles.y, 0) *
          Vector3.forward);

     rbody.position = new Vector3(
         rbody.position.x, TerrainGenerator.waterHeight, rbody.position.z);

     cam.transform.rotation =
         Quaternion.Euler(cam.transform.eulerAngles.x - lookV,
                          cam.transform.eulerAngles.y + lookH, 0);

     Vector3 newCameraPos =
         Vector3.ClampMagnitude(
             (Vector3.left *
                  (Mathf.Sin(cam.transform.eulerAngles.y / 180f * Mathf.PI) -
                   Mathf.Sin(cam.transform.eulerAngles.y / 180f * Mathf.PI) *
                       Mathf.Sin((-45f + cam.transform.eulerAngles.x) / 90f *
                                 Mathf.PI)) +
              Vector3.back *
                  (Mathf.Cos(cam.transform.eulerAngles.y / 180f * Mathf.PI) -
                   Mathf.Cos(cam.transform.eulerAngles.y / 180f * Mathf.PI) *
                       Mathf.Sin((-45f + cam.transform.eulerAngles.x) / 90f *
                                 Mathf.PI)) +
              Vector3.up *
                  Mathf.Sin(cam.transform.eulerAngles.x / 180f * Mathf.PI)),
             1.0f) *
         camDistance;
     newCameraPos += rbody.position + Vector3.up * 2f;
     cam.transform.position = newCameraPos;
   }
 }
