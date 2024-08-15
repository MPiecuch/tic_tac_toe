using System.Collections.Generic;
using System.Linq;

public class TicTacToeGameTree {
    public Dictionary<int, List<TicTacToeGameTreeNode>> adjacencyGraph = new Dictionary<int,List<TicTacToeGameTreeNode>>();
    public Dictionary<int, TicTacToeGameTreeNode> parent = new Dictionary<int, TicTacToeGameTreeNode>();
    public List<TicTacToeGameTreeNode> distanceSortedNodes = new List<TicTacToeGameTreeNode>();
    public List<int> parentId = new List<int>();
    private int[] numNodesAtDist;
    private int treeWidth, treeHeight, numNodes;
    public int GetNodesNum() {
        return parent.Count;
    }
    public void Finished() {
        ComputeTreeParams();
    }
    public int GetTreeHeight() {
        return treeHeight;
    }
    public int GetTreeWidth() {
        return treeWidth;
    }
    public int NumNodesAtDist(int dist) {
        return numNodesAtDist[dist];
    }
    public void ComputeTreeParams() {
        numNodes = distanceSortedNodes.Count;
        treeHeight = distanceSortedNodes[numNodes - 1].distance + 1;
        numNodesAtDist = new int[treeHeight + 1];
        
        foreach (var node in distanceSortedNodes)
            numNodesAtDist[node.distance]++;
        treeWidth = numNodesAtDist.Max();
    }
}