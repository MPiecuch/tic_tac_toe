using Godot;

public struct TreeCoord {
    public int row;
    public int col;
}
public class TreeNodeView {
    private TicTacToeGameTree tree;
    private TicTacToeGameTreeNode treeNode;
    private TreeCoord treeCoord;
    private Texture2D board, circle, cross;
    private Rect2 viewport;
    public int boardSizePx;
    private float scale;
    public Vector2 boardCenter;
    public TreeNodeView(TicTacToeGameTreeNode node,
                        TicTacToeGameTree tree,
                        Rect2 viewport,
                        int boardSizePx,
                        Texture2D board,
                        Texture2D circle,
                        Texture2D cross) {
        int col = 0;
        foreach (var n in tree.distanceSortedNodes)
            if (n == node)
                break;
            else if (n.distance == node.distance)
                col++;

        treeCoord = new TreeCoord {
            row = node.distance,
            col = col
        };
        scale = (float)boardSizePx / board.GetWidth();
        this.tree = tree;
        treeNode = node;
        this.viewport = viewport;
        this.boardSizePx = boardSizePx;
        this.board = board;
        this.circle = circle;
        this.cross = cross;
    }
    public void Draw(Node2D parentNode) {
        boardCenter = ComputeCenter();
        DrawBoard(boardCenter, parentNode);
    }
    private void DrawBoard(Vector2 boardCenter,
                           Node2D drawingNode) {
        Sprite2D newBoard = new Sprite2D {
            Texture = board,
            Scale = new Vector2(scale, scale),
            Position = boardCenter
        };
        drawingNode.AddChild(newBoard);

        float offset = newBoard.Texture.GetWidth() / 3 * newBoard.Scale.X;
        Vector2 leftUpper = boardCenter - newBoard.Texture.GetSize() * newBoard.Scale.X / 3;
        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 3; c++) {
                Sprite2D elem = new Sprite2D();
                if (treeNode.GetBoardState().GetBoard()[r, c] == TicTacToeMark.CIRCLE) {
                    elem.Texture = circle;
                }
                if (treeNode.GetBoardState().GetBoard()[r, c] == TicTacToeMark.CROSS) {
                    elem.Texture = cross;
                }
                elem.Position = leftUpper + new Vector2(c * offset, r * offset);
                elem.Scale = newBoard.Scale;
                drawingNode.AddChild(elem);
            }
    }
    private Vector2 ComputeCenter() {
        int freeXSpace = (int)viewport.Size.X - tree.NumNodesAtDist(treeCoord.row) * boardSizePx;
        int xSpaceBetweenNodes = freeXSpace / (tree.NumNodesAtDist(treeCoord.row) + 1);
        int ySpaceBetweenNodes = (int)viewport.Size.Y - tree.GetTreeHeight() * boardSizePx;
        ySpaceBetweenNodes /= tree.GetTreeHeight() + 1;
        int x = (int)(0.5 * boardSizePx) + (treeCoord.col + 1) * xSpaceBetweenNodes + treeCoord.col * boardSizePx;
        int y = (int)(0.5 * boardSizePx) + (treeCoord.row + 1) * ySpaceBetweenNodes + treeCoord.row * boardSizePx;

        return new Vector2(x, y);
    }
}