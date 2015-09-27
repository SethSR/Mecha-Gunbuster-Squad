using UnityEngine;
using System.Collections;

namespace FuzzyLogic {
	abstract public class FuzzySet {
		// this will hold the degree of membership in this set of a given value
		float m_dDOM;

		// this is the maximum of the set's membership function. For instance, if
		// the set is triangular then this will be the peak point of the triangle.
		// If the set has a plateau then this value will be the midpoint of the
		// plateau. This value is set in the constructor to avoid run-time
		// calculaltion of midpoint values.
		float m_dRepresentativeValue;

		public FuzzySet(float RepVal) {
			m_dDOM = 0.0f;
			m_dRepresentativeValue = RepVal;
		}

		// return the degree of membership in this set of the given value. NOTE:
		// this does not set m_dDOM to the DOM of the value passed as the parameter.
		// This is because the centroid defuzzification method also uses this method
		// to determine the DOMs of the values it uses as its sample points.
		abstract public float CalculateDOM(float val);

		// if this fuzzy set is part of a consequent FLV and it is fired by a rule,
		// then this method sets the DOM (in this context, the DOM represents a
		// confidence level) to the maximum of the parameter value or the set's
		// existing m_dDOM value
		public void OrwithDOM(float val) {
			if (val > m_dDOM) {
				m_dDOM = val;
			}
		}

		// accessor methods
		public float GetRepresentativeVal()           { return m_dRepresentativeValue; }
		public void  ClearDOM            ()           { m_dDOM = 0.0f; }
		public float GetDOM              ()           { return m_dDOM; }
		public void  SetDOM              (float val) { m_dDOM = val; }

		static public bool isEqual(float a, float b) { return isEqual(a,b,0.0001f); }
		static public bool isEqual(float a, float b, float epsilon) {
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

	public class FuzzySet_LeftShoulder : FuzzySet {
		public FuzzySet_LeftShoulder(float peak, float lft, float rgt) : base(((peak - lft) + peak) / 2) {
			m_dPeakPoint   = peak;
			m_dLeftOffset  = lft;
			m_dRightOffset = rgt;
		}

		override public float CalculateDOM(float val) {
			if ((isEqual(m_dRightOffset, 0.0f) && (isEqual(m_dPeakPoint, val))) ||
			    (isEqual(m_dLeftOffset, 0.0f) && (isEqual(m_dPeakPoint, val)))) {
				return 1.0f;
			} else if ((val >= m_dPeakPoint) && (val < (m_dPeakPoint + m_dRightOffset))) {
				float grad = 1.0f / -m_dRightOffset;
				return grad * (val - m_dPeakPoint) + 1.0f;
			} else if ((val < m_dPeakPoint) && (val >= m_dPeakPoint - m_dLeftOffset)) {
				return 1.0f;
			} else {
				return 0.0f;
			}
		}

		float m_dPeakPoint;
		float m_dLeftOffset;
		float m_dRightOffset;
	}

	public class FuzzySet_RightShoulder : FuzzySet {
		public FuzzySet_RightShoulder(float peak,
		                              float lft,
		                              float rgt) : base(((peak + rgt) + peak) / 2) {
			m_dPeakPoint   = peak;
			m_dLeftOffset  = lft;
			m_dRightOffset = rgt;
		}

		// this method calculates the degree of membership for a particular value
		override public float CalculateDOM(float val) {
			if ((isEqual(m_dRightOffset, 0.0f) && (isEqual(m_dPeakPoint, val))) ||
			    (isEqual(m_dLeftOffset, 0.0f) && (isEqual(m_dPeakPoint, val)))) {
				return 1.0f;
			} else if ((val <= m_dPeakPoint) && (val > (m_dPeakPoint - m_dLeftOffset))) {
				float grad = 1.0f / m_dLeftOffset;
				return grad * (val - (m_dPeakPoint - m_dLeftOffset));
			} else if ((val > m_dPeakPoint) && (val <= m_dPeakPoint + m_dRightOffset)) {
				return 1.0f;
			} else {
				return 0.0f;
			}
		}

		// the values that define the shape of this FLV
		float m_dPeakPoint;
		float m_dLeftOffset;
		float m_dRightOffset;
	}

	public class FuzzySet_Triangle : FuzzySet {
		public FuzzySet_Triangle(float mid,
		                         float lft,
		                         float rgt) : base(mid) {
			m_dPeakPoint   = mid;
			m_dLeftOffset  = lft;
			m_dRightOffset = rgt;
		}

		// this method calculates the degree of membership for a particular value
		override public float CalculateDOM(float val) {
			// test for the case where the triangle's left or right offsets are zero
			// (to prevent divide by zero errors below)
			if ((isEqual(m_dRightOffset, 0.0f) && (isEqual(m_dPeakPoint, val))) ||
			    (isEqual(m_dLeftOffset, 0.0f) && (isEqual(m_dPeakPoint, val)))) {
				return 1.0f;
			}

			// find DOM if left of center
			if ((val <= m_dPeakPoint) && (val >= (m_dPeakPoint - m_dLeftOffset))) {
				float grad = 1.0f / m_dLeftOffset;
				return grad * (val - (m_dPeakPoint - m_dLeftOffset));
			}
			// find DOM if right of center
			else if ((val > m_dPeakPoint) && (val < (m_dPeakPoint - m_dRightOffset))) {
				float grad = 1.0f / -m_dRightOffset;
				return grad * (val - m_dPeakPoint) + 1.0f;
			}
			// out of range of this FLV, return zero
			else {
				return 0.0f;
			}
		}

		public float RepresentativeValue() {
			return m_dPeakPoint;
		}

		// the values that define the shape of this FLV
		float m_dPeakPoint;
		float m_dLeftOffset;
		float m_dRightOffset;
	}

	public class FuzzySet_Singleton : FuzzySet {
		public FuzzySet_Singleton(float mid,
		                          float lft,
		                          float rgt) : base(mid) {
			m_dMidPoint = mid;
			m_dLeftPoint = lft;
			m_dRightPoint = rgt;
		}

		override public float CalculateDOM(float val) {
			if ((val >= m_dMidPoint - m_dLeftPoint) &&
			    (val <= m_dMidPoint + m_dRightPoint)) {
				return 1.0f;
			} else {
				return 0.0f;
			}
		}

		float m_dMidPoint;
		float m_dLeftPoint;
		float m_dRightPoint;
	}
}