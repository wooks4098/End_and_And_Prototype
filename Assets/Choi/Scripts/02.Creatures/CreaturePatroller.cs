using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreaturePatroller : MonoBehaviour, ICreatureAction
{

    public void StartPatrolBehaviour()
    {
        GetComponent<CreatureActionScheduler>().StartAction(this);
    }

    public void Cancel()
    {
        
    }
}
