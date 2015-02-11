using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour 
{

	public GameObject[] groups, groups2;
	int spawnInt;

	void Start () 
	{
		spawnInt = SpawnGenerator ();
		SpawnNext ();

	}

	public void SpawnNext() 
	{
		// Spawn Group at current Position
		Instantiate (groups [spawnInt], transform.position = new Vector3 
			            (5, 0, 18), Quaternion.identity);
		spawnInt = SpawnGenerator ();

		SpawnPreview ();
	}

	public void SpawnPreview()
	{
		Instantiate (groups2 [spawnInt], transform.position = new Vector3 
		             (14.5f, 1, 4.5f), Quaternion.identity);

	}

	private int SpawnGenerator()
	{
		// Random Index
		int i = Random.Range(0, groups.Length);
		
		return i;
	}

	void Update()
	{

	}

}
