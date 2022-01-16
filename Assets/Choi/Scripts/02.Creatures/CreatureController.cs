using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    CreatureMover mover;
    CreatureHp hp;
    CreatureFighter fighter;


    private void Awake()
    {
        mover = GetComponent<CreatureMover>();
        hp = GetComponent<CreatureHp>();
        fighter = GetComponent<CreatureFighter>();
    }


}
