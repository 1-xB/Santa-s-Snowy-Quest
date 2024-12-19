using Godot;

public partial class Box : CharacterBody2D
{
    [Export]
    public float PushForce = 1000f;
    private bool _isPushed = false;
    private float _pushDirection = 0;
    private float _gravity = 400f;
    
    
    Vector2 _velocity = Vector2.Zero;

    public override void _PhysicsProcess(double delta)
    {
        if (!IsOnFloor())
        {
            _velocity.Y += _gravity * (float)delta;
        }
        if (_isPushed)
        {
            ApplyPush(delta);
        }
        
        Velocity = _velocity;
        MoveAndSlide();
        base._PhysicsProcess(delta);
    }
    
    private void ApplyPush(double delta)
    {
        if (_pushDirection == 0)
        {
            _isPushed = false;
            _velocity.X = 0;
            return;
        }
        
        _velocity.X = PushForce * _pushDirection * (float)delta;
    }
    
    private void OnLeftPushArea2DBodyEntered(Node body)
    {
        if (body is Santa)
        {
            Santa santa = body as Santa;
            santa.EnterBox();
            _isPushed = true;
            _pushDirection = 1;
        }
    }
    
    private void OnRightPushArea2DBodyEntered(Node body)
    {
        if (body is Santa)
        {
            Santa santa = body as Santa;
            santa.EnterBox();
            _isPushed = true;
            _pushDirection = -1;
        }
    }
    
    private void OnLeftPushArea2DBodyExited(Node body)
    {
        if (body is Santa)
        {
            Santa santa = body as Santa;
            santa.ExitBox();
            _isPushed = false;
            _pushDirection = 0;
            _velocity.X = 0; // Reset velocity
        }
    }
    
    private void OnRightPushArea2DBodyExited(Node body)
    {
        if (body is Santa)
        {
            Santa santa = body as Santa;
            santa.ExitBox();
            _isPushed = false;
            _pushDirection = 0;
            _velocity.X = 0; // Reset velocity
        }
    }
}