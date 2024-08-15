public class GameState {
    public TicTacToeMark[, ] state = new TicTacToeMark[3, 3];
    public GameState() {}
    public GameState(string board) {
        for (int row = 0; row < 3; row++)
            for (int col = 0; col < 3; col++) {
                TicTacToeMark f = TicTacToeMark.EMPTY;
                switch (board[row * 3 + col]) {
                    case 'o':
                        f = TicTacToeMark.CIRCLE;
                        break;
                    case 'x':
                        f = TicTacToeMark.CROSS;
                        break;
                }
                state[row, col] = f;
        }
    }
    public GameState(GameState s) {
        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 3; c++) {
                this.state[r, c] = s.state[r, c];
            }
    }
}