using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DroneMissionData : MonoBehaviour {

	public ArrayList waypoints;

	public static DroneMissionData CreateFromJSON(string jsonString){
		return JsonUtility.FromJson<DroneMissionData>(jsonString);
	}

}
