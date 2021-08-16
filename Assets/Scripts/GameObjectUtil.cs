using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectUtil {
    
    private static Dictionary<RecycleGameObject, ObjectPool> _pools = new Dictionary<RecycleGameObject, ObjectPool>();

    
    public static GameObject Instantiate(GameObject prefab, Vector3 pos) {
        GameObject instance = null;
        var recycleScript = prefab.GetComponent<RecycleGameObject>();
        
        if (recycleScript != null) {
            var pool = GetObjectPool(recycleScript);
            instance = pool.NextObject(pos).gameObject;
        }
        else {
            instance = GameObject.Instantiate(prefab);
            instance.transform.position = pos;
        }

        return instance;
    }
    
    private static ObjectPool GetObjectPool(RecycleGameObject reference) {
        ObjectPool pool = null;
        if (_pools.ContainsKey(reference)) {
            pool = _pools[reference];
        }
        else {
            var poolContainer = new GameObject(reference.gameObject.name + "ObjectPool");
            pool = poolContainer.AddComponent<ObjectPool>();
            pool.prefab = reference;
            _pools.Add(reference, pool);
        }

        return pool;
    }
}