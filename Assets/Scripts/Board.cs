using System.Collections;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject[] dots;
    public GameObject[,] allDots;
    private bool isRefilling = false;

    void Start()
    {
        allDots = new GameObject[width, height];
        SetUp();
    }

    private void SetUp()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j);
                GameObject dot = CreateNewDot(i, j);
                dot.transform.parent = this.transform;
                dot.name = $"({i}, {j})";
                allDots[i, j] = dot;
            }
        }
    }

    private GameObject CreateNewDot(int x, int y)
    {
        int dotToUse = Random.Range(0, dots.Length);
        int maxIterations = 0;

        while (MatchesAt(x, y, dots[dotToUse]) && maxIterations < 100)
        {
            dotToUse = Random.Range(0, dots.Length);
            maxIterations++;
        }

        GameObject dot = Instantiate(dots[dotToUse], new Vector2(x, y), Quaternion.identity);
        return dot;
    }

    private bool MatchesAt(int col, int row, GameObject piece)
    {
        if (col > 1)
        {
            if (allDots[col - 1, row]?.tag == piece.tag && allDots[col - 2, row]?.tag == piece.tag)
            {
                return true;
            }
        }
        if (row > 1)
        {
            if (allDots[col, row - 1]?.tag == piece.tag && allDots[col, row - 2]?.tag == piece.tag)
            {
                return true;
            }
        }
        return false;
    }

    public void DestroyMatchesAt(int col, int row)
    {
        if (allDots[col, row] != null && allDots[col, row].GetComponent<Dot>().isMatched)
        {
            // Instead of destroying, just deactivate the game object
            allDots[col, row].SetActive(false);
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
        StartCoroutine(DecreaseRowCo());
    }

    private IEnumerator DecreaseRowCo()
    {
        yield return new WaitForSeconds(.4f);

        // First, move all pieces down
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height - 1; j++)
            {
                if (allDots[i, j] == null)
                {
                    // Look for the next non-null piece above
                    for (int k = j + 1; k < height; k++)
                    {
                        if (allDots[i, k] != null)
                        {
                            // Move the piece down
                            allDots[i, j] = allDots[i, k];
                            allDots[i, k] = null;

                            // Update the piece's position
                            allDots[i, j].GetComponent<Dot>().row = j;
                            allDots[i, j].GetComponent<Dot>().col = i;

                            // Start movement animation
                            Vector2 tempPosition = new Vector2(i, j);
                            StartCoroutine(Movepiece(allDots[i, j], tempPosition));

                            break;
                        }
                    }
                }
            }
        }

        // After moving pieces down, fill empty spaces at the top
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    // Find any deactivated pieces we can reuse
                    GameObject inactivePiece = FindInactivePiece();
                    if (inactivePiece != null)
                    {
                        // Reuse the piece
                        inactivePiece.SetActive(true);
                        inactivePiece.transform.position = new Vector2(i, height);
                        allDots[i, j] = inactivePiece;

                        // Set up the piece
                        Dot dot = inactivePiece.GetComponent<Dot>();
                        dot.row = j;
                        dot.col = i;

                        // Move it to its position
                        Vector2 tempPosition = new Vector2(i, j);
                        StartCoroutine(Movepiece(inactivePiece, tempPosition));
                    }
                }
            }
        }

        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }

    private GameObject FindInactivePiece()
    {
        // Look through all child objects for an inactive piece
        foreach (Transform child in transform)
        {
            if (!child.gameObject.activeSelf)
            {
                return child.gameObject;
            }
        }
        return null;
    }

    private IEnumerator Movepiece(GameObject piece, Vector2 endPosition)
    {
        float elapsedTime = 0f;
        float moveTime = 0.2f;
        Vector2 startPosition = piece.transform.position;

        while (elapsedTime < moveTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveTime;
            piece.transform.position = Vector2.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        piece.transform.position = endPosition;
    }


    private IEnumerator FillBoardCo()
    {
        yield return new WaitForSeconds(.5f);

        while (MatchesOnBoard())
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
    }

    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    Vector2 tempPosition = new Vector2(i, j);
                    GameObject piece = CreateNewDot(i, j);
                    allDots[i, j] = piece;
                }
            }
        }
    }

    private bool MatchesOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (allDots[i, j].GetComponent<Dot>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}