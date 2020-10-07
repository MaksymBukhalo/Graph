using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlayer : MonoBehaviour
{
	public int PositionX;
	public int PositionY;

	[SerializeField]
	private BuildPlane buildPlane;

	private void Start()
	{
		Transform transform = buildPlane.gridArray[0, 0].transform;
		gameObject.transform.position = new Vector3(transform.position.x, 0.3f, transform.position.z);
	}

	private void OnCollisionEnter(Collision collision)
	{
		GridStats gridStats = collision.gameObject.GetComponent<GridStats>();
		PositionX = gridStats.x;
		PositionY = gridStats.y;
	}

}
