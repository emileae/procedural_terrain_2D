using UnityEngine;
using System.Collections;

public class NPCInstructions : MonoBehaviour {

	// target types:
	// 0 -> process
	// 1 -> package
	// 2 -> work target
	//
	public int targetType = 0;// target types -> resource to process / package to build / boat to board / enemy to attack

	// Process types
	// 0 -> poles (makes a fishing spot?)
	public int processType = 0;// chop tree A / chop tree B etc. / break rock / level ground / kill crab parasite thingy 

	// Package types
	public int packageType = 0;// build a fishing spot / build a torch / build a bridge / etc...
}


/// Notes
// think about making a more resource gathering style game... maybe different combinations of packages add up to a specific building??? might be more interesting and simpler for the palyer
