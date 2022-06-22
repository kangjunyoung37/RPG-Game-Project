using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LoadingScene : MonoBehaviour
{
    const float TextUpdateInterval = 1.0f;
    const string LoadingTextValue = "...";
    

    [SerializeField]
    private TMP_Text LoadingText;

    int TextIndex = 0;
    float LastUpdateTime;

    void Update()
    {
        float currentTime = Time.time;
        if(currentTime - LastUpdateTime > TextUpdateInterval)
        {

            if (TextIndex >= LoadingTextValue.Length)
            {
                TextIndex = 0;
            }

            LoadingText.text = "Loading" + LoadingTextValue.Substring(0,TextIndex+1);
            TextIndex++;


            LastUpdateTime = currentTime;
        }
    }
}
