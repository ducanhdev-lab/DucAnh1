using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Dot : MonoBehaviour
{
    [Header("Board Variables")]
    public int col;
    public int row;
    public int previousCol;
    public int previousRow;
    public bool isMatched = false;
    public int targetX;
    public int targetY;

    private Board board;
    private GameObject otherDot;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;
    public float swipeAngle = 0;
    public float swipeResist = 1f;


    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        row = targetY;
        col = targetX;
        previousCol = col;
        previousRow = row;
    }

    // Update is called once per frame
    void Update()
    {
        // find matches
        FindMatches();



        targetX = col;
        targetY = row;
        // move to horizontal
        if (Mathf.Abs(targetX - transform.position.x) > 1)
        {
            // move towards the target
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allDots[col, row] == this.gameObject)
            {
                board.allDots[col, row] = this.gameObject;
            }
        }
        else
        {
            //directly set the position
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            // board.allDots[col, row] = this.gameObject;
        }

        // move to vertical
        if (Mathf.Abs(targetY - transform.position.y) > 1)
        {
            // move towards the target
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allDots[col, row] = this.gameObject)
            {
                board.allDots[col, row] = this.gameObject;
            }
        }
        else
        {
            //directly set the position
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            // board.allDots[col, row] = this.gameObject;
        }
    }

    public IEnumerator CheckMove()
    {
        yield return new WaitForSeconds(.5f);
        if (otherDot != null)
        {
            if (!isMatched && !otherDot.GetComponent<Dot>().isMatched)
            {
                otherDot.GetComponent<Dot>().row = row;
                otherDot.GetComponent<Dot>().col = col;
                row = previousRow;
                col = previousCol;
            }
            else
            {
                board.DestroyMatches();
            }
            otherDot = null;
        }
    }

    private void OnMouseDown()
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    
        // Hiển thị col, row trước khi di chuyển
        Debug.Log($"[Trước di chuyển] Dot tại col: {col}, row: {row}");
    }

    private void OnMouseUp()
    {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    
        // Hiển thị col, row sau khi di chuyển
        Debug.Log($"[Sau di chuyển] Dot tại col: {col}, row: {row}");
    }


    void CalculateAngle()
    {
        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist
        || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            // Debug.Log(swipeAngle);
            MovePieces();
        }
    }

    void MovePieces()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && col < board.width - 1)
        {
            // right swipe
            otherDot = board.allDots[col + 1, row];
            otherDot.GetComponent<Dot>().col -= 1;
            col += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
        {
            // up swipe
            otherDot = board.allDots[col, row + 1];
            otherDot.GetComponent<Dot>().row -= 1;
            row += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && col > 0)
        {
            // left swipe
            otherDot = board.allDots[col - 1, row];
            otherDot.GetComponent<Dot>().col += 1;
            col -= 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            // down swipe
            otherDot = board.allDots[col, row - 1];
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;
        }
        StartCoroutine(CheckMove());
    }

    public void FindMatches()
    {
        // --- Kiểm tra theo chiều ngang ---
        List<GameObject> horizontalMatches = new List<GameObject>();
        horizontalMatches.Add(this.gameObject);  // Thêm dot hiện tại

        // Check bên trái
        int leftIndex = col - 1;
        while (leftIndex >= 0)
        {
            GameObject leftDot = board.allDots[leftIndex, row];
            if (leftDot != null && leftDot.tag == this.gameObject.tag)
            {
                horizontalMatches.Add(leftDot);
                leftIndex--;
            }
            else
            {
                break;
            }
        }

        // Check bên phải
        int rightIndex = col + 1;
        while (rightIndex < board.width)
        {
            GameObject rightDot = board.allDots[rightIndex, row];
            if (rightDot != null && rightDot.tag == this.gameObject.tag)
            {
                horizontalMatches.Add(rightDot);
                rightIndex++;
            }
            else
            {
                break;
            }
        }

        // Nếu có ít nhất 3 dot liền nhau theo hàng thì đánh dấu
        if (horizontalMatches.Count >= 3)
        {
            foreach (GameObject dot in horizontalMatches)
            {
                dot.GetComponent<Dot>().isMatched = true;
            }
        }

        // --- Kiểm tra theo chiều dọc ---
        List<GameObject> verticalMatches = new List<GameObject>();
        verticalMatches.Add(this.gameObject);  // Thêm dot hiện tại

        // Check phía trên
        int upIndex = row + 1;
        while (upIndex < board.height)
        {
            GameObject upDot = board.allDots[col, upIndex];
            if (upDot != null && upDot.tag == this.gameObject.tag)
            {
                verticalMatches.Add(upDot);
                upIndex++;
            }
            else
            {
                break;
            }
        }

        // Check phía dưới
        int downIndex = row - 1;
        while (downIndex >= 0)
        {
            GameObject downDot = board.allDots[col, downIndex];
            if (downDot != null && downDot.tag == this.gameObject.tag)
            {
                verticalMatches.Add(downDot);
                downIndex--;
            }
            else
            {
                break;
            }
        }

        // Nếu có ít nhất 3 dot liền nhau theo cột thì đánh dấu
        if (verticalMatches.Count >= 3)
        {
            foreach (GameObject dot in verticalMatches)
            {
                dot.GetComponent<Dot>().isMatched = true;
            }
        }
    }

}
