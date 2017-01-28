using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawOnMesh : MonoBehaviour {

    private ColorProvider mColorProvider;

    private bool mDirty = true;

    private static DrawOnMesh mInstance;

    public static DrawOnMesh GetInstance() {
        return mInstance;
    }
    
    // Use this for initialization
    void Start () {

        mInstance = this;

        mColorProvider = GetComponent<ColorProvider>();

	}

    // Update is called once per frame
    void Update()
    {

        if (mDirty)
        {
            Redraw();
            mDirty = false;
        }
    }

    private void Redraw()
    {   
        

        Texture2D texture = new Texture2D(Screen.width, Screen.height);
        GetComponent<Renderer>().material.mainTexture = texture;

        Color[] colors = new Color[texture.width * texture.height];

        for (int y = 0; y < texture.height; ++y)
        {
            for (int x = 0; x < texture.width; ++x)
            {
                colors[x + texture.width * y] = mColorProvider.getColorForPixel(x, y);
            }
        }
        texture.SetPixels(colors);
        texture.Apply();
    }

    public void SetDirty()
    {
        mDirty = true;
    }
}
