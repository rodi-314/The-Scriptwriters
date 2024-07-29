using System;
using System.Collections;
using System.Collections.Generic;
using RPGM.Core;
using RPGM.Gameplay;
using RPGM.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPGM.Gameplay
{
    [Serializable]
    public class GameModel
    {
        public MC_Controller MC;
        public DialogController dialog;
        public InputController input;
        public InventoryController inventoryController;
        public MusicController musicController;

        Dictionary<GameObject, HashSet<string>> conversations = new Dictionary<GameObject, HashSet<string>>();
        Dictionary<string, int> inventory = new Dictionary<string, int>();
        Dictionary<string, Sprite> inventorySprites = new Dictionary<string, Sprite>();
        HashSet<string> storyItems = new HashSet<string>();

        public IEnumerable<string> InventoryItems => inventory.Keys;

        public Sprite GetInventorySprite(string name)
        {
            inventorySprites.TryGetValue(name, out Sprite s);
            return s;
        }

        public int GetInventoryCount(string name)
        {
            inventory.TryGetValue(name, out int c);
            return c;
        }

        public void AddInventoryItem(InventoryItem item)
        {
            inventory.TryGetValue(item.name, out int c);
            c += item.count;
            inventorySprites[item.name] = item.sprite;
            inventory[item.name] = c;
            inventoryController.Refresh();
        }

        public bool HasInventoryItem(string name, int count = 1)
        {
            inventory.TryGetValue(name, out int c);
            return c >= count;
        }

        public bool RemoveInventoryItem(InventoryItem item, int count)
        {
            inventory.TryGetValue(item.name, out int c);
            c -= count;
            if (c < 0) return false;
            inventory[item.name] = c;
            inventoryController.Refresh();
            return true;
        }

        public void RegisterStoryItem(string ID)
        {
            storyItems.Add(ID);
        }

        public bool HasSeenStoryItem(string ID)
        {
            return storyItems.Contains(ID);
        }

        public void RegisterConversation(GameObject owner, string id)
        {
            if (!conversations.TryGetValue(owner, out HashSet<string> ids))
                conversations[owner] = ids = new HashSet<string>();
            ids.Add(id);
        }

        public bool HasHadConversationWith(GameObject owner, string id)
        {
            if (!conversations.TryGetValue(owner, out HashSet<string> ids))
                return false;
            return ids.Contains(id);
        }

        public bool HasMet(GameObject owner)
        {
            return conversations.ContainsKey(owner);
        }

        public SaveData CreateSaveData()
        {
            var saveData = new SaveData
            {
                currentScene = SceneManager.GetActiveScene().buildIndex,
                playerPosition = MC.transform.position,
                inventory = new Dictionary<string, int>(inventory),
                inventorySprites = new Dictionary<string, string>(),
                storyItems = new List<string>(storyItems),
                conversations = new Dictionary<string, List<string>>()
            };

            foreach (var kvp in inventorySprites)
            {
                saveData.inventorySprites[kvp.Key] = kvp.Value.name; // Assuming sprite name is enough to load the sprite
            }

            foreach (var kvp in conversations)
            {
                saveData.conversations[kvp.Key.name] = new List<string>(kvp.Value);
            }

            // Log the saved data
            Debug.Log("Saving Game Data:");
            Debug.Log("Current Scene: " + saveData.currentScene);
            Debug.Log($"Player Position: {saveData.playerPosition.x}, {saveData.playerPosition.y}, {saveData.playerPosition.z}");

            foreach (var item in saveData.inventory)
            {
                Debug.Log("Inventory Item: " + item.Key + ", Count: " + item.Value);
            }

            foreach (var item in saveData.storyItems)
            {
                Debug.Log("Story Item: " + item);
            }

            foreach (var kvp in saveData.conversations)
            {
                Debug.Log("Conversation with " + kvp.Key + ": " + string.Join(", ", kvp.Value));
            }

            return saveData;
        }

        public void LoadFromSaveData(SaveData saveData)
        {
            if (saveData == null) return;

            // Log the loaded data
            Debug.Log("Loading Game Data:");
            Debug.Log("Current Scene: " + saveData.currentScene);
            Debug.Log($"Player Position: {saveData.playerPosition.x}, {saveData.playerPosition.y}, {saveData.playerPosition.z}");

            MC.transform.position = saveData.playerPosition;

            inventory.Clear();
            inventorySprites.Clear();

            if (inventoryController != null)
            {
                foreach (var item in saveData.inventory)
                {
                    // Reconstruct inventory items
                    var sprite = Resources.Load<Sprite>(item.Key); // Load the sprite from the path
                    var newItem = new InventoryItem { name = item.Key, count = item.Value, sprite = sprite };
                    AddInventoryItem(newItem);
                    Debug.Log("Loaded Inventory Item: " + item.Key + ", Count: " + item.Value);
                }

                foreach (var kvp in saveData.inventorySprites)
                {
                    var sprite = Resources.Load<Sprite>(kvp.Value);
                    if (sprite != null)
                    {
                        inventorySprites[kvp.Key] = sprite;
                    }
                    else
                    {
                        Debug.LogWarning($"Sprite for {kvp.Key} could not be loaded from {kvp.Value}");
                    }
                }
            }
            else
            {
                Debug.LogError("InventoryController is null.");
            }

            storyItems = new HashSet<string>(saveData.storyItems);

            conversations.Clear();
            foreach (var kvp in saveData.conversations)
            {
                var go = GameObject.Find(kvp.Key);
                if (go != null)
                {
                    conversations[go] = new HashSet<string>(kvp.Value);
                }
            }
        }
    }
}
