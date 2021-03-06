using System;
using System.Collections.Generic;
using System.Linq;
using GameScene;
using UnityEngine;

namespace Data {
    [Serializable]
    public class GameData {
        [SerializeField] private int coins;
        [SerializeField] private int highScore;
        [SerializeField] private string customSkin;
        [SerializeField] private string skinType;
        [SerializeField] private string currentSkin;
        [SerializeField] private string boughtSkins;

        public GameData(
            int coins = 0,
            int highScore = 0,
            string customSkin = "Images/Skins/Custom",
            string skinType = "none",
            string currentSkin = "none",
            string boughtSkins = ""
        ) {
            this.coins = coins;
            this.highScore = highScore;
            this.customSkin = customSkin;
            this.skinType = skinType;
            this.currentSkin = currentSkin;
            this.boughtSkins = boughtSkins;
        }

        public void CheckData() {
            var save = false;
            if (customSkin == null) {
                customSkin = "Images/Skins/Custom";
                save = true;
            }
            if (skinType == null) {
                skinType = "none";
                save = true;
            }
            if (currentSkin == null) {
                currentSkin = "none";
                save = true;
            }
            if (!save) {
                return;
            }
            GameManager.GetInstance().DataFileManager.Save(this);
        }

        public int GetCoins() => coins;

        public void SetCoins(int newCoins) {
            coins = newCoins;
            GameManager.GetInstance().DataFileManager.Save(this);
        }

        public int GetHighScore() => highScore;

        public void SetHighScore(int newHighScore) {
            highScore = newHighScore;
            GameManager.GetInstance().DataFileManager.Save(this);
        }

        public string GetCustomSkin() {
            CheckData();
            return customSkin;
        }

        public void SetCustomSkin(string newCustomSkin) {
            customSkin = newCustomSkin;
            GameManager.GetInstance().DataFileManager.Save(this);
        }

        public SkinType GetSkinType() {
            CheckData();
            return SkinType.GetFromName(skinType);
        }

        public void SetSkinType(SkinType newSkinType) {
            skinType = newSkinType.GetName();
            GameManager.GetInstance().DataFileManager.Save(this);
        }

        public SkinType GetCurrentSkin() {
            CheckData();
            var skin = SkinType.GetFromName(currentSkin);
            if (GetBoughtSkins().Contains(skin)) {
                return skin;
            }
            SetCurrentSkin(SkinType.None);
            return SkinType.None;
        }

        public void SetCurrentSkin(SkinType newCurrentSkin) {
            currentSkin = newCurrentSkin.GetName();
            GameManager.GetInstance().DataFileManager.Save(this);
        }

        public List<SkinType> GetBoughtSkins() {
            CheckData();
            var list = boughtSkins.Split('|');
            var newList = new List<SkinType> { SkinType.None };
            foreach (var skin in list.ToList()) {
                SkinType newSkinType;
                try {
                    newSkinType = SkinType.GetFromName(skin);
                } catch (Exception) {
                    continue;
                }
                if (newList.Contains(newSkinType)) {
                    continue;
                }
                newList.Add(newSkinType);
            }
            return newList;
        }

        public void SetBoughtSkins(IEnumerable<SkinType> newBoughtSkins) {
            var newList = newBoughtSkins.ToList();
            if (!newList.Contains(SkinType.None)) {
                newList.Add(SkinType.None);
            }
            boughtSkins = string.Join("|", newList.Select(s => s.GetName()));
            GameManager.GetInstance().DataFileManager.Save(this);
        }

        public override string ToString() => JsonUtility.ToJson(this);
    }
}