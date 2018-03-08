using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DroneLocation {
	public string coordinates;
	public string latitud;
	public string longitud;

	public static DroneLocation CreateFromJSON(string jsonString){
		return JsonUtility.FromJson<DroneLocation>(jsonString);
	}

}
