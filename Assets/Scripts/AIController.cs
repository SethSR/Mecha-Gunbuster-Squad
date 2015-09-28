using UnityEngine;
using System.Collections;

using FuzzyLogic;

using DefuzzType = FuzzyLogic.FuzzyModule.DefuzzifyType;

public class AIController : RobotController {
	public Transform target;
	public Transform tempBullet;
	public float     bulletSpeed;
	public int       ammo = 100;

	FuzzyModule fm = new FuzzyModule();
	FuzzyVariable distToTarget, ammoCount, need;

	new Rigidbody2D rigidbody2D;

	const string DIST_TO_TARGET = "DistToTarget";
	const string AMMO_COUNT     = "AmmoCount";
	const string NEED           = "Need";

	// Use this for initialization
	void Start() {
		rigidbody2D  = GetComponent<Rigidbody2D>();
		distToTarget = fm.CreateFLV(DIST_TO_TARGET);
		ammoCount    = fm.CreateFLV(AMMO_COUNT);
		need         = fm.CreateFLV(NEED);

		var targetClose  = distToTarget.AddLeftShoulderSet ("Target_Close" ,   0,  25,  50);
		var targetMedium = distToTarget.AddTriangularSet   ("Target_Medium",  25,  50, 150);
		var targetFar    = distToTarget.AddRightShoulderSet("Target_Far"   , 150, 300, 500);

		var ammoLow  = ammoCount.AddLeftShoulderSet ("Ammo_Low" ,  0, 25,  50);
		var ammoMid  = ammoCount.AddTriangularSet   ("Ammo_Mid" , 25, 50,  75);
		var ammoHigh = ammoCount.AddRightShoulderSet("Ammo_High", 50, 75, 100);

		var undesirable = need.AddLeftShoulderSet ("Undesirable",  0, 33,  67);
		var desirable   = need.AddRightShoulderSet(  "Desirable", 33, 67, 100);

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
		var to_target = (Vector2)(target.position - transform.position);
		fm.Fuzzify(DIST_TO_TARGET, Mathf.Min(Mathf.Max(to_target.magnitude, 0), 500));
		fm.Fuzzify(    AMMO_COUNT, Mathf.Min(Mathf.Max(               ammo, 0), 100));

		var need_value = fm.Defuzzify(NEED, DefuzzType.max_av);

		Debug.Log(need_value);

		if (Random.value * 100 < need_value) {
			Rigidbody2D bullet = ((Transform)Instantiate(tempBullet, transform.position, Quaternion.identity)).GetComponent<Rigidbody2D>();
			Destroy(bullet.gameObject, 1);
			bullet.velocity = to_target.normalized * bulletSpeed;
			--ammo;
		}
		rigidbody2D.velocity = Arrive.calculate((Vector2)transform.position, (Vector2)rigidbody2D.velocity, (Vector2)target.position, maxSpeed, 3) / rigidbody2D.mass;
	}
}