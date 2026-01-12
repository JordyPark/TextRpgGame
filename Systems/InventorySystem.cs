using System;
using System.Collections.Generic;
using BSsenTextRPG.Models;
using BSsenTextRPG.Utils;

namespace BSsenTextRPG.Systems;

public class InventorySystem
{
    #region 프로퍼티
    // 아이템 목록
    private List<Item> Items { get; set; }
    
    // 아이템 갯수 (읽기 전용)
    public int Count => Items.Count;       // goes to  // { get { return Items.Count; } }
    #endregion

    #region 생성자
    
    public InventorySystem()
    {
        Items = new List<Item>();
    }
    
    #endregion

    #region 아이템 관리
    
    // 아이템 추가 
    public void AddItem(Item item)
    {
        Items.Add(item);
        Console.WriteLine($"{item.Name}을 인벤토리에 추가했습니다.");
    }
    
    // 아이템 삭제 
    public bool RemoveItem(Item item)
    {
        if (Items.Remove(item))
        {
            Console.WriteLine($"{item.Name}을 인벤토리에서 제거했습니다.");
            return true;
        }
        
        return false;
    }
    
    // 인덱스 값으로 아이템 반환 
    public Item? GetItem(int index)
    {
        if(index >= 0 && index < Items.Count)
        {
            return Items[index];
        }
        
        return null;
    }
    
    #endregion

    #region  인벤토리 표시 
    
    public void DisplayInventory()
    {
        Console.Clear();
        Console.WriteLine("\n╔════════════════════════════════╗");
        Console.WriteLine("║         인벤토리               ║");
        Console.WriteLine("╚════════════════════════════════╝");

        if (Items.Count == 0)
        {
            Console.WriteLine("인벤토리가 비어 있습니다.");
            return;
        }
        
        Console.WriteLine("\n[보유 아이템]");
        
        for(int i = 0; i < Items.Count; i++)
        {
            
            Console.Write($"[{i + 1}] ");
            Items[i].DisplayInfo();
        }
    }

    public void ShowInventoryMenu(Player player)
    {
        while (true)
        {
            DisplayInventory();
            
            Console.WriteLine("\n선탁하세요.");
            Console.WriteLine("1. 아이템 사용");
            Console.WriteLine("2. 아이템 버리기");
            Console.WriteLine("0. 나가기");
            Console.Write("선택: ");
            
            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    // 아이템 사용로직 
                    UseItem(player);
                    break;
                case "2":
                    // 아이템 버리기 로직
                    DropItem(player);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("잘못된 입력입니다. 다시 선택해주세요.");
                    break;

            }

        }
    }
    
    #endregion

    #region 아이템 사용

    private void UseItem(Player player)
    {
        if (Items.Count == 0)
        {
            Console.WriteLine("인벤토리가 비어 있습니다.");
            return;
        }
        
        Console.Write("\n사용할 아이템의 번호를 입력하세요 (0: 취소)> ");
        
        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= Items.Count)
        {
            Item item = Items[index - 1];
            
            if (item.Use(player))
            {
                // 소모품일 경우 사용 후 리스트에서 제거 
                if (item is Consumable)         // item이 Consumable 타입인지 확인
                {
                    RemoveItem(item);
                }
            }
            
        }
        else if(index != 0)
        {
            Console.WriteLine("잘못된 입력입니다.");
            ConsoleUI.PressAnyKey();
        }
        
    }

    #endregion

    #region 아이템 버리기

    private void DropItem(Player player)
    {
        if(Items.Count == 0)
        {
            return;
        }
        
        Console.Write("\n버릴 아이템의 번호를 입력하세요 (0: 취소)> ");

        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= Items.Count)
        {
            Item item = Items[index - 1];    
            
            Console.WriteLine($"정말 {item.Name}을(를) 버리시겠습니까? (Y/N)");
            
            if (Console.ReadLine()?.ToUpper() == "Y")
            {
                // 장착 해제 로직 
                if(item is Equipment equipment)
                {
                    if (equipment == player.EquipmentWeapon)
                    {
                        player.UnEquipItem(EquipmentSlot.Weapon);
                    }
                    else if (equipment == player.EquipmentArmor)
                    {
                        player.UnEquipItem(EquipmentSlot.Armor);
                    }
                }
             
                RemoveItem(item);
                
                Console.WriteLine($"{item.Name}을 버렸습니다.");
                ConsoleUI.PressAnyKey();
            }
        }
        else if (index != 0)
        {
            Console.WriteLine("잘못된 입력입니다.");
            ConsoleUI.PressAnyKey();
        }
        
        
    }
    

    #endregion
}