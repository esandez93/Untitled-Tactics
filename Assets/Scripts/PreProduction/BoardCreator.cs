using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class BoardCreator : MonoBehaviour {
  [SerializeField]
  GameObject tileViewPrefab;
  [SerializeField]
  GameObject tileSelectionIndicatorPrefab;

  Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

  Transform _marker;
  Transform marker {
    get {
      if (_marker == null) {
        GameObject instance = Instantiate(tileSelectionIndicatorPrefab) as GameObject;
        _marker = instance.transform;
      }
			
      return _marker;
    }
  }

  [SerializeField]
  int width = 20;
  [SerializeField]
  int depth = 20;
  [SerializeField]
  int height = 8;

	[SerializeField]
	Point position = new Point (0, 0);

  [SerializeField]
  LevelData levelData;

	public string save_name = null;
	public string load_name = null;

  // Use this for initialization
  void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	Debug.Log(position);
		
	}

	public void MoveMarker (string direction) {
		switch (direction.ToUpper()) {
			case "UP":		--position.x; break;
			case "DOWN":	++position.x; break;
			case "LEFT":	--position.y; break;
			case "RIGHT":	++position.y; break;
			default: return;
		}

		UpdateMarker();
	}

  Rect RandomRect () {
    int x = UnityEngine.Random.Range(0, this.width);
    int y = UnityEngine.Random.Range(0, depth);
    int width = UnityEngine.Random.Range(1, this.width - x + 1);
    int height = UnityEngine.Random.Range(1, depth - y + 1);

    return new Rect(x, y, width, height);
  }

  public void GrowArea () {
    Rect rect = RandomRect();
    GrowRect(rect);
  }

	void GrowRect (Rect rect) {
		for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y) {
			for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x) {
				Point point = new Point(x, y);
				GrowSingle(point);
			}
		}
	}

	void GrowSingle (Point p) {
		Tile tile = GetOrCreate(p);

		if (tile.height < height)
			tile.Grow();
	}

	public void Grow () {
    GrowSingle(position);
	}

  public void ShrinkArea () {
    Rect rect = RandomRect();
    ShrinkRect(rect);
  }
 
	void ShrinkRect (Rect rect) {
		for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y) {
			for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x) {
				Point point = new Point(x, y);
				ShrinkSingle(point);
			}
		}
	}

	void ShrinkSingle (Point point) {
    if (!tiles.ContainsKey(point))
      return;
     
    Tile tile = tiles[point];
    tile.Shrink();
     
    if (tile.height <= 0) {
      tiles.Remove(point);
      DestroyImmediate(tile.gameObject);
    }
	}

	public void Shrink () {
		ShrinkSingle(position);
	}

	public void GenerateRandom () {
		int grow_times = UnityEngine.Random.Range(10, 25);
		int shrink_times = UnityEngine.Random.Range(5, 25);
		int grow_count = 0;
		int shrink_count = 0;
		
		for (int i = 0; i < (grow_times + shrink_times); ++i) {
			int random = UnityEngine.Random.Range(0, 1);

			if (grow_times > grow_count && shrink_times > shrink_count) {
				if (random == 0) {
					GrowArea();
					++grow_count;
				}
				else {
					ShrinkArea();
					++shrink_count;
				}
			}
			else if (grow_times > grow_count && shrink_times <= shrink_count) {
				GrowArea();
				++grow_count;
			}
			else if (grow_times <= grow_count && shrink_times > shrink_count) {
				ShrinkArea();
				++shrink_count;
			}
		}
	}

	Tile Create () {
		GameObject instance = Instantiate(tileViewPrefab) as GameObject;
		instance.transform.parent = transform;

		return instance.GetComponent<Tile>();
	}
 
	Tile GetOrCreate (Point point) {
		if (tiles.ContainsKey(point))
			return tiles[point];
     
		Tile tile = Create();
		tile.Load(point, 0);
		tiles.Add(point, tile);
     
		return tile;
	}
	
	public void UpdateMarker () {
    Tile tile = (tiles.ContainsKey(position)) ? tiles[position] : null;
    marker.localPosition = (tile != null) ? tile.center : new Vector3(position.x, 0, position.y);
	}

	public void Clear () {
    for (int i = transform.childCount - 1; i >= 0; --i) {
			DestroyImmediate(transform.GetChild(i).gameObject);
		}

    tiles.Clear();

		/*var markers = GameObject.FindGameObjectsWithTag("Player");
		Debug.Log(markers.ToString());
		foreach(GameObject mark in markers) {
			DestroyImmediate(mark);
		}*/
	}

	public void Save () {
    string filePath = Application.dataPath + "/Resources/Levels";
    if (!Directory.Exists(filePath))
       CreateSaveDirectory ();
     
    LevelData board = ScriptableObject.CreateInstance<LevelData>();
    board.tiles = new List<Vector3>( tiles.Count );

    foreach (Tile tile in tiles.Values) {
			board.tiles.Add( new Vector3(tile.position.x, tile.height, tile.position.y) );
		}
     
    string fileName = string.Format("Assets/Resources/Levels/{1}.asset", filePath, save_name);
    AssetDatabase.CreateAsset(board, fileName);
	}
 
	void CreateSaveDirectory () {
    string filePath = Application.dataPath + "/Resources";
    if (!Directory.Exists(filePath))
       AssetDatabase.CreateFolder("Assets", "Resources");

    filePath += "/Levels";
    if (!Directory.Exists(filePath))
       AssetDatabase.CreateFolder("Assets/Resources", "Levels");

    AssetDatabase.Refresh();
	}

	public void Load () {
    Clear();

		if (load_name != null) {
			string filePath = "Assets/Resources/Levels/" + load_name + ".asset";
			levelData = (LevelData)AssetDatabase.LoadAssetAtPath(filePath, typeof(LevelData));
		}

    if (levelData == null)
     return;
 
    foreach (Vector3 vector in levelData.tiles) {
      Tile tile = Create();
      tile.Load(vector);
      tiles.Add(tile.position, tile);
    }
	}
}