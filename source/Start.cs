using Godot;

public partial class Start : Node2D
{
    Sprite2D board;
    TextureButton circleBtn, crossBtn;
    Texture2D circle, cross, circleSel, crossSel;
    TicTacToeMark active = TicTacToeMark.CIRCLE;
    GlobalVars initialState;
    Sprite2D[,] spriteState = new Sprite2D[3, 3];
    int boardSideLenPx;
    Vector2 boardCenter;
    Vector2 boardUpperLeftCorner;

    public override void _Ready()
    {
        board = GetNode<Sprite2D>("Board");
        circleBtn = GetNode<TextureButton>("CircleBtn");
        circleBtn.Pressed += OnCirclePressed;
        crossBtn = GetNode<TextureButton>("CrossBtn");
        crossBtn.Pressed += OnCrossPressed;
        var startBtn = GetNode<Button>("StartBtn");
        startBtn.Pressed += OnStartBtnPressed;

        circle = GD.Load<Texture2D>("res://resource/img/circle.png");
        cross = GD.Load<Texture2D>("res://resource/img/cross.png");
        circleSel = GD.Load<Texture2D>("res://resource/img/circle_selected.png");
        crossSel = GD.Load<Texture2D>("res://resource/img/cross_selected.png");

        initialState = GetNode<GlobalVars>("/root/GlobalVars");

        boardSideLenPx = (int)board.GetRect().Size.X;
        boardCenter = board.Position;
        boardUpperLeftCorner = new Vector2(
            boardCenter.X - boardSideLenPx / 2,
            boardCenter.Y - boardSideLenPx / 2
        );
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event is InputEventMouseButton eventMouseButton)
        {
            (int row, int col) = GetRowColOnBoardClicked(eventMouseButton);
            UpdateBoardView(row, col);
        }
    }

    (int, int) GetRowColOnBoardClicked(InputEventMouseButton click)
    {
        float row = (click.Position.Y - boardUpperLeftCorner.Y) / (boardSideLenPx / 3);
        if (row < 0 || row > 3)
            row = -1;
        float col = (click.Position.X - boardUpperLeftCorner.X) / (boardSideLenPx / 3);
        if (col < 0 || col > 3)
            col = -1;

        return ((int)row, (int)col);
    }

    void UpdateBoardView(int row, int col)
    {
        if (row == -1 || col == -1)
            return;

        initialState.boardState.board[row, col] = active;
        if (initialState.boardState.board[row, col] != TicTacToeMark.EMPTY)
            RemoveChild(spriteState[row, col]);

        Texture2D t = circle;
        if (active == TicTacToeMark.CROSS)
            t = cross;
        Sprite2D c = new Sprite2D
        {
            Texture = t,
            Position = new Vector2(boardUpperLeftCorner.X + col * 0.33f * boardSideLenPx,
                                   boardUpperLeftCorner.Y + row * 0.33f * boardSideLenPx),
            Centered = false
        };
        spriteState[row, col] = c;
        AddChild(c);
    }

    private void OnCirclePressed() 
    {
        active = TicTacToeMark.CIRCLE;
        circleBtn.TextureNormal = circleSel;
        crossBtn.TextureNormal = cross;
    }

    private void OnCrossPressed() 
    {
        active = TicTacToeMark.CROSS;
        crossBtn.TextureNormal = crossSel;
        circleBtn.TextureNormal = circle;
    }

    private void OnStartBtnPressed() 
    {
        initialState.boardState.nextMove = active;
        GetTree().ChangeSceneToFile("res://resource/scn/main.tscn");
    }
}
