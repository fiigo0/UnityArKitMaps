using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class FBConnectionManager:MonoBehaviour {
	
	private static FirebaseApp firebaseInstance;
	public static string DatabaseUrl = "https://arunityproyect.firebaseio.com/";

	private static void InitializeInstance()
	{

		if (firebaseInstance == null)
		{
			firebaseInstance = FirebaseApp.DefaultInstance;
			firebaseInstance.SetEditorDatabaseUrl(DatabaseUrl);
		}
	}


	public static void FirebaseGetDataSync(string reference,  IFirebaseCallback callback){

		InitializeInstance ();
		FirebaseDatabase.DefaultInstance
			.GetReference (reference)
			.GetValueAsync ()
			.ContinueWith ( task => {
				if (task.IsFaulted)	{
				
			}else if (task.IsCompleted){					 
					callback.SuccessResponse(task.Result,reference);
			}
		});
	}

	public static void FirebaseConsult(string reference, IFirebaseCallback callback)
	{
		InitializeInstance();

		FirebaseDatabase.DefaultInstance.GetReference(reference)
			.ValueChanged += (object sender2, ValueChangedEventArgs e2) =>
		{
			if (e2.DatabaseError != null)
			{
				callback.FailResponse(e2.DatabaseError.Message, reference);
				Debug.LogError(e2.DatabaseError.Message);
				return;
			}
			if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0)
			{
				callback.SuccessResponse(e2.Snapshot, reference);
			}
		};

	}

	public static void FirebasePutData(string reference, Dictionary<string, object> json)
	{
		InitializeInstance();
		FirebaseDatabase.DefaultInstance.GetReference(reference).UpdateChildrenAsync(json).ContinueWith
		(task =>
			{
				if (task.IsCanceled)
				{
					Debug.LogError("Error");
					return;
				}

				if (task.IsFaulted)
				{
					Debug.LogError("Error");
					return;
				}            
			});
	}




	public interface IFirebaseCallback
	{
		void FailResponse(string response, string node);
		void SuccessResponse(DataSnapshot snapshot, string node);
	}

}
