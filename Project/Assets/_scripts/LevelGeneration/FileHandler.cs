using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

public static class FileReader{
	public static int gridWidth = 16;
	public static int gridHeight = 16;
	
	public static TileType[,] LoadFile(string filePath){
		
		//t1.text will give the single string for all the entires in the file
		Debug.Log ("Start File handling!!!");
		TextAsset t1 = (TextAsset)Resources.Load (filePath, typeof(TextAsset));
		Debug.Log (t1.text);
		string input = t1.text;
		string[] vertLines = input.Split(new[] {'\r','\n'}, System.StringSplitOptions.RemoveEmptyEntries);// split the string on the basis of occurance of "\n" in the text files
		// above function actually returns the string without empty spaces
		//split each vert line from above with comma
		TileType[,] tiles = new TileType[vertLines.Length, gridWidth];
		for (int i = 0; i < vertLines.Length; i++) {
			string hoLine = vertLines[i];
			string[] inputLine  = hoLine.Split(new[] {','});
			if(inputLine.Length != gridWidth){
			}
			for(int j = 0; j < gridWidth; j++){
				int t;
				if(int.TryParse(inputLine[j], out t)){
					if(t == 1){
						tiles[i, j] = TileType.Solid;
					}else{
						tiles[i, j] = TileType.Empty;
					}
					
				}else{
					tiles[i, j] = TileType.Empty;
				}
			}
			
		}
		Debug.Log ("Done file reading!!");
		return tiles;
		
	}
}