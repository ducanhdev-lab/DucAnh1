using UnityEngine;
using System.Collections;

public class Dot : MonoBehaviour
{
    [Header("Board Variables")]
    public int col;
    public int row;
    public int previousCol;
    public int previousRow;
    public bool isMatched = false;

    private Board board;
    private GameObject otherDot;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private float swipeAngle = 0;
    public float swipeResist = 1f;
    private float moveSpeed = 8f;
    private bool isMoving = false;

    void Start()
    {
        board = FindObjectOfType<Board>();
        row = Mathf.RoundToInt(transform.position.y);
        col = Mathf.RoundToInt(transform.position.x);
        previousRow = row;
        previousCol = col;
    }

    void Update()
    {
        if (isMoving)
        {
            Vector2 targetPosition = new Vector2(col, row);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if ((Vector2)transform.position == targetPosition)
            {
                isMoving = false;
                board.allDots[col, row] = gameObject;
            }
        }
    }

    private void OnMouseDown()
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    void CalculateAngle()
    {
        if (Vector2.Distance(firstTouchPosition, finalTouchPosition) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y,
                                    finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            MovePieces();
        }zzzz
    }

    void MovePieces()
    {
        previousRow = row;
        previousCol = col;

        if (swipeAngle > -45 && swipeAngle <= 45 && col < board.width - 1)
        {
            // Right swipe
            SwapPieces(1, 0);
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
        {
            // Up swipe
            SwapPieces(0, 1);
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && col > 0)
        {
            // Left swipe
            SwapPieces(-1, 0);
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            // Down swipe
            SwapPieces(0, -1);
        }

        StartCoroutine(CheckMoveCo());
    }

    private void SwapPieces(int colDir, int rowDir)
    {
        otherDot = board.allDots[col + colDir, row + rowDir];
        if (otherDot != null)
        {
            board.allDots[col, row] = otherDot;
            board.allDots[col + colDir, row + rowDir] = gameObject;

            col += colDir;
            row += rowDir;

            otherDot.GetComponent<Dot>().col -= colDir;
            otherDot.GetComponent<Dot>().row -= rowDir;

            isMoving = true;
            otherDot.GetComponent<Dot>().isMoving = true;
        }
    }

    private IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(.5f);

        if (otherDot != null)
        {
            FindMatches();
            otherDot.GetComponent<Dot>().FindMatches();

            if (!isMatched && !otherDot.GetComponent<Dot>().isMatched)
            {
                // Move pieces back
                otherDot.GetComponent<Dot>().row = row;
                otherDot.GetComponent<Dot>().col = col;
                row = previousRow;
                col = previousCol;

                isMoving = true;
                otherDot.GetComponent<Dot>().isMoving = true;
            }
            else
            {
                board.DestroyMatches();
            }
            otherDot = null;
        }
    }

    public void FindMatches()
    {
        // Horizontal matches
        if (col > 0 && col < board.width - 1)
        {
            GameObject leftDot = board.allDots[col - 1, row];
            GameObject rightDot = board.allDots[col + 1, row];
            if (leftDot != null && rightDot != null)
            {
                if (leftDot.tag == gameObject.tag && rightDot.tag == gameObject.tag)
                {
                    leftDot.GetComponent<Dot>().isMatched = true;
                    rightDot.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
        }

        // Vertical matches
        if (row > 0 && row < board.height - 1)
        {
            GameObject upDot = board.allDots[col, row + 1];
            GameObject downDot = board.allDots[col, row - 1];
            if (upDot != null && downDot != null)
            {
                if (upDot.tag == gameObject.tag && downDot.tag == gameObject.tag)
                {
                    upDot.GetComponent<Dot>().isMatched = true;
                    downDot.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
        }
    }
}