using UnityEngine;
using System.Collections;
using SWorker;


public class ButtonEvents : MonoBehaviour {
    public void retryButton()
    {
        Debug.Log("retry");
        Application.LoadLevel("Main");
    }

    public void TweetButton()
    {
        Debug.Log("tweet");

#if UNITY_IPHONE || UNITY_ANDROID
        string message = "message";
        string url = "http://siawase-kingdom.co.jp/";
        SocialWorker.PostTwitter(message, "");
#else
        Application.OpenURL("http://twitter.com/intent/tweet?text="
            + WWW.EscapeURL("テキスト #hashtag"));
#endif

    }
}
