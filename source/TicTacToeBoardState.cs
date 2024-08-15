public class TicTacToeBoardState {
    public TicTacToeMark[,] board = new TicTacToeMark[3, 3];
    public TicTacToeMark nextMove;
    public TicTacToeBoardState() {
        for (int row = 0; row < 3; row++)
            for (int col = 0; col < 3; col++)
                board[row, col] = TicTacToeMark.EMPTY;
        nextMove = TicTacToeMark.CIRCLE;
    }
    public TicTacToeBoardState(string board, TicTacToeMark nextMove) {
        this.board = ConvertStrToBoardArr(board);
        this.nextMove = nextMove;
    }
    public TicTacToeBoardState(TicTacToeBoardState board) {
        for (int row = 0; row < 3; row++)
            for (int col = 0; col < 3; col++) {
                this.board[row, col] = board.GetBoard()[row, col];
            }
        this.nextMove = board.nextMove;
    }
    public TicTacToeMark[,] GetBoard() {
        return board;
    }
    public void NextPlayer() {
        if (nextMove == TicTacToeMark.CIRCLE) {
            nextMove = TicTacToeMark.CROSS;
        }
        else if (nextMove == TicTacToeMark.CROSS) {
            nextMove = TicTacToeMark.CIRCLE;
        }
    }
    private TicTacToeMark[, ] ConvertStrToBoardArr(string board) {
        TicTacToeMark[,] boardArr = new TicTacToeMark[3, 3];
        for (int row = 0; row < 3; row++)
            for (int col = 0; col < 3; col++) {
                TicTacToeMark mark = TicTacToeMark.EMPTY;
                switch (board[row * 3 + col]) {
                    case 'o':
                        mark = TicTacToeMark.CIRCLE;
                        break;
                    case 'x':
                        mark = TicTacToeMark.CROSS;
                        break;
                }
                boardArr[row, col] = mark;
        }
        return boardArr;
    }
}