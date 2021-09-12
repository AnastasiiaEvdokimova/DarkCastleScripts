using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void IceCreation();
    public static event IceCreation CreationFinished;

  public static void StopIceSupport()
    {
        CreationFinished?.Invoke();
    }
}
