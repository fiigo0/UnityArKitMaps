using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DroneData {

	public string coordinates;
	public string current_altitude;
	public string max_altitude;
	public string orientation;
	public string velocity;

	public static DroneData CreateFromJSON(string jsonString){
		return JsonUtility.FromJson<DroneData>(jsonString);
	}
}
