using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PoolObjectType
{
    DialogAll = 0,
}

[System.Serializable]
public class ObjectPool
{
    public PoolObjectType objectType;
    public List<GameObject> Objects = new List<GameObject>();
    public GameObject Object_Prefab;
}
public class ObjectPoolManager : MonoBehaviour
{

    private static ObjectPoolManager instance;
    [ArrayElementTitle("objectType")]
    public ObjectPool[] objectPool;

    public static ObjectPoolManager Instance { get { return instance; } }

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        Setobject();
    }

    void Setobject()
    {
        for (int i = 0; i < objectPool.Length; i++)
        {
            for (int j = 0; j < objectPool[i].Objects.Count; j++)
            {
                objectPool[i].Objects[j] = Instantiate(objectPool[i].Object_Prefab, transform);
                objectPool[i].Objects[j].SetActive(false);
            }
        }
    }

    //사용할 오브젝트 리턴
    public GameObject ReturnObject(PoolObjectType _object)
    {
        var objects = objectPool[(int)_object].Objects;
        var findobject = objects.Find(obj => !obj.activeSelf);
        if (null == findobject)
        {//모두 사용중이라면 생성
            findobject = Instantiate(objectPool[(int)_object].Object_Prefab, transform);
            objects.Add(findobject);
            findobject.SetActive(false);
        }
        return findobject;

        //return null; //사용가능한 오브젝트가 없음
    }


}
