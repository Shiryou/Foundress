using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Foundress
{
    public class Preferences
    {
        /// <summary>
        /// The preferences currently set.
        /// </summary>
        protected Dictionary<string, string> _preferences;
        /// <summary>
        /// The file path of the loaded preferences file.
        /// </summary>
        protected string _filePath;

        protected string[] _acceptedKeys = ["ScreenW", "ScreenH", "ScreenWindowed"];

        /// <summary>
        /// The game resolution.
        /// </summary>
        public Rectangle Resolution => new Rectangle(0, 0, Int32.Parse(_preferences["ScreenW"]), Int32.Parse(_preferences["ScreenH"]));
        public bool IsFullScreen => _preferences["ScreenWindowed"] != "true";

        /// <summary>
        /// Load the default preferences file or specify a path to a different file.
        /// </summary>
        /// <param name="path">the path to the file to load</param>
        public Preferences(string path = null) {
            _preferences = LoadFile(path);
        }

        /// <summary>
        /// Load a dictionary of preferences.
        /// </summary>
        /// <param name="preferences">the dictionary of preferences to load</param>
        public Preferences(Dictionary<string, string> preferences)
        {
            _preferences = LoadDictionary(preferences);
        }

        /// <summary>
        /// Process a dictionary to extract only the needed items.
        /// </summary>
        /// <param name="preferences">the dictionary of preferences to load</param>
        /// <returns>the dictionary of validated items</returns>
        public Dictionary<string, string> LoadDictionary(Dictionary<string, string> preferences)
        {
            Dictionary<string, string> checkedPreferences = GetDefaults();
            foreach (string key in preferences.Keys)
            {
                if (_acceptedKeys.Contains(key)) {
                    checkedPreferences[key] = preferences[key];
                }
            }

            return checkedPreferences;
        }

        /// <summary>
        /// Load a file of preferences.
        /// </summary>
        /// <param name="filePath">the path to the file</param>
        /// <returns>the dictionary of validated items</returns>
        public Dictionary<string, string> LoadFile(string filePath)
        {
            Dictionary<string, string> preferences = new();
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = GetDefaultFilePath();
            }
            string dirPath = Directory.GetParent(filePath).FullName;
            _filePath = Path.Combine(filePath, "preferences.txt");

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            if (!File.Exists(filePath))
            {
                // if the file doesn't exist, then we create it with some defaults
                Dictionary<string, string> defaults = GetDefaults();
                SavePreferences(filePath, defaults);
                return defaults;
            }

            foreach (var line in File.ReadLines(filePath))
            {
                // Skip empty lines and comments
                var trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("#"))
                {
                    continue;
                }

                // Skip lines that don't contain an '='
                var separatorIndex = trimmedLine.IndexOf('=');
                if (separatorIndex == -1)
                {
                    continue;
                }

                // Skip lines that have an empty key
                var key = trimmedLine.Substring(0, separatorIndex).Trim();
                if (string.IsNullOrEmpty(key))
                {
                    continue;
                }

                var value = trimmedLine.Substring(separatorIndex + 1).Trim();

                preferences[key] = value;
            }

            return LoadDictionary(preferences);
        }

        /// <summary>
        /// Save preferences to a file.
        /// 
        /// If the preferences were loaded from a file initially and a path is not provided, this will overwrite the original file.
        /// </summary>
        /// <param name="path">the path to save the preferences to.</param>
        public void SavePreferences(string path = null, Dictionary<string, string> defaults = null)
        {
            if (string.IsNullOrEmpty(path)) {
                if (string.IsNullOrEmpty(_filePath))
                {
                    path = GetDefaultFilePath();
                } else
                {
                    path = _filePath;
                }
            }
            if (defaults == null)
            {
                defaults = _preferences;
            }

            using (StreamWriter outputFile = new StreamWriter(path))
            {
                foreach (KeyValuePair<string, string> pref in defaults)
                    outputFile.WriteLine(pref.Key+"="+pref.Value);
            }
        }

        /// <summary>
        /// Get the default path for the preferences file.
        /// </summary>
        /// <returns>the path to the preferences file</returns>
        public string GetDefaultFilePath(string path = null)
        {
            if (File.Exists(path))
            {
                return path;
            }
            else if (Directory.Exists(path))
            {
                return Path.Combine(path, "preferences.txt");
            }
            else
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Foundress", "preferences.txt");
            }
        }

        /// <summary>
        /// Get a dictionary of default values.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetDefaults() {
            Dictionary<string, string> defaults = new();
            defaults["ScreenH"] = "720";
            defaults["ScreenW"] = "1280";
            defaults["ScreenWindowed"] = "false";
            return defaults;
        }
    }
}
