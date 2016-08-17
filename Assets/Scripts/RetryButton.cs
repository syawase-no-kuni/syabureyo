using UnityEngine;
using System.Collections;

public class RetryButton : MonoBehaviour {
    public void retryButton()
    {
        Debug.Log("retry");
        Application.LoadLevel("Main");
    }
}
