namespace Pacman_Game;

public partial class Form1 : Form
{
    private bool _goup, _godown, _goleft, _goright;
    private bool _noup, _nodown, _noleft, _noright;
    private readonly List<PictureBox> _walls = [];
    private readonly List<PictureBox> _coins = [];
    private readonly int _speed = 12;
    private int _score;

    private Ghost _red, _yellow, _blue, _pink;
    private readonly List<Ghost> _ghosts = [];

    public Form1()
    {
        InitializeComponent();
        SetUp();
    }

    private void KeyIsDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Left && !_noleft)
        {
            _goright = _godown = _goup = false;
            _noright = _nodown = _noup = false;
            _goleft = true;
            pacman.Image = Properties.Resources.pacman_left;
        }

        if (e.KeyCode == Keys.Right && !_noright)
        {
            _goleft = _godown = _goup = false;
            _noleft = _nodown = _noup = false;
            _goright = true;
            pacman.Image = Properties.Resources.pacman_right;
        }

        if (e.KeyCode == Keys.Up && !_noup)
        {
            _goleft = _godown = _goright = false;
            _noleft = _nodown = _noright = false;
            _goup = true;
            pacman.Image = Properties.Resources.pacman_up;
        }

        if (e.KeyCode == Keys.Down && !_nodown)
        {
            _goleft = _goup = _goright = false;
            _noleft = _noup = _noright = false;
            _godown = true;
            pacman.Image = Properties.Resources.pacman_down;
        }
    }

    private void GameTimerEvent(object sender, EventArgs e)
    {
        PlayerMovements();

        foreach (var wall in _walls)
        {
            CheckBoundaries(pacman, wall);
        }

        foreach (var coin in _coins)
        {
            CollectingCoins(pacman, coin);
        }

        if (_score == _coins.Count)
        {
            GameOver("You Win, Score: " + _score);
        }

        _red.GhostMovement(pacman);
        _blue.GhostMovement(pacman);
        _yellow.GhostMovement(pacman);
        _pink.GhostMovement(pacman);

        foreach (var ghost in _ghosts)
        {
            GhostCollision(ghost, pacman, ghost._image);
        }
    }

    private void StartButtonClick(object sender, EventArgs e)
    {
        panelMenu.Enabled = false;
        panelMenu.Visible = false;

        _goleft = _goright = _goup = _godown = false;
        _noleft = _noright = _noup = _nodown = false;
        _score = 0;

        _red._image.Location = new Point(100, 100);
        _blue._image.Location = new Point(848, 597);
        _yellow._image.Location = new Point(132, 584);
        _pink._image.Location = new Point(877, 130);

        GameTimer.Start();
    }

    private void SetUp()
    {
        foreach (Control control in Controls)
        {
            if (control is PictureBox && ReferenceEquals(control.Tag, "wall"))
            {
                _walls.Add((PictureBox)control);
            }

            if (control is PictureBox && ReferenceEquals(control.Tag, "coin"))
            {
                _coins.Add((PictureBox)control);
            }
        }

        _red = new Ghost(this, Properties.Resources.red, 100, 100);
        _ghosts.Add(_red);
        _blue = new Ghost(this, Properties.Resources.blue, 848, 597);
        _ghosts.Add(_blue);
        _yellow = new Ghost(this, Properties.Resources.yellow, 132, 584);
        _ghosts.Add(_yellow);
        _pink = new Ghost(this, Properties.Resources.pink, 877, 130);
        _ghosts.Add(_pink);
    }

    private void PlayerMovements()
    {
        if (_goleft) pacman.Left -= _speed;
        if (_goright) pacman.Left += _speed;
        if (_goup) pacman.Top -= _speed;
        if (_godown) pacman.Top += _speed;

        if (pacman.Left < -30)
        {
            pacman.Left = ClientSize.Width - pacman.Width;
        }

        if (pacman.Left + pacman.Width > ClientSize.Width)
        {
            pacman.Left = -10;
        }

        if (pacman.Top < -30)
        {
            pacman.Top = ClientSize.Height - pacman.Height;
        }

        if (pacman.Top + pacman.Width > ClientSize.Height)
        {
            pacman.Top = -10;
        }
    }

    private void ShowCoins()
    {
        foreach (var coin in _coins)
        {
            coin.Visible = true;
        }
    }

    private void CheckBoundaries(PictureBox pacman, PictureBox wall)
    {
        if (pacman.Bounds.IntersectsWith(wall.Bounds))
        {
            if (_goleft)
            {
                _noleft = true;
                _goleft = false;
                pacman.Left = wall.Right + 2;
            }

            if (_goright)
            {
                _noright = true;
                _goright = false;
                pacman.Left = wall.Left - pacman.Width - 2;
            }

            if (_goup)
            {
                _noup = true;
                _goup = false;
                pacman.Top = wall.Bottom + 2;
            }

            if (_godown)
            {
                _nodown = true;
                _godown = false;
                pacman.Top = wall.Top - pacman.Height - 2;
            }
        }
    }

    private void CollectingCoins(PictureBox pacman, PictureBox coin)
    {
        if (pacman.Bounds.IntersectsWith(coin.Bounds))
        {
            if (coin.Visible)
            {
                coin.Visible = false;
                _score += 1;
            }
        }
    }

    private void GhostCollision(Ghost g, PictureBox pacman, PictureBox ghost)
    {
        if (!pacman.Bounds.IntersectsWith(ghost.Bounds)) return;
        GameOver("You Died, Score: " + _score);
        g.ChangeDirection();
    }

    private void GameOver(string message)
    {
        panelMenu.Visible = true;
        panelMenu.Enabled = true;
        GameTimer.Stop();
        ShowCoins();
        pacman.Location = new Point(461, 371);
        lblInfo.Text = message;
    }
}