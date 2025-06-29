using System;
using Foundress.Supports;

try
{
    Serilog.Events.LogEventLevel level = Serilog.Events.LogEventLevel.Information;
#if DEBUG
    level = Serilog.Events.LogEventLevel.Debug;
#endif
    Logger.Init("logs/foundress.log", level);
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
