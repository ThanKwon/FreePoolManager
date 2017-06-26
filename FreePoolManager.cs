//[Created by Tae-Han on 2017-01-20.]

using System.Collections.Generic;
using UnityEngine;

public class FreePoolManager : MonoBehaviour
{
    private Dictionary<int, Queue<PoolObjectInstance>> poolQueue = new Dictionary<int, Queue<PoolObjectInstance>>();

    private Dictionary<string, Transform> deadPoolsParent = new Dictionary<string, Transform>();

    private static FreePoolManager _instance;

    public static FreePoolManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<FreePoolManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("Pool Manager");
                    var script = obj.AddComponent<FreePoolManager>();
                    _instance = script;
                }
            }
            return _instance;
        }
    }

    private void OnDestroy()
    {
        foreach (KeyValuePair<int, Queue<PoolObjectInstance>> queues in poolQueue)
        {
            queues.Value.Clear();
            Debug.Log("Pool Deleted");
        }
        poolQueue.Clear();
        deadPoolsParent.Clear();
        _instance = null;
    }

    #region setPool

    private int SetPoolQueue(GameObject prefabOnly)
    {
        int poolKey = prefabOnly.GetInstanceID();
        if (!poolQueue.ContainsKey(poolKey))
        {
            poolQueue.Add(poolKey, new Queue<PoolObjectInstance>());
        }
        return poolKey;
    }

    /// <summary>
    /// create empty pool
    /// </summary>
    public void CreatePool(GameObject prefabOnly)
    {
        SetPoolQueue(prefabOnly);
    }

    #endregion setPool

    #region Reuse

    private PoolObjectInstance GetReuseableObject(GameObject prefabOnly)
    {
        int poolKey = prefabOnly.GetInstanceID ();

        if (!poolQueue.ContainsKey(poolKey))
            CreatePool(prefabOnly);

        for (int i = 0; i < poolQueue[poolKey].Count; i++)
        {
            var obj = poolQueue[poolKey].Dequeue();
            if (obj.gameObject.activeInHierarchy)
            {
                continue;
            }
            return obj;
        }
        {
            //풀에 여유가 없으면 재생성
            GameObject freshObject = Instantiate(prefabOnly);
            return new PoolObjectInstance(freshObject, poolKey);
        }
    }

    public T ReuseByPrefab<T>(GameObject prefab) where T : FreePoolableBehavior
    {
        PoolObjectInstance objectToReuse = GetReuseableObject(prefab);
        objectToReuse.PoolFromGrave();
        return objectToReuse._poolObjectScript as T;
    }

    #endregion Reuse

    public void ReturnToPool(PoolObjectInstance poolItem, string parentKey)
    {
        if (poolItem != null)
        {
            if (!deadPoolsParent.ContainsKey(parentKey))
            {
                GameObject thing = new GameObject(parentKey);
                deadPoolsParent[parentKey] = thing.transform;
            }

            poolItem.transform.SetParent(deadPoolsParent[parentKey]);

            poolItem.gameObject.SetActive(false);

            poolQueue[poolItem.prefabKey].Enqueue(poolItem);
        }
        else
        {
            Debug.LogError("Null Object Disabled");
        }
    }
}

public class PoolObjectInstance
{
    public int prefabKey;
    public GameObject gameObject;
    public Transform transform;
    public FreePoolableBehavior _poolObjectScript;

    public PoolObjectInstance(GameObject f_objectInstance, int f_prefabKey)
    {
        this.prefabKey = f_prefabKey;
        this.gameObject = f_objectInstance;
        this.transform = gameObject.transform;
        _poolObjectScript = gameObject.GetComponent<FreePoolableBehavior>();
    }

    public void PoolFromGrave()
    {
        transform.SetParent(null);
        gameObject.SetActive(true);
        _poolObjectScript.ReuseObject(this);
    }
}