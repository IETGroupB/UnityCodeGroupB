using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
	public void LoadLevel(string scene)
	{
		Application.LoadLevel(scene);
	}
	
	public void Exit()
	{
		Application.Quit(); 
	}
}