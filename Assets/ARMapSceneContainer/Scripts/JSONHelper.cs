using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class JSONHelper {

	public static ArrayList SplitJsonStringCoordenates(string json)
	{
		var charsToRemove = new char[] {'[', ']', '"' };
		foreach (var c in charsToRemove)
		{
			json = json.Replace(c, ' ');
		}

		var list = new ArrayList();

		var strArray = json.Split (',');


		for (int i = 0; i < strArray.Length - 1; i = i + 2) {			
			Coordinate c = new Coordinate ();
			c.latitude = strArray [i];
			c.longitude = strArray [i + 1];
			list.Add (c);
		}			
		return list;
	}

}
