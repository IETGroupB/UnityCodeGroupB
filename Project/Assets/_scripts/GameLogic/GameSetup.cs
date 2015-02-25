using UnityEngine;
using System.Collections;

public class GameSetup : MonoBehaviour {
    LevelGenerator levelGen;
    SwitchSystem switchSystem;


    public void Initialise(int width, int height, float goDownProb)
    {
        levelGen = gameObject.AddComponent<LevelGenerator>();
        levelGen.GenerateLevel(width, height, goDownProb);
        levelGen.DrawLevel();

        switchSystem = gameObject.GetComponent<SwitchSystem>();
        switchSystem.SetUp(levelGen.roomGrid);
    }
}
