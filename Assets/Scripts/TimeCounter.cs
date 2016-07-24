using UnityEngine;
using System.Collections;

public class TimeCounter : MonoBehaviour {

    float timeCount = 30.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        timeCount -= Time.deltaTime;
        if (timeCount <= 0.0f)
        {
            // 終了
        }

        Debug.Log(timeCount);
	}
}
