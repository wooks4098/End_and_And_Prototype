using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSpawn : MonoBehaviour
{
    [SerializeField] GameObject ropePrefab, parentObject;

    [SerializeField]
    [Range(1, 1000)]
    int length = 1;

    [SerializeField] float ropeDistance = 0.21f;
    [SerializeField] bool reset, spawn, snapFirst, snapLast;

    private void Update()
    {
        if (reset)
        {
            foreach (GameObject tmp in GameObject.FindGameObjectsWithTag("Player1"))
            {
                Destroy(tmp);
            }
            reset = false;
        }

        if (spawn)
        {
            Spawn();
            spawn = false;
        }
    }

    private void Spawn()
    {
        int count = (int)(length / ropeDistance);
        for (int x = 0; x < count; x++)
        {
            GameObject tmp;
            tmp = Instantiate(ropePrefab, new Vector3(parentObject.transform.position.x, parentObject.transform.position.y - ropeDistance * (x * 1), parentObject.transform.position.z), Quaternion.identity, parentObject.transform);
            tmp.transform.eulerAngles = new Vector3(0, 0, 0);
            tmp.name = parentObject.transform.childCount.ToString();
            if (x == 0)
            {
                Destroy(tmp.GetComponent<HingeJoint>());
                if(snapFirst)
                {
                    tmp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                }
            }
            else
            {
                tmp.GetComponent<HingeJoint>().connectedBody =
                    parentObject.transform.Find((parentObject.transform.childCount - 1).ToString()).GetComponent<Rigidbody>();
            }

        }

        if(snapLast)
        {
            parentObject.transform.Find((parentObject.transform.childCount).ToString()).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

}
