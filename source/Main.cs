using Godot;

public partial class Main : Node2D
{
    GameTreeView gameTreeView;
	public override void _Ready()
	{
        var globalVars = GetNode<GlobalVars>("/root/GlobalVars");
        GameTreeGenerator gameTreeGenerator = new GameTreeGenerator(globalVars.boardState);
        TicTacToeGameTree gameTree = gameTreeGenerator.UnrollTree();
        gameTreeGenerator.AssignPositions();
        gameTreeView = new GameTreeView(gameTree, 
                                        this,
                                        "res://resource/img/board.png",
                                        "res://resource/img/circle.png",
                                        "res://resource/img/cross.png",
                                        GetViewportRect());
        gameTreeView.Start();
	}

    public override void _Draw()
    {
        base._Draw();
        gameTreeView.Draw();
    }

    public override void _Process(double delta)
	{
        gameTreeView.Process(delta);
	}
}
