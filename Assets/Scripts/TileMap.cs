using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TileMap : MonoBehaviour {

	public GameObject foodPrefab;

	public GameObject selectedPlayer;

	public GameObject map;

	public TileType[] tileTypes;

	public int[,] tiles;
	Node[,] graph;

	public int mapSizeX;
	public int mapSizeY;

	void Start(){

		selectedPlayer.GetComponent<Unit> ().tileX = (int)selectedPlayer.transform.position.x;
		selectedPlayer.GetComponent<Unit> ().tileY = (int)selectedPlayer.transform.position.y;
		selectedPlayer.GetComponent<Unit> ().map = this;
		selectedPlayer.GetComponent<Snake> ().map = this;

		SetFieldData ();
		CreateGraph ();
		CreateField ();
		//InvokeRepeating("Spawn", 3, 4 );
		Spawn();
	}

	void SetFieldData(){
		tiles = new int[mapSizeX, mapSizeY];

		int x, y;

		// grass
		for( x = 0 ; x < mapSizeX ; x++){
			for( y = 0 ; y < mapSizeY ; y++){
				tiles [x, y] = 0;
			}
		}

		// ground
		for( x = 4 ; x < 11 ; x++){
			for( y = 0 ; y < 3 ; y++){
				tiles [x, y] = 1;
			}
		}

		for( x = 4 ; x < 11 ; x++){
			for( y = 12 ; y < 15 ; y++){
				tiles [x, y] = 1;
			}
		}

		tiles [7, 8] = 1;
		tiles [7, 7] = 1;
		tiles [7, 6] = 1;
		tiles [6, 7] = 1;
		tiles [8, 7] = 1;

		// obstacle
		tiles[1, 13] = 2;
		tiles[2, 13] = 2;
		tiles[3, 13] = 2;
		tiles[1, 12] = 2;
		tiles[1, 11] = 2;

		tiles[11, 13] = 2;
		tiles[12, 13] = 2;
		tiles[13, 13] = 2;
		tiles[13, 12] = 2;
		tiles[13, 11] = 2;

		tiles[1, 1] = 2;
		tiles[2, 1] = 2;
		tiles[3, 1] = 2;
		tiles[1, 2] = 2;
		tiles[1, 3] = 2;

		tiles[11, 1] = 2;
		tiles[12, 1] = 2;
		tiles[13, 1] = 2;
		tiles[13, 2] = 2;
		tiles[13, 3] = 2;

		tiles [9, 10] = 2;
		tiles [9, 9] = 2;
		tiles [10, 9] = 2;

		tiles [9, 5] = 2;
		tiles [10, 5] = 2;
		tiles [9, 4] = 2;

		tiles [5, 10] = 2;
		tiles [5, 9] = 2;
		tiles [4, 9] = 2;

		tiles [5, 5] = 2;
		tiles [4, 5] = 2;
		tiles [5, 4] = 2;



	}

	public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY) {

		TileType tt = tileTypes[ tiles[targetX,targetY] ];

		if(UnitCanEnterTile(targetX, targetY) == false)
			return Mathf.Infinity;

		float cost = tt.movementCost;

		return cost;

	}

	void CreateGraph (){
		graph = new Node[mapSizeX,mapSizeY];

		for (int x = 0; x < mapSizeX; x++) {
			for (int y = 0; y < mapSizeY; y++) {
				graph [x,y] = new Node();
				graph [x, y].x = x;
				graph [x, y].y = y;
			}
		}

		for (int x = 0; x < mapSizeX; x++) {
			for (int y = 0; y < mapSizeY; y++) {
				if(x > 0)
					graph[x,y].neighbours.Add( graph[x-1, y] );
				if(x < mapSizeX-1)
					graph[x,y].neighbours.Add( graph[x+1, y] );
				if(y > 0)
					graph[x,y].neighbours.Add( graph[x, y-1] );
				if(y < mapSizeY-1)
					graph[x,y].neighbours.Add( graph[x, y+1] );


			}
		}
	}

	void CreateField (){
		map = new GameObject ("Map");

		for(int x = 0 ; x < mapSizeX ; x++){
			for(int y = 0 ; y < mapSizeY ; y++){
				TileType tt = tileTypes[ tiles[x,y] ];
				GameObject go = (GameObject)Instantiate (tt.tileVisualPrefab, new Vector3 (x, y, 1), Quaternion.identity);

				ClickableTile ct = go.GetComponent<ClickableTile>();
				ct.tileX = x;
				ct.tileY = y;
				ct.map = this;

				go.transform.parent = map.transform;
			}
		}

		//Center camera
		Camera.main.transform.position = new Vector3(
			(mapSizeX-1) / 2.0f, ((mapSizeY-1) / 2.0f), -(mapSizeY)* 1f);
	}

	public Vector3 TileCoordToWorldCoord(int x, int y){
		return new Vector3 (x , y, 0f);
	}

	public bool UnitCanEnterTile(int x, int y){

		return tileTypes [tiles [x, y]].isWalkAble;

	}

	public void GeneratePathTo(int x, int y){
		
		selectedPlayer.GetComponent<Unit>().currentPath = null;

		if (UnitCanEnterTile(x,y) == false) {
			return;
		}

		Dictionary<Node, float> dist = new Dictionary<Node, float> ();
		Dictionary<Node, Node> prev = new Dictionary<Node, Node> ();

		List<Node> Q = new List<Node>();

		Node source = graph[
			selectedPlayer.GetComponent<Unit>().tileX,
			selectedPlayer.GetComponent<Unit>().tileY
		];

		Node target = graph[x,y];

		dist[source] = 0;
		prev[source] = null;

		foreach(Node v in graph){
			if (v != source) {
				dist [v] = Mathf.Infinity;
				prev [v] = null;
			}
			Q.Add(v);
		}

		while(Q.Count > 0)
		{
			Node u = null;

			foreach (Node possibleU in Q) {
				if (u == null || dist[possibleU] < dist[u]) {
					u = possibleU;
				}
			}

			if (u == target) {
				break;
			}

			Q.Remove (u);

			foreach (Node v in u.neighbours) {
//				float alt = dist [u] + u.DistanceTo (v);
				float alt = dist [u] + CostToEnterTile(u.x, u.y, v.x, v.y);
				if (alt < dist [v]) {
					dist [v] = alt;
					prev [v] = u;
				}
			}
		}

		if (prev [target] == null) {
			return;
		}
			
		List<Node> currentPath = new List<Node>();

		currentPath = new List<Node> ();

		Node curr = target;

		while (curr != null) {
			currentPath.Add (curr);
			curr = prev [curr];
		}

		currentPath.Reverse ();

		selectedPlayer.GetComponent<Unit>().currentPath = currentPath;
	}

	public void Spawn (){

		int foodPosX = (int)Random.Range (0, mapSizeX);
		int foodPosY = (int)Random.Range (0, mapSizeY);

		if (UnitCanEnterTile(foodPosX,foodPosY) == false) {
			Spawn ();
			return;
		}

		GameObject fd = Instantiate (foodPrefab, new Vector3 (foodPosX, foodPosY, -.9f), Quaternion.identity) as GameObject;
		TargetFood tf = fd.GetComponent<TargetFood> ();
		tf.tileX = foodPosX;
		tf.tileY = foodPosY;
		tf.map = this;

	}

}
