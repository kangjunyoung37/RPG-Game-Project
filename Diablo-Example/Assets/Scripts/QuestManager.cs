using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class QuestManager : MonoBehaviour
{

    private static QuestManager instance;
    public static QuestManager Instance => instance;

    public QuestDatabaseObject questDatabase;

    public event Action<QuestObject> OnCompletedQuest;

    private void Awake()
    {
        instance = this;
    }
    public void ProcessQuest(QuestType type , int targetID)
    {
        foreach(QuestObject questObject in questDatabase.questObjects)
        {
            if(questObject.status == QuestStatus.Accepted && questObject.data.type == type && questObject.data.targetID == targetID)
            {
                questObject.data.completedCount++;
                if(questObject.data.completedCount >= questObject.data.count)
                {
                    questObject.status = QuestStatus.Completed;
                    OnCompletedQuest?.Invoke(questObject);
                }
            }
        }
    }
}
