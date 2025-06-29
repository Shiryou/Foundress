using System;
using Foundress.Supports;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Foundress;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private Preferences _preferences;
    private Camera _camera;
    private Console _console;
    private SpriteBatch _spriteBatch;

    private KeyboardState _previousKeyboard;

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

        _console = null;
        _spriteBatch = null;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();

        ChangeResolution();

        _camera = new Camera(GraphicsDevice);

        _spriteBatch = new SpriteBatch(GraphicsDevice);
        Logger.Debug($"SpriteBatch initialized: {_spriteBatch != null}");

        // Initialize keyboard state tracking
        _previousKeyboard = Keyboard.GetState();

        // Enable depth buffer
        GraphicsDevice.DepthStencilState = DepthStencilState.Default;
    }

    protected override void LoadContent()
    {
        // TODO: use this.Content to load your game content here
        
        // Load a font for the console
        SpriteFont consoleFont = null;
        SpriteFont consoleBoldFont = null;
        try
        {
            consoleFont = Content.Load<SpriteFont>("ConsoleFont");
            consoleBoldFont = Content.Load<SpriteFont>("ConsoleFontBold");
            Logger.Info("Console font loaded successfully");
        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to load console font: {ex.Message}");
        }

        // Initialize console with the loaded font
        try
        {
            _console = new Console(GraphicsDevice, consoleFont, () => this.Exit(), consoleBoldFont);
            Logger.Info($"Console initialized with font: {consoleFont != null}");
        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to initialize console: {ex.Message}");
            _console = null;
        }
    }

    protected override void Update(GameTime gameTime)
    {
        Logger.Verbose("Game1.Update called");
        // Get keyboard states for console
        KeyboardState currentKeyboard = Keyboard.GetState();
        KeyboardState previousKeyboard = _previousKeyboard;
        _previousKeyboard = currentKeyboard;

        // Check for exit conditions using the already retrieved keyboard state
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || currentKeyboard.IsKeyDown(Keys.Escape))
            Exit();

        // Update console (with null check)
        if (_console != null)
        {
            _console.Update(gameTime, currentKeyboard, previousKeyboard);
        }
        else
        {
            Logger.Error("Console is null in Update method! Attempting to initialize...");
            try
            {
                _console = new Console(GraphicsDevice);
                _spriteBatch = new SpriteBatch(GraphicsDevice);
                Logger.Warn("Console initialized in Update method");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to initialize console in Update: {ex.Message}");
            }
        }

        // Only process game input if console is not active
        if (_console == null || !_console.IsActive)
        {
            UnpausedUpdate(gameTime, currentKeyboard);
        }
        else
        {
            // Console is active, skip game input processing
        }

        base.Update(gameTime);
    }

    protected void UnpausedUpdate(GameTime gameTime, KeyboardState currentKeyboard) {
        _camera.Update(gameTime, currentKeyboard);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        RasterizerState rasterizerState = new RasterizerState();
        rasterizerState.CullMode = CullMode.None;

        GraphicsDevice.RasterizerState = rasterizerState;

        // Draw console on top of everything
        if (_spriteBatch != null && _console != null)
        {
            _spriteBatch.Begin();
            _console.Draw(_spriteBatch, GraphicsDevice);
            _spriteBatch.End();
        }
        else
        {
            Logger.Error($"SpriteBatch or Console is null in Draw method! SpriteBatch: {_spriteBatch != null}, Console: {_console != null}");
        }

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
