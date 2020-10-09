using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlayerManager : MonoBehaviour
{
	public List<GameObject> Path;
	public Stack<GameObject> path = new Stack<GameObject>();

	[SerializeField]
	private Camera _mainCamera;
	[SerializeField]
	private GameObject _player;
	[SerializeField]
	private BuildPlane _buildPlane;

	private int rows;
	private int columns;
	private int scale;
	private GameObject[,] gridArray;
	private int startX = 0;
	private int startY = 0;
	private int endX = 2;
	private int endY = 2;
	private MovingPlayer movingPlayer;


	private void Start()
	{
		gridArray = _buildPlane.gridArray;
		rows = _buildPlane.rows;
		columns = _buildPlane.columns;
		scale = _buildPlane.scale;
		movingPlayer = _player.GetComponent<MovingPlayer>();
	}

	private void Update()
	{
		if (Input.GetMouseButtonUp(0))
		{
			RaycastHit hit;
			Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit))
			{
				startX = movingPlayer.PositionX;
				startY = movingPlayer.PositionY;
				GridStats gridStats = hit.collider.gameObject.GetComponent<GridStats>();
				endX = gridStats.x;
				endY = gridStats.y;
				SetDistance();
				SetPath();
				StartCoroutine(MovePlayer());
			}
		}
	}

	IEnumerator MovePlayer()
	{
		Transform oldPosition = path.Pop().transform;
		int i = path.Count - 1;
		while (i > -1)
		{
			Transform newPosition = path.Pop().transform;
			_player.transform.position = new Vector3(newPosition.position.x, 0.3f, newPosition.position.z);
			yield return new WaitForSeconds(0.5f);
			i--;
		}
	}

	private void SetDistance()
	{
		InitialSetUp();
		int x = startX;
		int y = startY;
		int[] testArray = new int[rows * columns];
		for (int step = 1; step < rows * columns; step++)
		{
			foreach (GameObject obj in gridArray)
			{
				if (obj)
				{
					GridStats gridStats = obj.GetComponent<GridStats>();
					if (gridStats.visited == step - 1 && !gridStats.isBlocked)
					{
						TestFourDirection(gridStats.x, gridStats.y, step);
					}
				}
			}
		}
	}

	private void SetPath()
	{
		int step;
		int x = endX;
		int y = endY;
		List<GameObject> tempList = new List<GameObject>();
		//Path.Clear();
		if (gridArray[endX, endY])
		{
			GridStats gridStats = gridArray[endX, endY].GetComponent<GridStats>();
			if (gridStats.visited > 0)
				path.Push(gridArray[x, y]);
			step = gridStats.visited - 1;
		}
		else
		{
			print("Can't reach the desired location");
			return;
		}
		for (int i = step; step > -1; step--)
		{
			if (TestDirection(x, y, step, 1))
			{
				tempList.Add(gridArray[x, y + 1]);
			}
			if (TestDirection(x, y, step, 2))
			{
				tempList.Add(gridArray[x + 1, y]);
			}
			if (TestDirection(x, y, step, 3))
			{
				tempList.Add(gridArray[x, y - 1]);
			}
			if (TestDirection(x, y, step, 4))
			{
				tempList.Add(gridArray[x - 1, y]);
			}
			GameObject tempObj = FindClosest(gridArray[endX, endY].transform, tempList);
			path.Push(tempObj);
			GridStats gridStats = tempObj.GetComponent<GridStats>();
			x = gridStats.x;
			y = gridStats.y;
			tempList.Clear();
		}
	}

	private void InitialSetUp()
	{
		foreach (GameObject obj in gridArray)
		{
			obj.GetComponent<GridStats>().visited = -1;
		}
		gridArray[startX, startY].GetComponent<GridStats>().visited = 0;
	}

	private bool TestDirection(int x, int y, int step, int direction)
	{
		switch (direction)
		{
			case 4:
				if (x - 1 > -1 && gridArray[x - 1, y] && IsCorrectCell(gridArray[x - 1, y].GetComponent<GridStats>(), step))
				{
					return true;
				}
				else
				{
					return false;
				}
			case 3:
				if (y - 1 > -1 && gridArray[x, y - 1] && IsCorrectCell(gridArray[x, y - 1].GetComponent<GridStats>(), step))
				{
					return true;
				}
				else
				{
					return false;
				}
			case 2:
				if (x + 1 < columns && gridArray[x + 1, y] && IsCorrectCell(gridArray[x + 1, y].GetComponent<GridStats>(), step))
				{
					return true;
				}
				else
				{
					return false;
				}
			case 1:
				if (y + 1 < rows && gridArray[x, y + 1] && IsCorrectCell(gridArray[x, y + 1].GetComponent<GridStats>(), step))
				{
					return true;
				}
				else
				{
					return false;
				}
		}
		return false;
	}

	private bool IsCorrectCell(GridStats grid, int step)
	{
		if (grid.visited == step && !grid.isBlocked)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private void TestFourDirection(int x, int y, int step)
	{
		if (TestDirection(x, y, -1, 1))
		{
			SetVisited(x, y + 1, step);
		}
		if (TestDirection(x, y, -1, 2))
		{
			SetVisited(x + 1, y, step);
		}
		if (TestDirection(x, y, -1, 3))
		{
			SetVisited(x, y - 1, step);
		}
		if (TestDirection(x, y, -1, 4))
		{
			SetVisited(x - 1, y, step);
		}
	}
	private void SetVisited(int x, int y, int step)
	{
		if (gridArray[x, y])
		{
			gridArray[x, y].GetComponent<GridStats>().visited = step;
		}
	}

	private GameObject FindClosest(Transform targetLocation, List<GameObject> list)
	{
		float curentDistance = scale * rows * columns;
		int indexNumber = 0;
		for (int i = 0; i < list.Count; i++)
		{
			if (Vector3.Distance(targetLocation.position, list[i].transform.position) < curentDistance)
			{
				curentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
				indexNumber = i;
			}
		}
		return list[indexNumber];
	}
}
