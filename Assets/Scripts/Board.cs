using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject[] dots;
    public GameObject[,] allDots;
    public bool isRefilling;
    private int maxRefillIterations = 10;
    private float spacing = 60f;
    private int movingDotsCount = 0;

    public bool IsRefilling => isRefilling;
    public bool IsMovingDots => movingDotsCount > 0;

    private Vector2 GetLocalPosition(int col, int row)
    {
        float offsetX = width / 2f - 0.5f;
        float offsetY = height / 2f - 0.5f;
        return new Vector2((col - offsetX) * spacing, (row - offsetY) * spacing);
    }

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
                Vector2 localPos = GetLocalPosition(i, j);
                GameObject dot = CreateNewDot(i, j);
                dot.transform.parent = this.transform;
                dot.transform.localPosition = localPos;
                dot.name = $"({i}, {j})";
                allDots[i, j] = dot;

                // Đảm bảo col và row được gán đúng
                Dot dotComponent = dot.GetComponent<Dot>();
                if (dotComponent != null)
                {
                    dotComponent.col = i;
                    dotComponent.row = j;
                }
            }
        }
    }

    private GameObject CreateNewDot(int x, int y)
    {
        List<int> availableDots = new List<int>();

        for (int i = 0; i < dots.Length; i++)
        {
            if (!WillCreateMatch(x, y, dots[i]))
            {
                availableDots.Add(i);
            }
        }

        int dotToUse = availableDots.Count > 0
            ? availableDots[Random.Range(0, availableDots.Count)]
            : Random.Range(0, dots.Length);

        GameObject dot = Instantiate(dots[dotToUse], Vector3.zero, Quaternion.identity);
        Dot dotComponent = dot.GetComponent<Dot>();
        if (dotComponent != null)
        {
            dotComponent.col = x;
            dotComponent.row = y;
        }
        return dot;
    }

    private bool WillCreateMatch(int col, int row, GameObject piece)
    {
        string pieceTag = piece.tag;

        if (col > 1)
        {
            if (allDots[col - 1, row] != null && allDots[col - 2, row] != null)
            {
                if (allDots[col - 1, row].tag == pieceTag && allDots[col - 2, row].tag == pieceTag)
                {
                    return true;
                }
            }
        }

        if (col > 0 && col < width - 1)
        {
            if (allDots[col - 1, row] != null && allDots[col + 1, row] != null)
            {
                if (allDots[col - 1, row].tag == pieceTag && allDots[col + 1, row].tag == pieceTag)
                {
                    return true;
                }
            }
        }

        if (col < width - 2)
        {
            if (allDots[col + 1, row] != null && allDots[col + 2, row] != null)
            {
                if (allDots[col + 1, row].tag == pieceTag && allDots[col + 2, row].tag == pieceTag)
                {
                    return true;
                }
            }
        }

        if (row > 1)
        {
            if (allDots[col, row - 1] != null && allDots[col, row - 2] != null)
            {
                if (allDots[col, row - 1].tag == pieceTag && allDots[col, row - 2].tag == pieceTag)
                {
                    return true;
                }
            }
        }

        if (row > 0 && row < height - 1)
        {
            if (allDots[col, row - 1] != null && allDots[col, row + 1] != null)
            {
                if (allDots[col, row - 1].tag == pieceTag && allDots[col, row + 1].tag == pieceTag)
                {
                    return true;
                }
            }
        }

        if (row < height - 2)
        {
            if (allDots[col, row + 1] != null && allDots[col, row + 2] != null)
            {
                if (allDots[col, row + 1].tag == pieceTag && allDots[col, row + 2].tag == pieceTag)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void DestroyMatchesAt(int col, int row)
    {
        if (allDots[col, row] != null && allDots[col, row].GetComponent<Dot>().isMatched)
        {
            Dmg dmg = allDots[col, row].GetComponent<Dmg>();
            if (dmg != null)
            {
                dmg.ProcessMatch();
            }
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

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height - 1; j++)
            {
                if (allDots[i, j] == null)
                {
                    for (int k = j + 1; k < height; k++)
                    {
                        if (allDots[i, k] != null)
                        {
                            allDots[i, j] = allDots[i, k];
                            allDots[i, k] = null;

                            allDots[i, j].GetComponent<Dot>().row = j;
                            allDots[i, j].GetComponent<Dot>().col = i;

                            Vector2 tempPosition = new Vector2(i, j);
                            StartCoroutine(MovePiece(allDots[i, j], tempPosition));

                            break;
                        }
                    }
                }
            }
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    GameObject inactivePiece = FindInactivePiece();

                    if (inactivePiece != null)
                    {
                        Dot dotComponent = inactivePiece.GetComponent<Dot>();
                        if (dotComponent != null)
                        {
                            dotComponent.isMatched = false;
                            dotComponent.row = j;
                            dotComponent.col = i;

                            UpdateDotTypeToAvoidMatch(inactivePiece, i, j);
                        }

                        inactivePiece.SetActive(true);
                        inactivePiece.transform.localPosition = GetLocalPosition(i, height);
                        allDots[i, j] = inactivePiece;

                        Vector2 tempPosition = new Vector2(i, j);
                        StartCoroutine(MovePiece(inactivePiece, tempPosition));
                    }
                    else
                    {
                        Vector2 spawnLocalPosition = GetLocalPosition(i, height);
                        GameObject newDot = CreateNonMatchingDot(i, j);
                        newDot.transform.parent = this.transform;
                        newDot.transform.localPosition = spawnLocalPosition;
                        newDot.name = $"({i}, {j})";
                        allDots[i, j] = newDot;

                        Vector2 tempPosition = new Vector2(i, j);
                        StartCoroutine(MovePiece(newDot, tempPosition));
                    }
                }
            }
        }

        yield return new WaitForSeconds(.4f);

        CheckMatchesAfterRefill();

        StartCoroutine(FillBoardCo());
    }

    private void CheckMatchesAfterRefill()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    allDots[i, j].GetComponent<Dot>().FindMatches();
                }
            }
        }
    }

    private GameObject CreateNonMatchingDot(int col, int row)
    {
        List<int> availableDots = new List<int>();

        for (int i = 0; i < dots.Length; i++)
        {
            if (!WillCreateMatch(col, row, dots[i]))
            {
                availableDots.Add(i);
            }
        }

        int dotToUse = availableDots.Count > 0
            ? availableDots[Random.Range(0, availableDots.Count)]
            : Random.Range(0, dots.Length);

        GameObject dot = Instantiate(dots[dotToUse], Vector3.zero, Quaternion.identity);
        Dot dotComponent = dot.GetComponent<Dot>();
        if (dotComponent != null)
        {
            dotComponent.row = row;
            dotComponent.col = col;
            dotComponent.isMatched = false;
        }

        return dot;
    }

    private void UpdateDotTypeToAvoidMatch(GameObject dot, int col, int row)
    {
        List<int> availableDots = new List<int>();

        for (int i = 0; i < dots.Length; i++)
        {
            if (!WillCreateMatch(col, row, dots[i]))
            {
                availableDots.Add(i);
            }
        }

        if (availableDots.Count > 0)
        {
            int dotToUse = availableDots[Random.Range(0, availableDots.Count)];

            dot.tag = dots[dotToUse].tag;

            SpriteRenderer dotRenderer = dot.GetComponent<SpriteRenderer>();
            SpriteRenderer templateRenderer = dots[dotToUse].GetComponent<SpriteRenderer>();

            if (dotRenderer != null && templateRenderer != null)
            {
                dotRenderer.sprite = templateRenderer.sprite;
            }
        }
    }

    private GameObject FindInactivePiece()
    {
        foreach (Transform child in transform)
        {
            if (!child.gameObject.activeSelf)
            {
                return child.gameObject;
            }
        }
        return null;
    }

    public IEnumerator MovePiece(GameObject piece, Vector2 endPosition)
    {
        movingDotsCount++;

        int targetCol = (int)endPosition.x;
        int targetRow = (int)endPosition.y;
        Vector2 adjustedEndLocalPosition = GetLocalPosition(targetCol, targetRow);

        float elapsedTime = 0f;
        float moveTime = 0.2f;
        Vector2 startLocalPosition = piece.transform.localPosition;

        while (elapsedTime < moveTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveTime;
            piece.transform.localPosition = Vector2.Lerp(startLocalPosition, adjustedEndLocalPosition, t);
            yield return null;
        }

        piece.transform.localPosition = adjustedEndLocalPosition;
        movingDotsCount--;
    }

    private IEnumerator FillBoardCo()
    {
        isRefilling = true;
        int iterations = 0;

        yield return new WaitForSeconds(.5f);

        bool matchesExist = MatchesOnBoard();

        while (matchesExist && iterations < maxRefillIterations)
        {
            iterations++;

            DestroyMatches();

            yield return new WaitForSeconds(.5f);

            matchesExist = MatchesOnBoard();
        }

        if (iterations >= maxRefillIterations && MatchesOnBoard())
        {
            Debug.Log("Đạt giới hạn refill - đặt lại bảng");
            yield return StartCoroutine(ResetBoardCo());
        }

        isRefilling = false;
    }

    private IEnumerator ResetBoardCo()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    allDots[i, j].SetActive(false);
                    allDots[i, j] = null;
                }
            }
        }

        yield return new WaitForSeconds(0.5f);

        SetUp();
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

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    allDots[i, j].GetComponent<Dot>().FindMatches();

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