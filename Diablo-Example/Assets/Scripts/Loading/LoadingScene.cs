using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LoadingScene : MonoBehaviour
{
    const float TextUpdateInterval = 1.0f;
    const string LoadingTextValue = "...";
    const float NextSceneInterval = 3.0f;
    

    [SerializeField]
    private TMP_Text LoadingText;

    int TextIndex = 0;
    float LastUpdateTime;
    float LoadingTime;
    private void Start()
    {
        LoadingTime = Time.time;
    }


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
        
        if (currentTime - LoadingTime > NextSceneInterval)
        {
            SceneController.Instance.LoadScene(SceneNameConstants.MainScene);
        }
    }
}
