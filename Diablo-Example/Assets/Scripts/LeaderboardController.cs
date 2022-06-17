using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase;
using Firebase.Database;
using Firebase.Unity;

public class UserScoreArgs: EventArgs
{
    public UserScore score;
    public string message;

    public UserScoreArgs(UserScore score, string message)
    {
        this.score = score;
        this.message = message;
    }
}
public class LeaderboardArgs : EventArgs
{
    public DateTime startData;
    public DateTime endData;

    public List<UserScore> scores;
}
public class LeaderboardController : MonoBehaviour
{
    private bool initailzied = false;
    private bool readyToInitailize = false;
    private DatabaseReference databaseRef;
    private void Start()
    {
        FirebaseInitailizer.Initailize(dependencyStatus => { 
        if(dependencyStatus == Firebase.DependencyStatus.Available)
        {
            readyToInitailize = true;
                InitialzieDatebase();
        }
        else
        {
            Debug.LogError("Could not resolve all Firebase dependencies" + dependencyStatus);
        }
        });
    }
    private void InitialzieDatebase()
    {
        if(initailzied)
        {
            return;
        }
        FirebaseApp app = FirebaseApp.DefaultInstance;
        if(app.Options.DatabaseUrl != null)
        {
            app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
        }
        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;

        initailzied = true;
        readyToInitailize = false;
        Oninitialized(this, null);

    }



    void Update()
    {
        
    }
}
