using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnakeNode {

	public GameObject segment;
	public SnakeNode next;
}

public class TailList{
	private SnakeNode headNode;

	public void AddFirst(GameObject s){
		SnakeNode toAdd = new SnakeNode ();
		toAdd.segment = s;
		toAdd.next = headNode;
		headNode = toAdd;
		Debug.Log ("hoi");
	}

	public void AddLast(GameObject s){
		if (headNode == null) {
			headNode = new SnakeNode ();
			headNode.segment = s;
			headNode.next = null;
		} else {
			SnakeNode toAdd = new SnakeNode ();
			toAdd.segment = s;

			SnakeNode current = headNode;
			while (current.next != null) {
				current = current.next;
				Debug.Log ("doei");
			}
			current.next = toAdd;
			Debug.Log ("mazzel");
		}

	}
		
}
