using UnityEngine;
using System.Collections;

using FuzzyLogic;

public class AIController : RobotController {
	public Transform target;

	FuzzyModule fm = new FuzzyModule();

	FuzzyVariable distToTarget, ammoCount, need;

	FzSet targetClose, targetMedium, targetFar;
	FzSet ammoLow, ammoMid, ammoHigh;
	FzSet undesirable, desirable;

	new Rigidbody2D rigidbody2D;

	// Use this for initialization
	void Start() {
		rigidbody2D  = GetComponent<Rigidbody2D>();
		distToTarget = fm.CreateFLV("DistToTarget");
		ammoCount    = fm.CreateFLV("AmmoCount");
		need         = fm.CreateFLV("Need");

		targetClose  = distToTarget.AddLeftShoulderSet ("Target_Close" ,   0,  25,  50);
		targetMedium = distToTarget.AddTriangularSet   ("Target_Medium",  25,  50, 150);
		targetFar    = distToTarget.AddRightShoulderSet("Target_Far"   , 150, 300, 500);

		ammoLow  = ammoCount.AddLeftShoulderSet ("Ammo_Low" ,  0, 25,  50);
		ammoMid  = ammoCount.AddTriangularSet   ("Ammo_Mid" , 25, 50,  75);
		ammoHigh = ammoCount.AddRightShoulderSet("Ammo_High", 50, 75, 100);

		undesirable = need.AddLeftShoulderSet ("Undesirable",  0, 33,  67);
		desirable   = need.AddRightShoulderSet(  "Desirable", 33, 67, 100);

		fm.AddRule(targetClose.and(ammoLow) , undesirable);
		fm.AddRule(targetClose.and(ammoMid) ,   desirable);
		fm.AddRule(targetClose.and(ammoHigh),   desirable);

		fm.AddRule(targetMedium.and(ammoLow) , undesirable);
		fm.AddRule(targetMedium.and(ammoMid) , undesirable);
		fm.AddRule(targetMedium.and(ammoHigh),   desirable);

		fm.AddRule(targetFar.and(ammoLow) , undesirable);
		fm.AddRule(targetFar.and(ammoMid) , undesirable);
		fm.AddRule(targetFar.and(ammoHigh), undesirable);
	}
	
	// Update is called once per frame
	void Update() {
		rigidbody2D.velocity = Arrive.calculate((Vector2)transform.position, (Vector2)rigidbody2D.velocity, (Vector2)target.position, maxSpeed, 3) / rigidbody2D.mass;
	}
}