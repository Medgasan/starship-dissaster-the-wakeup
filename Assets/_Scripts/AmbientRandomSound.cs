using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


public class AmbientRandomSound : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public float tiempoMin = 300;
    public float tiempoMax = 900;
    public AudioClip[] clips;
    public bool spatial = false;
    [Range(0f, 1f)]
    public float volume = 0.05f;
    public Vector3 tamañoCubo = Vector3.one * 5f;


    private bool loop = true;
    private AudioSource source;
    private int lastIndex;


    void Start()
    {
        source = GetComponent<AudioSource>();
        source.loop = false;
        StartCoroutine(BucleAleatorio());
        Debug.Log("SonidoRandom Activado");
    }


    IEnumerator BucleAleatorio()
    {
        while (loop)
        {
            Debug.Log("En el loop de SonidoRandom");
            yield return new WaitForSeconds(Random.Range(tiempoMin, tiempoMax));
            ReproducirSonido();
        }
    }

    private void ReproducirSonido()
    {
        Debug.Log("SonidoRandom lanzado");
        if (clips.Length < 1) return;
        int index = 0;

        do 
        {
            index = Random.Range(0, clips.Length - 1);
        } while (index == lastIndex);

        source.volume = volume;
        source.clip = clips[index];
        source.pitch = Random.Range(0.90f, 1.10f);
        if (spatial) SpatialBehaviour();
        source.Play();
        lastIndex = index;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, tamañoCubo);
    }

    private void SpatialBehaviour()
    {
        Vector3 posAleatoria = transform.position + new Vector3(
            Random.Range(-tamañoCubo.x / 2, tamañoCubo.x / 2),
            Random.Range(-tamañoCubo.y / 2, tamañoCubo.y / 2),
            Random.Range(-tamañoCubo.z / 2, tamañoCubo.z / 2)
        );
        transform.position = posAleatoria;
    }

}
