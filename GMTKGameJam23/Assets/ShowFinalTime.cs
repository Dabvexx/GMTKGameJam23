using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

///<summary>
/// 
///</summary>
public class ShowFinalTime : MonoBehaviour
{
    #region Variables
    // Variables.

    [SerializeField] private TextMeshProUGUI timeText;
    #endregion

    #region Unity Methods

    void Awake()
    {
        var timer = FindObjectOfType<SpeedrunTimer>().GetComponent<SpeedrunTimer>();
        var timeInSeconds = timer.timer;
        timeText.text = $"Your Time: {FormatTime(timeInSeconds)}";
        Destroy(timer.gameObject);
    }

    void Update()
    {
        
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

    #region Public Methods
    // Public Methods.

    #endregion
}