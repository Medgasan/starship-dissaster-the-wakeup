using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.GenericScritps
{
    public class SonidoAnimacion : MonoBehaviour
    {
        public AudioClip clip;
        private AudioSource audioSource;
        private void Start()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = false;
            audioSource.spatialBlend = 1.0f;
            audioSource.clip = clip;
            audioSource.volume = 0.45f;
        }

        public void Play()
        {
            if (audioSource.clip == null || audioSource.isPlaying) return;
            audioSource.Play();
        }




    }
}
