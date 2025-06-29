using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Text;

namespace Foundress.Utilities
{
    public class Console
    {
        private bool _everBeenVisible = false;
        private bool _isVisible = false;
        private bool _isActive = false;
        private StringBuilder _inputBuffer = new StringBuilder();
        private List<string> _outputLines = new List<string>();
        private List<string> _commandHistory = new List<string>();
        private int _historyIndex = -1;
        
        private SpriteFont _font;
        private SpriteFont _boldFont;
        private Texture2D _backgroundTexture;
        private Rectangle _consoleBounds;
        private Vector2 _inputPosition;
        private Vector2 _outputPosition;
        
        private const int MAX_OUTPUT_LINES = 17;
        private const int CONSOLE_HEIGHT = 300;
        private const int PADDING = 10;
        
        private readonly Action _quitAction;
        
        public bool IsVisible => _isVisible;
        public bool IsActive => _isActive;

        public Console(GraphicsDevice graphicsDevice, SpriteFont font = null, Action quitAction = null, SpriteFont boldFont = null)
        {
            _font = font;
            _boldFont = boldFont;
            _quitAction = quitAction;
            CreateBackgroundTexture(graphicsDevice);
            
            // Subscribe to log messages
            Logger.OnLogMessage += OnLogMessage;
        }

        private void CreateBackgroundTexture(GraphicsDevice graphicsDevice)
        {
            _backgroundTexture = new Texture2D(graphicsDevice, 1, 1);
            _backgroundTexture.SetData(new[] { Color.Black });
        }

        public void LoadContent(SpriteFont font)
        {
            _font = font;
        }

        public void LoadBoldFont(SpriteFont boldFont)
        {
            _boldFont = boldFont;
        }

        public void Update(GameTime gameTime, KeyboardState currentKeyboard, KeyboardState previousKeyboard)
        {
            Logger.Verbose($"Console Update - Visible: {_isVisible}, Active: {_isActive}");
            
            // Toggle console with Tilde key (~)
            if (currentKeyboard.IsKeyDown(Keys.OemTilde) && previousKeyboard.IsKeyUp(Keys.OemTilde))
            {
                ToggleConsole();
            }

            if (!_isVisible) return;

            foreach (Keys key in currentKeyboard.GetPressedKeys())
            {
                if (previousKeyboard.IsKeyUp(key))
                {
                    Logger.Verbose($"Console detected key: {key}");
                }
            }

            // Handle input when console is active
            if (_isActive)
            {
                HandleInput(currentKeyboard, previousKeyboard);
            }
            else
            {
                // Activate console with Enter
                if (currentKeyboard.IsKeyDown(Keys.Enter) && previousKeyboard.IsKeyUp(Keys.Enter))
                {
                    _isActive = true;
                    Logger.Debug("Console activated for input");
                }
                else if (currentKeyboard.IsKeyDown(Keys.Enter))
                {
                    Logger.Verbose($"Enter pressed but not activating console. Previous: {previousKeyboard.IsKeyDown(Keys.Enter)}");
                }
            }

            // Deactivate console with Escape
            if (currentKeyboard.IsKeyDown(Keys.Escape) && previousKeyboard.IsKeyUp(Keys.Escape))
            {
                if (_isActive)
                {
                    _isActive = false;
                    _inputBuffer.Clear();
                    Logger.Debug("Console input deactivated");
                }
                else
                {
                    _isVisible = false;
                    Logger.Debug("Console hidden");
                }
            }
        }

        private void HandleInput(KeyboardState currentKeyboard, KeyboardState previousKeyboard)
        {
            // Handle text input
            foreach (Keys key in currentKeyboard.GetPressedKeys())
            {
                if (previousKeyboard.IsKeyUp(key))
                {
                    Logger.Verbose($"HandleInput: Key pressed: {key}");
                    HandleKeyPress(key, currentKeyboard);
                }
            }
        }

        private void HandleKeyPress(Keys key, KeyboardState keyboardState)
        {
            switch (key)
            {
                case Keys.Enter:
                    ExecuteCommand();
                    break;
                case Keys.Back:
                    if (_inputBuffer.Length > 0)
                        _inputBuffer.Length--;
                    break;
                case Keys.Up:
                    NavigateHistory(-1);
                    break;
                case Keys.Down:
                    NavigateHistory(1);
                    break;
                default:
                    // Add character to input buffer
                    char? character = GetCharacterFromKey(key, keyboardState);
                    if (character.HasValue)
                        _inputBuffer.Append(character.Value);
                    break;
            }
        }

        private char? GetCharacterFromKey(Keys key, KeyboardState keyboardState)
        {
            bool shift = keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift);
            
            return key switch
            {
                Keys.A => shift ? 'A' : 'a',
                Keys.B => shift ? 'B' : 'b',
                Keys.C => shift ? 'C' : 'c',
                Keys.D => shift ? 'D' : 'd',
                Keys.E => shift ? 'E' : 'e',
                Keys.F => shift ? 'F' : 'f',
                Keys.G => shift ? 'G' : 'g',
                Keys.H => shift ? 'H' : 'h',
                Keys.I => shift ? 'I' : 'i',
                Keys.J => shift ? 'J' : 'j',
                Keys.K => shift ? 'K' : 'k',
                Keys.L => shift ? 'L' : 'l',
                Keys.M => shift ? 'M' : 'm',
                Keys.N => shift ? 'N' : 'n',
                Keys.O => shift ? 'O' : 'o',
                Keys.P => shift ? 'P' : 'p',
                Keys.Q => shift ? 'Q' : 'q',
                Keys.R => shift ? 'R' : 'r',
                Keys.S => shift ? 'S' : 's',
                Keys.T => shift ? 'T' : 't',
                Keys.U => shift ? 'U' : 'u',
                Keys.V => shift ? 'V' : 'v',
                Keys.W => shift ? 'W' : 'w',
                Keys.X => shift ? 'X' : 'x',
                Keys.Y => shift ? 'Y' : 'y',
                Keys.Z => shift ? 'Z' : 'z',
                Keys.D0 => shift ? ')' : '0',
                Keys.D1 => shift ? '!' : '1',
                Keys.D2 => shift ? '@' : '2',
                Keys.D3 => shift ? '#' : '3',
                Keys.D4 => shift ? '$' : '4',
                Keys.D5 => shift ? '%' : '5',
                Keys.D6 => shift ? '^' : '6',
                Keys.D7 => shift ? '&' : '7',
                Keys.D8 => shift ? '*' : '8',
                Keys.D9 => shift ? '(' : '9',
                Keys.OemMinus => shift ? '_' : '-',
                Keys.OemPlus => shift ? '+' : '=',
                Keys.OemOpenBrackets => shift ? '{' : '[',
                Keys.OemCloseBrackets => shift ? '}' : ']',
                Keys.OemPipe => shift ? '|' : '\\',
                Keys.OemSemicolon => shift ? ':' : ';',
                Keys.OemQuotes => shift ? '"' : '\'',
                Keys.OemComma => shift ? '<' : ',',
                Keys.OemPeriod => shift ? '>' : '.',
                Keys.OemQuestion => shift ? '?' : '/',
                Keys.Space => ' ',
                _ => null
            };
        }

        private void NavigateHistory(int direction)
        {
            if (_commandHistory.Count == 0) return;

            if (direction > 0) // Down
            {
                if (_historyIndex < _commandHistory.Count - 1)
                    _historyIndex++;
                else
                    _historyIndex = -1; // Clear input
            }
            else // Up
            {
                if (_historyIndex > 0)
                    _historyIndex--;
                else if (_historyIndex == -1)
                    _historyIndex = _commandHistory.Count - 1;
            }

            _inputBuffer.Clear();
            if (_historyIndex >= 0)
                _inputBuffer.Append(_commandHistory[_historyIndex]);
        }

        private void ExecuteCommand()
        {
            string command = _inputBuffer.ToString().Trim();
            if (string.IsNullOrEmpty(command)) return;

            // Add to history
            _commandHistory.Add(command);
            if (_commandHistory.Count > 50) // Keep last 50 commands
                _commandHistory.RemoveAt(0);

            // Add command to output
            WriteLine($"> {command}");

            // Execute command
            ProcessCommand(command);

            // Clear input and reset history index
            _inputBuffer.Clear();
            _historyIndex = -1;
            _isActive = false;
        }

        private void ProcessCommand(string command)
        {
            string[] parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return;

            switch (parts[0].ToLower())
            {
                case "help":
                    WriteLine("Available commands:");
                    WriteLine("  help - Show this help");
                    WriteLine("  clear - Clear console output");
                    WriteLine("  echo <text> - Echo text back");
                    WriteLine("  loglevel <level> - Change the log level");
                    WriteLine("  quit - Exit the game");
                    break;
                case "clear":
                    _outputLines.Clear();
                    break;
                case "echo":
                    if (parts.Length > 1)
                        WriteLine(string.Join(" ", parts, 1, parts.Length - 1));
                    break;
                case "loglevel":
                    if (parts.Length > 1)
                    {
                        if (int.TryParse(parts[1], out int number))
                        {
                            Serilog.Events.LogEventLevel level = (Serilog.Events.LogEventLevel)number;
                            Logger.ChangeLevel(level);
                            WriteLine($"Changing log level to: {level.ToString()}");
                        }
                        else
                        {
                            Logger.Warn("Invalid integer string.");
                        }
                    }
                    else
                    {
                        WriteLine($"Current logging level: {Logger.Level.ToString()}");
                        WriteLine("Log levels:");
                        WriteLine("  0 - Verbose");
                        WriteLine("  1 - Debug");
                        WriteLine("  2 - Information");
                        WriteLine("  3 - Warning");
                        WriteLine("  4 - Error");
                        WriteLine("  5 - Fatal");
                    }
                        break;
                case "quit":
                    WriteLine("Quit command received");
                    _quitAction?.Invoke();
                    break;
                default:
                    WriteLine($"Unknown command: {parts[0]}");
                    break;
            }
        }

        public void WriteLine(string text)
        {
            _outputLines.Add(text);
            if (_outputLines.Count > MAX_OUTPUT_LINES)
                _outputLines.RemoveAt(0);
        }

        public void ToggleConsole()
        {
            _isVisible = !_isVisible;
            if (_isVisible)
            {
                _isActive = false;
                _inputBuffer.Clear();
                Logger.Verbose("Console made visible, waiting for Enter to activate input");
                if (!_everBeenVisible)
                {
                    _everBeenVisible = true;
                    WriteLine("Welcome to **Foundress**!");
                    WriteLine("This is the game's console, where you can view debugging information and run some simple");
                    WriteLine("commands. The console is currently not active (takes no input). Press <Enter> to activate");
                    WriteLine("it. To see available commands, type 'help'.");
                }
            }
            else
            {
                _isActive = false;
                _inputBuffer.Clear();
                Logger.Verbose("Console hidden and deactivated");
            }
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            if (!_isVisible || spriteBatch == null || graphicsDevice == null) return;

            // Calculate console bounds
            int screenWidth = graphicsDevice.Viewport.Width;
            int screenHeight = graphicsDevice.Viewport.Height;
            
            _consoleBounds = new Rectangle(0, 0, screenWidth, CONSOLE_HEIGHT);
            _inputPosition = new Vector2(PADDING, CONSOLE_HEIGHT - PADDING - (_font?.LineSpacing ?? 20));
            _outputPosition = new Vector2(PADDING, PADDING);

            // Draw background
            if (_backgroundTexture != null)
            {
                spriteBatch.Draw(_backgroundTexture, _consoleBounds, new Color(0, 0, 0, 200));
            }

            // Only draw text if font is available
            if (_font != null)
            {
                // Draw output lines
                Vector2 currentPos = _outputPosition;
                foreach (string line in _outputLines)
                {
                    // Check if line starts with a log level indicator
                    if (line.StartsWith("[") && line.Contains("]"))
                    {
                        int bracketEnd = line.IndexOf(']');
                        string level = line.Substring(1, bracketEnd - 1);
                        string message = line.Substring(bracketEnd + 1);
                        
                        // Draw level in bold if available, otherwise in different color
                        if (_boldFont != null)
                        {
                            spriteBatch.DrawString(_boldFont, $"[{level}]", currentPos, GetLevelColor(level));
                            DrawFormattedText(spriteBatch, message, currentPos + new Vector2(_font.MeasureString($"[{level}]").X, 0));
                        }
                        else
                        {
                            spriteBatch.DrawString(_font, $"[{level}]", currentPos, GetLevelColor(level));
                            DrawFormattedText(spriteBatch, message, currentPos + new Vector2(_font.MeasureString($"[{level}]").X, 0));
                        }
                    }
                    else
                    {
                        DrawFormattedText(spriteBatch, line, currentPos);
                    }
                    
                    currentPos.Y += _font.LineSpacing;
                }

                // Draw input line
                string inputText = _inputBuffer.ToString();
                if (_isActive)
                {
                    // Add cursor (use ASCII character instead of Unicode)
                    inputText += "_";
                }
                Color promptColor = _isActive ? Color.Yellow : Color.Gray;
                spriteBatch.DrawString(_font, $"> {inputText}", _inputPosition, promptColor);
            }
            else
            {
                // Fallback: Draw simple colored rectangles to indicate console is active
                // Draw a simple indicator that console is open
                if (_backgroundTexture != null)
                {
                    Rectangle indicatorRect = new Rectangle(PADDING, PADDING, 20, 20);
                    spriteBatch.Draw(_backgroundTexture, indicatorRect, Color.Red);
                }
            }
        }

        public void Dispose()
        {
            // Unsubscribe from log messages
            Logger.OnLogMessage -= OnLogMessage;
            
            _backgroundTexture?.Dispose();
        }

        private void OnLogMessage(string level, string message)
        {
            // Format the log message for display
            string formattedMessage = $"[{level.ToUpper()}] {message}";
            WriteLine(formattedMessage);
        }

        private Color GetLevelColor(string level)
        {
            return level switch
            {
                "VERBOSE" => Color.Gray,
                "DEBUG" => Color.Yellow,
                "INFORMATION" => Color.Green,
                "WARNING" => Color.Orange,
                "ERROR" => Color.Red,
                "FATAL" => Color.DarkRed,
                _ => Color.White
            };
        }

        private void DrawFormattedText(SpriteBatch spriteBatch, string text, Vector2 position)
        {
            Vector2 currentPos = position;
            int i = 0;
            
            while (i < text.Length)
            {
                // Look for **bold** markers
                if (i + 1 < text.Length && text[i] == '*' && text[i + 1] == '*')
                {
                    // Find the closing **
                    int endBold = text.IndexOf("**", i + 2);
                    if (endBold != -1)
                    {
                        // Draw text before bold
                        if (i > 0)
                        {
                            string beforeBold = text.Substring(0, i);
                            spriteBatch.DrawString(_font, beforeBold, currentPos, Color.White);
                            currentPos.X += _font.MeasureString(beforeBold).X;
                        }
                        
                        // Draw bold text
                        string boldText = text.Substring(i + 2, endBold - i - 2);
                        if (_boldFont != null)
                        {
                            spriteBatch.DrawString(_boldFont, boldText, currentPos, Color.White);
                            currentPos.X += _boldFont.MeasureString(boldText).X;
                        }
                        else
                        {
                            spriteBatch.DrawString(_font, boldText, currentPos, Color.White);
                            currentPos.X += _font.MeasureString(boldText).X;
                        }
                        
                        // Continue with remaining text
                        text = text.Substring(endBold + 2);
                        i = 0;
                        continue;
                    }
                }
                i++;
            }
            
            // Draw any remaining text
            if (text.Length > 0)
            {
                spriteBatch.DrawString(_font, text, currentPos, Color.White);
            }
        }
    }
} 
