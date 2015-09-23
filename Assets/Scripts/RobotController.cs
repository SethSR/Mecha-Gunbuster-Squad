using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Robot))]
[RequireComponent(typeof(Rigidbody2D))]
abstract public class RobotController : MonoBehaviour {
	public float maxSpeed = 10; // 10 m/s
	
	// Use this for initialization
	void Start() {
	}
	
	// Update is called once per frame
	void Update() {
	}
}