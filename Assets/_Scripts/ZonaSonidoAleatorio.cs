using System.Collections;
using Unity.AppUI.UI;
using UnityEngine;

public class ZonaSonidoAleatorio : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioClip[] clips;
    [Range(0f, 1f)]
    public float volume = 0.45f;
    public Vector3 tamañoCubo = Vector3.one * 5f;
    public bool loop;
    public float tiempoMin = 0f;
    public float tiempoMax = 10f;
    private GameObject go;
    private AudioSource src;


    void Start()
    {
        go = new GameObject("SonidoAleatorio");
        src = go.AddComponent<AudioSource>();
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
        Vector3 posAleatoria = transform.position + new Vector3(
            Random.Range(-tamañoCubo.x / 2, tamañoCubo.x / 2),
            Random.Range(-tamañoCubo.y / 2, tamañoCubo.y / 2),
            Random.Range(-tamañoCubo.z / 2, tamañoCubo.z / 2)
        );
        go.transform.position = posAleatoria;
        
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        src.clip = clip;
        src.loop = false;
        src.volume = volume;
        src.spatialBlend = 1f;
        src.Play();

    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, tamañoCubo);
    }

}
