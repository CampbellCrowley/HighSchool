using UnityEngine;
using System.Collections;

public class Random : MonoBehaviour {
  public Rigidbody2D rbody;
  public Vector2 InitialVelocity;
  private LineRenderer line;
  private Vector3[] pathHistory;

  void Start() {
    pathHistory = new Vector3[1];
    line = gameObject.AddComponent<LineRenderer>();
    line.SetVertexCount(1);
    line.useWorldSpace = true;
    line.SetPositions(pathHistory);
    line.SetWidth(0.01F, 0.01F);
    rbody.velocity = InitialVelocity;
  }
	void FixedUpdate () {
    Vector3[] pathHistory_ = new Vector3[pathHistory.Length+1];
    for(int i=0; i<pathHistory_.Length-1; i++) {
      pathHistory_[i] = pathHistory[i];
    }
    pathHistory = pathHistory_;
    pathHistory[pathHistory.Length-1] = transform.position;
    line.SetVertexCount(pathHistory.Length);
    line.SetPositions(pathHistory);

	  rbody.AddForce(
        new Vector2(
             -transform.position.x*UnityEngine.Random.Range(0f,1f),
             -transform.position.y*UnityEngine.Random.Range(0f,1f)
        )
    );
    float maxVelocity = 50f;
    if(rbody.velocity.x> maxVelocity) rbody.velocity = new Vector2(maxVelocity, rbody.velocity.y);
    if(rbody.velocity.y> maxVelocity) rbody.velocity = new Vector2(rbody.velocity.x, maxVelocity);
    if(rbody.velocity.x<-maxVelocity) rbody.velocity = new Vector2(-maxVelocity, rbody.velocity.y);
    if(rbody.velocity.y<-maxVelocity) rbody.velocity = new Vector2(rbody.velocity.x, -maxVelocity);
	}
}
