using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ControladorPersonaje : MonoBehaviour
{
    // --- Variables de configuración ---
    public float velocidadCaminar = 5f;
    public float velocidadCorrer = 10f;
    public float velocidadAgachado = 2.5f;

    public float alturaNormal = 2f;
    public float alturaAgachado = 1f;

    // --- NUEVO: Control de la cámara al agacharse ---
    public float alturaCamaraNormal = 0.8f;
    public float alturaCamaraAgachado = 0.2f;

    // --- Referencias ---
    public Transform orientation;
    public Transform cameraPos; // ¡NUEVO! Referencia para mover los ojos

    private CharacterController controller;
    private InputSystem_Actions _input;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        _input = new InputSystem_Actions();
    }

    private void OnEnable() => _input.Player.Enable();
    private void OnDisable() => _input.Player.Disable();

    private void Update()
    {
        GestionarMovimiento();
        // GestionarInteraccion(); (Lo oculto por brevedad)
    }

    private void GestionarMovimiento()
    {
        Vector2 inputMovimiento = _input.Player.Move.ReadValue<Vector2>();
        bool estaCorriendo = _input.Player.Sprint.IsPressed();
        bool estaAgachado = _input.Player.Crouch.IsPressed();

        float velocidadActual = velocidadCaminar;

        if (estaAgachado)
        {
            velocidadActual = velocidadAgachado;

            // 1. Encogemos la cápsula
            controller.height = alturaAgachado;
            // 2. Bajamos el centro para que los pies sigan tocando el suelo
            controller.center = new Vector3(0, -0.5f, 0);
            // 3. Bajamos la cámara
            cameraPos.localPosition = new Vector3(0, alturaCamaraAgachado, 0);
        }
        else
        {
            // 1. Restauramos la cápsula
            controller.height = alturaNormal;
            // 2. Restauramos el centro (normalmente 0,0,0)
            controller.center = new Vector3(0, 0, 0);
            // 3. Restauramos la cámara
            cameraPos.localPosition = new Vector3(0, alturaCamaraNormal, 0);

            if (estaCorriendo)
            {
                velocidadActual = velocidadCorrer;
            }
        }

        Vector3 movimiento = orientation.right * inputMovimiento.x + orientation.forward * inputMovimiento.y;

        // Aquí suele faltar la gravedad en un controlador básico. 
        // Si no tienes gravedad, al bajar escaleras flotarás.
        controller.Move(movimiento * velocidadActual * Time.deltaTime);
    }
}