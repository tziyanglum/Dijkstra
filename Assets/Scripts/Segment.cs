using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Segment : MonoBehaviour {

	public Segment previousBodyPart;
	public Segment nextBodyPart;

	public TileMap map;

	public void AddPart(GameObject bodyPrefab, Segment previousBodyPart)
	{
		previousBodyPart = previousBodyPart;

		if (nextBodyPart != null) {
			nextBodyPart.AddPart (bodyPrefab, this);
		} else {
			CreateBodyPart (bodyPrefab, transform.localPosition);
		}
	}

	void CreateBodyPart(GameObject bodyPrefab, Vector3 location){
		var part = Instantiate (bodyPrefab);
//		part.transform.parent = transform.parent;
		var script = part.GetComponent<Segment> ();
		script.previousBodyPart = this;
		script.MoveTo (location);

		nextBodyPart = script;
	}

	public void MoveTo(Vector3 localPosition){
		if (nextBodyPart != null) {
			nextBodyPart.GetComponent<Segment> ().MoveTo (transform.localPosition);
		}
		transform.localPosition = localPosition;
	}
			
}
