using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._Scripts.GenericScritps
{
    [AddComponentMenu("Gameplay/Game Timer")]
    [DisallowMultipleComponent]
    public class GameTimer : MonoBehaviour
    {
        [Header("Timer Settings")]
        [SerializeField] private float waitTime = 1.0f;
        [SerializeField] private bool oneShot = true;
        [SerializeField] private bool autoStart = false;

        [Header("Events")]
        public UnityEvent onTimeout;

        // Propiedades públicas
        public float WaitTime { get => waitTime; set => waitTime = Mathf.Max(0, value); }
        public bool OneShot { get => oneShot; set => oneShot = value; }
        public bool IsActive => isActive;

        private float timeLeft;
        private bool isActive = false;

        private void Start()
        {
            if (autoStart) StartTimer();
        }

        private void Update()
        {
            if (!isActive) return;

            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                onTimeout.Invoke();
                if (oneShot) isActive = false;
                else timeLeft = waitTime;
            }
        }

        public void StartTimer()
        {
            timeLeft = waitTime;
            isActive = true;
        }

        public void Stop() => isActive = false;

        public void Resume()
        {
            if (timeLeft > 0) isActive = true;
        }

        public void ResetTimer()
        {
            timeLeft = waitTime;
        }

    }
}
