using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectUI : MonoBehaviour
{
    public Transform Canvas;
    public Transform player;
    private void Update()
    {
        Vector3 l_vector = player.transform.position - transform.position;
        l_vector.y = 0;
        transform.rotation = Quaternion.LookRotation(l_vector).normalized;

        //Canvas.LookAt(player);

    }
}
