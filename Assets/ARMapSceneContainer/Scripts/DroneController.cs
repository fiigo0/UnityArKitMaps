using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Firebase.Database;
using System.Text;

public class DroneController : MonoBehaviour,FBConnectionManager.IFirebaseCallback  {

	[SerializeField]
	public AbstractMap _map;
	[SerializeField]
	public GameObject trail;
	public GameObject waypointObject;
	private GameObject drone;
	private DroneData droneData;
	private DroneMissionData missionData;


	private Mapbox.Utils.Vector2d dronePosition;
	private string dronePositionString;
	private float droneDefaultAltitude;


	Vector3 initialPosition;
	// Use this for initialization
	void Start () {

		drone = GameObject.FindGameObjectWithTag ("drone");
		FBConnectionManager.FirebaseConsult ("DroneData", this);
		FBConnectionManager.FirebaseConsult ("DroneMissionData/waypoints", this);
	}

	// Update is called once per frame
	void Update () {	
		updateDronePosition ();
	}

	void updateDronePosition(){
		if (_map.isActiveAndEnabled){	
			if (string.IsNullOrEmpty (dronePositionString)) {

				Mapbox.Utils.Vector2d mapPosition = _map.CenterLatitudeLongitude;
				dronePositionString = string.Format("{0},{1}", mapPosition.x,mapPosition.y);
			}

			//37.792159, -122.401723
//			print(dronePositionString);
			dronePosition = Conversions.StringToLatLon (dronePositionString);
			drone.transform.MoveToGeocoordinate (dronePosition, _map.CenterMercator, _map.WorldRelativeScale);
			droneDefaultAltitude = drone.transform.position.z;
			dronePosition = drone.transform.GetGeoPosition (_map.CenterMercator, _map.WorldRelativeScale);
		}
	}

	void updateDroneAltitude(){
		
	}

	void addTrail(){
		var y = drone.transform.position.y;
		var trailPoint = Instantiate(trail, new Vector3(0,y,0), Quaternion.identity);
		trailPoint.transform.MoveToGeocoordinate (dronePosition, _map.CenterMercator, _map.WorldRelativeScale);
	}

	public void FailResponse(string response, string node){

	}

	public void SuccessResponse(DataSnapshot snapshot , string node){
		
		string json = snapshot.GetRawJsonValue();


		if (string.Equals (node, "DroneData")) {
			droneData = DroneData.CreateFromJSON (json);	
			dronePositionString = droneData.coordinates;
			if (_map.isActiveAndEnabled) {
				addTrail ();
			}
		} else if (string.Equals (node, "DroneMissionData/waypoints")) {
			GameObject[] pins = GameObject.FindGameObjectsWithTag ("waypoint");

			for(var i = 0 ; i < pins.Length ; i ++)
			{
				Destroy(pins[i]);
			}

			print ("DroneMissionData/waypoints : " + json);
			List<Coordinate> waypointCoordinates = JSONHelper.SplitJsonStringCoordenates (json);
			print ("Count : " + waypointCoordinates.Count);
			addWaypoints (waypointCoordinates);
		}
	}

	public void addWaypoints(List<Coordinate> coordinates){
		for (int i = 0; i < coordinates.Count; i++) {
			Coordinate coord = coordinates [i];
			var pos = string.Format("{0},{1}", coord.latitude,coord.longitude);
			Mapbox.Utils.Vector2d pinPosition = Conversions.StringToLatLon (pos);

			var y = drone.transform.position.y;
			var waypoint = Instantiate(waypointObject, new Vector3(0,y,0), Quaternion.identity);
//			waypoint.transform.Rotate(new Vector3(90,0,0));
			waypoint.transform.MoveToGeocoordinate (pinPosition, _map.CenterMercator, _map.WorldRelativeScale);

		}

	}
		
}
