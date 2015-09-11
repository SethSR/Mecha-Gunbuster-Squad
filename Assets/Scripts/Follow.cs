using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {
	public Transform target;

	// Update is called once per frame
	void Update () {
		var pos = (Vector2)transform.position;
		var tgt = (Vector2)target.position;
		var dist = (tgt - pos).magnitude;
		var pos2 = Vector2.Lerp(pos, tgt, 1 / dist);
		transform.position = new Vector3(pos2.x, pos2.y, transform.position.z);
	}
}
