using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawOnMesh : MonoBehaviour {

	ColorProvider l_colorProvider;

	// Use this for initialization
	void Start () {

		Texture2D texture = new Texture2D(Screen.currentResolution.width, Screen.currentResolution.height);
		GetComponent<Renderer>().material.mainTexture = texture;

		for (int y = 0; y < texture.height; y++) {
			for (int x = 0; x < texture.width; x++) {
				Color color = ((x & y) != 0 ? Color.red : Color.green);
				texture.SetPixel(x, y, color);
			}
		}
		texture.Apply();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
