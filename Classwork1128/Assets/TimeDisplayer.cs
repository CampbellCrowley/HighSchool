using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeDisplayer : MonoBehaviour {

  private GUIText text;
  private float time = 0f;
  private float realTimeAdjusted = 0f;
  private float offset = 0f;
	// Use this for initialization
	void Start () {
	  text = GetComponent<GUIText>();
    offset = Time.realtimeSinceStartup;
    realTimeAdjusted = Time.realtimeSinceStartup;
	}

	// Update is called once per frame
	void Update () {
    time += Time.deltaTime;
    realTimeAdjusted += Time.deltaTime;
	  text.text = "RealStartup: " + Time.realtimeSinceStartup +
	              "\nRealLoad: " + Time.timeSinceLevelLoad +
                "\nDelta: " + Time.deltaTime +
                "\nDeltaReal: " + time +
                "\nDeltaErrorStartup: " + (Time.realtimeSinceStartup - time) +
                "\nDeltaErrorLoad: " + (Time.timeSinceLevelLoad - time) +
                "\nLoadErrorStartup: " + (Time.realtimeSinceStartup - Time.timeSinceLevelLoad) +
                "\n\nFrames: " + Time.frameCount +
                "\nFPSRealLoad: " + Time.frameCount / Time.timeSinceLevelLoad;
	}
}
