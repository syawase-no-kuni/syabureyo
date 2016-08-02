using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

    GaugeCounter gaugeCounter;
    TimeCounter timeCounter;

    MatsutakeManager matsutakeManager;
    SimpleModel simpleModel;
    AdonSpawner adonSpawner;

    bool gameStart = false;

    bool piston = false;
    float backPosY = -1f;

	// Use this for initialization
	void Start () {
        // GameManagerオブジェクト自身が持っている他のクラスはこの書き方で取得
        gaugeCounter = GetComponent<GaugeCounter>();
        timeCounter = GetComponent<TimeCounter>();

        // ゲーム開始時のまつたけクラスのgameStartイベントをフック
        matsutakeManager = FindObjectOfType<MatsutakeManager>();
        matsutakeManager.gameStart += GameStart;

        // SimpleModelクラス（ルナチャ）を取得
        simpleModel = FindObjectOfType<SimpleModel>();

        adonSpawner = FindObjectOfType<AdonSpawner>();
    }

    // Update is called once per frame
    void Update () {
	}

    // ゲーム開始処理
    public void GameStart()
    {
        simpleModel.GameStart();
        gameStart = true;
        StartCoroutine("GameMain");
    }

    // ゲームメインループ
    IEnumerator GameMain()
    {
        while (true)
        {
            // きのこのY座標を取得
            float dragMgrY = simpleModel.GetDragManagerY();
            Debug.Log(dragMgrY);

            // きのこを口に入れたらピストンフラグをオンにし、
            // 後ろに下げた分だけスコアが増える
            if (dragMgrY >= 0.99f && !piston)
            {
                AddGauge(1 - backPosY);
                backPosY = 1.0f;
                piston = true;
            }
            // きのこをどれだけ後ろに下げたかを取得
            backPosY = Math.Min(backPosY, dragMgrY);
            if (backPosY <= 0.0f)
            {
                piston = false;
            }

            yield return null;
        }
    }


    // エクスタシーゲームを増やす
    public void AddGauge(float add)
    {
        gaugeCounter.AddGauge(add);
        simpleModel.SetMatsutakeScale(gaugeCounter.GaugeCount / 50f);
        adonSpawner.Spawn();
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
