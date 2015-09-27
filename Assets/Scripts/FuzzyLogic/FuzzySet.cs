using UnityEngine;
using System.Collections;

using FuzzyLogic;

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
			m_dDOM = 0;
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
		public float GetRepresentativeVal()          { return m_dRepresentativeValue; }
		public void  ClearDOM            ()          { m_dDOM = 0; }
		public float GetDOM              ()          { return m_dDOM; }
		public void  SetDOM              (float val) { m_dDOM = val; }
	}

	public class FuzzySet_LeftShoulder : FuzzySet {
		public FuzzySet_LeftShoulder(float peak, float lft, float rgt) : base(((peak - lft) + peak) / 2) {
			m_dPeakPoint   = peak;
			m_dLeftOffset  = lft;
			m_dRightOffset = rgt;
		}

		// A := m_dRightOffset == 0
		// B := m_dLeftOffset == 0
		// C := m_dPeakPoint == val
		// (A*C)+(B*C) == (A+B)*C

		override public float CalculateDOM(float val) {
			if ((m_dRightOffset.isEqual(0) || m_dLeftOffset.isEqual(0)) && m_dPeakPoint.isEqual(val)) {
				return 1;
			} else if ((val >= m_dPeakPoint) && (val < (m_dPeakPoint + m_dRightOffset))) {
				float grad = 1 / -m_dRightOffset;
				return grad * (val - m_dPeakPoint) + 1;
			} else if ((val < m_dPeakPoint) && (val >= m_dPeakPoint - m_dLeftOffset)) {
				return 1;
			} else {
				return 0;
			}
		}

		float m_dPeakPoint;
		float m_dLeftOffset;
		float m_dRightOffset;
	}

	public class FuzzySet_RightShoulder : FuzzySet {
		public FuzzySet_RightShoulder(float peak, float lft, float rgt) : base(((peak + rgt) + peak) / 2) {
			m_dPeakPoint   = peak;
			m_dLeftOffset  = lft;
			m_dRightOffset = rgt;
		}

		// this method calculates the degree of membership for a particular value
		override public float CalculateDOM(float val) {
			if ((m_dRightOffset.isEqual(0) || m_dLeftOffset.isEqual(0)) && m_dPeakPoint.isEqual(val)) {
				return 1;
			} else if ((val <= m_dPeakPoint) && (val > (m_dPeakPoint - m_dLeftOffset))) {
				float grad = 1 / m_dLeftOffset;
				return grad * (val - (m_dPeakPoint - m_dLeftOffset));
			} else if ((val > m_dPeakPoint) && (val <= m_dPeakPoint + m_dRightOffset)) {
				return 1;
			} else {
				return 0;
			}
		}

		// the values that define the shape of this FLV
		float m_dPeakPoint;
		float m_dLeftOffset;
		float m_dRightOffset;
	}

	public class FuzzySet_Triangle : FuzzySet {
		public FuzzySet_Triangle(float mid, float lft, float rgt) : base(mid) {
			m_dPeakPoint   = mid;
			m_dLeftOffset  = lft;
			m_dRightOffset = rgt;
		}

		override public float CalculateDOM(float val) {
			if ((m_dRightOffset.isEqual(0) || m_dLeftOffset.isEqual(0)) && m_dPeakPoint.isEqual(val)) {
				return 1;
			} else if ((val <= m_dPeakPoint) && (val >= (m_dPeakPoint - m_dLeftOffset))) {
				float grad = 1 / m_dLeftOffset;
				return grad * (val - (m_dPeakPoint - m_dLeftOffset));
			} else if ((val > m_dPeakPoint) && (val < (m_dPeakPoint - m_dRightOffset))) {
				float grad = 1 / -m_dRightOffset;
				return grad * (val - m_dPeakPoint) + 1;
			} else {
				return 0;
			}
		}

		public float RepresentativeValue() { return m_dPeakPoint; }

		float m_dPeakPoint;
		float m_dLeftOffset;
		float m_dRightOffset;
	}

	public class FuzzySet_Singleton : FuzzySet {
		public FuzzySet_Singleton(float mid, float lft, float rgt) : base(mid) {
			m_dMidPoint   = mid;
			m_dLeftPoint  = lft;
			m_dRightPoint = rgt;
		}

		override public float CalculateDOM(float val) {
			if ((val >= m_dMidPoint - m_dLeftPoint) &&
			    (val <= m_dMidPoint + m_dRightPoint)) {
				return 1;
			} else {
				return 0;
			}
		}

		float m_dMidPoint;
		float m_dLeftPoint;
		float m_dRightPoint;
	}
}