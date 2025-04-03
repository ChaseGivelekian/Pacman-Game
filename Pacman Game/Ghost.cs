namespace Pacman_Game;

internal class Ghost
{
    private const int Speed = 8;
    private const int XSpeed = 4;
    private const int YSpeed = 4;
    private const int MaxHeight = 622;
    private const int MaxWidth = 900;
    private const int MinHeight = 96;
    private const int MinWidth = 85;
    private int _change;
    private readonly Random _random = new();
    private readonly string[] _directions = ["up", "down", "left", "right", "seek"];
    private string _direction = "left";
    public readonly PictureBox _image = new();

    public Ghost(Form game, Image img, int x, int y)
    {
        _image.Image = img;
        _image.SizeMode = PictureBoxSizeMode.StretchImage;
        _image.Width = 50;
        _image.Height = 50;
        _image.Left = x;
        _image.Top = y;

        game.Controls.Add(_image);
    }

    public void GhostMovement(PictureBox pacman)
    {
        if (_change > 0)
        {
            _change--;
        }
        else
        {
            _change = _random.Next(50, 80);
            _direction = _directions[_random.Next(_directions.Length)];
        }

        switch (_direction)
        {
            case "left":
                _image.Left -= Speed;
                break;
            case "right":
                _image.Left += Speed;
                break;
            case "up":
                _image.Top -= Speed;
                break;
            case "down":
                _image.Top += Speed;
                break;
            case "seek":
                if (_image.Left > pacman.Left)
                {
                    _image.Left -= XSpeed;
                }

                if (_image.Left < pacman.Left)
                {
                    _image.Left += XSpeed;
                }

                if (_image.Top > pacman.Top)
                {
                    _image.Top -= YSpeed;
                }

                if (_image.Top < pacman.Top)
                {
                    _image.Top += YSpeed;
                }

                break;
        }

        if (_image.Left < MinWidth)
        {
            _direction = "right";
        }

        if (_image.Left + _image.Width > MaxWidth)
        {
            _direction = "left";
        }

        if (_image.Top < MinHeight)
        {
            _direction = "down";
        }

        if (_image.Top + _image.Height > MaxHeight)
        {
            _direction = "up";
        }
    }

    public void ChangeDirection()
    {
        _direction = _directions[_random.Next(_directions.Length)];
    }
}