using UnityEngine;
using System.Collections;

public class MatsutakeManager : MonoBehaviour {

    bool freeMoveFlg = true;
    Vector3 screenPoint;

	// Use this for initialization
	void Start () {
        StartCoroutine("MouseTest");
    }
	
	// Update is called once per frame
	void Update () {
        /*
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Debug.Log(screenPoint);

        if (freeMoveFlg)
        {
            // 自由にきのこを動かすことができる
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
        }
        else
        {
            // 上下にのみきのこを動かすことができる
        }
        */
	}

    IEnumerator MouseTest()
    {
        while (true)
        {
            screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            transform.position = Camera.main.ScreenToWorldPoint(mousePosition);

            yield return null;
        }
    }
}
