using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager instance;

    public int worldSceneIndex = 1;

    private void Awake()
    {
        // Il ne peut y avoir qu'une seule instance de WorldSaveGameManager a la fois, si une instance existe deja, on detruit l'objet
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator LoadNewGame()
    {
       AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

       yield return null;


    }

    public int GetWorldSceneIndex()
    {
        return worldSceneIndex;
    }

    
}
