using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleTargetCmaeraTarget : MonoBehaviour
{
    [SerializeField] Transform[] target;

    private void Update()
    {
        transform.position = new Vector3((target[0].position.x + target[1].position.x) / 2,
                                            (target[0].position.y + target[1].position.y) / 2,
                                            (target[0].position.z + target[1].position.z) / 2);


    }

}
