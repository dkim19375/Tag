using System;
using System.IO;
using UnityEngine;

namespace Data {
    public class DataFileManager {
        private readonly FileInfo _fileInfo = new FileInfo($"data{Path.DirectorySeparatorChar}data.json");
        public GameData CurrentData;

        internal DataFileManager() {
            Reload();
        }

        private void CreateFile() {
            if (_fileInfo.Exists) {
                return;
            }
            _fileInfo.Directory?.Create();
            _fileInfo.Create().Close();
        }

        public void Save(GameData newData) {
            CurrentData = newData;
            File.WriteAllText(_fileInfo.FullName, JsonUtility.ToJson(newData));
        }

        public void Reload() {
            CreateFile();
            try {
                CurrentData = JsonUtility.FromJson<GameData>(File.ReadAllText(_fileInfo.FullName));
            } catch (NullReferenceException) {
                Save(new GameData());
            }
            Debug.Log($"CurrentData: {CurrentData}");
        }
    }
}