using UnityEngine;
using System.Collections;

namespace FuzzyLogic {
	public class FuzzyRule {
		public FuzzyRule(FuzzyTerm ant,
		                 FuzzyTerm con) {
			m_pAntecedent  = ant.Clone();
			m_pConsequence = con.Clone();
		}

		public void SetConfidenceOfConsequentToZero() {
			m_pConsequence.ClearDOM();
		}

		// this method updates the DOM (the confidence) of the consequent term with
		// the DOM of the antecedent term.
		public void Calculate() {
			m_pConsequence.OrwithDOM(m_pAntecedent.GetDOM());
		}

		// antecedent (usually a composite of several fuzzy sets and operators)
		FuzzyTerm m_pAntecedent;

		// consequence (usually a single fuzzy set, but can be several ANDed together)
		FuzzyTerm m_pConsequence;
	}
}