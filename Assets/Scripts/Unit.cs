using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Unit : MonoBehaviour {

	public int tileX;
	public int tileY;

	public TileMap map;

	public GameObject cube;

	public List<Node> currentPath = null;

	float remainingMovement = 10;

	void Update(){
		if(currentPath != null) {
			int currNode = 0;

			while( currNode < currentPath.Count-1 ) {

				Vector3 start = map.TileCoordToWorldCoord( currentPath[currNode].x, currentPath[currNode].y ) + 
					new Vector3(0, 0, -0.5f) ;
				Vector3 end   = map.TileCoordToWorldCoord( currentPath[currNode+1].x, currentPath[currNode+1].y )  + 
					new Vector3(0, 0, -0.5f) ;

				Debug.DrawLine(start, end, Color.blue);

				currNode++;
			}
		}

		AdvancePathing ();

		transform.position = map.TileCoordToWorldCoord (tileX, tileY);

//		if (Vector3.Distance (transform.position, map.TileCoordToWorldCoord (tileX, tileY)) < 0.001f) {
//			AdvancePathing ();
//		}

//		transform.position = Vector3.Lerp(transform.position, map.TileCoordToWorldCoord (tileX, tileY), 10f * Time.deltaTime);
	}

	void AdvancePathing() {

		if(currentPath==null)
			return;

//		if(remainingMovement <= 0)
//			return;

		transform.position = map.TileCoordToWorldCoord( tileX, tileY);

		remainingMovement -= map.CostToEnterTile(currentPath[0].x, currentPath[0].y, currentPath[1].x, currentPath[1].y );

		tileX = currentPath[1].x;
		tileY = currentPath[1].y;

		GameObject snake = this.gameObject;
		var script = snake.GetComponent<Snake> ();
		script.snakePositionX = tileX;
		script.snakePositionY = tileY;

		currentPath.RemoveAt(0);

		if(currentPath.Count == 1) {
			currentPath = null;
		}
	}
}
