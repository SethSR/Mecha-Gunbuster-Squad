using UnityEngine;

namespace FuzzyLogic {
	static public class FuzzyHelpers {
			static public FzSet set(this FuzzySet fs) { return new FzSet(fs); }
			static public FzAND and(this FzSet fs) { return new FzAND(fs); }
			static public FzOR  or (this FzSet fs) { return new FzOR (fs); }
			static public FzAND and(this FuzzyTerm op, params FuzzyTerm [] ops) { return new FzAND(op, ops); }
			static public FzOR  or (this FuzzyTerm op, params FuzzyTerm [] ops) { return new FzOR (op, ops); }

		static public bool isEqual(this float a, float b) { return isEqual(a,b,0.0001f); }
		static public bool isEqual(this float a, float b, float epsilon) {
			float absA = Mathf.Abs(a);
			float absB = Mathf.Abs(b);
			float diff = Mathf.Abs(a - b);

			if (a == b) { // shortcut, handles infinities
				return true;
			} else if (a == 0 || b == 0 || diff < float.MinValue) {
				// a or b is zero or both are extremely close to it
				// relative error is less meaningful here
				return diff < (epsilon * float.MinValue);
			} else { // use relative error
				return diff / (absA + absB) < epsilon;
			}
		}
	}
}