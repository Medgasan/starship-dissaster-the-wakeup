using Assets._Scripts.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts
{
    public class CanInteract : MonoBehaviour
    {
        [Header("Configuración del Raycast")]
        public float distanciaInteraccion = 3.0f;
        public LayerMask capaInteractuables;
        public Camera camara;

        [Header("Inventario del Jugador")]
        [Tooltip("Escribe aquí los nombres de los objetos que el jugador ya tiene (ej: 'TarjetaRoja')")]
        public List<string> objetosRecogidos = new List<string>();

        void Update()
        {
            Vector3 origen = camara.transform.position;
            Vector3 direccion = camara.transform.forward;
            RaycastHit hit;

            if (Physics.Raycast(origen, direccion, out hit, distanciaInteraccion, capaInteractuables))
            {
                if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        // EL CAMBIO CLAVE: Le pasamos 'this' (este script entero) a la puerta
                        interactable.Interact(this);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (camara != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(camara.transform.position, camara.transform.forward * distanciaInteraccion);
            }
        }
    }
}