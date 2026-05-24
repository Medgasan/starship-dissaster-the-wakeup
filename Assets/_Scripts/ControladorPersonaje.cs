using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ControladorPersonaje : MonoBehaviour
{
    [Header("Velocidades")]
    public float velocidadCaminar = 5f;
    public float velocidadCorrer = 10f;
    public float velocidadAgachado = 2.5f;

    [Header("Alturas y Agacharse")]
    public float alturaNormal = 2f;
    public float alturaAgachado = 1f;
    public float alturaCamaraNormal = 0.8f;
    public float alturaCamaraAgachado = 0.2f;

    [Header("Físicas")]
    public float gravedad = -9.81f;
    private Vector3 velocidadCaida;

    [Header("Referencias")]
    public Transform orientation;
    public Transform cameraPos;

    private CharacterController controller;
    private Animator anim;
    private InputSystem_Actions _input;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        // Buscamos el Animator en el modelo 3D que es hijo de este objeto
        anim = GetComponentInChildren<Animator>();
        _input = new InputSystem_Actions();
    }

    private void OnEnable() => _input.Player.Enable();
    private void OnDisable() => _input.Player.Disable();

    private void Update()
    {
        GestionarMovimiento();
        GestionarInteraccion();
    }

    private void GestionarMovimiento()
    {
        // 1. LECTURA DE INPUTS
        Vector2 inputMovimiento = _input.Player.Move.ReadValue<Vector2>();
        bool estaCorriendo = _input.Player.Sprint.IsPressed();
        bool estaAgachado = _input.Player.Crouch.IsPressed();

        // 2. LÓGICA DE ESTADOS (Agacharse y Velocidad)
        float velocidadActual = velocidadCaminar;

        if (estaAgachado)
        {
            velocidadActual = velocidadAgachado;
            // Encogemos la cápsula y bajamos la cámara
            controller.height = alturaAgachado;
            controller.center = new Vector3(0, -0.5f, 0);
            cameraPos.localPosition = new Vector3(0, alturaCamaraAgachado, 0);
        }
        else
        {
            // Restauramos los valores de estar de pie
            controller.height = alturaNormal;
            controller.center = new Vector3(0, 0, 0);
            cameraPos.localPosition = new Vector3(0, alturaCamaraNormal, 0);

            // Solo podemos correr si no estamos agachados
            if (estaCorriendo)
            {
                velocidadActual = velocidadCorrer;
            }
        }

        // 3. APLICAR ANIMACIONES AL MODELO 3D
        // Calculamos a qué velocidad se está moviendo realmente
        float velocidadParaAnimacion = inputMovimiento.magnitude * velocidadActual;

        // Si soltamos las teclas, forzamos a 0 para que pase al estado Idle
        if (inputMovimiento.magnitude == 0) velocidadParaAnimacion = 0;

        if (anim != null)
        {
            anim.SetFloat("Velocidad", velocidadParaAnimacion);
        }

        // 4. MOVIMIENTO FÍSICO
        // Caminar en base a donde apunta la cámara (Orientation)
        Vector3 movimiento = orientation.right * inputMovimiento.x + orientation.forward * inputMovimiento.y;
        controller.Move(movimiento * velocidadActual * Time.deltaTime);

        // 5. GRAVEDAD
        // Si estamos tocando el suelo, reiniciamos la fuerza de caída
        if (controller.isGrounded && velocidadCaida.y < 0)
        {
            velocidadCaida.y = -2f; // Un pequeño empujón extra para que se pegue bien al suelo
        }
        // Aplicamos la aceleración de la gravedad constante
        velocidadCaida.y += gravedad * Time.deltaTime;
        controller.Move(velocidadCaida * Time.deltaTime);
    }

    private void GestionarInteraccion()
    {
        if (_input.Player.Interact.WasPressedThisFrame())
        {
            Debug.Log("¡Has pulsado el botón de interactuar!");
        }
    }
}