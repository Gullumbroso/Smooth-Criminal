using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticDepthSim : MonoBehaviour {

	void Update () {
        Vector3 pos = transform.position;
        pos.z = pos.y;
        transform.position = pos;
	}
}