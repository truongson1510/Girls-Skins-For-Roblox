using UnityEngine;

public class NeverSleep : MonoBehaviour
{
    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

}
