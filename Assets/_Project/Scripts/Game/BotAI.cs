using System.Collections.Generic;
using UnityEngine;

public static class BotAI
{
    public static (int row, int col) GetMove(int[,] board, int botPlayer, int difficulty)
    {
        List<(int row, int col)> emptyCells = GetEmptyCells(board);
        if (emptyCells.Count == 0) return (-1, -1);

        switch (difficulty)
        {
            case 1:
                return GetRandomMove(emptyCells);

            case 2:
                if (Random.value < 0.5f)
                {
                    var smartMove = GetSmartMove(board, botPlayer);
                    if (smartMove.hasValue) return (smartMove.row, smartMove.col);
                }
                return GetRandomMove(emptyCells);

            case 3:
            default:
                if (Random.value < 0.333f)
                {
                    return GetRandomMove(emptyCells);
                }
                return GetBestMoveMinimax(board, botPlayer);
        }
    }

    private static (int row, int col) GetRandomMove(List<(int row, int col)> emptyCells)
    {
        int index = Random.Range(0, emptyCells.Count);
        return emptyCells[index];
    }

    private static (bool hasValue, int row, int col) GetSmartMove(int[,] board, int botPlayer)
    {
        int humanPlayer = (botPlayer == 1) ? 2 : 1;
        List<(int row, int col)> emptyCells = GetEmptyCells(board);

        foreach (var cell in emptyCells)
        {
            board[cell.row, cell.col] = botPlayer;
            if (CheckWin(board, botPlayer))
            {
                board[cell.row, cell.col] = 0;
                return (true, cell.row, cell.col);
            }
            board[cell.row, cell.col] = 0;
        }

        foreach (var cell in emptyCells)
        {
            board[cell.row, cell.col] = humanPlayer;
            if (CheckWin(board, humanPlayer))
            {
                board[cell.row, cell.col] = 0;
                return (true, cell.row, cell.col);
            }
            board[cell.row, cell.col] = 0;
        }

        if (board[1, 1] == 0)
        {
            return (true, 1, 1);
        }

        return (false, -1, -1);
    }

    private static (int row, int col) GetBestMoveMinimax(int[,] board, int botPlayer)
    {
        int bestScore = int.MinValue;
        List<(int row, int col)> bestMoves = new List<(int row, int col)>();
        List<(int row, int col)> emptyCells = GetEmptyCells(board);

        foreach (var cell in emptyCells)
        {
            board[cell.row, cell.col] = botPlayer;
            int score = Minimax(board, 0, false, botPlayer);
            board[cell.row, cell.col] = 0;

            if (score > bestScore)
            {
                bestScore = score;
                bestMoves.Clear();
                bestMoves.Add(cell);
            }
            else if (score == bestScore)
            {
                bestMoves.Add(cell);
            }
        }

        if (bestMoves.Count > 0)
        {
            int index = Random.Range(0, bestMoves.Count);
            return bestMoves[index];
        }

        return (-1, -1);
    }

    private static int Minimax(int[,] board, int depth, bool isMaximizing, int botPlayer)
    {
        int humanPlayer = (botPlayer == 1) ? 2 : 1;

        if (CheckWin(board, botPlayer)) return 10 - depth;
        if (CheckWin(board, humanPlayer)) return depth - 10;
        if (GetEmptyCells(board).Count == 0) return 0;

        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            foreach (var cell in GetEmptyCells(board))
            {
                board[cell.row, cell.col] = botPlayer;
                int score = Minimax(board, depth + 1, false, botPlayer);
                board[cell.row, cell.col] = 0;
                bestScore = Mathf.Max(score, bestScore);
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            foreach (var cell in GetEmptyCells(board))
            {
                board[cell.row, cell.col] = humanPlayer;
                int score = Minimax(board, depth + 1, true, botPlayer);
                board[cell.row, cell.col] = 0;
                bestScore = Mathf.Min(score, bestScore);
            }
            return bestScore;
        }
    }

    private static List<(int row, int col)> GetEmptyCells(int[,] board)
    {
        List<(int row, int col)> list = new List<(int row, int col)>();
        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                if (board[r, c] == 0)
                {
                    list.Add((r, c));
                }
            }
        }
        return list;
    }

    private static bool CheckWin(int[,] board, int player)
    {
        for (int r = 0; r < 3; r++)
        {
            if (board[r, 0] == player && board[r, 1] == player && board[r, 2] == player) return true;
        }
        for (int c = 0; c < 3; c++)
        {
            if (board[0, c] == player && board[1, c] == player && board[2, c] == player) return true;
        }
        if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player) return true;
        if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player) return true;

        return false;
    }
}
