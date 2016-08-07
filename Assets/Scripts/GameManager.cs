using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

    // ゲージ管理クラス
    GaugeCounter gaugeCounter;

    // タイム管理クラス
    TimeCounter timeCounter;

    // スライダーのグラデーション情報
    [SerializeField]
    UnityEngine.UI.Image outlineTop;

    // 松茸管理クラス
    MatsutakeManager matsutakeManager;

    // live2dモデルクラス
    SimpleModel simpleModel;

    // ゲーム開始？
    bool gameStart = false;

    // 口に入れられた状態か（スコア計算用）
    bool piston = false;
    float backPosY = -1f;

    // Use this for initialization
    void Start () {
        // GameManagerオブジェクト自身が持っている他のクラスはこの書き方で取得
        gaugeCounter = GetComponent<GaugeCounter>();
        timeCounter = GetComponent<TimeCounter>();
        timeCounter.isGameStart += isGameStart;

        // ゲーム開始時のまつたけクラスのgameStartイベントをフック
        matsutakeManager = FindObjectOfType<MatsutakeManager>();
        matsutakeManager.gameStart += GameStart;

        // SimpleModelクラス（ルナチャ）を取得
        simpleModel = FindObjectOfType<SimpleModel>();
        simpleModel.isGameStart += isGameStart;
        
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
        timeCounter.StartTimeCount();
    }

    // ゲームメインループ
    IEnumerator GameMain()
    {
        while (true)
        {
            // きのこのY座標を取得
            float dragMgrY = simpleModel.GetDragManagerY();
            //Debug.Log(dragMgrY);

            // きのこを口に入れたらピストンフラグをオンにし、
            // 後ろに下げた分だけスコアが増える
            if (dragMgrY >= 0.99f && !piston)
            {
                AddGauge((1 - backPosY) * 3f);
                backPosY = 1.0f;
                piston = true;
            }
            // きのこをどれだけ後ろに下げたかを取得
            backPosY = Math.Min(backPosY, dragMgrY);
            if (backPosY <= 0.0f)
            {
                piston = false;
            }

            // ゲージの枠の色を変化
            SetSliderOutlineColor();

            yield return null;
        }
    }

    // スライダーの枠の色を徐々に赤くする
    void SetSliderOutlineColor()
    {
        Debug.Log(timeCounter.getNormalizedTime());
        outlineTop.fillAmount = timeCounter.getNormalizedTime();
    }

    // エクスタシーゲージを増やす
    public void AddGauge(float add)
    {
        gaugeCounter.AddGauge(add);
        simpleModel.SetMatsutakeScale(gaugeCounter.GaugeCount / 50f);
    }

    // ゲーム開始の判定
    public bool isGameStart()
    {
        return gameStart;
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
