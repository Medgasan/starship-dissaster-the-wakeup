using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    private Animator animator;
    private bool dead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Die()
    {
        if (dead) return;

        dead = true;

        animator.SetTrigger("Die");

        // Desactivar controles
        MonoBehaviour movement =
            GetComponent<FirstPersonController>();

        if (movement != null)
            movement.enabled = false;

        StartCoroutine(RestartAfterDeath());
    }

    IEnumerator RestartAfterDeath()
    {
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex);
    }
}
