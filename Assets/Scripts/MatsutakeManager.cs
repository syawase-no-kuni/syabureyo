using System;
using UnityEngine;
using System.Collections;

public class MatsutakeManager : MonoBehaviour {

    bool freeMoveFlg = false;
    Vector3 screenPoint;

    readonly float movableMaxY = Screen.height / 2.5f;
    readonly float movableMinY = Screen.height / 8.0f;

    GameManager gameManager;

    public event Action<float> gotScore;

    // Use this for initialization
    void Start () {
        gameManager = FindObjectOfType<GameManager>();

        StartCoroutine("MouseTest");
    }
	
	// Update is called once per frame
	void Update () {
	}

    IEnumerator MouseTest()
    {
        while (true)
        {
            // ???
            screenPoint = Camera.main.WorldToScreenPoint(transform.position);

            // ???
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

            // マウスの移動量を取得
            /*
            float sensitivity = 0.1f;
            float mouse_move_x = Input.GetAxis("Mouse X") * sensitivity;
            float mouse_move_y = Input.GetAxis("Mouse Y") * sensitivity;
            Debug.Log(mouse_move_x + " : " + mouse_move_y);
            */

            if(gotScore != null)
            {
                gotScore(10);
            }

            if (freeMoveFlg)
            {
                // マウスに追従するようにきのこを動かせる
                transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
            }
            else
            {
                // 上下にのみきのこを動かせる(動ける範囲に制限あり)
                // X軸を中央（口のある座標）に固定
                mousePosition = new Vector3(Screen.width / 2f - 5f, Mathf.Clamp(mousePosition.y, movableMinY, movableMaxY), mousePosition.z);

                // きのこを出し入れするときにOffsetをいじることで
                // きのこ上部が隠れて咥えているように見える
                //float offset = - (mousePosition.y - movableMinY) / ((movableMaxY - movableMinY) * 4);
                float offset = 0;
                float kuriY = (Screen.height / 4.0f);
                if (mousePosition.y >= kuriY)
                {
                    offset = -0.3f + (movableMaxY - mousePosition.y) / movableMaxY;
                }
                GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, offset*2));

                transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
            }

            yield return null;
        }
    }
}
