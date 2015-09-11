using UnityEngine;

public class Seek {
	static public Vector2 calculate(Vector2 pos, Vector2 vel, Vector2 target, float max_speed) {
		var desired_velocity = (target - pos).normalized * max_speed;
		return (desired_velocity - vel);
	}
}

public class Arrive {
	static public Vector2 calculate(Vector2 pos, Vector2 vel, Vector2 target, float max_speed, float decel) {
		var to_target = target - pos;
		var dist = to_target.magnitude;

		if (dist > 0) {
			const float decel_tweaker = 0.03F;
			var speed = dist / (decel * decel_tweaker);
			speed = Mathf.Min(speed, max_speed);
			var desired_velocity = to_target * speed / dist;
			return (desired_velocity - vel);
		} else {
			return Vector2.zero;
		}
	}
}