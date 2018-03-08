using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;

public class PlayerControllerTest : MonoBehaviour {

	[SerializeField]
	public AbstractMap _map;
	private GameObject drone;
	private Mapbox.Utils.Vector2d mapPosition;
	private Mapbox.Utils.Vector2d dronePosition;

	Vector3 initialPosition;
	// Use this for initialization
	void Start () {

		drone = GameObject.FindGameObjectWithTag ("drone");


		print ("Map : " + _map.isActiveAndEnabled);
	}
	
	// Update is called once per frame
	void Update () {	
		if (_map.isActiveAndEnabled){
			mapPosition = _map.CenterLatitudeLongitude;
			dronePosition = drone.transform.GetGeoPosition (_map.CenterMercator, _map.WorldRelativeScale);
			print ("Map position : " + mapPosition);
			print ("Drone position : " + dronePosition);
			//37.792159, -122.401723

			var updaterPosition = Conversions.StringToLatLon ("37.792159, -122.401723");
			drone.transform.MoveToGeocoordinate (updaterPosition, _map.CenterMercator, _map.WorldRelativeScale);
			dronePosition = drone.transform.GetGeoPosition (_map.CenterMercator, _map.WorldRelativeScale);
			print ("Drone position updated : " + dronePosition);
		}	
	}
}
