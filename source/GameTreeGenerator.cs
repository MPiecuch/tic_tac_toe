using System.Collections.Generic;

public class GameTreeGenerator {
    private TicTacToeBoardState initialGameState;
    private TicTacToeGameTree gameTree = new TicTacToeGameTree();
    public GameTreeGenerator() {}

    public GameTreeGenerator(TicTacToeBoardState gameState) {
        initialGameState = gameState;
    }

    public TicTacToeGameTree UnrollTree() {
        int nodeIdCounter = 0;
        Queue<TicTacToeGameTreeNode> treeNodesQueue = new Queue<TicTacToeGameTreeNode>();
        TicTacToeGameTreeNode node = new(initialGameState, nodeIdCounter);
        treeNodesQueue.Enqueue(node);
        gameTree.parent.Add(nodeIdCounter++, null);
        gameTree.parentId.Add(-1);

        while (treeNodesQueue.Count > 0) {
            node = treeNodesQueue.Dequeue();
            gameTree.distanceSortedNodes.Add(node);
            gameTree.adjacencyGraph.Add(node.id, new List<TicTacToeGameTreeNode>());
            var gameRes = node.IsTerminalNode();
            if (gameRes != GameResult.UNKNOWN) {
                node.gameResult = gameRes;
                continue;
            }
            var neighbours = node.GetNeighbours();
            if (neighbours.Count == 0)
                node.gameResult = GameResult.DRAW;
            foreach (var neighbour in neighbours) {
                neighbour.id = nodeIdCounter++;
                neighbour.distance = node.distance + 1;
                gameTree.adjacencyGraph[node.id].Add(neighbour);
                gameTree.parent.Add(neighbour.id, node);
                treeNodesQueue.Enqueue(neighbour);
                gameTree.parentId.Add(node.id);
            }
        }
        gameTree.Finished();
        return gameTree;
    }
    public void AssignPositions() {
        for (int i = gameTree.distanceSortedNodes.Count - 1; i >= 0; i--) {
            var node = gameTree.distanceSortedNodes[i];
            if (node.gameResult == GameResult.UNKNOWN) {
                var children = gameTree.adjacencyGraph[node.id];
                bool done = false;
                if (node.GetBoardState().nextMove == TicTacToeMark.CIRCLE)
                    foreach (var child in children)
                        if (child.gameResult == GameResult.CIRCLE) {
                            node.gameResult = GameResult.CIRCLE;
                            done = true;
                            break;
                        }
                if (node.GetBoardState().nextMove == TicTacToeMark.CROSS)
                    foreach (var child in children)
                        if (child.gameResult == GameResult.CROSS) {
                            node.gameResult = GameResult.CROSS;
                            done = true;
                            break;
                        }
                if (!done) {
                    foreach (var child in children)
                        if (child.gameResult == GameResult.DRAW) {
                            node.gameResult = GameResult.DRAW;
                            done = true;
                            break;
                        }
                }
                if (!done) {
                    if (node.GetBoardState().nextMove == TicTacToeMark.CIRCLE)
                        node.gameResult = GameResult.CROSS;
                    else
                        node.gameResult = GameResult.CIRCLE;
                }
            }
        }
    }
}