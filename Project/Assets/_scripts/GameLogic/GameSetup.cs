using UnityEngine;
using System.Collections;

public class GameSetup : MonoBehaviour {
    LevelGenerator levelGen;
    SwitchSystem switchSystem;
    GameObject character;
    GameObject camera;
	GameObject EnemySpawner;

    void Awake()
    {
        character = GameObject.Find("Character");
        camera = GameObject.Find("Camera");
		EnemySpawner = GameObject.Find ("EnemySpawner");
    }

    public void Initialise(int width, int height, float goDownProb)
    {
        levelGen = gameObject.AddComponent<LevelGenerator>();
        levelGen.GenerateLevel(width, height, goDownProb);
        levelGen.DrawLevel();

        var Start = levelGen.roomGrid.solutionPath[0];
        var Entrance = levelGen.roomGrid.GetRoom(Start).Exit;
        var StartPosition = new Vector3(Entrance.x + Start.x * 16, 0.3f + -Entrance.y + -Start.y, 0.0f);

        character.transform.position = StartPosition;
		EnemySpawner.transform.position = StartPosition;
        camera.transform.position = new Vector3(StartPosition.x, StartPosition.y, camera.transform.position.z);

        switchSystem = gameObject.GetComponent<SwitchSystem>();
        switchSystem.SetUp(levelGen.roomGrid);
    }


    public void DestroyLevel()
    {
        levelGen.DeleteLevel();
        switchSystem = null;
    }
}
