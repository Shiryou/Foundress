using Foundress.Entities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Foundress;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private Preferences _preferences;
    private Camera _camera;

    public Game1(Preferences prefs = null)
    {
        if (prefs == null)
        {
            prefs = new Preferences();
        }
        _preferences = prefs;
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        this.Window.Title = "Foundress";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();

        ChangeResolution();
        _camera = new Camera(GraphicsDevice);
    }

    protected override void LoadContent()
    {
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _camera.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }

    public void ChangeResolution(int width = 0, int height = 0, bool? windowed = null )
    {
        if( width <= 0 || height <= 0 )
        {
            width = _preferences.Resolution.Width;
            height = _preferences.Resolution.Height;
        }
        if (windowed == null)
        {
            windowed = !_preferences.IsFullScreen;
        }
        _graphics.PreferredBackBufferWidth = width;
        _graphics.PreferredBackBufferHeight = height;
        _graphics.IsFullScreen = !(bool)windowed;
        _graphics.ApplyChanges();
    }
}
