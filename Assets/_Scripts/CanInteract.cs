using Assets._Scripts.Interfaces;
using UnityEngine;

namespace Assets._Scripts
{
    public class CanInteract : MonoBehaviour
    {

        [Header("Configuración del Raycast")]
        [Tooltip("Distancia máxima a la que el jugador puede alcanzar los objetos.")]
        public float distanciaInteraccion = 3.0f;

        [Tooltip("Capa (Layer) donde están los objetos interactuables para optimizar el Raycast.")]
        public LayerMask capaInteractuables;

        // Ya no necesitamos almacenar el objeto en un Trigger, lo buscamos en tiempo real
        void Update()
        {
            // Lanzamos el rayo hacia adelante desde la posición del jugador/cámara
            // Si estás en 1ª persona, usa la posición de la cámara principal. Si es 3ª persona, el transform del jugador funciona.
            Vector3 origen = transform.position;
            Vector3 direccion = transform.forward;

            // Creamos una variable para almacenar la información del impacto
            RaycastHit hit;

            // Lanzamos el rayo físico
            if (Physics.Raycast(origen, direccion, out hit, distanciaInteraccion, capaInteractuables))
            {
                // Si lo que golpea el rayo tiene la interfaz...
                if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
                {
                    // Mostramos el mensaje (puedes conectarlo a tu UI)
                    // Debug.Log("Presiona 'E' para interactuar con: " + hit.collider.name);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactable.Interact();
                    }
                }
            }
        }

        // Dibuja el rayo en la ventana de Escena para que puedas calibrar la distancia visualmente
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * distanciaInteraccion);
        }


    }
}
