namespace Mapbox.Unity.Map
{
	using Mapbox.Unity.Utilities;
	using Utils;
	using Mapbox.Map;
	using Firebase.Database;

	/// <summary>
	/// Abstract Map (Basic Map etc)
	/// This is one of the few monobehaviours we use in the system and used mainly to tie scene and map visualization object/system 
	/// together.It's a replacement for the application (or map controller class in a project) in our demos.
	/// Ideally devs should have their own map initializations and tile call logic in their app and make calls to 
	/// map visualization object from their own controllers directly. It can also be used as an interface for 
	/// small projects or tests.
	/// </summary>
	/// 
	public class MapController: AbstractMap, FBConnectionManager.IFirebaseCallback
	{

		string mapCoordinates = "";
		private Mapbox.Utils.Vector2d mapLatLon;
		AbstractMap _map;
		public override void Initialize(Vector2d latLon, int zoom)
		{
			FBConnectionManager.FirebaseGetDataSync ("MapConfiguration/coordinates", this);

			_worldHeightFixed = false;

			_zoom = zoom;
			_initialZoom = zoom;

			if (string.IsNullOrEmpty (mapCoordinates)) {
				print ("map coord empty");
				_centerLatitudeLongitude = latLon;
			
			} else {			
				print ("map coods FB");
				mapLatLon = Conversions.StringToLatLon (mapCoordinates);
				_centerLatitudeLongitude = mapLatLon;
			}

			var referenceTileRect = Conversions.TileBounds(TileCover.CoordinateToTileId(_centerLatitudeLongitude, AbsoluteZoom));
			_centerMercator = referenceTileRect.Center;

			_worldRelativeScale = (float)(_unityTileSize / referenceTileRect.Size.x);
			_mapVisualizer.Initialize(this, _fileSource);
			_tileProvider.Initialize(this);

			SendInitialized();


		}
		public void FailResponse(string response, string node){

		}

		public void SuccessResponse(DataSnapshot snapshot , string node){
			print ("Map Coords : " + snapshot.Value);
			mapCoordinates = snapshot.Value.ToString();
			_map.Reset ();
		}

		public void mapInit(){
			
		}
	}
}