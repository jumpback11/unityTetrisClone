using UnityEngine;
using System.Collections;

public class Group : MonoBehaviour 
{
	float lastFall = 0;
	public static bool gameover = false;
	public static bool nextSpawn;
	public GameObject excluded, special1, special2;
	private int rotateCount;
	public GameObject endAudio;
	public GameObject preview;

	// Use this for initialization
	void Start () 
	{

		gameover = false;
		// Default position not valid? Then it's game over
		if (!IsValidGridPosition()) 
		{
			//Instantiate(explosion, transform.position, Quaternion.identity);
			Debug.Log("GAME OVER" + GameController.pause);
			Destroy(gameObject);
			Instantiate(endAudio, Vector3.zero, Quaternion.identity);
			gameover = true;

		}
	}

	void Update()
	{

			if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.A)) {
				// Move Left
				Move (-1, 0);

			} else if (Input.GetKeyDown (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.D)) {
				// Move Right
				Move (1, 0);

			} else if (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W)) {
				// Rotate Shape
				#region Rotate code
				if (special1 || special2) {
						switch (rotateCount) {
						case 1:
								// Modify rotation
								transform.Rotate (Vector3.down * 90, Space.Self);
								rotateCount = 0;
								break;
						default:
								// Modify rotation
								transform.Rotate (Vector3.up * 90, Space.Self);
								rotateCount += 1;
								break;
						}

						// See if valid
						if (IsValidGridPosition ()) {	
								// Its valid. Update grid.
								UpdateGrid ();

						} else {
								// Modify rotation
								transform.Rotate (Vector3.down * 90, Space.Self);
						}

				} else {
						// Modify rotation
						transform.Rotate (Vector3.up * 90, Space.Self);

						// See if valid
						if (IsValidGridPosition ()) {	
								// Its valid. Update grid.
								UpdateGrid ();

						} else {
								// Modify rotation
								transform.Rotate (Vector3.down * 90, Space.Self);
						}
				}
				#endregion

			} else if (Input.GetKeyDown (KeyCode.Space)) {
			// Drop
			try
			{
				while (IsValidGridPosition ()) {

					// Modify position
					transform.position = new Vector3 
						(transform.position.x, transform.position.y, transform.position.z - 1);
					
				} 
				
				//Revert position
				transform.position = new Vector3 
						(transform.position.x, transform.position.y, transform.position.z + 1);
				// Its valid. Update grid.
				UpdateGrid ();

				lastFall += 100;

				audio.Play ();
			}
			catch (UnityException e)
			{
				Debug.Log(e);
			}

			} else if (Input.GetKeyDown (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.S) ||
			           lastFall >= 100) {
			//Move down
				// Modify position
				transform.position = new Vector3 
					(transform.position.x, transform.position.y, transform.position.z - 1);
				
				// See if valid
				if (IsValidGridPosition ()) {	
					// Its valid. Update grid.
					UpdateGrid ();

					lastFall = 0;

				} else {
					//Revert position
					transform.position = new Vector3 
						(transform.position.x, transform.position.y, transform.position.z + 1);
					// Clear filled horizontal lines
					Grid.DeleteFullRows ();
					
					if (!GameController.sirensBool)
					{
						nextSpawn = true;
						// Spawn next Group
						FindObjectOfType<Spawner> ().SpawnNext ();
					}

					
					
					audio.Play ();

					transform.DetachChildren();

					Destroy (gameObject);
					
					
				}

			}

			lastFall += 1;

	}

	void UpdateGrid () 
	{
		//remove old children from grid
		for (int z = 0; z < Grid.gridHeight; ++z) 
		{
			for (int x = 0; x < Grid.gridWidth; ++x)
			{
				if (Grid.grid [x,z] != null)
				{
					if (Grid.grid [x,z].parent == transform)
					{
						Grid.grid [x,z] = null;
					}
				}
			}
		}
		// Add new children to grid
		foreach (Transform child in transform) 
		{
			Vector3 v = Grid.RoundVector(child.position);
			Grid.grid[(int)v.x, (int)v.z] = child;

		} 
	}

	bool IsValidGridPosition()
	{
		foreach (Transform child in transform) 
		{
			Vector3 v = Grid.RoundVector(child.position);

			//Not inside border?
			if (!Grid.InsideBoarder(v))
				return false;
			

			//Block in grid cell but not in group?
			if (Grid.grid[(int)v.x, (int)v.z] != null &&
			    Grid.grid[(int)v.x, (int)v.z].parent != transform)
				return false;


		}
		return true;
	}

	void Move(int X, int Z)
	{
		int x = X;
		int z = Z;

		// Modify position
		transform.position = new Vector3 
			(transform.position.x + x, transform.position.y, transform.position.z + z);
		
		// See if valid
		if (IsValidGridPosition ()) {	
			// Its valid. Update grid.
			UpdateGrid ();
		} else {
			//Revert position
			transform.position = new Vector3 
				(transform.position.x - x, transform.position.y, transform.position.z - z);
		}
	}

	void BlockColision()
	{
		// Clear filled horizontal lines
		Grid.DeleteFullRows ();
		
		nextSpawn = true;
		
		// Spawn next Group
		FindObjectOfType<Spawner> ().SpawnNext ();
		
		// Disable script
		enabled = false;
		
		rotateCount = 0;
		
		audio.Play ();
	}

}
