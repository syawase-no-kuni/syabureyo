using UnityEngine;
using System.Collections;

public class TimeCounter : MonoBehaviour {

    float timeCount = 30.0f;
    bool timeOutFlg = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (timeCount <= 0.0f)
        {
            // 終了
            timeOutFlg = true;
        }
        else
        {
            timeCount -= Time.deltaTime;
        }

        //Debug.Log(timeCount);
    }

    public bool isTimeOut()
    {
        return timeOutFlg;
    }

}
