using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBasedOnObject : MonoBehaviour {
	public GameObject gameObject;

        // Dimensions
	public Vector3 physicalBox, physicalDrone, virtualDrone;
	public bool updateAtRuntime = false;
	private Transform tx;


	    void setVirtualBoxDim()
	    {
		Vector3 droneRatios;
		if (gameObject == null)
		{

			droneRatios = new Vector3(
				virtualDrone.x / physicalDrone.x,
				virtualDrone.y / physicalDrone.y,
				virtualDrone.z / physicalDrone.z
			);

		}
		else
		{
			Bounds bounds = BoundingBox.getBounds(gameObject);
			droneRatios = new Vector3(
				bounds.extents.x * 2 / physicalDrone.x,
				bounds.extents.y * 2 / physicalDrone.y,
				bounds.extents.z * 2 / physicalDrone.z
			);
		}	

		tx.localScale = Vector3.Scale(physicalBox, droneRatios);

	    }
	// Use this for initialization
	void Start () {
		tx = GetComponent<Transform>();
		setVirtualBoxDim();
	}
	
	// Update is called once per frame
	void Update () {
		if (updateAtRuntime)
		    setVirtualBoxDim();
	}
}
