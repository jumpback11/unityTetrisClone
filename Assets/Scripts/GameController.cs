using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GUIText scoreText;
	public GUIText rowsText;
	public GUIText multiplierText;
	public GUIText gameoverText;
	public GUIText maxMultiplierText;
	private int score, rows, maxMultiplier;
	private int multiplier = 1;
	private int timer, soundTimer;
	public GameObject sirenFar, sirenMid, sirenClose, airRaid;
	public static bool sirensBool;
	public static bool pause;


	// Use this for initialization
	void Start () {
		gameoverText.text = "";
		score = 0;
		rows = 0;
		maxMultiplier = 1;
		sirensBool = false;
		soundTimer = 0;
		pause = false;
		UpdateScore ();
		UpdateRows ();
		UpdateMultiplier ();
		UpdateMaxMultiplier ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer += 1;
		soundTimer += 1;
		UpdateScore ();
		UpdateRows ();
		UpdateMultiplier ();
		UpdateMaxMultiplier ();
		if (Group.gameover) 
		{
			audio.Stop();
			gameoverText.text = ("Game Over" + '\n' + "Press -R- to" + '\n' + "Restart.");

			if (Input.GetKeyDown (KeyCode.R))
			{
				Application.LoadLevel (Application.loadedLevel);
				gameoverText.text = "";
				score = 0;
				rows = 0;
				maxMultiplier = 1;
				UpdateScore ();
				UpdateRows ();
				UpdateMultiplier ();
				UpdateMaxMultiplier ();
			}

		}
		if (timer >= 300) 
		{
			multiplier = 1;
			UpdateMultiplier ();
			timer = 0;
		}

		if (multiplier >= 10) 
		{
			if (!sirensBool) 
			{
				sirensBool = true;
				soundTimer = 0;
			}

		} 
		if (sirensBool)
		{
		StartSirens ();
		}
	}

	void UpdateScore()
	{
		scoreText.text = "Score: " + score;
	}

	void UpdateRows()
	{
		rowsText.text = "Rows: " + rows;
	}

	void UpdateMultiplier()
	{
		multiplierText.text = "Multiplier: " + multiplier;

	}

	void UpdateMaxMultiplier()
	{

		maxMultiplierText.text = "Highest Multiplier: " + maxMultiplier;
	}

	void CheckMax(int newMax)
	{
		if (newMax > maxMultiplier) 
		{
			maxMultiplier = newMax;
		}
		UpdateMaxMultiplier ();
	}

	public void AddScore(int newValue)
	{
		score += newValue * multiplier;

		UpdateScore ();
	}

	public void AddMultiplier(int newValue)
	{
		multiplier += newValue;
		timer = 0;

		CheckMax (multiplier);
		UpdateMultiplier ();
	}

	public void AddRows(int newValue)
	{
		rows += newValue;
		
		UpdateRows ();
	}

	public void StartSirens()
	{
		//pause game
		pause = true;
		//start Air Raid
		if (soundTimer == 2) {
			Instantiate (sirenFar, transform.position, transform.rotation);
		}else if (soundTimer == 200) {
			Instantiate (sirenMid, transform.position, transform.rotation);
		} else if (soundTimer == 400) {
			Instantiate (sirenClose, transform.position, transform.rotation);
		} else if (soundTimer == 700) {
			Instantiate (airRaid, transform.position, transform.rotation);
		} else if (soundTimer == 1300) {
			Bomb ();
		}else if (soundTimer == 1500){
			//unpause game
			pause = false;
			//clear preview
			Group.nextSpawn = true;
			// Spawn next Group
			FindObjectOfType<Spawner> ().SpawnNext ();
			sirensBool = false;

		}
	}

	public void Bomb() 
	{
		for (int z = 0; z < Grid.gridHeight; ++z) 
		{
			for (int x = 0; x < Grid.gridWidth; x++) 
			{
				if (Grid.grid[x,z] != null)
				{
					Destroy (Grid.grid[x,z].gameObject);
					Grid.grid[x,z] = null;
					Instantiate(Grid.explosion,new Vector3(x, 0, z), Quaternion.identity);
					int score = 10;
					AddScore (score);


				}
			}
		}
		

	}

}
