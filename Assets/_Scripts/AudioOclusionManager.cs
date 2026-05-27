using UnityEngine;
using UnityEngine.Audio;

public class AudioOclusionManager : MonoBehaviour
{
    public AudioMixer mixer;
    private Transform listener;
    public LayerMask wallMask;
    public float rangoMaximo = 200f;
    public float cutoffObstruido = 800f;
    public float cutoffLibre = 22000f;

    private AudioSource[] fuentes;

    void Start()
    {
        fuentes = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        listener = Camera.main.transform;
    }

    void Update()
    {
        bool obstruido = false;

        foreach (var src in fuentes)
        {
            if (!src.isPlaying) continue;
            if (Vector3.Distance(src.transform.position, listener.position) > rangoMaximo) continue;

            if (Physics.Linecast(src.transform.position, listener.position, wallMask))
            {
                Debug.Log(src.ToString());
                obstruido = true;
                break;
            }
        }

        mixer.SetFloat("LowPassCutoff", obstruido ? cutoffObstruido : cutoffLibre);
    }
}
