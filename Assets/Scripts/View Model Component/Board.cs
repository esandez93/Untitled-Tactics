using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
 
public class Board : MonoBehaviour {
  [SerializeField]
	GameObject tilePrefab;

  public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();
	private Point[] directions = new Point[4] {
    new Point(0, 1),
    new Point(0, -1),
    new Point(1, 0),
    new Point(-1, 0)
	};
	Color selectedTileColor = new Color(0, 1, 1, 1);
	Color defaultTileColor = new Color(1, 1, 1, 1);
 
  public void Load (LevelData data) {
    for (int i = 0; i < data.tiles.Count; ++i) {
      GameObject instance = Instantiate(tilePrefab) as GameObject;
      Tile tile = instance.GetComponent<Tile>();
      tile.Load(data.tiles[i]);
      tiles.Add(tile.position, tile);
    }
  }

	void ClearSearch () {
    foreach (Tile tile in tiles.Values) {
      tile.prev = null;
      tile.distance = int.MaxValue;
    }
	}

	public List<Tile> Search (Tile start, Func<Tile, Tile, bool> addTile) {
		List<Tile> retValue = new List<Tile>();
		retValue.Add(start);
 
		ClearSearch();
		Queue<Tile> checkNext = new Queue<Tile>();
		Queue<Tile> checkNow = new Queue<Tile>();

		start.distance = 0;
		checkNow.Enqueue(start);

		while (checkNow.Count > 0) {
			Tile tile = checkNow.Dequeue();
			
			for (int i = 0; i < 4; ++i) {
				Tile next = GetTile(tile.position + directions[i]);
				
				if (next == null || next.distance <= tile.distance + 1)
					continue;

				if (addTile(tile, next)) {
					next.distance = tile.distance + 1;
					next.prev = tile;

					checkNext.Enqueue(next);
					retValue.Add(next);
				}
			}

			if (checkNow.Count == 0)
			  SwapReference(ref checkNow, ref checkNext);
		}
 
		return retValue;
	}

	public Tile GetTile (Point point) {
    return tiles.ContainsKey(point) ? tiles[point] : null;
	}

	void SwapReference (ref Queue<Tile> tile1, ref Queue<Tile> tile2) {
    Queue<Tile> temp = tile1;
    tile1 = tile2;
    tile2 = temp;
	}

	public void SelectTiles (List<Tile> tiles) {
    for (int i = tiles.Count - 1; i >= 0; --i)
      tiles[i].GetComponent<Renderer>().material.SetColor("_Color", selectedTileColor);
	}
 
	public void DeSelectTiles (List<Tile> tiles) {
    for (int i = tiles.Count - 1; i >= 0; --i)
      tiles[i].GetComponent<Renderer>().material.SetColor("_Color", defaultTileColor);
	}
}