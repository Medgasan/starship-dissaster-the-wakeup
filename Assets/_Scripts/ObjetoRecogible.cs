using Assets._Scripts.Interfaces;
using Assets._Scripts; // Necesario para acceder al script CanInteract del jugador
using UnityEngine;

public class ObjetoRecogible : MonoBehaviour, IInteractable
{
    [Header("Configuración del Objeto")]
    [Tooltip("El nombre exacto que pedirá la puerta (ej: 'TarjetaMedica', 'LlaveIngenieria')")]
    public string nombreDelObjeto = "LlaveIngenieria";

    public void Interact(object parametro = null)
    {
        // 1. Convertimos el parámetro genérico en el script del jugador
        CanInteract inventarioJugador = parametro as CanInteract;

        // 2. Comprobamos que el jugador existe de verdad
        if (inventarioJugador != null)
        {
            // 3. Añadimos el nombre de este objeto a la mochila del jugador
            inventarioJugador.objetosRecogidos.Add(nombreDelObjeto);

            Debug.Log("Has recogido un objeto clave: " + nombreDelObjeto);

            // 4. Destruimos el objeto del mundo 3D para que desaparezca
            Destroy(gameObject);
        }
    }
}