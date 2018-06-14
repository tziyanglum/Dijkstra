using UnityEngine;
using System.Collections;

public class TargetFood : MonoBehaviour {

	public int tileX;
	public int tileY;
	public TileMap map;

	void Update(){

		map.GeneratePathTo (tileX,tileY);

	}
}
