using UnityEngine;
using System.Collections;
using System;

public class TimeCounter : MonoBehaviour {

    const float maxTime = 20.0f;

    [SerializeField]
    float timeCount;

    bool timeOutFlg = false;

    public event Func<bool> isGameStart;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void StartTimeCount()
    {
        timeCount = maxTime;
        StartCoroutine("TimeCount");
    }

    public IEnumerator TimeCount()
    {
        while (true)
        {
            if (timeCount <= 0.0f)
            {
                // 終了
                timeOutFlg = true;
                break;
            }
            else
            {
                timeCount -= Time.deltaTime;
            }
            yield return null;
        }
    }

    public float getNormalizedTime()
    {
        return timeCount / maxTime;
    }

    public bool isTimeOut()
    {
        return timeOutFlg;
    }

}
