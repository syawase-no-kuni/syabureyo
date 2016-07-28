using System;
using UnityEngine;
using System.Collections;

public class MatsutakeManager : MonoBehaviour {

    //bool freeMoveFlg = false;
    Vector3 screenPoint;

    readonly float movableMaxY = Screen.height / 2.0f;
    readonly float movableMinY = Screen.height / 4.0f;

    GameManager gameManager;

    public event Action<float> gotScore;

    [SerializeField]
    private Transform targetPosition = null;

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

            //if (freeMoveFlg)
            //{
            //    // マウスに追従するようにきのこを動かせる
            //    transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
            //}
            //else
            //{
            //    // 上下にのみきのこを動かせる(動ける範囲に制限あり)
            //    // X軸を中央（口のある座標）に固定
            //    mousePosition = new Vector3(Screen.width / 2.0f, mousePosition.y, mousePosition.z);

            //    // 上下の上限
            //    if (mousePosition.y >= movableMaxY)
            //    {
            //        mousePosition = new Vector3(mousePosition.x, movableMaxY, mousePosition.z);
            //    }
            //    else if (mousePosition.y <= movableMinY)
            //    {
            //        mousePosition = new Vector3(mousePosition.x, movableMinY, mousePosition.z);
            //    }

            //transform.RotateAround(new Vector3(0, 0, 0), new Vector3(0, 0, 1), 10);
            //transform.rotation = Quaternion.FromToRotation(this.transform.position, targetPosition.transform.position);
            Vector2 targetPos = targetPosition.transform.position;
            Vector2 nowPos = transform.position;
            //float angle = Vector2.Angle(nowPos, targetPos);
            //float angle = GetAim(nowPos, targetPos);
            float angle = GetAim(targetPos, nowPos);
            Debug.Log(angle);
            transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
            transform.Rotate(new Vector3(0, 0, 90));
            //transform.LookAt(targetPosition, new Vector3(0, 1, 0));
            transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
            //}

            yield return null;
        }
    }
    public float GetAim(Vector2 p1, Vector2 p2)
    {
        float dx = p2.x - p1.x;
        float dy = p2.y - p1.y;
        float rad = Mathf.Atan2(dy, dx);
        return rad * Mathf.Rad2Deg;
    }
}
