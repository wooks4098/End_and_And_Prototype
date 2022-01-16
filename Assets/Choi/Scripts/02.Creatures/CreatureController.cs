using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    //CreatureMovement patroller;
    CreaturePatroller patroller;
    CreatureTracker tracker;    
    CreatureCaster caster;
    CreatureFighter fighter;

    [SerializeField] Creature creature;
    CreaturePlayer target;

    private void Awake()
    {
        patroller = GetComponent<CreaturePatroller>();
        tracker = GetComponent<CreatureTracker>();
        caster = GetComponent<CreatureCaster>();
        fighter = GetComponent<CreatureFighter>();
    }


    private void Update()
    {
        if (target.GetIsDead()) return;

    }


    private void PatrolBehaviour()
    {
        patroller.StartPatrolBehaviour();
    }

    private void TrackBehaviour()
    {
        tracker.StartTrackingBehaviour();
    }

    private void CastBehaviour()
    {
        caster.StartSpellCastBehaviour();
    }

    private void AttackBehaviour()
    {
        fighter.StartAttackBehaviour();
    }
}
