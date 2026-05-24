using UnityEngine;
using UnityEngine.Events;

public class DoorStatus : MonoBehaviour
{

    public UnityEvent Opened;
    public UnityEvent Closed;


    public void DoorOpened()
    {
        Opened.Invoke();
    }


    public void DoorClosed()
    {
        Closed.Invoke();
    }


}
