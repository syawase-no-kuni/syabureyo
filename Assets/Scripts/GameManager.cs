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

    // ゲーム終了？
    bool gameEnd = false;

    // 口に入れられた状態か（スコア計算用）
    bool piston = false;
    float backPosY = -1f;

    // スコアの伸び方係数
    float scoreWeight = 0.7f;

    // ゲームクリア失敗時のリトライ画面
    [SerializeField]
    Canvas failedCanvas;

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
        simpleModel.isGameEnd += isGameEnd;
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
            float mousePostionY = simpleModel.MousePostionY;
            //Debug.Log(dragMgrY);

            // きのこを口に入れたらピストンフラグをオンにし、
            // 後ろに下げた分だけスコアが増える
            if (mousePostionY >= 0.99f && !piston)
            {
                AddGauge((1 - backPosY) * scoreWeight);
                scoreWeight = (100f - gaugeCounter.GaugeCount / 2.3f) / 10f;

                backPosY = 1.0f;
                piston = true;

                // エクスタシーゲージが満タンになった
                if (isGameClear())
                {
                    Debug.Log("iku");
                    timeCounter.EndTimeCount();
                    StartCoroutine("GameClear");
                    break;
                }

            }
            // きのこをどれだけ後ろに下げたかを取得
            backPosY = Math.Min(backPosY, mousePostionY);
            if (backPosY <= 0.0f)
            {
                piston = false;
            }

            // 時間切れ処理
            if (TimeOut())
            {
                Debug.Log("time out");
                StartCoroutine("GameFailed");
                break;
            }

            // ゲージの枠の色を変化
            SetSliderOutlineColor();

            yield return null;
        }
    }

    // ゲームクリア時
    IEnumerator GameClear()
    {
        gameEnd = true;
        for (int i = 0; i < 20; i++)
        {
            yield return null;
        }
        var screenOverlay = Camera.main.GetComponent<UnityStandardAssets.ImageEffects.ScreenOverlay>();
        for (int i = 0; i < 60; i++)
        {
            screenOverlay.intensity += 0.05f;
            yield return null;
        }
        for (int i = 0; i < 60; i++)
        {
            screenOverlay.intensity -= 0.05f;
            yield return null;
        }
        while (true)
        {
            simpleModel.MousePostionX = 0.5f;

            // きのこが徐々に小さく・下に落ちていく
            if (simpleModel.MousePostionY > -1.0f)
            {
                simpleModel.MousePostionY -= (simpleModel.MousePostionY + 1.0f) / 60f;
            }

            yield return null;
        }
    }

    // ゲームクリア失敗時
    IEnumerator GameFailed()
    {
        gameEnd = true;
        for (int i = 0; i < 30; i++)
        {
            yield return null;
        }
        for (int i = 0; i < 180; i++)
        {
            simpleModel.MousePostionX = 0.5f;

            // きのこが徐々に小さく・下に落ちていく
            if (simpleModel.MousePostionY > -1.0f)
            {
                simpleModel.MousePostionY -= (simpleModel.MousePostionY + 1.0f) / 60f;
            }
            if (simpleModel.MatsutakeSize > 0f)
            {
                simpleModel.SetMatsutakeScale(simpleModel.MatsutakeSize - simpleModel.MatsutakeSize / 60f);
            }
            yield return null;
        }
        var screenOverlay = Camera.main.GetComponent<UnityStandardAssets.ImageEffects.ScreenOverlay>();
        for (int i = 0; i < 60; i++)
        {
            screenOverlay.intensity -= 0.05f;
            yield return null;
        }
        
        // ゲームクリア失敗画面を表示
        failedCanvas.gameObject.SetActive(true);
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
        simpleModel.SetFaceRed(gaugeCounter.GaugeCount / 100f);
    }

    // ゲーム開始の判定
    public bool isGameStart()
    {
        return gameStart;
    }

    // ゲーム終了の判定（成否問わず）
    public bool isGameEnd()
    {
        return gameEnd;
    }

    // ゲームクリアの判定
    public bool isGameClear()
    {
        return gaugeCounter.isGaugeMax();
    }

    // 制限時間終了
    public bool TimeOut()
    {
        return timeCounter.isTimeOut();
    }


}
