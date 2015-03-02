using UnityEngine;
using System;
using System.Collections.Generic;

public static class FileReader{
	private static int gridWidth = 16;
	private static int gridHeight = 16;
	public static int[,] radiusInput = new int[16,16];
    private static Dictionary<string, TileType> TileIDs = new Dictionary<string, TileType>()
        {
            {"G", TileType.Solid},
            {"_", TileType.Empty},
            {"S", TileType.Switch},
            {"T", TileType.Trap},
			{"L", TileType.RoomLight},
            {"C", TileType.CeilingLight},
            {"E", TileType.Exit},
        };

	public static TileType[,] LoadFile(TextAsset t1){
		// split input on new line, remove whitespace
		var vertLines = t1.text.Split(new[] {'\r','\n'}, System.StringSplitOptions.RemoveEmptyEntries);
		
        var tiles = new TileType[vertLines.Length, gridWidth];

        for (int j = 0; j < gridHeight; j++) 
        {
			// split on comma
           var inputLine = vertLines[j].Split(new[] { ',' });
			
			for(int i = 0; i < gridWidth; i++)
            {
				if (TileIDs.ContainsKey(inputLine[i][0].ToString()))
                {
					if(inputLine[i][0].ToString() == "L"){
						if(inputLine[i].Length > 1){
							int radius = (int)Char.GetNumericValue(inputLine[i][1]);
							tiles[i, j] = TileIDs[inputLine[i][0].ToString()];
							radiusInput[i, j] = radius;
						}else{
							radiusInput[i, j] = 9;
							tiles[i, j] = TileIDs[inputLine[i][0].ToString()];
						}
					}else{
						tiles[i, j] = TileIDs[inputLine[i][0].ToString()];
					}
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