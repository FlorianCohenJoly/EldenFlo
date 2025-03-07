using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour
{
   public AudioSource audioSource;

   protected virtual void Awake()
   {
        audioSource = GetComponent<AudioSource>();
   }

   public void PlayRollSoundFX()
   {
        audioSource.PlayOneShot(WorldSoundFXManager.instance.rollSFX);
   }
}
