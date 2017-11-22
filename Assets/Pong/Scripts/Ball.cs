using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour {

	public float speed = 5f;
	//private int count2;
	//public Text winText;
	public Text countText;
	private int count1;

    // Use this for initialization
    float sx, sy, sz;
	void Start () {

        sx =  Random.Range (0, 2) == 0 ? -1 : 1;
        sy =  Random.Range (0, 2) == 0 ? -1 : 1;
        sz =  Random.Range (0f, 1f) == 0f ? -.5f : .5f;

		GetComponent<Rigidbody> ().velocity = new Vector3 (speed * sx, speed * sy, speed * sz);
		//winText.text = "";
		count1 = 0;

		//SetCountText ();
	}
	
	// Update is called once per frame
	void Update () {
        

    }

  //  void OnTriggerEnter(Collider other)
  //  {

  //      if (other.gameObject.CompareTag("goal1"))
  //      {

  //          count1 = count1 + 1;
  //          GetComponent<Rigidbody>().position = Vector3.zero;
  //          SetCountText();

  //      }
  //      /*if (other.gameObject.CompareTag ("goal2")) 
		//{
		//	count2 = count2 + 1;
		//	GetComponent<Rigidbody> ().position = Vector3.zero;
		//}*/
  //  }

    void SetCountText ()
	{
		countText.text = "Count 1: " + count1.ToString ();
		/*if (count1 >= 3)
		{
			winText.text = "You Win!";
		}*/
	}
}
