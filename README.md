# Foundress - Ant Colony Simulator

Foundress is an ant colony simulator where players take on the role of a queen ant establishing and growing her colony.

## Building the game

Foundress is currently in the early development stage and does not yet provide precompiled binaries. To try the game, please clone the repository and build the project using the .NET SDK.

```sh
git clone https://github.com/Shiryou/Foundress.git
cd Foundress
dotnet build
dotnet run
```

## Development Setup

1. Ensure you have the .NET SDK installed
2. Open `Foundress.sln` in your preferred IDE
3. Restore NuGet packages
4. Build the solution

## Contributing

Contributions are greatly appreciated.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the terms of the [MIT license](./LICENSE).

## Bug Reports

If you find a bug, please open an issue with:
1. A clear description of the bug
2. Steps to reproduce
3. Expected behavior
4. Actual behavior
5. Screenshots (if applicable)

## Project Structure

- `FormicidaeLib/`: Core library containing ant colony simulation logic
- `Foundress/`: MonoGame-based graphics logic
