using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBasedOnObject : MonoBehaviour {
	public GameObject virtualRefObject;
    [Tooltip("Not needed if Virtual Ref Object is provided")]
    public Vector3 virtualRefObjectDimension;

    // Dimensions
    public Vector3 physicalRefObjectDimension, physicalTargetObjectDimension;
	private Transform tx;
    private Bounds originalSize;


	    void setVirtualBoxDim()
	    {
		Vector3 virtualPhysicalRatios;
        Bounds bounds;

        if (virtualRefObject != null)
		{
            bounds = BoundingBox.getBounds(virtualRefObject);
            virtualRefObjectDimension = bounds.extents * 2;
		}

        virtualPhysicalRatios = new Vector3(
				virtualRefObjectDimension.x / physicalRefObjectDimension.x,
				virtualRefObjectDimension.y / physicalRefObjectDimension.y,
				virtualRefObjectDimension.z / physicalRefObjectDimension.z
		);


		tx.localScale = Vector3.Scale(tx.localScale, Vector3.Scale(physicalTargetObjectDimension, virtualPhysicalRatios));
    

	    }
	// Use this for initialization
	void Start () {
        tx = GetComponent<Transform>();

        // figure out the actual size in rendering unit of the current game object.
        tx.localScale = Vector3.one;
        originalSize = BoundingBox.getBounds(gameObject);
        // scale down to 1 rendering unit (1 meter in physics engine)
        tx.localScale = Vector3.Scale(tx.localScale, new Vector3(
            1 / (originalSize.extents.x * 2),
            1 / (originalSize.extents.y * 2),
            1 / (originalSize.extents.z * 2)
        ));
        
		setVirtualBoxDim();
	}
	
	// Update is called once per frame
	void Update () {
		//if (updateAtRuntime)
		//    setVirtualBoxDim();
	}
}
