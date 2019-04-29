using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour 
{

    void OnCollisionEnter(Collision collision)
    {
    	var target = collision.gameObject;
		var health = target.GetComponent<Health>();
		if (health != null)
		{
		    health.TakeDamage(10);
		}

        Destroy(gameObject);

    }
}
