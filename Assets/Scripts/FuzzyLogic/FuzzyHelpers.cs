namespace FuzzyLogic {
	static public class FuzzyHelpers {
			static public FzSet set(this FuzzySet fs) { return new FzSet(fs); }
			static public FzAND and(this FzSet fs) { return new FzAND(fs); }
			static public FzOR  or (this FzSet fs) { return new FzOR (fs); }
			static public FzAND and(this FuzzyTerm op, params FuzzyTerm [] ops) { return new FzAND(op, ops); }
			static public FzOR  or (this FuzzyTerm op, params FuzzyTerm [] ops) { return new FzOR (op, ops); }

		// static public FzAND    and   (FzAND fa) { return new FzAND   (fa); }
		// static public FzOR     or    (FzOR  fo) { return new FzOR    (fo); }
		// static public FzVery   very  (FzSet fs) { return new FzVery  (fs); }
		// static public FzFairly fairly(FzSet fs) { return new FzFairly(fs); }
	}
}