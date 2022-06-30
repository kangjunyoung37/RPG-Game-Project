using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase.Database;
using kang.InventorySystem.Inventory;
public class LoadingScene : MonoBehaviour
{
    const float TextUpdateInterval = 1.0f;
    const string LoadingTextValue = "...";
    const float NextSceneInterval = 3.0f;

    private DatabaseReference databaseRef;
    private string UserDataPath => "users";// /users/
    private string StatsDataPath => "stats";// /users/uid/stats
    private string EquipmentDataPath => "equipment"; // /users/uid/equipment
    private string InventoryDataPath => "inventory"; // /users/uid/inventory

    public StatsObject playerStats;
    public InventoryObject playerEquipment;
    public InventoryObject playerInventory;

    [SerializeField]
    private TMP_Text LoadingText;

    int TextIndex = 0;
    float LastUpdateTime;
    float LoadingTime;
    private void Start()
    {
        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        LoadingTime = Time.time;
        OnclickedLoad();

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
    public void OnclickedLoad()
    {
        var userId = FireBaseAuthController.Instance.UserId;
        if (userId == string.Empty)
        {
            return;
        }

        databaseRef.Child(UserDataPath).Child(userId).Child(StatsDataPath).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Load user data was canceled");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Load user data encouneterd an error : " + task.Exception);
                return;
            }
            DataSnapshot snapshot = task.Result;
            playerStats.FromJson(snapshot.GetRawJsonValue());
            Debug.LogFormat("Load User data in successfully : {0} {1}", userId, snapshot.GetRawJsonValue());
        });

        databaseRef.Child(UserDataPath).Child(userId).Child(EquipmentDataPath).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Load Equipment data was canceled");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Load Equipment data encouneterd an error : " + task.Exception);
                return;
            }
            DataSnapshot snapshot = task.Result;
            playerEquipment.FromJson(snapshot.GetRawJsonValue());
            Debug.LogFormat("Load Equipment data in successfully : {0} {1}", userId, snapshot.GetRawJsonValue());
        });

        string inventoryJson = playerInventory.ToJson();
        databaseRef.Child(UserDataPath).Child(userId).Child(InventoryDataPath).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Load Inventory data was canceled");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Load Inventory data encouneterd an error : " + task.Exception);
                return;
            }

            DataSnapshot snapshot = task.Result;
            playerInventory.FromJson(snapshot.GetRawJsonValue());
            Debug.LogFormat("Load Inventory data in successfully : {0} {1}", userId, snapshot.GetRawJsonValue());
        });
        Debug.Log("OK");
    }
}
