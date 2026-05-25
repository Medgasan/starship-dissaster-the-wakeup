using Assets._Scripts.GenericScritps;
using Assets._Scripts.Interfaces;
using UnityEngine;

public class PuertaBehavior : MonoBehaviour, IInteractable
{

    public bool abrir = false;
    public Animator doorMechanism;
    public DoorStatus doorStatus;
    private GameTimer gameTimer;
    public bool cierreAutomatico = false;


    public void Start() { 
        if (cierreAutomatico)
        {
            gameTimer = GetComponent<GameTimer>();
            gameTimer.OneShot = true;
            gameTimer.onTimeout.AddListener(() => Interact());
            doorStatus.Opened.AddListener(() => DoorStatusIsOpened());
            doorStatus.Closed.AddListener(() => DoorStatusIsClosed());
        }

    }


    public void Interact(object parametro = null)
    {
        if (doorMechanism.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f) return;
        abrir = !abrir;
        doorMechanism.SetBool("Abrir", abrir);
    }


    private void DoorStatusIsOpened()
    {
        Debug.Log("DoorStatusIsOpened event");
        if (cierreAutomatico) { gameTimer.StartTimer(); }
    }


    private void DoorStatusIsClosed()
    {
        Debug.Log("DoorStatusIsClosed event");
        gameTimer.Stop();
    }

}
