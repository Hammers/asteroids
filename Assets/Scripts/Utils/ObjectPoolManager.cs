using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectPoolInfo
{
	public GameObject prefab;
	public int initialPool = 20;
	public bool willGrow = true;
}

public class ObjectPoolManager : MonoBehaviour
{
	#region Vars
	
	[SerializeField] private ObjectPoolInfo[] initialPools;
	
	private Dictionary<int, ObjectPool> objectPools = new Dictionary<int, ObjectPool>();

	#endregion

	
	#region Initialize

	public void Awake()
	{
		foreach (ObjectPoolInfo poolInfo in initialPools)
		{
			SetupNewPool(poolInfo.prefab,poolInfo.initialPool,poolInfo.willGrow);
		}
	}

	#endregion

	
	#region Pooling

	public GameObject GetPooledObject(GameObject prefab,Vector3 spawnPos, Quaternion spawnRot)
	{
		return GetPooledObject(prefab.GetInstanceID(),spawnPos,spawnRot);
	}
	
	public GameObject GetPooledObject(int gameObjectInstanceId, Vector3 spawnPos, Quaternion spawnRot)
	{
		if( objectPools.ContainsKey( gameObjectInstanceId ) )
		{
			return objectPools[gameObjectInstanceId].GetPooledObject(spawnPos,spawnRot);
		}

		return null;
	}

	public void SetupNewPool(GameObject prefab, int initialPool = 20, bool willGrow = true)
	{
		if (objectPools.ContainsKey(prefab.GetInstanceID()))
		{
			Debug.LogError($"Already created an object pool for {prefab.name}");
			return;
		}
		ObjectPool pool = new ObjectPool(prefab,initialPool,willGrow);
		objectPools.Add(prefab.GetInstanceID(),pool);
	}


	public void DeactivateAllPooledObjects()
	{
		foreach (var pool in objectPools.Values)
		{
			pool.DeactivateAllPooledObjects();
		}
	}
	
	#endregion
	
}
