using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FOV2DEyes : MonoBehaviour
{
	public bool raysGizmosEnabled;
	//public float updateRate = 0.02f;
	public int quality = 4;
	public int fovAngle = 90;
	public float fovMaxDistance = 15;
	public LayerMask cullingMask;
	public List<RaycastHit> hits = new List<RaycastHit>();
	
	int numRays;
	float currentAngle;
	Vector3 direction;
	RaycastHit hit;
	
	void Update()
	{
		CastRays();
	}
	
	void Start() 
	{
		//InvokeRepeating("CastRays", 0, updateRate);
	}
	
	void CastRays()
	{
		numRays = fovAngle * quality;
		currentAngle = fovAngle / -2;
		
		hits.Clear();
		
		for (int i = 0; i < numRays; i++)
		{
			direction = Quaternion.AngleAxis(currentAngle, transform.up) * transform.forward;
			hit = new RaycastHit();
			
			if(Physics.Raycast(transform.position, direction, out hit, fovMaxDistance, cullingMask) == false)
			{
				hit.point = transform.position + (direction * fovMaxDistance);
			}
			
			hits.Add(hit);

			currentAngle += 1f / quality;
		}
	}
	
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		
		if (raysGizmosEnabled && hits.Count() > 0) 
		{
			foreach (RaycastHit hit in hits)
			{
				Gizmos.DrawSphere(hit.point, 0.04f);
				Gizmos.DrawLine(transform.position, hit.point);
			}
		}
	}
	
}
