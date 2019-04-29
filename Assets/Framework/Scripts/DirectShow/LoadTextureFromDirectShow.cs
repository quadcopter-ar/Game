using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// For type IntPtr.
using System;
// For DllImport.
using System.Runtime.InteropServices;

public class LoadTextureFromDirectShow : MonoBehaviour {
    [DllImport("DirectShowDLL", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr _DirectShow_create();

    [DllImport("DirectShowDLL", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool _DirectShow_isFrameBufferAvailable(IntPtr obj);

    [DllImport("DirectShowDLL", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr _DirectShow_getFrameBuffer(IntPtr obj);

    [DllImport("DirectShowDLL", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void _DirectShow_clearFrameBuffer(IntPtr obj);

    private IntPtr obj, rawImage;
    private Texture2D tex;
    private Material screen;
    public int 
	imageWidth = 1920, 
	imageHeight = 1080;
    // Use this for initialization
    void Start () {
        obj = _DirectShow_create();
        tex = new Texture2D(imageWidth, imageHeight, TextureFormat.RGB24, false);
        screen = GetComponent<Renderer>().material;
    }
	
    // Update is called once per frame
    void Update () {
        if (_DirectShow_isFrameBufferAvailable(obj))
        {
	    // Get raw image through DirectShow.
            rawImage = _DirectShow_getFrameBuffer(obj);
            _DirectShow_clearFrameBuffer(obj);
	    
	    // Convert the raw image into a texture and then apply it on an object. 
            tex.LoadRawTextureData(rawImage, imageWidth * imageHeight * 3);
            tex.Apply();
            screen.mainTexture = tex;
            //screen.SetTexture(Shader.PropertyToID("_MainTex"), tex);
        }
    }
}
