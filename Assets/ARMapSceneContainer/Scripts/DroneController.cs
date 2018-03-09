using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Firebase.Database;

public class DroneController : MonoBehaviour,FBConnectionManager.IFirebaseCallback  {

	[SerializeField]
	public AbstractMap _map;
	[SerializeField]
	public GameObject trail;
	private GameObject drone;
	private DroneLocation droneManager;


	private Mapbox.Utils.Vector2d dronePosition;
	private string dronePositionString;

	Vector3 initialPosition;
	// Use this for initialization
	void Start () {

		drone = GameObject.FindGameObjectWithTag ("drone");
		print ("Map : " + _map.isActiveAndEnabled);

		FBConnectionManager.FirebaseConsult ("droneLocation", this);
	}

	// Update is called once per frame
	void Update () {	
		if (_map.isActiveAndEnabled){	
			if (string.IsNullOrEmpty (dronePositionString)) {

				Mapbox.Utils.Vector2d mapPosition = _map.CenterLatitudeLongitude;
				dronePositionString = string.Format("{0},{1}", mapPosition.x,mapPosition.y);
				print ("initial positioning on the center of the map : " + dronePositionString);
			}

			//37.792159, -122.401723
			print(dronePositionString);
			dronePosition = Conversions.StringToLatLon (dronePositionString);
			drone.transform.MoveToGeocoordinate (dronePosition, _map.CenterMercator, _map.WorldRelativeScale);
			dronePosition = drone.transform.GetGeoPosition (_map.CenterMercator, _map.WorldRelativeScale);

		}	
	}


	void addTrail(){
		var y = drone.transform.position.y;
		var trailPoint = Instantiate(trail, new Vector3(0,y,0), Quaternion.identity);
		trailPoint.transform.MoveToGeocoordinate (dronePosition, _map.CenterMercator, _map.WorldRelativeScale);
	}

	public void FailResponse(string response){

	}
	public void SuccessResponse(DataSnapshot snapshot){
		string json = snapshot.GetRawJsonValue();
		print (json);
		droneManager = DroneLocation.CreateFromJSON (json);	

		dronePositionString = droneManager.coordinates;

		print (dronePositionString);

		if (_map.isActiveAndEnabled) {
			addTrail();
		}
	}

}
