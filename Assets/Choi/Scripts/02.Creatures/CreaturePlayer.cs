using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreaturePlayer : MonoBehaviour
{
    [SerializeField] float playerHp;
    [SerializeField] bool isDead;


    public bool GetIsDead()
    {
        return isDead;
    }
}
