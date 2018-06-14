using UnityEngine;
using System.Collections;

public class SpawnFood : MonoBehaviour {

	public GameObject foodPrefab;

	public TileMap tileMap;

	// Use this for initialization
	void Start () {
		//InvokeRepeating("Spawn", 3, 4 );

		//Spawn ();
	}

	void Spawn (){
		int mapSizeX = tileMap.mapSizeX;
		int mapSizeY = tileMap.mapSizeY;

		int x = (int)Random.Range (0, mapSizeX);

		int y = (int)Random.Range (0, mapSizeY);

		Instantiate (foodPrefab, new Vector3 (x, y, -0.75f), Quaternion.identity);

		//tileMap.GeneratePathTo (x,y);
	}

}
