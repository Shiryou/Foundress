using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Foundress;

public class Camera
{
    private Vector3 _position;  // Where the camera is
    private Vector3 _target;    // What the camera is looking at
    private Vector3 _up;        // Which direction is up?
    private Matrix _view;       // Camera's position in space
    private Matrix _projection; // Camera's lens (turns 3D to 2D)


    private float _fieldOfView;
    private float _aspectRatio;
    private float _nearPlaneDistance;
    private float _farPlaneDistance;
    private float _moveSpeed = 0.5f;
    private float _rotationSpeed = 0.1f;
    private float _yaw = 0f;
    private float _pitch = 0f;

    public Camera(GraphicsDevice graphicsDevice)
    {
        _position = new Vector3(0, 0, 50);
        _target = Vector3.Zero;
        _up = Vector3.Up;
        _fieldOfView = MathHelper.PiOver4;
        _aspectRatio = graphicsDevice.Viewport.AspectRatio;
        _nearPlaneDistance = 0.1f;
        _farPlaneDistance = 1000f;
        UpdateMatrices();
    }

    public Matrix View => _view;
    public Matrix Projection => _projection;
    public Vector3 Position
    {
        get => _position;
        set => _position = value;
    }
    public Vector3 Target
    {
        get => _target;
        set => _target = value;
    }

    private void UpdateMatrices()
    {
        // Calculate the target based on the camera's position and rotation
        Vector3 direction = Vector3.Transform(Vector3.Forward, 
            Matrix.CreateRotationY(_yaw) * Matrix.CreateRotationX(_pitch));
        _target = _position + direction;

        _view = Matrix.CreateLookAt(_position, _target, _up);
        _projection = Matrix.CreatePerspectiveFieldOfView(
            _fieldOfView,
            _aspectRatio,
            _nearPlaneDistance,
            _farPlaneDistance);
    }

    public void Update(GameTime gameTime)
    {
        var keyboard = Keyboard.GetState();
        var mouse = Mouse.GetState();

        // Handle keyboard input for camera movement
        if (keyboard.IsKeyDown(Keys.W))
            _position += Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(_yaw)) * _moveSpeed;
        if (keyboard.IsKeyDown(Keys.S))
            _position += Vector3.Transform(Vector3.Backward, Matrix.CreateRotationY(_yaw)) * _moveSpeed;
        if (keyboard.IsKeyDown(Keys.A))
            _position += Vector3.Transform(Vector3.Left, Matrix.CreateRotationY(_yaw)) * _moveSpeed;
        if (keyboard.IsKeyDown(Keys.D))
            _position += Vector3.Transform(Vector3.Right, Matrix.CreateRotationY(_yaw)) * _moveSpeed;
        if (keyboard.IsKeyDown(Keys.Q))
            _yaw -= _rotationSpeed;
        if (keyboard.IsKeyDown(Keys.E))
            _yaw += _rotationSpeed;

        // Clamp pitch to prevent camera flipping
        _pitch = MathHelper.Clamp(_pitch, -MathHelper.PiOver2 + 0.1f, MathHelper.PiOver2 - 0.1f);

        UpdateMatrices();
    }
} 
