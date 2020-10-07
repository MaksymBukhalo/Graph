using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPlane : MonoBehaviour
{
	public int rows = 10;
	public int columns = 10;
	public int scale = 1;
	public GameObject gridPrefab;
	public Vector3 leftBottomLocation = new Vector3(0, 0, 0);
	public GameObject[,] gridArray;

	private void Awake()
	{
		gridArray = new GameObject[columns, rows];
		if (gridPrefab)
		{
			GeneateGrid();
		}
		else
		{
			print("missing gridPrefab, please assign.");
		}
	}

	private void GeneateGrid()
	{
		for (int i = 0; i < columns; i++)
		{
			for (int j = 0; j < rows; j++)
			{
				GameObject obj = Instantiate(gridPrefab, new Vector3(leftBottomLocation.x + scale * i, leftBottomLocation.y, leftBottomLocation.z + scale * j), Quaternion.identity);
				obj.transform.SetParent(gameObject.transform);
				obj.GetComponent<GridStats>().x = i;
				obj.GetComponent<GridStats>().y = j;
				gridArray[i, j] = obj;
			}
		}
	}


}
