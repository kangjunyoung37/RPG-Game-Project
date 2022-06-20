using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TMPro.EditorUtilities;
using System;
using kang.firebase.Leaderboard;
using System.Net.Sockets;

namespace kang.firebase.Leaderboard
{


    public class LeaderboardUIController : MonoBehaviour
    {

        public LeaderboardController leaderboardController;
        public TMP_Text outputText;

        public int MaxRetrievableScroes = 100;
        public RectTransform scoreContentContainer;
        public GameObject scorePrefab;
        public List<GameObject> scoreObjects = new List<GameObject>();
        private enum TopScoreElement
        {
            Username = 1,
            Timestamp = 2,
            Score = 3
        }
        void Start()
        {
            StartCoroutine(CreateTopscorePrefabs());
        }
        private void OnEnable()
        {
            leaderboardController.OnAddedScore += OnAddedUserScore;
            leaderboardController.OnRetrivedScore += OnRetrievedUserScore;
            leaderboardController.OnUpdatedLeaderborard += OnUpdatedLeaderboard;
        }
        private void OnDisable()
        {
            leaderboardController.OnAddedScore -= OnAddedUserScore;
            leaderboardController.OnRetrivedScore -= OnRetrievedUserScore;
            leaderboardController.OnUpdatedLeaderborard -= OnUpdatedLeaderboard;
        }


        private void OnAddedUserScore(object sender, UserScoreArgs args)
        {
            outputText.text = args.message;
        }
        private void OnRetrievedUserScore(object sender, UserScoreArgs args)
        {
            outputText.text = args.message;

        }

        private void OnUpdatedLeaderboard(object sender, LeaderboardArgs args)
        {
            var scores = args.scores;

            for(var i = 0; i<= Mathf.Min(scores.Count, scoreObjects.Count); i++)
            {
                var score = scores[i];

                var scoreObject = scoreObjects[i];
                scoreObject.SetActive(true);

                var textElements = scoreObject.GetComponentsInChildren<TMP_Text>();
                textElements[(int)TopScoreElement.Username].text = string.IsNullOrEmpty(score.userName) ? score.userId : score.userName;
                textElements[(int)TopScoreElement.Timestamp].text = score.ShortDateString;
                textElements[(int)TopScoreElement.Score].text = score.score.ToString();

                
            }
            for (var i = scores.Count; i< scoreObjects.Count; i++)
            {
                scoreObjects[i].SetActive(false);
            }
        }

           private IEnumerator CreateTopscorePrefabs()
        {
            var textElements = scorePrefab.GetComponentsInChildren<TMP_Text>();
            var topScoreElementValues = Enum.GetValues(typeof(TopScoreElement));
            var lastTopScoreElementValue = (int)topScoreElementValues.GetValue(topScoreElementValues.Length - 1);
            if (textElements.Length < lastTopScoreElementValue)
            {
                throw new InvalidOperationException(String.Format(
                    "At least {0} Text components must be present on TopScorePrefab. Found {1}",
                    lastTopScoreElementValue,
                    textElements.Length));
            }

            for (int i = 0; i < MaxRetrievableScroes; i++)
            {
                GameObject scoreObject = Instantiate(scorePrefab, scoreContentContainer.transform);
                scoreObject.GetComponentInChildren<TMP_Text>().text = (i + 1).ToString();
                scoreObject.name = "Leaderboard Score Record " + i;
                scoreObject.SetActive(false);

                scoreObjects.Add(scoreObject);

                yield return null;
            }
        }
    }

}
