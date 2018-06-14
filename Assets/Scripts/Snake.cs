using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Snake : MonoBehaviour {

	int score;

	public int snakePositionX;
	public int snakePositionY;

	bool ate = false;

	public GameObject headPrefab;

	public GameObject tailPrefab;

	public TileMap map;

	public TailList tailList;

	private Segment _segmentScript
	{
		get { return GetComponent<Segment> (); }
	}

	private Vector3 _nexPosition;

	void Start(){
		score = 0;
//		tailList = new TailList ();
//		tailList.AddFirst (headPrefab);
	}
	
	// Update is called once per frame
	void Update () {
		move ();
	}

	void move (){
//		Vector3 currPos =  transform.position;
//		GameObject g;
		_nexPosition = new Vector3(snakePositionX,snakePositionY);
		_segmentScript.MoveTo (_nexPosition);
		_segmentScript.map = map;

		if (ate) {
//			g = (GameObject)Instantiate (tailPrefab, currPos + new Vector3 (0f, 0f, -.9f), Quaternion.identity);
//			Segment s = g.GetComponent<Segment> ();
//			s.head = this.gameObject;
//			tailList.AddLast (g);

			_segmentScript.AddPart (tailPrefab, null);
			_segmentScript.MoveTo (_nexPosition);
			ate = false;
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag("Food")) {
			ate = true;
			score++;
			//Debug.Log (score);
			Destroy (other.gameObject);
			map.Spawn ();
		}
	}
}

