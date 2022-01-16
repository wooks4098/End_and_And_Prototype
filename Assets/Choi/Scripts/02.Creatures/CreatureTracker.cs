using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureTracker : MonoBehaviour, ICreatureAction
{

    public void StartTrackingBehaviour()
    {
        GetComponent<CreatureActionScheduler>().StartAction(this);
    }



    public void Cancel()
    {
        
    }
}
