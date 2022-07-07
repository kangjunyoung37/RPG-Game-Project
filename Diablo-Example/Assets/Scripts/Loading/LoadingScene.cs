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

    private bool exist = true;
    private DatabaseReference databaseRef;
    private string UserDataPath => "users";// /users/
    private string StatsDataPath => "stats";// /users/uid/stats
    private string EquipmentDataPath => "equipment"; // /users/uid/equipment
    private string InventoryDataPath => "inventory"; // /users/uid/inventory

    public StatsObject playerStats;
    public InventoryObject playerEquipment;
    public InventoryObject playerInventory;

    public StatsObject initPlayerStates;
    public InventoryObject initInitPlayerEquipment;
    public InventoryObject initInitPlayerInventory;

    [SerializeField]
    private TMP_Text LoadingText;

    int TextIndex = 0;
    float LastUpdateTime;
    float LoadingTime;
    private void Start()
    {
        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        LoadingTime = Time.time;
        Check();
     
       
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
    public void OnClickedSave()
    {
        var userId = FireBaseAuthController.Instance.UserId;
        if (userId == string.Empty)
        {
            return;
        }
        string statsJson = initPlayerStates.ToJson();
        databaseRef.Child(UserDataPath).Child(userId).Child(StatsDataPath).SetRawJsonValueAsync(statsJson).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Save user data was canceled");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Save user data encouneterd an error : " + task.Exception);
                return;
            }
            Debug.LogFormat("Save user data in successfully : {0} {1}", userId, statsJson);
        }

        );
        string equipmentJson = initInitPlayerEquipment.ToJson();
        databaseRef.Child(UserDataPath).Child(userId).Child(EquipmentDataPath).SetRawJsonValueAsync(equipmentJson).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Save Equipment data was canceled");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Save Equipment data encouneterd an error : " + task.Exception);
                return;
            }
            Debug.LogFormat("Save Equipment data in successfully : {0} {1}", userId, statsJson);
        });
        string inventoryJson = initInitPlayerInventory.ToJson();
        databaseRef.Child(UserDataPath).Child(userId).Child(InventoryDataPath).SetRawJsonValueAsync(inventoryJson).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Save Inventory data was canceled");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Save Inventory data encouneterd an error : " + task.Exception);
                return;
            }
            Debug.LogFormat("Save Inventory data in successfully : {0} {1}", userId, statsJson);
        });
    }
    public void Check()
    {
        var userId = FireBaseAuthController.Instance.UserId;
        databaseRef.Child(UserDataPath).Child(userId).Child(StatsDataPath).GetValueAsync().ContinueWith(task =>
        {
            if(task.IsFaulted)
            {
                Debug.LogError("Don't have data");
                
            }
            else if(task.IsCompleted)
            {
                Debug.Log("You Have data");
               
            }
            DataSnapshot snapshot = task.Result;
            
            if(snapshot.Value== null)
            {
                exist = false;
            }
            else
            {
                exist = true;
            }
            if (exist)
            {
                OnclickedLoad();
            }
            if (!exist)
            {
                OnClickedSave();
                OnclickedLoad();
            }
        });
       
    }
}
