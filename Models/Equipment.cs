using System;

namespace BSsenTextRPG.Models;

public class Equipment : Item
{
    #region 프로퍼티

    // 장착 슬롯 
    public EquipmentSlot Slot { get; private set; }
    // 공격력 보너스
    public int AttackBonus { get; private set; }
    // 방어력 보너스
    public int DefenseBonus { get; private set; }

    #endregion
    
    #region 생성자

    public Equipment(
        string name, 
        string description, 
        int price, 
        EquipmentSlot slot,
        int attackBonus = 0,
        int defenseBonus = 0) 
        : base(name, description, price, slot == EquipmentSlot.Weapon ? ItemType.Weapon : ItemType.Armor)
    {
        Slot = slot;
        AttackBonus = attackBonus;
        DefenseBonus = defenseBonus;
    }

    public override bool Use(Player player)
    {
       // 장비 착용 로직 
       player.EquipItem(this);
       
       return true;
    }
    #endregion

    #region 장착아이템 생성 메서드

    // 무기 생성 메서드
    public static Equipment CreateWeapon(string weaponType)
    {
        switch (weaponType)
        {
            case "목검" :
                return new Equipment("목검", "기본 무기", 100, EquipmentSlot.Weapon, 5);
            case "철검" :
                return new Equipment("철검", "강력한 무기", 500, EquipmentSlot.Weapon, 15);
            case "전설검" :
                return new Equipment("전설검", "최고의 무기", 1000, EquipmentSlot.Weapon, 25);
            default:
                return null;
        }
    }
    
    // 방어구 생성 메서드 
    public static Equipment CreateArmor(string armorType)
    {
        switch (armorType)
        {
            case "천갑옷" :
                return new Equipment("천갑옷", "기본 방어구", 100, EquipmentSlot.Armor, 0, 5);
            case "철갑옷" :
                return new Equipment("철갑옷", "강력한 방어구", 500, EquipmentSlot.Armor, 0, 20);
            case "전설갑옷" : 
                return new Equipment("전설갑옷", "최고의 방어구", 1000, EquipmentSlot.Armor, 0, 30);
            default:
                return null;
        }
    }
    
    #endregion
}