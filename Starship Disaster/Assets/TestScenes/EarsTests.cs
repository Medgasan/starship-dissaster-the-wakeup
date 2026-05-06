using UnityEngine;
using UnityEngine.Events;

public class EarsTests : MonoBehaviour
{
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private float threshold = 0.2f;
    [SerializeField] private UnityEvent onHeard;

    private void Update()
    {
        float dist = Vector3.Distance(transform.position, playerAudioSource.transform.position);
        float vol = playerAudioSource.volume * Mathf.Clamp01(1f - dist / playerAudioSource.maxDistance);

        if (vol >= threshold)
            onHeard?.Invoke();
    }
}
