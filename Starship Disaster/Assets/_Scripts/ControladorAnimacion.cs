using UnityEngine;

public class ControladorAnimacion : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rb;
    private float velocidadSuave;

    [Header("Controles")]
    [Tooltip("Pon aquí la misma tecla que usas para agacharte en el FirstPersonController")]
    public KeyCode teclaAgacharse = KeyCode.LeftControl;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        if (rb != null && anim != null)
        {
            // 1. Control de la velocidad (Caminar/Correr)
            Vector3 velocidadHorizontal = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            float velocidadActual = velocidadHorizontal.magnitude;

            velocidadSuave = Mathf.Lerp(velocidadSuave, velocidadActual, Time.deltaTime * 10f);
            anim.SetFloat("Velocidad", velocidadSuave);

            // 2. Control de Agacharse
            bool agachado = Input.GetKey(teclaAgacharse);
            anim.SetBool("EstaAgachado", agachado);
        }
    }

    // NUEVO: Magia para ocultar la cabeza
    void LateUpdate()
    {
        if (anim != null)
        {
            // Unity busca automáticamente cuál es el hueso de la cabeza en el esqueleto
            Transform huesoCabeza = anim.GetBoneTransform(HumanBodyBones.Head);

            if (huesoCabeza != null)
            {
                // Encogemos la cabeza (y todo lo que cuelgue de ella) a un tamaño de 0
                huesoCabeza.localScale = Vector3.zero;
            }
        }
    }
}