using System;
using UnityEngine;
using System.Collections;

public class MatsutakeManager : MonoBehaviour {

    bool freeMoveFlg = true;
    Vector3 screenPoint;

    // ゲーム開始フラグが立つ位置
    readonly Vector3 gameStartPosition = new Vector3(-0.02f, -0.4f, -5f);

    // 上下の動ける下限・上限
    readonly float movableMaxY = Screen.height / 2.5f;
    readonly float movableMinY = Screen.height / 8.0f;
    
    // GameManagerクラスのgameStartメソッドを呼び出すマン
    public event Action gameStart;

    // Use this for initialization
    void Start () {
        StartCoroutine("MouseTest");
    }
	
	// Update is called once per frame
	void Update () {
	}

    IEnumerator MouseTest()
    {
        while (true)
        {
            // マウス座標を取得する
            screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

            // マウスの移動量を取得
            /*
            float sensitivity = 0.1f;
            float mouse_move_x = Input.GetAxis("Mouse X") * sensitivity;
            float mouse_move_y = Input.GetAxis("Mouse Y") * sensitivity;
            Debug.Log(mouse_move_x + " : " + mouse_move_y);
            */

            /*
            if(gotScore != null)
            {
                gotScore(10);
            }
            */

            if (freeMoveFlg)
            {
                // マウスに追従するようにきのこを動かせる
                transform.position = Camera.main.ScreenToWorldPoint(mousePosition);

                // ゲーム開始フラグが立つ位置にきのこが動いたら開始
                if (Math.Abs(transform.position.x - gameStartPosition.x) <= 0.01f &&
                    Math.Abs(transform.position.y - gameStartPosition.y) <= 0.02f)
                {
                    Debug.Log("game start");
                    gameStart();
                    Destroy(this.gameObject);
                }
            }
            else
            {
                // 上下にのみきのこを動かせる(動ける範囲に制限あり)
                // X軸を中央（口のある座標）に固定
                mousePosition = new Vector3(Screen.width / 2f - 5f, Mathf.Clamp(mousePosition.y, movableMinY, movableMaxY), mousePosition.z);
                
                transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
            }

            yield return null;
        }
    }
}
