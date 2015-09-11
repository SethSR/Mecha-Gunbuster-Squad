using UnityEngine;
using System.Collections;

public class Robot : MonoBehaviour {
	public Offense  offense;
	public Defense  defense;
	public Movement movement;
	public int structure = 100; // Health
	public int squad     =   0;
}