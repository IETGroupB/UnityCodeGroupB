using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

public static class FileHandler{

	public static int mapWidth = 16;

	public static TileType[,] Load(string filePath){
		try{
			Debug.Log("Loading File...");
			using(StreamReader sr = new StreamReader(filePath)){
				string input = sr.ReadToEnd();
				string[] lines = input.Split(new[]{'\r','\n'}, System.StringSplitOptions.RemoveEmptyEntries);
				TileType[,] tiles = new TileType[lines.Length, mapWidth];
				Debug.Log("Parsing...");
				for(int i = 0; i<lines.Length; i++)
				{
					string st = lines[i];
					string[] nums = st.Split(new[] {',' });
					if(nums.Length != mapWidth){
					}
					for(int j = 0; j< Mathf.Min(nums.Length, mapWidth); j++){
						int val;
						if(int.TryParse(nums[j], out val)){
							if(val == 1){
								tiles[i,j] = TileType.Solid;
							}else{
								tiles[i, j] = TileType.Empty;
							}

						}else{
							tiles[i, j] = TileType.Empty;
						}
					}
				}
				Debug.Log("Parsing Completed!");
				return tiles;
			}
		}
		catch(IOException e){
			Debug.Log(e.Message);
		}
		return null;
	}
}
