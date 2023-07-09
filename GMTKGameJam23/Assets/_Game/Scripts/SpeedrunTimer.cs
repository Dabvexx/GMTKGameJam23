using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// 
///</summary>
public class SpeedrunTimer : MonoBehaviour
{
    #region Variables
    // Variables.
    public float timer = 0f;
    public int unusedShots = 0;

    public static SpeedrunTimer Instance { get; private set; }

    #endregion

    #region Unity Methods
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    void Update()
    {
        timer += Time.deltaTime;
    }

    #endregion
}
