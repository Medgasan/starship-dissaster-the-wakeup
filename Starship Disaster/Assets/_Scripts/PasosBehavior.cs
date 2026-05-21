using System;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace Assets._Scripts
{
    public class PasosBehavior : MonoBehaviour
    {
        public AudioClip[] footSteps;
        private AudioSource audioSource;


        public void Start()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = false;
            audioSource.spatialBlend = 0f;
            audioSource.volume = 0.45f;
        }


        public void FootStep()
        {
            if (audioSource.isPlaying) return;
            audioSource.clip = footSteps[Random.Range(0, footSteps.Length)];
            audioSource.Play();
        }
    }
}
