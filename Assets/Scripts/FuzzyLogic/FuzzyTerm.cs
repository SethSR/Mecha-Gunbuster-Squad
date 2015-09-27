using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Linq;

namespace FuzzyLogic {
	abstract public class FuzzyTerm {
		// all terms must implement a virtual constructor
		abstract public FuzzyTerm Clone();

		// retrieves the degree of membership of the term
		abstract public float GetDOM();

		// clears the degree of membership of the term
		abstract public void ClearDOM();

		// method for updating the DOM of a consequent when a rule fires
		abstract public void OrwithDOM(float val);
	}

	public class FzSet : FuzzyTerm {
		public FuzzySet m_Set;

		public FzSet(FuzzySet fs) { m_Set = fs; }
		override public FuzzyTerm Clone    ()          { return new FzSet(this); }
		override public float     GetDOM   ()          { return m_Set.GetDOM();  }
		override public void      ClearDOM ()          { m_Set.ClearDOM();       }
		override public void      OrwithDOM(float val) { m_Set.OrwithDOM(val);   }

		FzSet(FzSet fs) { m_Set = fs.m_Set; }
	}

	public class FzAND : FuzzyTerm {
		public FzAND(FzAND fa) {
			fa.m_Terms.ForEach(cur_term => m_Terms.Add(cur_term.Clone()));
		}

		public FzAND(FuzzyTerm op1, params FuzzyTerm [] ops) {
			m_Terms.Add(op1.Clone());
			foreach (FuzzyTerm cur_term in ops) {
				m_Terms.Add(cur_term.Clone());
			}
		}

		override public FuzzyTerm Clone() { return new FzAND(this); }

		override public float GetDOM() {
			return m_Terms.Aggregate((a,b) => a.GetDOM() < b.GetDOM() ? a : b).GetDOM();
		}

		override public void OrwithDOM(float val) {
			m_Terms.ForEach(cur_term => cur_term.OrwithDOM(val));
		}

		override public void ClearDOM() {
			m_Terms.ForEach(cur_term => cur_term.ClearDOM());
		}

		List<FuzzyTerm> m_Terms = new List<FuzzyTerm>();
	}

	public class FzOR : FuzzyTerm {
		public FzOR(FzOR fo) {
			fo.m_Terms.ForEach(cur_term => m_Terms.Add(cur_term.Clone()));
		}

		public FzOR(FuzzyTerm op1, params FuzzyTerm [] ops) {
			m_Terms.Add(op1.Clone());
			foreach (FuzzyTerm cur_term in ops) {
				m_Terms.Add(cur_term.Clone());
			}
		}

		override public FuzzyTerm Clone() { return new FzOR(this); }

		override public float GetDOM() {
			return m_Terms.Aggregate((a,b) => a.GetDOM() > b.GetDOM() ? a : b).GetDOM();
		}

		override public void ClearDOM() {}
		override public void OrwithDOM(float val) {}

		List<FuzzyTerm> m_Terms = new List<FuzzyTerm>();
	}

	public class FzVery : FuzzyTerm {
		public FzVery(FzSet ft) { m_Set = ft.m_Set; }
		override public float     GetDOM   ()          { return m_Set.GetDOM() * m_Set.GetDOM(); }
		override public FuzzyTerm Clone    ()          { return new FzVery(this);                }
		override public void      ClearDOM ()          { m_Set.ClearDOM();                       }
		override public void      OrwithDOM(float val) { m_Set.OrwithDOM(val * val);             }

		FzVery(FzVery fv) { m_Set = fv.m_Set; }

		FuzzySet m_Set;
	}

	public class FzFairly : FuzzyTerm {
		public FzFairly(FzSet ft) { m_Set = ft.m_Set; }
		override public float     GetDOM   ()          { return Mathf.Sqrt(m_Set.GetDOM()); }
		override public FuzzyTerm Clone    ()          { return new FzFairly(this);         }
		override public void      ClearDOM ()          { m_Set.ClearDOM();                  }
		override public void      OrwithDOM(float val) { m_Set.OrwithDOM(Mathf.Sqrt(val));   }

		FzFairly(FzFairly ff) { m_Set = ff.m_Set; }

		FuzzySet m_Set;
	}
}