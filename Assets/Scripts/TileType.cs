﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class TileType {

	public string name;
	public GameObject tileVisualPrefab;

	public bool isWalkAble = true;
	public float movementCost = 1;
}
