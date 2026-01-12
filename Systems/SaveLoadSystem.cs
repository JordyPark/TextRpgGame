using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using BSsenTextRPG.Data;
using BSsenTextRPG.Models;

namespace BSsenTextRPG.Systems;

public class SaveLoadSystem
{
    // 저장 경로 및 파일명 
    private const string SaveFilePath = "savegame.json";
    
    // JSON 직렬화 옵션
    // 직렬화 의미 : 객체 -> 문자열 
    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        WriteIndented = true, // 가독성 있는 형식으로 저장 
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping       // 한글 인코딩 설정
    };

    #region 저장 기능

    public static bool SaveGame(Player player, InventorySystem inventorySystem)
    {
        try
        {
            // 1. 게임 객체 (클래스) -> DTO(Data Transfer Object) 변환
            var saveData = new GameSaveData
            {
                Player = ConvertToPlayerData(player),
                Inventory = ConvertToItemData(inventorySystem)
            };
            
            // 2. DTO 객체 -> JSON 문자열 변환
            string jsonString = JsonSerializer.Serialize(saveData, jsonOptions);
            
            // 3. JSON 문자열을 파일로 저장
            File.WriteAllText(SaveFilePath, jsonString);
            return true;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
    
    // Player -> PlayerDTO 변환
    private static PlayerData ConvertToPlayerData(Player player)
    {
        PlayerData playerData = new PlayerData
        {
            Name = player.Name,
            Job = player.Job.ToString(),
            Level = player.Level,
            MaxHp = player.MaxHp,
            CurrentHp = player.CurrentHp,
            MaxMp = player.MaxMp,
            CurrentMp = player.CurrentMp,
            AttackPower = player.AttackPower,
            Defense = player.Defense,
            Gold = player.Gold,
            EquipedWeaponName = player.EquipmentWeapon?.Name,
            EquipedArmorName = player.EquipmentArmor?.Name
        };

        return playerData;
    }
    
    // InventorySystem -> ItemData 변환
    private static List<ItemData> ConvertToItemData(InventorySystem inventorySystem)
    {
        var itemDataList = new List<ItemData>();

        for (int i = 0; i < inventorySystem.Count; i++)
        {
            var item = inventorySystem.GetItem(i);
            
            if(item == null) continue;

            var itemData = new ItemData
            {
                Name = item.Name
            };

            if (item is Equipment equipment)
            {
                itemData.ItemType = "Equipment";
                itemData.Slot = equipment.Slot.ToString();
            }
            else if (item is Consumable consumable)
            {
                itemData.ItemType = "Consumable";
                
            }
            
            itemDataList.Add(itemData);
        }
        
        return itemDataList;
    }

    #endregion
    
    
    
}