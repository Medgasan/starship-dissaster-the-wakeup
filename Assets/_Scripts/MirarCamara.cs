using UnityEngine;

public class MirarCamara : MonoBehaviour
{
    private Camera camaraPrincipal;

    void Start()
    {
        // El script busca automáticamente la cámara de tu jugador al empezar
        camaraPrincipal = Camera.main;
    }

    void LateUpdate()
    {
        // Hace que el texto rote continuamente para encarar la lente de la cámara
        transform.LookAt(transform.position + camaraPrincipal.transform.forward);
    }
}