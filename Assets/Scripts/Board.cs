using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;

    public int height;

    //public GameObject tilePrefab;
    public GameObject[] dots;

    //private BackgroundTile[,] allTiles;
    public GameObject[,] allDots;


    void Start()
    {
        //allTiles = new BackgroundTile[width, height];
        allDots = new GameObject[width, height];
        SetUp();
        //PrintAllTiles();
        //PrintAllDot();
    }

    private void SetUp()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j);
                //GameObject backgroundTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                //backgroundTile.transform.parent = this.transform;
                //backgroundTile.name = "( " + i + ", " + j + " )";
                int dotToUse = Random.Range(0, dots.Length);
                int maxIterations = 0;

                while (MatchesAt(i, j, dots[dotToUse]) && maxIterations < 100)
                {
                    dotToUse = Random.Range(0, dots.Length);
                    maxIterations++;
                    Debug.Log(maxIterations);
                }

                maxIterations = 0;

                GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                dot.transform.parent = this.transform;
                dot.name = "( " + i + ", " + j + " )";
                allDots[i, j] = dot;
            }
        }
    }

    private bool MatchesAt(int col, int row, GameObject piece)
    {
        if (col > 1 && row > 1)
        {
            if (allDots[col - 1, row].tag == piece.tag && allDots[col - 2, row].tag == piece.tag)
            {
                return true;
            }

            if (allDots[col, row - 1].tag == piece.tag && allDots[col, row - 2].tag == piece.tag)
            {
                return true;
            }
        }
        else if (col <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (allDots[col, row - 1].tag == piece.tag && allDots[col, row - 2].tag == piece.tag)
                {
                    return true;
                }
            }

            if (col > 1)
            {
                if (allDots[col - 1, row].tag == piece.tag && allDots[col - 2, row].tag == piece.tag)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void DestroyMatchesAt(int col, int row)
    {
        if (allDots[col, row].GetComponent<Dot>().isMatched)
        {
            Destroy(allDots[col, row]);
            allDots[col, row] = null;
        }
    }

    public void DestroyMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }

        // Cập nhật vị trí của tất cả dot ngay sau khi destroy
        UpdateBoardPositions();

    }

    private void UpdateBoardPositions()
    {
        // Duyệt từng cột của board
        for (int i = 0; i < width; i++)
        {
            // Đếm số ô trống ở cột hiện tại
            int emptyCount = 0;
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    emptyCount++;
                }
                else if (emptyCount > 0)
                {
                    // Di chuyển dot xuống đúng số ô trống
                    allDots[i, j].GetComponent<Dot>().row -= emptyCount;
                    // Cập nhật lại vị trí trong mảng
                    allDots[i, j - emptyCount] = allDots[i, j];
                    allDots[i, j] = null;
                }
            }
        }
    }
}



//void PrintAllTiles()
    //{
    //    for (int i = 0; i < width; i++)
    //    {
    //        for (int j = 0; j < height; j++)
    //        {
    //            if (allTiles[i, j] != null)
    //            {
    //                Debug.Log("Tile at (" + i + ", " + j + "): " + allTiles[i, j].name);
    //            }
    //            else
    //            {
    //                Debug.Log("Tile at (" + i + ", " + j + ") is NULL");
    //            }
    //        }
    //    }
    //}

    //void PrintAllDot()
    //{
    //    for (int i = 0; i < width; i++)
    //    {
    //        for (int j = 0; j < height; j++)
    //        {
    //            if (allDots[i, j] != null)
    //            {
    //                Debug.Log("Dot at (" + i + ", " + j + "): " + allDots[i, j].name);
    //            }
    //            else
    //            {
    //                Debug.Log("Dot at (" + i + ", " + j + ") is NULL");
    //            }
    //        }
    //    }
    //}


