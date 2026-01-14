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

    #region 불러오기 기능
    // 저장 파일 여부 확인 
    public static bool IsSaveFileExists()
    {
        return File.Exists(SaveFilePath);
    }
    

    public static GameSaveData? LoadGame()
    {
        try
        {
            // 1. JSON 파일에서 문자열 읽기 
            string jsonString = File.ReadAllText(SaveFilePath);
            // Console.WriteLine(jsonString);
            
            // 2. JSON 문자열 -> DTO 객체 변환 (역직렬화)
            var saveData = JsonSerializer.Deserialize<GameSaveData>(jsonString, jsonOptions);
            Console.WriteLine("\n게임데이터가 로드되었습니다.");
            return saveData;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
    
    // PlayerData DTO -> Player 클래스로 변환
    public static Player LoadPlayer(PlayerData data)
    {
        // JobType을 문자열 -> 열거형으로 변환
        var job = Enum.Parse<JobType>(data.Job);
        
        // Player 객체 생성
        var player = new Player(
            data.Name,
            job
        );
        
        // 스탯 설정 
        player.Level = data.Level;
        player.CurrentHp = data.CurrentHp;
        player.MaxHp = data.MaxHp;
        player.CurrentMp = data.CurrentMp;
        player.MaxMp = data.MaxMp;
        player.AttackPower = data.AttackPower;
        player.Defense = data.Defense;
        player.Gold = data.Gold;
        
        return player;
    }
    
    // Itemdata DTO -> Inventory 클래스로 변환 
    public static InventorySystem LoadInventorySystem(List<ItemData> itemDataList, Player player)
    {
        var inventorySystem = new InventorySystem();

        foreach (var itemData in itemDataList)
        {
            Item? item = null;

            if (itemData.ItemType == "Equipment")
            {
                // 장착 슬롯 확인 
                var slot = Enum.Parse<EquipmentSlot>(itemData.Slot);

                if (slot == EquipmentSlot.Weapon)
                {
                    item = Equipment.CreateWeapon(itemData.Name);
                } 
                else if (slot == EquipmentSlot.Armor)
                {
                    item = Equipment.CreateArmor(itemData.Name);    
                }
            }
            else if (itemData.ItemType == "Consumable")
            {
                item = Consumable.CreatePotion(itemData.Name);
            }

            if (item != null)
            {
                inventorySystem.AddItem(item);
            }
        }
        
        return inventorySystem;
    }

    // 저장된 장착 아이템을 복원하는 메서드(무기 / 방어구)
    public static void LoadEquippedItems(Player player, PlayerData data, InventorySystem inventory)
    {
        // 무기 복원
        if (!string.IsNullOrEmpty(data.EquipedWeaponName))
        {
            // 인벤토리에서 같은 무기를 찾기 
            for (int i = 0; i < inventory.Count; ++i)
            {
                var item = inventory.GetItem(i);

                if (item is Equipment equipment
                    && equipment.Slot == EquipmentSlot.Weapon
                    && equipment.Name == data.EquipedWeaponName)
                {
                    player.EquipItem(equipment);
                    break;
                }
            }
        }
        
        // 방어구 복원
        if (!string.IsNullOrEmpty(data.EquipedArmorName))
        {
            for (int i = 0; i < inventory.Count; ++i)
            {
                var item = inventory.GetItem(i);

                if (item is Equipment equipment
                    && equipment.Slot == EquipmentSlot.Armor
                    && equipment.Name == data.EquipedArmorName)
                {
                    player.EquipItem(equipment);
                    break;
                }
            }
        }
    }
    
    #endregion 
    
}