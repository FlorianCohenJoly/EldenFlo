using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerUIManager : MonoBehaviour
{

    public static PlayerUIManager instance;

   [Header("NETWORK JOIN")]
   [SerializeField] bool startGameAsClient;

    private void Awake()
    {
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

   private void Update()
   {
       if (startGameAsClient)
       {
           startGameAsClient = false;
           
           // Il faut d'abord arreter le réseau pour le démarrer en tant que client psq on veut nous connecter en tant que client
           NetworkManager.Singleton.Shutdown();

           // On démarre le réseau en tant que client
           NetworkManager.Singleton.StartClient();
       }
   }
}
