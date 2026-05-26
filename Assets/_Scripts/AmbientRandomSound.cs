using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


public class AmbientRandomSound : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public float tiempoMin;
    public float tiempoMax;
    public AudioClip[] clips;
    public bool loop = true;

    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
        StartCoroutine(BucleAleatorio());
    }


    IEnumerator BucleAleatorio()
    {
        while (loop)
        {
            ReproducirSonido();
            yield return new WaitForSeconds(Random.Range(tiempoMin, tiempoMax));
        }
    }

    private void ReproducirSonido()
    {
        if (clips.Length < 1) return;
        source.clip = clips[Random.Range(0,clips.Length-1)];
        source.Play();
    }


}
