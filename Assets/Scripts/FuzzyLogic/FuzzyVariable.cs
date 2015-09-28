using UnityEngine;
using System.Collections;
using System.Linq;
using Functional;

using FuzzyLogic;

namespace FuzzyLogic {
	using MemberSets = System.Collections.Generic.Dictionary<string, FuzzySet>;

	public class FuzzyVariable {
		public FuzzyVariable() {
			m_dMinRange = 0.0f;
			m_dMaxRange = 0.0f;
		}

		// the following methods create instances of the sets named in the method
		// name and adds them to the member set map. Each time a set of any type is
		// added the m_dMinRange and m_dMaxRange are adjusted accordingly. All of the
		// methods return a proxy class representing the newly created instance. This
		// proxy set can be used as an operand when creating the rule base.
		public FzSet AddLeftShoulderSet(string name, float minBound, float peak, float maxBound) {
			m_MemberSets[name] = new FuzzySet_LeftShoulder(peak, peak-minBound, maxBound-peak);
			AdjustRangeToFit(minBound,maxBound);
			return new FzSet(m_MemberSets[name]);
		}

		public FzSet AddRightShoulderSet(string name, float minBound, float peak, float maxBound) {
			m_MemberSets[name] = new FuzzySet_RightShoulder(peak, peak-minBound, maxBound-peak);
			AdjustRangeToFit(minBound,maxBound);
			return new FzSet(m_MemberSets[name]);
		}

		public FzSet AddTriangularSet(string name, float minBound, float peak, float maxBound) {
			m_MemberSets[name] = new FuzzySet_Triangle(peak, peak-minBound, maxBound-peak);
			AdjustRangeToFit(minBound,maxBound);
			return new FzSet(m_MemberSets[name]);
		}

		public FzSet AddSingletonSet(string name, float minBound, float peak, float maxBound) {
			m_MemberSets[name] = new FuzzySet_Singleton(peak, peak-minBound, maxBound-peak);
			AdjustRangeToFit(minBound, maxBound);
			return new FzSet(m_MemberSets[name]);
		}

		// fuzzify a value by calculating its DOM in each of this variable's subsets
		public void Fuzzify(float val) {
			m_MemberSets
				.Select (item    => item.Value)
			  .ForEach(cur_set => cur_set.SetDOM(cur_set.CalculateDOM(val)));
		}

		// defuzzify the variable using the MaxAv method
		public float DeFuzzifyMaxAv() {
			var top    = m_MemberSets.Select(item => item.Value.GetDOM() * item.Value.GetRepresentativeVal()).Aggregate((a,b) => a + b);
			var bottom = m_MemberSets.Select(item => item.Value.GetDOM()).Aggregate((a,b) => a + b);
			return bottom.isEqual(0) ? 0 : (top / bottom);
		}

		// defuzzify the variable using the centroid method
		public float DeFuzzifyCentroid(int NumSamples) {
			var step_size      = (m_dMaxRange - m_dMinRange) / (float)NumSamples;
			var total_area     = 0.0f;
			var sum_of_moments = 0.0f;

			Enumerable.Range(1, NumSamples).ForEach(samp => {
				var samp_value = m_dMinRange + samp * step_size;
				var contribution_values = m_MemberSets.Select(item => item.Value).Select(cur => Mathf.Min(cur.CalculateDOM(samp_value), cur.GetDOM()));
				total_area     += contribution_values.Aggregate((a,b) => a + b);
				sum_of_moments += contribution_values.Select(v => v * samp_value).Aggregate((a,b) => a + b);
			});
			return total_area.isEqual(0) ? 0 : (sum_of_moments / total_area);
		}

		// a map of the fuzzy sets that comprise this variable
		MemberSets m_MemberSets = new MemberSets();

		// the minimum and maximum value of the range of this variable
		float m_dMinRange;
		float m_dMaxRange;

		// this method is called with the upper and lower bound of a set each time a
		// new set is added to adjust the upper and lower range values accordingly
		void AdjustRangeToFit(float min, float max) {
			if (min < m_dMinRange) m_dMinRange = min;
			if (max > m_dMaxRange) m_dMaxRange = max;
		}

		public string WriteDOMs {
			get {
				return m_MemberSets
					.Select(item => "\n" + item.Key + " is " + item.Value.GetDOM())
					.Aggregate((a,b) => a + b)
					+ "\nMin Range: " + m_dMinRange + "\nMax Range: " + m_dMaxRange;
				// var os = "";
				// foreach (var it in m_MemberSets) {
				// 	os += "\n" + it.Key + " is " + it.Value.GetDOM();
				// }
				// os += "\nMin Range: " + m_dMinRange + "\nMax Range: " + m_dMaxRange;
				// return os;
			}
		}

		// a client retrieves a reference to a fuzzy variable when an instance is
		// created via FuzzyModule::CreateFLV(). To prevent the client from deleting
		// the instance the FuzzyVariable destructor is made private and the
		// FuzzyModule class made a friend.
		~FuzzyVariable() {}
	}
}