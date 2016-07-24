using UnityEngine;
using System.Collections;

public class AudioTest : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        AudioManager.LoadBgm("title", "bgm_maoudamashii_healing10");
        AudioManager.LoadBgm("game", "bgm_maoudamashii_ethnic14");
    }

    // Update is called once per frame
    void Update()
    {
        // 右クリック
        if(Input.GetMouseButtonDown(0))
        {
            AudioManager.PlayBgm("title");
        }

        // 左クリック
        if(Input.GetMouseButtonDown(1))
        {
            AudioManager.PlayBgm("game");
        }
    }
}