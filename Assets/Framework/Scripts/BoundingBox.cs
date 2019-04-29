using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox : MonoBehaviour {
    public GameObject gameObject;
	// Use this for initialization

    public static Bounds getBounds(GameObject gameObject)
    {
        Renderer[] colliders = gameObject.GetComponentsInChildren<Renderer>();
        Bounds bounds = new Bounds(gameObject.transform.position, Vector3.zero);

        foreach (Renderer collider in colliders)
        {
            bounds.Encapsulate(collider.bounds);
            //Debug.Log(collider.bounds);
        }
	bounds.center -= gameObject.transform.position; 
        return bounds;
    }

    void Start () {
        Bounds bounds = getBounds(gameObject);
	//GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.position = bounds.center;
        //cube.transform.localScale = bounds.extents * 2;

        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();

        // Remove the offset.
        boxCollider.center = bounds.center;// - gameObject.transform.position;

        boxCollider.size = bounds.extents * 2;
    }

    // Update is called once per frame
    void Update () {
  
	}
}
