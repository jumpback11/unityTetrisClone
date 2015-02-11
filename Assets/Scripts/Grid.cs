using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour 
{
	// The Grid itself
	public static int gridWidth = 10;
	public static int gridHeight = 25;
	public static Transform[,] grid = new Transform[gridWidth,gridHeight];
	public static GameObject explosion = Resources.Load<GameObject>("explosion");
	public static GameController gameController;
	private static GameObject preview;

	void Start()
	{
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) 
		{
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		if (gameController == null) {
			Debug.Log ("cannot find 'GameController' script");
		}
	}
	public static Vector3 RoundVector(Vector3 v)
	{
		return new Vector3 (Mathf.RoundToInt (v.x), Mathf.RoundToInt (v.y), Mathf.RoundToInt (v.z));
	}

	public static bool InsideBoarder(Vector3 position)
	{
		return ((int)position.x >= 0 && (int)position.x < gridWidth 
				&& (int)position.y >= 0 && (int)position.z >= 0); 
		
	}

	public static void DeleteFullRows() 
	{
		for (int z = 0; z < gridHeight; ++z) 
		{

			if (IsRowFull(z)) 
			{
				DeleteRow(z);
				DecreaseRowsAbove(z+1);
				--z;
			}
		}
		
	}

	public static void DeleteRow(int z)
	{
		int scoreMultiplier = 1;
		gameController.AddMultiplier (scoreMultiplier);
		gameController.AddRows (scoreMultiplier);

		for (int x = 0; x < gridWidth; x++) 
		{
			Destroy (grid[x,z].gameObject);
			grid[x,z] = null;
			Instantiate(explosion,new Vector3(x, 0, z), Quaternion.identity);
			int score = 10;
			gameController.AddScore (score);

		}
	}

	public static void DecreaseRow(int z)
	{
		for (int x = 0; x < gridWidth; x++) 
		{
			if (grid[x,z] != null)
			{
				//Move one towards the bottom
				grid[x,z-1] = grid[x,z];
				grid[x,z] = null;

				//update block position
				grid[x,z-1].position += new Vector3(0, 0, -1);
			}
		}
	}

	public static void DecreaseRowsAbove(int z) 
	{


		for (int i = z; i < gridHeight; ++i) 
		{

			DecreaseRow (i);
		}
	}

	public static bool IsRowFull(int z) 
	{
		
		for (int x = 0; x < gridWidth; ++x) 
		{
			if (grid [x,z] == null)
			{
				return false;
			}
		}
		return true;
		
	}

}
