using UnityEngine;
using System.Collections;

public class MoveToSpawner : MonoBehaviour {


	public void Update()
	{
		if (Group.nextSpawn)
		{
		Destroy (gameObject);
		Group.nextSpawn = false;
		}
	}
}
