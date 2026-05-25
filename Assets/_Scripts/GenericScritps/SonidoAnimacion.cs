using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets._Scripts.GenericScritps
{
    public class SonidoAnimacion : MonoBehaviour
    {
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 0.45f;
        private AudioSource audioSource;
        private void Start()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = false;
            audioSource.spatialBlend = 1.0f;
            audioSource.clip = clip;
            audioSource.volume = Mathf.Pow(volume, 2f);
            audioSource.pitch = Random.Range(0.95f, 1.05f);
        }

        public void Play()
        {
            if (audioSource.clip == null || audioSource.isPlaying) return;
            audioSource.Play();
        }

    }
}
