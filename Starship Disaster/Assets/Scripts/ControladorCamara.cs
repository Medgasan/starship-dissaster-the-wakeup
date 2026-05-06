using UnityEngine;

public class ControladorCamara : MonoBehaviour
{
    public float senX = 15f;
    public float senY = 15f;
    public Transform orientation;

    // Cambiamos el nombre de la clase aquí
    private InputSystem_Actions _input;

    float xRotation;
    float yRotation;

    void Awake()
    {
        // Y la instanciamos con el nuevo nombre
        _input = new InputSystem_Actions();
    }

    void OnEnable()
    {
        _input.Player.Enable();
    }

    void OnDisable()
    {
        _input.Player.Disable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // El resto del código se queda EXACTAMENTE IGUAL
        // porque la acción "Look" sigue existiendo y llamándose igual en la plantilla
        Vector2 mouseDelta = _input.Player.Look.ReadValue<Vector2>();

        float mouseX = mouseDelta.x * senX * 0.1f;
        float mouseY = mouseDelta.y * senY * 0.1f;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);

        if (orientation != null)
        {
            orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
        }
    }
}