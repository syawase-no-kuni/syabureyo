using UnityEngine;
using System.Collections;

public class GaugeCounter : MonoBehaviour {

    float gaugeCount = 0.0f;
    public float GaugeCount
    {
        get { return gaugeCount; }
    }

    const float gaugeMax = 100.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
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
