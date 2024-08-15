using System;
using System.Collections.Generic;
using Godot;

public enum DrawingState {
    TOP_DOWN,
    BOTTOM_UP,
    IDLE,
    END
}

public struct Line {
    public Vector2 from, to;
    public Color c;
    public Line(Vector2 from, Vector2 to, Color c) {
        this.from = from;
        this.to = to;
        this.c = c;
    }
}

public class GameTreeView {
    private TicTacToeGameTree gameTree;
    private Node2D drawingNode;
    private double timeCounter, timePerTreeNode;
    private int gameTreeNodeCounter, 
                numGameTreeNodes,
                boardSizePx,
                minXMargin,
                minYMargin,
                treeWidth,
                treeHeight;
    private DrawingState drawingState;
    private Texture2D board, circle, cross;
    private Rect2 viewport;
    private List<TreeNodeView> nodeViews = new List<TreeNodeView>();
    private List<Line> lines = new List<Line>();
    public GameTreeView(TicTacToeGameTree gameTree, 
                        Node2D drawingNode,
                        string boardImgPath,
                        string circleImgPath,
                        string crossImgPath,
                        Rect2 viewport,
                        double timePerTreeNode=0.5,
                        int minXMargin=2,
                        int minYMargin=2) {
        this.gameTree = gameTree;
        treeWidth = gameTree.GetTreeWidth();
        treeHeight = gameTree.GetTreeHeight();
        this.drawingNode = drawingNode;
        this.timePerTreeNode = timePerTreeNode;
        numGameTreeNodes = gameTree.GetNodesNum();
        board = GD.Load<Texture2D>(boardImgPath);
        circle = GD.Load<Texture2D>(circleImgPath);
        cross = GD.Load<Texture2D>(crossImgPath);
        this.viewport = viewport;
        this.minXMargin = minXMargin;
        this.minYMargin = minYMargin;
        Reset();
        ComputeBoardSize();
    }
    private void ComputeBoardSize() {
        int maxBoardWidth = (int)(viewport.Size.X / treeWidth);
        maxBoardWidth -= minXMargin;
        int maxBoardHeight = (int)(viewport.Size.Y / treeHeight);
        maxBoardHeight -= minYMargin;
        boardSizePx = Math.Min(maxBoardWidth, maxBoardHeight);
    }
    private void Reset() {
        timeCounter = 0;
        gameTreeNodeCounter = 0;
        drawingState = DrawingState.IDLE;
        gameTreeNodeCounter = 0;
    }
    private void DrawFromRootToLeaves(double deltaTime) {
        timeCounter += deltaTime;
        if (timeCounter >= timePerTreeNode) {
            DrawTreeNode();
        }
    }
    private void DrawPositionInfo(double deltaTime) {
        timeCounter += deltaTime;
        if (timeCounter >= timePerTreeNode) {
            DrawNodePosInfo();
        }
    }
    private void DrawNodePosInfo() {
        if (gameTreeNodeCounter < 0) {
            drawingState = DrawingState.END;
            return;
        }
        var node = gameTree.distanceSortedNodes[gameTreeNodeCounter];
        var nodeView = nodeViews[gameTreeNodeCounter];
        Vector2 labelPos = nodeView.boardCenter;
        labelPos.X -= nodeView.boardSizePx / 2;
        labelPos.Y -= nodeView.boardSizePx / 2 + 2 * minYMargin;
        labelPos.Y = Math.Max(0, labelPos.Y);
        Theme t = new Theme();
        t.SetColor("font_color", "Label", Colors.Black);
        string txt = "";
        switch (node.gameResult) {
            case GameResult.CIRCLE:
                txt = "O";
                break;
            case GameResult.CROSS:
                txt = "X";
                break;
            case GameResult.DRAW:
                txt = "=";
                break;
        }
        Label label = new Label
        {
            Text = txt,
            Position = labelPos,
            Theme = t,
        };
        drawingNode.AddChild(label);
        timeCounter = 0;
        gameTreeNodeCounter -= 1;
    }
    private void DrawTreeNode() {
        if (gameTreeNodeCounter >= numGameTreeNodes) {
            drawingState = DrawingState.BOTTOM_UP;
            gameTreeNodeCounter = numGameTreeNodes - 1;
            timeCounter = 0;
            return;
        }
        DrawBoard();
        timeCounter = 0;
        gameTreeNodeCounter += 1;
    }
    private void DrawBoard() {
        TicTacToeGameTreeNode node = gameTree.distanceSortedNodes[gameTreeNodeCounter];
        TreeNodeView nodeView = new TreeNodeView(node,
                                                 gameTree,
                                                 viewport,
                                                 boardSizePx,
                                                 board,
                                                 circle,
                                                 cross);
        nodeView.Draw(drawingNode);
        nodeViews.Add(nodeView);
        AddLine();
        drawingNode.QueueRedraw();
    }
    private void AddLine() {
        int parentNodeId = gameTree.parentId[gameTreeNodeCounter];
        if (parentNodeId != -1) {
            TreeNodeView parentNodeView = nodeViews[parentNodeId];
            TreeNodeView currNodeView = nodeViews[gameTreeNodeCounter];
            TicTacToeGameTreeNode parentNode = gameTree.parent[gameTreeNodeCounter];
            Color col = Colors.Blue;
            if (parentNode.GetBoardState().nextMove == TicTacToeMark.CROSS)
                col = Colors.Orange;
            Line line = new Line {
                from = parentNodeView.boardCenter,
                to = currNodeView.boardCenter,
                c = col
            };
            line.from.Y += currNodeView.boardSizePx / 2;
            line.to.Y -= currNodeView.boardSizePx / 2;
            lines.Add(line);
        }
    }
    public void Start() {
        drawingState = DrawingState.TOP_DOWN;
    }
    public void Draw() {
        foreach (var l in lines) {
            drawingNode.DrawLine(l.from, l.to, l.c);
        }
    }
    public void Process(double deltaTime) {
        switch (drawingState) {
            case DrawingState.TOP_DOWN:
                DrawFromRootToLeaves(deltaTime);
                break;
            case DrawingState.BOTTOM_UP:
                DrawPositionInfo(deltaTime);
                break;
        }
    }
}