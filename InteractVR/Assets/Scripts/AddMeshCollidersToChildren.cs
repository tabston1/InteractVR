using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMeshCollidersToChildren : MonoBehaviour
{


	// Use this for initialization
	void Start ()
	{
		Transform t = gameObject.transform;
		foreach (Transform child in t) {
			if (t.Equals (child))
				continue;

			MeshFilter meshFilter = child.gameObject.GetComponent<MeshFilter> ();
				
			if (meshFilter != null && meshFilter.mesh.triangles.Length >= 3 && meshFilter.mesh.triangles.Length < 255) {
				MeshCollider meshCollider = child.gameObject.AddComponent<MeshCollider> ();
				meshCollider.sharedMesh = null;
				meshCollider.sharedMesh = meshFilter.mesh;
				meshCollider.convex = true;
				meshCollider.inflateMesh = true;
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
