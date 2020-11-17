using UnityEngine;
using System.Collections.Generic;

public class ObjectPool
{
	#region Vars

	public GameObject prefab;
	public bool willGrow = true;

	private List<GameObject> pooledObjects;

	#endregion

	
	#region Contructors

	public ObjectPool(GameObject prefab, int initialPool = 20, bool willGrow = true)
	{
		this.prefab = prefab;
		this.willGrow = willGrow;
		pooledObjects = new List<GameObject>();
		for (int i = 0; i < initialPool; i++)
		{
			var obj = AddNewPooledObject();
			obj.SetActive(false);
		}
	}

	#endregion

	
	#region Pooling

	private GameObject AddNewPooledObject()
	{
		GameObject obj = Object.Instantiate(prefab);
		pooledObjects.Add(obj);
		return obj;
	}

	public GameObject GetPooledObject(Vector3 spawnPos, Quaternion spawnRot)
	{
		GameObject obj = pooledObjects.Find(x => !x.activeInHierarchy);
		
		if (obj == null && willGrow)
		{
			obj = AddNewPooledObject();
		}

		if (obj != null)
		{
			obj.transform.position = spawnPos;
			obj.transform.rotation = spawnRot;
			obj.SetActive(true);
		}

		return obj;
	}

	public void DeactivateAllPooledObjects()
	{
		foreach (var pooledObject in pooledObjects)
		{
			pooledObject.SetActive(false);
		}
	}
	
	#endregion
	
}
