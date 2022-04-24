using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objectPrefab;
    private Queue<GameObject> pool = new Queue<GameObject>();
    public int poolSize;

    public static ObjectPool Instance;
    private void Awake() {
        Instance = this;
        for (int i = 0; i  < poolSize; i ++)
        {
            GameObject obj = Instantiate(objectPrefab);
            pool.Enqueue(obj);
            obj.SetActive(false);
            obj.transform.SetParent(transform);
        }
    }

    public GameObject GetFromPool(Vector3 position)
    {
        GameObject obj = pool.Dequeue();
        obj.SetActive(true);
        obj.transform.position = position;
        pool.Enqueue(obj);

        return obj;
    }

}
