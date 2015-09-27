using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Linq;

namespace FuzzyLogic {
	using VarMap = System.Collections.Generic.Dictionary<string, FuzzyVariable>;

	public class FuzzyModule {
		public enum DefuzzifyType {
			max_av,
			centroid,
		}

		// creates a new "empty" fuzzy variable and returns a reference to it.
		public FuzzyVariable CreateFLV(string VarName) {
			m_Variables[VarName] = new FuzzyVariable();
			return m_Variables[VarName];
		}

		const int NumSamples = 15;

		// a map of all the fuzzy variables this module uses
		VarMap m_Variables = new VarMap();

		// a vector containing all the fuzzy rules
		List<FuzzyRule> m_Rules = new List<FuzzyRule>();

		// zeros the DOMs of the consequents of each rule. Used by Defuzzify()
		void SetConfidencesOfConsequentsToZero() {
			m_Rules.ForEach(cur_rule => cur_rule.SetConfidenceOfConsequentToZero());
		}

		// adds a rule to the module
		public void AddRule(FuzzyTerm antecedent, FuzzyTerm consequence) {
			m_Rules.Add(new FuzzyRule(antecedent, consequence));
		}

		// this method calls the Fuzzify method for the named FLV
		void Fuzzify(string NameOfFLV, double val) {}

		// given a fuzzy variable and a defuzzification method this returns a crisp value
		double DeFuzzify(string NameOfFLV, DefuzzifyType method) {
			// first make sure the named FLV exists in this module
			//TODO: that

			// clear the DOMs of all the consequents
			SetConfidencesOfConsequentsToZero();

			// process the rules
			foreach (FuzzyRule cur_rule in m_Rules) {
				cur_rule.Calculate();
			}

			// now defuzzify the resultant conclusion using the specified method
			switch (method) {
				case DefuzzifyType.centroid: return m_Variables[NameOfFLV].DeFuzzifyCentroid(NumSamples);
				case DefuzzifyType.max_av  : return m_Variables[NameOfFLV].DeFuzzifyMaxAv();
			}

			return 0;
		}
	}
}