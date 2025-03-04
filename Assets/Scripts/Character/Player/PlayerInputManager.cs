using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{

   public static PlayerInputManager instance;
   public PlayerManager player;


   PlayerControls playerControls;
   [Header("CAMERA MOVEMENT INPUT")]
   [SerializeField] Vector2 cameraInput;
   public float cameraHorizontalInput;
   public float cameraVerticalInput;


   [Header("PLAYER MOVEMENT INPUT")]
   [SerializeField] Vector2 movementInput;
   public float horizontalInput;
   public float verticalInput;
   public float moveAmount;


   [Header("PLAYER ACTION INPUT")]
   [SerializeField] bool dodgeInput = false;
   [SerializeField] bool sprintInput = false;


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

   public void Start()
   {

      DontDestroyOnLoad(gameObject);

      SceneManager.activeSceneChanged += OnSceneChange;

      instance.enabled = false;
   }

   private void OnSceneChange(Scene oldScene, Scene newScene)
   {
      if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
      {
         instance.enabled = true;
      }
      else
      {
         instance.enabled = false;
      }
   }

   private void OnEnable()
   {
      if (playerControls == null)
      {
         playerControls = new PlayerControls();

         playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
         playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
         playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;

         // HOLDING THE INPUT, SETS THE BOOL TO TRUE
         playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
         Debug.Log("Sprint Input: " + sprintInput);
         // RELEASE THE INPUT, SETS THE BOOL TO FALSE
         playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
         Debug.Log("Sprint Input: " + sprintInput);
      }
      playerControls.Enable();
   }

   private void Update()
   {
      HandleAllInputs();
   }

   private void HandleAllInputs()
   {
      HandlePlayerMovementInput();
      HandleCameraMovementInput();
      HandleDodgeInput();
      HandleSprinting();
   }

   private void OnDestroy()
   {
      SceneManager.activeSceneChanged -= OnSceneChange;
   }

   private void OnApplicationFocus(bool focus)
   {
      if (enabled)
      {
         if (focus)
         {
            playerControls.Enable();
         }
         else
         {
            playerControls.Disable();
         }
      }
   }

   private void HandlePlayerMovementInput()
   {
      verticalInput = movementInput.y;
      horizontalInput = movementInput.x;

      moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

      if (moveAmount <= 0.5 && moveAmount > 0)
      {
         moveAmount = 0.5f;
      }
      else if (moveAmount > 0.5 && moveAmount <= 1)
      {
         moveAmount = 1;
      }

      if (player == null)
         return;


      player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
   }

   private void HandleCameraMovementInput()
   {
      cameraVerticalInput = cameraInput.y;
      cameraHorizontalInput = cameraInput.x;
   }


   // ACTION 
   private void HandleDodgeInput()
   {
      if (dodgeInput)
      {
         dodgeInput = false;

         player.playerLocomotionManager.AttemptToPerformDodge();

      }
   }

   private void HandleSprinting()
   {
      if (sprintInput)
      {
         
         player.playerLocomotionManager.HandleSprinting();
      }
      else
      {
         player.playerNetworkManager.isSprinting.Value = false;
      }
   }
}
