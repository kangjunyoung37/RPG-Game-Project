using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;

public class PlayerInGameUI : MonoBehaviour
{

    public StatsObject playerStates;

    public Text levelText;
    public Image healthSlider;
    public Image manaSlider;
    void Start()
    {
        levelText.text = playerStates.level.ToString("n0");
        healthSlider.fillAmount = playerStates.HealthPercentage;
        manaSlider.fillAmount = playerStates.ManaPercentage;

    }

    private void OnEnable()
    {
        playerStates.OnChangedStats += OnChangedStats;
    }
    private void OnDisable()
    {
        playerStates.OnChangedStats -= OnChangedStats;
    }

    private void OnChangedStats(StatsObject statsObject)
    {
        levelText.text = playerStates.level.ToString("n0");
        healthSlider.fillAmount = playerStates.HealthPercentage;
        manaSlider.fillAmount = playerStates.ManaPercentage;
    }

}
