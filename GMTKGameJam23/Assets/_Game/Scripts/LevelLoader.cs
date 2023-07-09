using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


///<summary>
/// 
///</summary>
public class LevelLoader : MonoBehaviour
{
    #region Variables
    // Variables.
    public Animator transition;
    public float transitionLength = 1f;
    public static LevelLoader Instance { get; private set; }
    public Scenes nextScene;
    #endregion

    #region Unity Methods

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    #endregion

    #region Private Methods
    // Private Methods.

    #endregion

    #region Public Methods
    // Public Methods.
    public void LoadLevel()
    {
        StartCoroutine(LoadLevelCoroutine(nextScene));
    }

    public IEnumerator LoadLevelCoroutine(Scenes scene)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionLength);

        SceneManager.LoadSceneAsync((int)scene);
    }
    #endregion

    public enum Scenes
    {
        MainMenu = 0,
        Tutorial1 = 1,
        Tutorial2 = 2,
        Level1 = 3,
        Level2 = 4,
        Level3 = 5,
        EndScreen = 6
    };
}
