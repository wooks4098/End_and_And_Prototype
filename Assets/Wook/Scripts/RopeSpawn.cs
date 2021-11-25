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
        //길이만큼 로프 생성개수 정하기
        int count = (int)(length / ropeDistance);

        for (int x = 0; x < count; x++)
        {
            GameObject tmp;
            tmp = Instantiate(ropePrefab, new Vector3(parentObject.transform.position.x, parentObject.transform.position.y - ropeDistance * (x * 1), parentObject.transform.position.z), Quaternion.identity, parentObject.transform);
            tmp.transform.eulerAngles = new Vector3(0, 0, 0);
            tmp.name = parentObject.transform.childCount.ToString();
            if (x == 0)
            {//첫번째 인경우 HingeJoint제거
                Destroy(tmp.GetComponent<HingeJoint>());
                if(snapFirst)
                {//고정
                    tmp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                }
            }
            else
            {//위에 있는 Rope의 Rigidbody를 HingeJoint에 연결
                tmp.GetComponent<HingeJoint>().connectedBody =
                    parentObject.transform.Find((parentObject.transform.childCount - 1).ToString()).GetComponent<Rigidbody>();
            }

        }

        if(snapLast)
        {//고정
            parentObject.transform.Find((parentObject.transform.childCount).ToString()).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

}
