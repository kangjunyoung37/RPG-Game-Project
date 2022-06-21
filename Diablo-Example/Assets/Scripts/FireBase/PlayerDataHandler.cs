using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using kang.InventorySystem.Inventory;

public class PlayerDataHandler : MonoBehaviour
{
    private DatabaseReference databaseRef;
    private string UserDataPath => "users";// /users/
    private string StatsDataPath => "stats";// /users/uid/stats
    private string EquipmentDataPath => "equipment"; // /users/uid/equipment
    private string InventoryDataPath => "inventory"; // /users/uid/inventory

    public StatsObject playerStats;
    public InventoryObject playerEquipment;
    public InventoryObject playerInventory;

    void Start()
    {
        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void OnClickedSave()
    {
        var userId = FireBaseAuthController.Instance.UserId;
        if(userId == string.Empty)
        {
            return;
        }
        string statsJson = playerStats.ToJson();
        databaseRef.Child(UserDataPath).Child(userId).Child(StatsDataPath).SetRawJsonValueAsync(statsJson).ContinueWith(task =>
        {
            if(task.IsCanceled)
            {
                Debug.LogError("Save user data was canceled");
                return;
            }
            if(task.IsFaulted)
            {
                Debug.LogError("Save user data encouneterd an error : " + task.Exception);
                return;
            }
            Debug.LogFormat("Save user data in successfully : {0} {1}", userId, statsJson);
        }
        
        );
        string equipmentJson = playerEquipment.ToJson();
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
        string inventoryJson = playerInventory.ToJson();
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

    }

}
