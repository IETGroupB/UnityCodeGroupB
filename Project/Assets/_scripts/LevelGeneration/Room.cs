using UnityEngine;
using System.Collections.Generic;

public enum ExitType { LR, LRT, LRB, LRTB, None };
public enum LightingType {Dark, Dim, Bright};

public class Room    {
    public bool isSolutionPath = false;
    public bool isExit = false;
    public bool isStart = false;
    public Point Exit;
    public ExitType exits;
    public TileType[,] tiles = new TileType[16, 16];
    public LightingType lightState;
    public GameObject[] trapTiles;
	public GameObject[] lightTiles;
    public GameObject[,] solidTiles;
	public int[] radiusArray;

    private TilePrefabManager prefabs;
    private GameObject roomObj;
    private GameObject[] CeilingLights;

    //switch specific parameters
    private Point switchLocation;
    private bool hasSwitch;
    public Switch switchParams;
	public RoomLight lightParams;

    public Room(ExitType exits, TilePrefabManager prefabsLink)
    {
        this.exits = exits;
        prefabs = prefabsLink;
        solidTiles = new GameObject[16, 16];
    }

    public void DrawRoom(GameObject room)
    {
        if (exits == ExitType.None) return;

        roomObj = room;
        List<GameObject> trapTileList = new List<GameObject>();
		List<GameObject> roomLightList = new List<GameObject>();
        List<GameObject> ceilingLightList = new List<GameObject>();
		List<int> radiusLightList = new List<int>();
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                switch(tiles[x, y])
                {
                    case TileType.Solid:
                        var solidTile = (GameObject) MonoBehaviour.Instantiate(prefabs.tileGameObjects[tiles[x, y]] as GameObject);
                        solidTile.transform.parent = room.transform;
                        solidTile.transform.localPosition = new Vector3(x, -y, 0.0f);
                        solidTiles[x, y] = solidTile;
                        break;
                    case TileType.Switch:
                        switchLocation = new Point(x, y);
                        break;
                    case TileType.Trap:
                        var trapTile = (GameObject)MonoBehaviour.Instantiate(prefabs.tileGameObjects[tiles[x, y]] as GameObject);
                        trapTile.transform.parent = room.transform;
                        trapTile.transform.localPosition = new Vector3(x, -y, 0.0f);
                        trapTileList.Add(trapTile);
                        break;
					case TileType.RoomLight:
						var roomLightTile = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Tiles/RoomLight/roomLight", typeof(GameObject)))  as GameObject;
						roomLightTile.transform.parent = room.transform;
						roomLightTile.transform.localPosition = new Vector3(x, -y, -5.0f);
						roomLightList.Add (roomLightTile);
						int radius = FileReader.radiusInput[x, y];
						radiusLightList.Add (radius);
						break;
                    case TileType.CeilingLight:
                        var ceilingLight = (GameObject)MonoBehaviour.Instantiate(prefabs.tileGameObjects[tiles[x, y]] as GameObject);
                        ceilingLight.transform.parent = room.transform;
                        ceilingLight.transform.localPosition = new Vector3(x, -y, 0.0f);
                        ceilingLight.GetComponent<CeilingLight>().SetRange(FileReader.radiusInput[x, y]);
                        ceilingLightList.Add(ceilingLight);
                        break;
                    case TileType.Exit:
                        Exit = new Point(x, y);
                        if (isExit)    
                        {
                            var exitDoor = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Tiles/door", typeof(GameObject))) as GameObject;
                            exitDoor.transform.parent = room.transform;
                            exitDoor.transform.localPosition = new Vector3(x, -y, 0.002f);
                        }
                        break;
                }	
            }
        }

        trapTiles = trapTileList.ToArray();
		lightTiles = roomLightList.ToArray();
		radiusArray = radiusLightList.ToArray ();
        CeilingLights = ceilingLightList.ToArray();

        if (switchLocation == null && exits != ExitType.None)
        {
            Debug.LogError("Room file (Exit type " + exits + ") does not contain switch");
        }



        var bg = (GameObject)GameObject.Instantiate(GameObject.Find("LevelGeneration").GetComponent<RoomBackgrounds>().GetRoomBackground());
        bg.transform.parent = roomObj.transform;
        bg.transform.localPosition = new Vector3(7.5f, -7.5f, 0.003f);

        Resources.Load<GameObject>(Tile.prefabs[TileType.CeilingLight]);
    }

    public void AddSwitch() 
    {
        hasSwitch = true;

        var switchObj = (GameObject) MonoBehaviour.Instantiate(prefabs.tileGameObjects[TileType.Switch] as GameObject);

        switchObj.transform.parent = roomObj.transform;
        switchObj.transform.localPosition = new Vector3(switchLocation.x, -switchLocation.y, 0.0015f);

        switchParams = switchObj.GetComponent<Switch>();
    }

    public void ToggleTraps(bool on)
    {
        for (var i = 0; i < trapTiles.Length; i++)
        {
            trapTiles[i].GetComponent<TrapTile>().isActive = on;
        }
    }

	public void ToggleAlarm(bool on)
	{
		    //for just one alarm light in each room
			lightTiles[0].GetComponent<RoomLight>().isAlarmActive = on;
	}
	
	public void UpdateRoomLight(){
		var  c = new Color(Random.Range(0.55f, 1.0f),Random.Range(0.55f, 1.0f), Random.Range(0.55f, 1.0f));
		for (var i = 0; i < lightTiles.Length; i++) 
		{
			lightParams = lightTiles[i].GetComponent<RoomLight>();
			lightParams.gameObject.GetComponent<RoomLight>().SetRadius(radiusArray[i]);
			lightParams.gameObject.GetComponent<Light>().color = c;
			lightParams.UpdatePointLightIntensity(lightState);
		}

        for (var i = 0; i < CeilingLights.Length; i++)
        {
            CeilingLights[i].GetComponent<CeilingLight>().UpdateLightIntensity(lightState);
        }
	}
}
