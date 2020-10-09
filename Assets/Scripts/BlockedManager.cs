using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockedManager : MonoBehaviour
{
    public BuildPlane BuildPlane;
    public Material BlockedMaterial;
    private float _blockedCellsPercentage = 0.1f;

    private void Start()
    {
        BlockedGrid(BuildPlane.gridArray);
    }

    private void BlockedGrid(GameObject[,] gridArray)
	{
        int columns = BuildPlane.columns;
        int rows = BuildPlane.rows;
        int i = 0;
        int CountBlockedCells = (int)(columns * rows * _blockedCellsPercentage);
        while (i<CountBlockedCells)
		{
            int x = Random.Range(0, columns - 1);
            int y = Random.Range(0, rows-1);
            gridArray[x, y].GetComponent<GridStats>().isBlocked = true;
            gridArray[x, y].GetComponent<MeshRenderer>().material = BlockedMaterial;
            i++;
        }
	}

}
