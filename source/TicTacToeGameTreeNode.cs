using System.Collections.Generic;
using System.Runtime.ExceptionServices;

public enum GameResult {
    UNKNOWN,
    CIRCLE,
    CROSS,
    DRAW
}

public class TicTacToeGameTreeNode {
    private TicTacToeBoardState boardState;
    public GameResult gameResult;
    public int distance;
    public int id;
    public TicTacToeGameTreeNode(TicTacToeBoardState board, int id=-1) {
        boardState = new TicTacToeBoardState(board);
        this.id = id;
    }
    public TicTacToeGameTreeNode(TicTacToeGameTreeNode node) {
        boardState = new TicTacToeBoardState(node.GetBoardState());
    }
    public List<TicTacToeGameTreeNode> GetNeighbours() {
        List<TicTacToeGameTreeNode> neighbours = new List<TicTacToeGameTreeNode>();
        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 3; c++) {
                if (boardState.GetBoard()[r, c] == TicTacToeMark.EMPTY) {
                    TicTacToeGameTreeNode neighbour = new TicTacToeGameTreeNode(boardState);
                    neighbour.GetBoardState().GetBoard()[r, c] = boardState.nextMove;
                    neighbour.GetBoardState().NextPlayer();
                    neighbours.Add(neighbour);
                }
            }
        return neighbours;
    }
    public TicTacToeBoardState GetBoardState() {
        return boardState;
    }
    public GameResult IsTerminalNode() {
        bool isTerminal;
        GameResult gameResult = GameResult.CIRCLE;
        foreach (var mark in new TicTacToeMark[]{TicTacToeMark.CIRCLE, TicTacToeMark.CROSS}) {
            for (int r=0; r < 3; r++) {
                isTerminal = true;
                for (int c=0; c < 3; c++)
                    if (boardState.GetBoard()[r, c] != mark) {
                        isTerminal = false;
                        break;
                    }
                if (isTerminal)
                    return gameResult;
            }
            for (int c=0; c < 3; c++) {
                isTerminal = true;
                for (int r=0; r < 3; r++)
                    if (boardState.GetBoard()[r, c] != mark) {
                        isTerminal = false;
                        break;
                    }
                if (isTerminal)
                    return gameResult;
            }
            isTerminal = true;
            for (int i=0; i < 3; i++)
                if (boardState.GetBoard()[i, i] != mark) {
                    isTerminal = false;
                    break;
                }
            if (isTerminal)
                return gameResult;
            isTerminal = true;
            for (int i=0; i < 3; i++)
                if (boardState.GetBoard()[i, 2 - i] != mark) {
                    isTerminal = false;
                    break;
                }
            if (isTerminal)
                return gameResult;
            gameResult = GameResult.CROSS;
        }
        return GameResult.UNKNOWN;
    }
    public int Hash() {
        int h = 0;
        int mul = 1;
        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 3; c++) {
                int val = 0;
                if (boardState.GetBoard()[r, c] == TicTacToeMark.CIRCLE)
                    val = 1;
                if (boardState.GetBoard()[r, c] == TicTacToeMark.CROSS)
                    val = 2;
                h += val * mul;
                mul *= 3;
            }
        return h;
    }
}