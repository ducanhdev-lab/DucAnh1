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
    public Camera CombatCamera;

    private Board board;
    private GameObject otherDot;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private float swipeAngle = 0;
    public float swipeResist = 1f;

    void Start()
    {
        board = FindObjectOfType<Board>();
        CombatCamera = GameObject.FindWithTag("CombatCamera")?.GetComponent<Camera>();
    }

    private void OnMouseDown()
    {
        if (board != null && !CombatManager.Instance.IsBotTurn && !board.IsRefilling)
        {
            Vector2 worldPos = CombatCamera.ScreenToWorldPoint(Input.mousePosition);
            firstTouchPosition = board.transform.InverseTransformPoint(worldPos);
        }
        else
        {
            if (CombatManager.Instance.IsBotTurn)
            {
                Debug.Log("Player input blocked: Bot's turn.");
            }
            else if (board.IsRefilling)
            {
                Debug.Log("Player input blocked: Board is refilling.");
            }
        }
    }

    private void OnMouseUp()
    {
        if (board != null && !CombatManager.Instance.IsBotTurn)
        {
            Vector2 worldPos = CombatCamera.ScreenToWorldPoint(Input.mousePosition);
            finalTouchPosition = board.transform.InverseTransformPoint(worldPos);
            CalculateAngle();
        }
    }

    void CalculateAngle()
    {
        if (Vector2.Distance(firstTouchPosition, finalTouchPosition) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y,
                                    finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            MovePieces();
        }
    }

    void MovePieces()
    {
        previousRow = row;
        previousCol = col;

        if (swipeAngle > -45 && swipeAngle <= 45 && col < board.width - 1)
        {
            SwapPieces(1, 0);
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
        {
            SwapPieces(0, 1);
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && col > 0)
        {
            SwapPieces(-1, 0);
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            SwapPieces(0, -1);
        }

        StartCoroutine(CheckMoveCo());
    }

    private void SwapPieces(int colDir, int rowDir)
    {
        int targetCol = col + colDir;
        int targetRow = row + rowDir;

        if (targetCol >= 0 && targetCol < board.width && targetRow >= 0 && targetRow < board.height)
        {
            otherDot = board.allDots[targetCol, targetRow];

            if (otherDot != null)
            {
                board.allDots[col, row] = otherDot;
                board.allDots[targetCol, targetRow] = gameObject;

                col = targetCol; // Cập nhật trực tiếp thay vì += để tránh sai sót
                row = targetRow;

                Dot otherDotComponent = otherDot.GetComponent<Dot>();
                if (otherDotComponent != null)
                {
                    otherDotComponent.col = col - colDir;
                    otherDotComponent.row = row - rowDir;
                }

                // Log để kiểm tra giá trị col và row
                Debug.Log($"Swapping: Dot1 ({gameObject.name}) to (col: {col}, row: {row})");
                Debug.Log($"Swapping: Dot2 ({otherDot.name}) to (col: {otherDotComponent.col}, row: {otherDotComponent.row})");

                StartCoroutine(board.MovePiece(gameObject, new Vector2(col, row)));
                StartCoroutine(board.MovePiece(otherDot, new Vector2(otherDotComponent.col, otherDotComponent.row)));
            }
        }
        else
        {
            Debug.LogWarning($"Invalid swap target: (col: {targetCol}, row: {targetRow})");
        }
    }

    private IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(.5f);

        if (otherDot != null)
        {
            FindMatches();

            Dot otherDotComponent = otherDot.GetComponent<Dot>();
            if (otherDotComponent != null)
            {
                otherDotComponent.FindMatches();
                if (!isMatched && !otherDotComponent.isMatched)
                {
                    otherDotComponent.row = row;
                    otherDotComponent.col = col;
                    row = previousRow;
                    col = previousCol;

                    board.allDots[col, row] = gameObject;
                    board.allDots[otherDotComponent.col, otherDotComponent.row] = otherDot;

                    // Log để kiểm tra giá trị khi hoàn tác
                    Debug.Log($"Reverting: Dot1 ({gameObject.name}) to (col: {col}, row: {row})");
                    Debug.Log($"Reverting: Dot2 ({otherDot.name}) to (col: {otherDotComponent.col}, row: {otherDotComponent.row})");

                    StartCoroutine(board.MovePiece(gameObject, new Vector2(col, row)));
                    StartCoroutine(board.MovePiece(otherDot, new Vector2(otherDotComponent.col, otherDotComponent.row)));
                }
                else
                {
                    board.DestroyMatches();
                    CombatManager.Instance.EndPlayerTurn();
                }
            }
            otherDot = null;
        }
    }

    public void FindMatches()
    {
        if (col >= 0 && col < board.width && row >= 0 && row < board.height)
        {
            if (col > 0 && col < board.width - 1)
            {
                GameObject leftDot = board.allDots[col - 1, row];
                GameObject rightDot = board.allDots[col + 1, row];
                if (leftDot != null && rightDot != null && leftDot.tag == gameObject.tag && rightDot.tag == gameObject.tag)
                {
                    leftDot.GetComponent<Dot>().isMatched = true;
                    rightDot.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }

            if (col < board.width - 2)
            {
                GameObject rightDot1 = board.allDots[col + 1, row];
                GameObject rightDot2 = board.allDots[col + 2, row];
                if (rightDot1 != null && rightDot2 != null && rightDot1.tag == gameObject.tag && rightDot2.tag == gameObject.tag)
                {
                    rightDot1.GetComponent<Dot>().isMatched = true;
                    rightDot2.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }

            if (col > 1)
            {
                GameObject leftDot1 = board.allDots[col - 1, row];
                GameObject leftDot2 = board.allDots[col - 2, row];
                if (leftDot1 != null && leftDot2 != null && leftDot1.tag == gameObject.tag && leftDot2.tag == gameObject.tag)
                {
                    leftDot1.GetComponent<Dot>().isMatched = true;
                    leftDot2.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }

            if (row > 0 && row < board.height - 1)
            {
                GameObject upDot = board.allDots[col, row + 1];
                GameObject downDot = board.allDots[col, row - 1];
                if (upDot != null && downDot != null && upDot.tag == gameObject.tag && downDot.tag == gameObject.tag)
                {
                    upDot.GetComponent<Dot>().isMatched = true;
                    downDot.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }

            if (row < board.height - 2)
            {
                GameObject upDot1 = board.allDots[col, row + 1];
                GameObject upDot2 = board.allDots[col, row + 2];
                if (upDot1 != null && upDot2 != null && upDot1.tag == gameObject.tag && upDot2.tag == gameObject.tag)
                {
                    upDot1.GetComponent<Dot>().isMatched = true;
                    upDot2.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }

            if (row > 1)
            {
                GameObject downDot1 = board.allDots[col, row - 1];
                GameObject downDot2 = board.allDots[col, row - 2];
                if (downDot1 != null && downDot2 != null && downDot1.tag == gameObject.tag && downDot2.tag == gameObject.tag)
                {
                    downDot1.GetComponent<Dot>().isMatched = true;
                    downDot2.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
        }
    }
}