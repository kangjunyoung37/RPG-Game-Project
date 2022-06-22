using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;


public class FireBaseAuthController 
{

    private static FireBaseAuthController instance = null;

    private FirebaseAuth auth;
    private FirebaseUser user;

    private string displayName;
    private string emailAddress;
    private Uri photoUrl;

    public Action<bool> OnChangedLoginState;
    public static FireBaseAuthController Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new FireBaseAuthController();
            }
            return instance;
        }
    }
    public string UserId => user?.UserId ?? string.Empty;
    public string DisplayName => displayName;
    public string EmailAddress => emailAddress;
    public Uri PhotoUrl => photoUrl;

    public void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += OnAuthStateChanged;
        OnAuthStateChanged(this,null);

    }
    public void CreateUser(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was cancled.");
                return;
            }
            if(task.IsFaulted)//아이디를 만들 때 실패했을 경우 오류 내용 찾기
            {
                Debug.LogError("CreateUserWithAndPasswordAsync encountered an error: " + task.Exception);

                int errorCode = GetFirebaseErrorCode(task.Exception);
                switch(errorCode)
                {
                    case (int)AuthError.EmailAlreadyInUse:
                        Debug.LogError("Eamil Already in Use");
                        break;
                    case(int)AuthError.InvalidEmail:
                        Debug.LogError("Invalid Email");
                        break;

                    case (int)AuthError.WeakPassword:
                        Debug.LogError("Weak Password");
                        break;
                }
                return;
            }

            FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user Created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
        });
    }
    public void SingIn(string email, string password)// 로그인 했을 때 
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was cancled.");
                return;
            }
            if (task.IsFaulted)// 실패했을경우 오류내용 찾기
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);

                int errorCode = GetFirebaseErrorCode(task.Exception);
                switch (errorCode)
                {
                    case (int)AuthError.WrongPassword:
                        Debug.LogError("WrongPassword");
                        break;
                    case (int)AuthError.UnverifiedEmail:
                        Debug.LogError("UnverifiedEmail");
                        break;

                    case (int)AuthError.InvalidEmail:
                        Debug.LogError("InvalidEmail");
                        break;
                }
                return;
            }


            FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
            SceneController.Instance.LoadScene(SceneNameConstants.LoadingScene);
        });
    }
    public void SignOut()
    {
        auth.SignOut();
    }
    AggregateException aggregateException = null;
    private int GetFirebaseErrorCode(AggregateException exception)//에러코드 알아내기 위해 사용 
    {


        FirebaseException firebaseException = null;
        foreach(Exception e in exception.Flatten().InnerExceptions)
        {
            firebaseException = e as FirebaseException;
            if(firebaseException != null)
            {
                break;
            }
        }
        return firebaseException?.ErrorCode ?? 0;
    }
    private void OnAuthStateChanged(object sender, EventArgs eventArgs)//로그인이나 로그아웃했을 때 실행하는 함수
    {
        if(auth.CurrentUser  != user)
        {
            bool signedIn = (user != auth.CurrentUser && auth.CurrentUser != null);
            if(!signedIn && user != null)
            {
                Debug.Log("Singed out: " + user.UserId);
                OnChangedLoginState?.Invoke(false);
            }
            user = auth.CurrentUser;
            if(signedIn)
            {
                Debug.Log("Signed in:" + user.UserId);

                displayName = user.DisplayName ?? string.Empty;
                emailAddress = user.Email?? string.Empty;
                photoUrl = user.PhotoUrl ?? null;

                OnChangedLoginState?.Invoke(true);
            }
        }
    }

}
