using UnityEngine;
using System.Collections;

public class BotPlayer : MonoBehaviour
{
    public Board board;
    [SerializeField] private bool isBotTurn = false;
    [SerializeField] private bool isPlaying = false;

    void Start()
    {
        if (board == null)
        {
            board = FindObjectOfType<Board>();
        }
        StartBot();
    }

    public void StartBot()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            Debug.Log("Bot started. isPlaying: " + isPlaying);
            StartCoroutine(WaitForBotTurn());
        }
    }

    public void StopBot()
    {
        isPlaying = false;
        StopAllCoroutines();
    }

    private IEnumerator WaitForBotTurn()
    {
        while (isPlaying)
        {
            yield return new WaitUntil(() => !board.IsRefilling && CombatManager.Instance.IsBotTurn);

            Debug.Log("Bot turn started.");
            isBotTurn = true;

            yield return new WaitForSeconds(1f);

            yield return StartCoroutine(PlayBotTurn());
            isBotTurn = false;

            CombatManager.Instance.EndBotTurn();
        }
    }

    private IEnumerator PlayBotTurn()
    {
        (int col1, int row1, int col2, int row2) move = FindValidMove();
        if (move.col1 != -1)
        {
            Debug.Log($"Bot move: ({move.col1}, {move.row1}) -> ({move.col2}, {move.row2})");
            yield return StartCoroutine(SwapDots(move.col1, move.row1, move.col2, move.row2));
            
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            Debug.Log("Bot không tìm thấy nước đi hợp lệ!");
            yield return new WaitForSeconds(0.5f);
            CombatManager.Instance.EndBotTurn();
        }
    }

    private (int col1, int row1, int col2, int row2) FindValidMove()
    {
        GameObject[,] allDots = board.allDots;

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                if (allDots[i, j] == null) continue;

                Dot dot1 = allDots[i, j].GetComponent<Dot>();
                if (dot1 == null) continue;

                if (i < board.width - 1 && allDots[i + 1, j] != null)
                {
                    Dot dot2 = allDots[i + 1, j].GetComponent<Dot>();
                    if (dot2 != null && WillCreateMatchAfterSwap(dot1, dot2, i, j, i + 1, j))
                    {
                        return (i, j, i + 1, j);
                    }
                }

                if (j < board.height - 1 && allDots[i, j + 1] != null)
                {
                    Dot dot2 = allDots[i, j + 1].GetComponent<Dot>();
                    if (dot2 != null && WillCreateMatchAfterSwap(dot1, dot2, i, j, i, j + 1))
                    {
                        return (i, j, i, j + 1);
                    }
                }
            }
        }

        return (-1, -1, -1, -1);
    }

    private bool WillCreateMatchAfterSwap(Dot dot1, Dot dot2, int col1, int row1, int col2, int row2)
    {
        GameObject[,] allDots = board.allDots;

        int dot1PrevCol = dot1.col;
        int dot1PrevRow = dot1.row;
        int dot2PrevCol = dot2.col;
        int dot2PrevRow = dot2.row;

        allDots[col1, row1] = dot2.gameObject;
        allDots[col2, row2] = dot1.gameObject;

        dot1.col = col2;
        dot1.row = row2;
        dot2.col = col1;
        dot2.row = row1;

        dot1.FindMatches();
        dot2.FindMatches();

        bool matchFound = dot1.isMatched || dot2.isMatched;

        dot1.isMatched = false;
        dot2.isMatched = false;

        allDots[col1, row1] = dot1.gameObject;
        allDots[col2, row2] = dot2.gameObject;

        dot1.col = dot1PrevCol;
        dot1.row = dot1PrevRow;
        dot2.col = dot2PrevCol;
        dot2.row = dot2PrevRow;

        return matchFound;
    }

    private IEnumerator SwapDots(int col1, int row1, int col2, int row2)
    {
        GameObject dot1Obj = board.allDots[col1, row1];
        GameObject dot2Obj = board.allDots[col2, row2];

        Dot dot1 = dot1Obj.GetComponent<Dot>();
        Dot dot2 = dot2Obj.GetComponent<Dot>();

        board.allDots[col1, row1] = dot2Obj;
        board.allDots[col2, row2] = dot1Obj;

        dot1.previousCol = dot1.col;
        dot1.previousRow = dot1.row;
        dot2.previousCol = dot2.col;
        dot2.previousRow = dot2.row;

        dot1.col = col2;
        dot1.row = row2;
        dot2.col = col1;
        dot2.row = row1;

        // Log để kiểm tra giá trị col và row
        Debug.Log($"Bot swapping: Dot1 ({dot1Obj.name}) to (col: {col2}, row: {row2})");
        Debug.Log($"Bot swapping: Dot2 ({dot2Obj.name}) to (col: {col1}, row: {row1})");

        StartCoroutine(board.MovePiece(dot1Obj, new Vector2(col2, row2)));
        StartCoroutine(board.MovePiece(dot2Obj, new Vector2(col1, row1)));

        yield return new WaitUntil(() => !board.IsMovingDots);

        dot1.FindMatches();
        dot2.FindMatches();

        if (dot1.isMatched || dot2.isMatched)
        {
            board.DestroyMatches();
        }
        else
        {
            board.allDots[col1, row1] = dot1Obj;
            board.allDots[col2, row2] = dot2Obj;

            dot1.col = dot1.previousCol;
            dot1.row = dot1.previousRow;
            dot2.col = dot2.previousCol;
            dot2.row = dot2.previousRow;

            // Log để kiểm tra giá trị khi hoàn tác
            Debug.Log($"Bot reverting: Dot1 ({dot1Obj.name}) to (col: {col1}, row: {row1})");
            Debug.Log($"Bot reverting: Dot2 ({dot2Obj.name}) to (col: {col2}, row: {row2})");

            StartCoroutine(board.MovePiece(dot1Obj, new Vector2(col1, row1)));
            StartCoroutine(board.MovePiece(dot2Obj, new Vector2(col2, row2)));

            yield return new WaitUntil(() => !board.IsMovingDots);
            board.DestroyMatches();
        }
    }
}