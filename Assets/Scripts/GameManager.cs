using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    GaugeCounter gaugeCounter;
    TimeCounter timeCounter;
    MatsutakeManager matsutakeManager;

	// Use this for initialization
	void Start () {
        // GameManagerオブジェクト自身が持っている他のクラスはこの書き方で取得
        gaugeCounter = GetComponent<GaugeCounter>();
        timeCounter = GetComponent<TimeCounter>();

        // 別オブジェクトのきのこを取得
        // FindObjectOfTypeメソッドは、シーン全てを検索するらしい
        matsutakeManager = FindObjectOfType<MatsutakeManager>();

        matsutakeManager.gotScore += AddGauge;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // エクスタシーゲームを増やす
    public void AddGauge(float add)
    {
        gaugeCounter.AddGauge(add);
    }

    // ゲームクリアの判定
    public bool isGameClear()
    {
        return gaugeCounter.isGaugeMax();
    }

    // 制限時間終了
    public void TimeOut()
    {
        timeCounter.isTimeOut();
    }


}
