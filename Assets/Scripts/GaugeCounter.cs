using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GaugeCounter : MonoBehaviour {
    
    float gaugeCount = 0.0f;
    public float GaugeCount
    {
        get { return gaugeCount; }
    }
    float gaugeCountBuf = 0.0f;

    const float gaugeMax = 100.0f;

    Slider slider;

	// Use this for initialization
	void Start () {
        slider = FindObjectOfType<Slider>();
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(gaugeCount);
        if (gaugeCountBuf < gaugeCount)
        {
            gaugeCountBuf += (gaugeCount - gaugeCountBuf) / 20.0f;
        }
        slider.value = gaugeCountBuf;
    }

    public void AddGauge(float add)
    {
        gaugeCount += add;
    }

    public bool isGaugeMax()
    {
        return (gaugeCount >= gaugeMax);
    }

}
