using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Firebase.Database;
using Mapbox.Geocoding;

public class MapManager : MonoBehaviour {

	[SerializeField]
	public AbstractMap map;
	AbstractMap _map;
	Camera _camera;
	Vector3 _cameraStartPos;

	private Mapbox.Utils.Vector2d mapLatLong;


	Coroutine _reloadRoutine;

	WaitForSeconds _wait;

	// Use this for initialization
	void Start () {
		print ("start");
		_camera = Camera.main;
		_cameraStartPos = _camera.transform.position;
		_map = FindObjectOfType<AbstractMap>();
		_wait = new WaitForSeconds(.3f);

		mapLatLong = Conversions.StringToLatLon ("37.7940690040815,-122.398403568999");
		Reload (1.0F);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	void Reload(float value)
	{
		print ("Reload");
		if (_reloadRoutine != null)
		{
			StopCoroutine(_reloadRoutine);
			_reloadRoutine = null;
		}
		_reloadRoutine = StartCoroutine(ReloadAfterDelay());
	}

	IEnumerator ReloadAfterDelay()
	{
		print ("ReloadAfterDelay");
		yield return _wait;
		_camera.transform.position = _cameraStartPos;
		_map.Initialize(mapLatLong, 16);
		_reloadRoutine = null;
	}
}
