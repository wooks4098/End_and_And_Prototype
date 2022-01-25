using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureActionScheduler : MonoBehaviour
{
    ICreatureAction currentAction;

    public void StartAction(ICreatureAction action)
    {
        if (currentAction == action) return;
        if (currentAction != null)
        {
            currentAction.Cancel();
        }
        currentAction = action;
    }

    public void CancelCurrentAction()
    {
        StartAction(null);
    }
}
