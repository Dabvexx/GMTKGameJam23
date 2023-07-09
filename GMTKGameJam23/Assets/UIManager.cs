using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

///<summary>
/// 
///</summary>
public class UIManager : MonoBehaviour
{
    #region Variables
    // Variables.

    [SerializeField] public TextMeshProUGUI shotText;
    [SerializeField] public TextMeshProUGUI enemyText;
    [SerializeField] public TextMeshProUGUI timeText;

    ShootScript shootScript;
    SpeedrunTimer timer;
    #endregion

    #region Unity Methods

    void Start()
    {
        shootScript = FindObjectOfType<ShootScript>().GetComponent<ShootScript>();
        timer = FindObjectOfType<SpeedrunTimer>().GetComponent<SpeedrunTimer>();
    }

    void Update()
    {
        var timeInSeconds = timer.timer;
        timeText.text = $"Time: {FormatTime(timeInSeconds)}";
        shotText.text = $"Shots: {shootScript.shots}";
        enemyText.text = $"Enemies: {GameObject.FindGameObjectsWithTag("Enemy").Length}";
    }

    #endregion

    #region Private Methods
    // Private Methods.
    private static string FormatTime(float time)
    {
        TimeSpan t = TimeSpan.FromSeconds(time);
        return string.Format("{0,1:0}:{1,2:00}", t.Minutes, t.Seconds);
    }
    #endregion
}
