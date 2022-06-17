using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase;
public static class FirebaseInitailizer 
{
    public static List<Action<DependencyStatus>> initailizeCallback = new List<Action<DependencyStatus>>();
    private static DependencyStatus dependencyStatus;

    private static bool initialzied = false;
    private static bool fetching = false;
    private static bool activateFetch = false;

    public static void Initailize(Action<DependencyStatus> callback)
    {
        lock(initailizeCallback)
        {
            if(initialzied)
            {
                callback(dependencyStatus);
                return;
            }
            initailizeCallback.Add(callback);
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                lock(initailizeCallback)
                {
                    dependencyStatus = task.Result;
                    initialzied = true;
                    CallInitailizedCallbakcs();
                }
            });

        }
    }
    private static void CallInitailizedCallbakcs()
    {
        lock(initailizeCallback)
        {
            foreach(var callback in initailizeCallback)
            {
                callback(dependencyStatus);
            }
            initailizeCallback.Clear();
        }
    }
}
