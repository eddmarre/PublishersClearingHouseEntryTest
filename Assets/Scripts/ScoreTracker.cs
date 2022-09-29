using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreTextMesh;
    [SerializeField] private TextMeshProUGUI _getOrPostText;
    public static int Score { get; private set; }

    private void Start()
    {
        Player.OnPlayerKick += Player_OnPlayerKick;
        Player.OnPlayerGetOrSetWebRequest += (string getOrPost) => { _getOrPostText.text = getOrPost; };
    }

    private void OnDestroy()
    {
        Player.OnPlayerKick -= Player_OnPlayerKick;
    }

    private void Player_OnPlayerKick()
    {
        ++Score;
        _scoreTextMesh.text = Score.ToString();
    }
}