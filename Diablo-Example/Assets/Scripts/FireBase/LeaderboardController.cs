using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Threading.Tasks;
using System.Linq;
using TMPro;


namespace kang.firebase.Leaderboard
{


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
        public DateTime startDate;
        public DateTime endDate;

        public List<UserScore> scores;
    }
    public class LeaderboardController : MonoBehaviour
    {
        public TMP_InputField userIdInputField;
        public TMP_InputField userNameInputField;
        public TMP_InputField scoreInputField;
        public TMP_Text outputText;

        private bool initailzied = false;
        private bool readyToInitailize = false;
        private DatabaseReference databaseRef;
        private bool addingUserScore = false;

        private List<UserScore> topScores = new List<UserScore>();
        public List<UserScore> TopScores => topScores;

        private Dictionary<string, UserScore> userScores = new Dictionary<string, UserScore>();
    
        private UserScoreArgs addedScoreArgs;
        private bool sendAddedScoreEvent = false;
        public event EventHandler<UserScoreArgs> OnAddedScore;

        private UserScoreArgs retrievedScroreArgs;
        private bool sendRetrievedScroeEvent = false;
        public event EventHandler<UserScoreArgs> OnRetrivedScore;
        public string AllScoreDataPath => "all_scores";
        public event EventHandler OnInitailized;

        private bool sendUpdatedLeaderboardEvent = false;
        public event EventHandler<LeaderboardArgs> OnUpdatedLeaderborard;

        public UDateTime startDateTime;
        public UDateTime endDateTime;

        private Query currentNewScoreQuery;

        public long StartTime
        {
            get => startDateTime.dateTime.Ticks / TimeSpan.TicksPerSecond;
        }

        public long EndTime
        {
            get
            {
                long endTimeticks = endDateTime.dateTime.Ticks / TimeSpan.TicksPerSecond;
                if (endTimeticks <= 0)
                {
                    endTimeticks = DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond;
                }
                return endTimeticks;
            }
        }



   


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
            OnInitailized(this, null);

        }



        void Update()
        {
            if (sendAddedScoreEvent)
            {
                sendAddedScoreEvent = false;
                OnAddedScore(this, addedScoreArgs);
            }
            if(sendRetrievedScroeEvent)
            {
                sendRetrievedScroeEvent = false;
                OnRetrivedScore(this, retrievedScroreArgs);
            }
            if(sendUpdatedLeaderboardEvent)
            {
                sendUpdatedLeaderboardEvent = false;
                OnUpdatedLeaderborard(this, new LeaderboardArgs
                {
                    scores = topScores,
                    startDate = startDateTime,
                    endDate = endDateTime
                });
            }
        }
        public Task AddScore(string userId, string userName, int score, long timestamp = -1L,Dictionary<string,object> otherData = null) //스코어를 데이터에 쓰기 작업해주는 메소드
        {
            if (timestamp <= 0)
            {
                timestamp = DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond;//유닉스에서의 틱타임과 C#에서의 틱타임이 다르기 때문에 나눠준다
            }
            var userScore = new UserScore(userId, userName, score, timestamp, otherData);
            return AddScore(userScore);
        }
    
        public Task<UserScore> AddScore(UserScore userScore)
        {
            if (addingUserScore)
            {
                Debug.LogError("Runnig add user score task");
                return null;
            }
            var scoreDictionary = userScore.ToDictionary();
            addingUserScore = true;

            return Task.Run(() =>
            {

                var newEntry = databaseRef.Child(AllScoreDataPath).Child(userScore.userId);
                return newEntry.SetValueAsync(scoreDictionary).ContinueWith(task =>
                {
                    if(task.Exception != null)
                    {
                        Debug.LogWarning("Exception adding scroe:" + task.Exception);
                        return null;
                    }
                    if(!task.IsCompleted)
                    {
                        return null;
                    }
                    addingUserScore = false;

                    addedScoreArgs = new UserScoreArgs(userScore, userScore.ToString() +"Added!");
                    sendAddedScoreEvent = true;

                    return userScore;



                }).Result;
            });
        }
        
        private bool gettingUserScore = false;
        public void GetUserScore(string userId)
        {
            gettingUserScore = true;
            
            databaseRef.Child(AllScoreDataPath).OrderByChild(UserScore.userIdPath).StartAt(userId).EndAt(userId).GetValueAsync().ContinueWith(task =>
            {
               
                if (task.Exception != null)
                {
                    throw task.Exception;
                }
                if(!task.IsCompleted)
                {
                    return;
                }

                if(task.Result.ChildrenCount == 0)
                {
                    retrievedScroreArgs = new UserScoreArgs(null, String.Format("No Scores for User{0}", userId));
                }
                else
                {
                    var scores = ParseVailidUserScoreRecords(task.Result, StartTime, EndTime).ToList();
                    if(scores.Count == 0)
                    {
                        retrievedScroreArgs = new UserScoreArgs(null, string.Format("No Scores for User{0} whithin time range ({1}-{2})", userId, StartTime, EndTime));

                    }
                    else
                    {
                        var orderedScored = scores.OrderBy(score => score.score);
                        var userScore = orderedScored.Last();

                        retrievedScroreArgs = new UserScoreArgs(userScore, userScore.ToString() + "Retrieved") ;
                    }
                }
                gettingUserScore = false;
                sendRetrievedScroeEvent = true;
            });
        }
    
        private List<UserScore> ParseVailidUserScoreRecords(DataSnapshot snapshot, long startTicks, long endTicks)
        {
            return snapshot.Children.Select(scoreRecord => UserScore.CreateScoreFromRecord(scoreRecord))
                .Where(score => score != null && score.timestamp > startTicks && score.timestamp <= endTicks)
                .Reverse()
                .ToList();
        }
        private bool gettingTopScores = false;
        private void GetIntialTopScores(long batchEnd)
        {
            gettingTopScores = true;

            //var query = databaseRef.Child(AllScoreDataPath).OrderByChild(UserScore.userIdPath).EndAt("e");
            //query = query.EndAt(batchEnd).LimitToLast(20);

            databaseRef.Child(AllScoreDataPath).GetValueAsync().ContinueWith(task =>
            {
                if(task.Exception != null)
                {

                    SetTopScores();
                    return;
                }
                if(!task.IsCompleted)
                {
                    return;
                }
                if(!task.Result.HasChildren)
                {
                    SetTopScores();
                    return;
                }
                var scores = ParseVailidUserScoreRecords(task.Result, StartTime, EndTime);
                foreach(var userScore in scores)
                {
                    if(!userScores.ContainsKey(userScore.userId))
                    {
                        userScores[userScore.userId] = userScore;
                    }
                    else
                    {
                        if(userScores[userScore.userId].score < userScore.score)
                        {
                            userScores[userScore.userId] = userScore;
                        }
                    }
                
                    }
                SetTopScores();
            });

        }


        public void RemoveUserScore(string userId)
        {
            databaseRef.Child(AllScoreDataPath).Child(userId).RemoveValueAsync().ContinueWith(task =>
            {
                if (task.Exception != null)
                {

                    Debug.LogWarning("Exception removing scroe:" + task.Exception);
                    return;
                }
                


            });       
        }
        private void SetTopScores()
        {
            topScores.Clear();
            topScores.AddRange(userScores.Values.OrderByDescending(score => score.score));
            sendUpdatedLeaderboardEvent = true;
            gettingTopScores = false;
        }

        public void AddScore()
        {
            AddScore(userIdInputField.text, userNameInputField.text, int.Parse(scoreInputField.text));
        }

        public void GetUserScore()
        {
            GetUserScore(userIdInputField.text);
        }

        public void RefreshScore()
        {
            if(initailzied)
            {
                GetIntialTopScores(Int64.MaxValue);
            }
        }
        public void RemoveScore()
        {
            RemoveUserScore(userIdInputField.text);
        }

    }
}
