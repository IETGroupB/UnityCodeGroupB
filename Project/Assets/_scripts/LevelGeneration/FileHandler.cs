using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

public static class FileHandler{

	public static int mapWidth = 16;

	public static int[,] Load(string filePath){
		try{
			Debug.Log("Loading File...");
			using(StreamReader sr = new StreamReader(filePath)){
				string input = sr.ReadToEnd();
				string[] lines = input.Split(new[]{'\r','\n'}, System.StringSplitOptions.RemoveEmptyEntries);
				int[,] tiles = new int[lines.Length, mapWidth];
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
							tiles[i,j] = val;
						}else{
							tiles[i, j] = 1;
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
