using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// 
///</summary>
public class EndLevel : MonoBehaviour
{
    #region Unity Methods

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                LevelLoader.Instance.LoadLevel();
            }

            // Start coroutine to show text not all the enemies are gone.
            // Maybe.
            // If i have time.
            // So probably not.
        }
    }

    #endregion
}
