using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Database;
[Serializable]
public class UserScore
{
    public static string userIdPath = "user_id";
    public static string userNamePath = "username";
    public static string scorePath = "score";
    public static string timestampPath = "timestamp";
    public static string otherDataPath = "data";
    public string userId;
    public string userName;
    public long score;
    public long timestamp;
    public Dictionary<string, object> otherData;

    public UserScore(string userId, string userName, long scroe, long timestamp, Dictionary<string, object> otherData = null)
    {
        this.userId = userId;
        this.userName = userName;
        this.score = scroe;
        this.timestamp = timestamp;
        this.otherData = otherData;
    }
    public string ShortDateString
    {
        get
        {
            var scoreData = new DateTimeOffset(new DateTime(timestamp * TimeSpan.TicksPerSecond, DateTimeKind.Utc)).LocalDateTime;
            return scoreData.ToShortDateString() + " " + scoreData.ToShortTimeString();
        }
    }
    public UserScore(DataSnapshot record)
    {
        userId = record.Child(userIdPath).Value.ToString();
        if(record.Child(userIdPath).Exists)
        {
            userName = record.Child(userNamePath).Value.ToString();
        }
        long score;
        if(Int64.TryParse(record.Child(scorePath).Value.ToString(),out score))
        {
            this.score = this.score;
        }
        else
        {
            this.score = Int64.MinValue;
        }
        long timestamp;
        if(Int64.TryParse(record.Child(timestampPath).Value.ToString(),out timestamp))
        {
            this.timestamp = timestamp;
        }
        if(record.Child(otherDataPath).Exists && record.Child(otherDataPath).HasChildren)
        {
            this.otherData = new Dictionary<string, object>();
            foreach(var keyValue in record.Child(otherDataPath).Children)
            {
                otherData[keyValue.Key] = keyValue.Value;
            }
        }
    }
    public static UserScore CreateScoreFromRecord(DataSnapshot record)//생성자가 바로 생성되지 않게 하기 위해서 static으로 널값 반환 처리를 해줌
    {
        if(record == null)
        {
            Debug.LogWarning("Null DAtaSanpshot record in UserScore,CreateScoreFromRecord");
            return null;
        }
        if(record.Child(userIdPath).Exists && record.Child(scorePath).Exists && record.Child(timestampPath).Exists)
        {
            return new UserScore(record);
        }
        Debug.LogWarning("Invalid record format in UserScore.CreateScoreFromRecord");
        return null; 
    }
    public Dictionary<string , object> ToDictionary()//Json으로 변환하기 위해서 
    {
        return new Dictionary<string, object>() {
            {userIdPath,userId },
            {userNamePath , userName },
            {scorePath , score },
            {timestampPath,timestamp},
            {otherDataPath,otherData }
        };
    }
}
