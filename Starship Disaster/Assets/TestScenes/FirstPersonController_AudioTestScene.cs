// Requiere: com.unity.inputsystem (Package Manager)
// En Project Settings > Player > Active Input Handling → "Input System Package (New)" o "Both"

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float slowWalkSpeed = 2f;
    [SerializeField] private float crouchSpeed = 1.5f;

    [Header("Crouch")]
    [SerializeField] private float standHeight = 2f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float crouchTransitionSpeed = 8f;

    [Header("Look")]
    [SerializeField] private float mouseSensitivity = 0.01f;
    [SerializeField] private float verticalLookLimit = 80f;
    [SerializeField] private Transform cameraTransform;

    // ── Input Actions ────────────────────────────────────────────
    //private InputAction _moveAction;
    //private InputAction _lookAction;
    //private InputAction _slowWalkAction;
    //private InputAction _crouchAction;

    // ── Internals ────────────────────────────────────────────────
    private CharacterController _cc;
    private float _verticalVelocity;
    private float _cameraPitch;
    private float _targetHeight;
    private bool _isCrouching;
    private Vector3 _moveInput;
    private bool _wantsCrouch;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _targetHeight = standHeight;

        if (cameraTransform == null)
            cameraTransform = Camera.main?.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Inicializa el pitch desde la rotación actual de la cámara
        if (cameraTransform != null)
            _cameraPitch = cameraTransform.localEulerAngles.x;

        // Convierte ángulos Unity (0-360) a (-180, 180)
        if (_cameraPitch > 180f) _cameraPitch -= 360f;

    }



    private void Update()
    {
        ApplyMovement();
        ApplyCrouchHeight();
        ApplyLook();
    }

    // ── Movement ─────────────────────────────────────────────────
    public void HandleMovement(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>(); // solo guarda
    }


    private void ApplyMovement()
    {
        Vector3 move = transform.right * _moveInput.x + transform.forward * _moveInput.y;
        move = Vector3.ClampMagnitude(move, 1f) * CurrentSpeed();

        if (_cc.isGrounded)
            _verticalVelocity = -0.5f;
        else
            _verticalVelocity += Physics.gravity.y * Time.deltaTime;

        move.y = _verticalVelocity;
        _cc.Move(move * Time.deltaTime);
    }


    //Todo: solucionar -> refactorizar
    private float CurrentSpeed()
    {
        if (_isCrouching) return crouchSpeed;
        return walkSpeed;
    }

    // ── Crouch ───────────────────────────────────────────────────
    public void HandleCrouch(InputAction.CallbackContext context)
    {

        if (context.performed) _wantsCrouch = true;
        else _wantsCrouch = false;

    }


    private void ApplyCrouchHeight()
    {
        if (_wantsCrouch)
        {
            _isCrouching = true;
            _targetHeight = crouchHeight;
        }
        else if (_isCrouching && CanStandUp())
        {
            _isCrouching = false;
            _targetHeight = standHeight;
        }

        float newHeight = Mathf.Lerp(_cc.height, _targetHeight, crouchTransitionSpeed * Time.deltaTime);
        float delta = newHeight - _cc.height;
        _cc.height = newHeight;
        _cc.center = new Vector3(0f, newHeight / 2f, 0f);
        transform.position += new Vector3(0f, delta / 2f, 0f);
    }



    private bool CanStandUp() =>
        !Physics.SphereCast(transform.position, _cc.radius, Vector3.up,
            out _, standHeight - crouchHeight, ~0, QueryTriggerInteraction.Ignore);

    // ── Look ─────────────────────────────────────────────────────
    public void HandleLook(InputAction.CallbackContext context)
    {
        Vector2 look = context.ReadValue<Vector2>();//_lookAction.ReadValue<Vector2>();

        transform.Rotate(Vector3.up, look.x * mouseSensitivity);

        _cameraPitch -= look.y * mouseSensitivity;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -verticalLookLimit, verticalLookLimit);

    }

    private void ApplyLook()
    {
        if (cameraTransform != null)
            cameraTransform.localRotation = Quaternion.Euler(_cameraPitch, 0f, 0f);
    }
}