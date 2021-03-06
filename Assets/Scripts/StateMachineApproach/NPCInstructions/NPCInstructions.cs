﻿using UnityEngine;
using System.Collections;

public class NPCInstructions : MonoBehaviour {

	// target types:
	// 0 -> process (chop trees, quarry rock, cut bushes)
	// 1 -> package (building blocks for building structures)
	// 2 -> work target
	//
	public int targetType = 0;

	// Process types
	// also refer to Blackboard (Board.cs) to ensure consistency between types
	// 0 -> wood
	// 1 -> rock
	// 2 -> bush
	public int resourceType = 0;

	// Package types
//	public int packageType = 0;// build a fishing spot / build a torch / build a bridge / etc...

	// Foundation type determines the type of building this resource can make
	// 0 -> no foundation --> wood on p0 = fishing spot, wood on p1 = Fish Rack, rock = wall
	// 1 -> shoreline/edge -> wood = jetty/boat / rock -> harbour / on higher platforms -> bridges
	// 		( 1 -> long bridge -> on edge of higher platforms)
	// 2 -> short bridge -> wood = 1 bridge section
	public int foundationType = 0;



	public void ResetFoundationType(){
		foundationType = 0;
	}
}


/// Notes
// think about making a more resource gathering style game... maybe different combinations of packages add up to a specific building??? might be more interesting and simpler for the palyer
