using System;
using Foundress.Supports;

try
{
    Logger.Init("logs/foundress.log", Serilog.Events.LogEventLevel.Debug);
    Logger.Info("Starting Foundress...");
    
    using var game = new Foundress.Game1();
    game.Run();
    
    Logger.Info("Game exited normally.");
}
catch (Exception ex)
{
    Logger.Fatal($"Game crashed with exception: {ex}");
    throw;
}
finally
{
    Logger.Close();
}
