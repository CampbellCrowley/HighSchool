using UnityEngine;
using System.Collections;

public class VehicleController : MonoBehaviour {
  public bool canFly = false;
  public bool isBoat = true;
  public bool bankTurn = true;
  public bool isChildScript = true;
  public float camDistance = 4f;
  public float moveSpeed = 3f;
  public float sprintMultiplier = 3f;
  public float acceleration = 0.2f;
  public float turnRate = 2.0f;
  public float maxSlope = 45f;
  public float fuelRemaining = 60f;

  private float forward = 0;
  private float forward_actual = 0;
  private float turn = 0;
  private float lookH = 0;
  private float lookV = 0;
  private float sprint = 0;
  private float lastVelocity = 0;
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
     if (isBoat) {
       rbody.rotation = Quaternion.Euler(0, rbody.rotation.eulerAngles.y, 0);
     }
     if (GameData.Vehicle != this) return;
     fuelRemaining -= Time.deltaTime;
     if (fuelRemaining < 0) {
       if (GameData.Vehicle = this) GameData.Vehicle = null;
       return;
     }
     if (!isChildScript) {
       forward = Input.GetAxis("Vertical");
       turn = Input.GetAxis("Horizontal");
       sprint = Input.GetAxis("Sprint");
       lookH = Input.GetAxis("Mouse X");
       lookV = Input.GetAxis("Mouse Y");
     }


     float forward_goal = forward;
     if (forward_actual < forward_goal)
       forward_actual += acceleration * Time.deltaTime;
     if (forward_actual > forward_goal)
       forward_actual -= acceleration * Time.deltaTime;
     if ((rbody.rotation.eulerAngles.x < maxSlope ||
          rbody.rotation.eulerAngles.x > 360 - maxSlope) &&
         (rbody.rotation.eulerAngles.z < maxSlope ||
          rbody.rotation.eulerAngles.z > 360 - maxSlope)) {
       if (Mathf.Abs(forward_actual) > 0.2 && bankTurn && isBoat) {
         rbody.rotation = Quaternion.Euler(
             0, rbody.rotation.eulerAngles.y + turn, turn * -10f);
       } else if (Mathf.Abs(forward_actual) > 0.2) {
         rbody.rotation = Quaternion.Euler(
             rbody.rotation.eulerAngles.x,
             rbody.rotation.eulerAngles.y + turn * forward_actual * turnRate,
             rbody.rotation.eulerAngles.z);
       }
       rbody.velocity =
           ((forward_actual > 0
                 ? (moveSpeed + moveSpeed * sprint * (sprintMultiplier - 1)) *
                       (Mathf.Abs(forward_actual))
                 : 0.5f * moveSpeed * forward_actual) *
            (Quaternion.Euler(0, rbody.rotation.eulerAngles.y, 0) *
             Vector3.forward)) +
           Vector3.up * rbody.velocity.y;
     }

     if(isBoat) {
       rbody.position = new Vector3(
           rbody.position.x, TerrainGenerator.waterHeight, rbody.position.z);
     } else {
       rbody.velocity += Vector3.up * -9.81f * Time.deltaTime;
     }

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

     if (lastVelocity - rbody.velocity.magnitude > 5) {
       GameData.health--;
     }
     Debug.Log(lastVelocity - rbody.velocity.magnitude);
     lastVelocity = rbody.velocity.magnitude;
   }
 }
