using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestStatus
{
    None,
    Accepted,
    Completed,
    Rewareded,
}
[CreateAssetMenu(fileName = "New Quest",menuName = "Quest System/Quests/New Quest")]
public class QuestObject : ScriptableObject
{
    public Quest data = new Quest();
    public QuestStatus status;
}
