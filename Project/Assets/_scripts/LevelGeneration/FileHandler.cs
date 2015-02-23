using UnityEngine;
using System.Collections.Generic;

public static class FileReader{
	private static int gridWidth = 16;
	private static int gridHeight = 16;

    private static Dictionary<string, TileType> TileIDs = new Dictionary<string, TileType>()
        {
            {"G", TileType.Solid},
            {"_", TileType.Empty},
            {"S", TileType.Switch},
        };

	public static TileType[,] LoadFile(TextAsset t1){
		// split input on new line, remove whitespace
		var vertLines = t1.text.Split(new[] {'\r','\n'}, System.StringSplitOptions.RemoveEmptyEntries);
		
        var tiles = new TileType[vertLines.Length, gridWidth];

		for (int j = 0; j < vertLines.Length; j++) 
        {
			// split on comma
           var inputLine = vertLines[j].Split(new[] { ',' });
			
			for(int i = 0; i < gridWidth; i++)
            {
                if (TileIDs.ContainsKey(inputLine[i]))
                {
                    tiles[i, j] = TileIDs[inputLine[i]];
                }
                else
                {
                    tiles[i, j] = TileType.Empty;
                    Debug.LogError("Invalid Tile Identifier " + inputLine[i] + " in file " + t1.name);
                }
			}
		}
		
		return tiles;
	}
}