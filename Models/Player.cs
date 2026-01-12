using System;

namespace BSsenTextRPG.Models;

public class Player : Character
{
    #region 프로퍼티 
    
    // 직업 
    public JobType Job { get; private set; }
    // 골드
    public int Gold { get; private set; }
    
    // 장착 무기
    public Equipment? EquipmentWeapon { get; private set; }
    // 장착 방어구
    public Equipment? EquipmentArmor { get; private set; }

    #endregion

    #region 생성자

    public Player(string name, JobType job) : base(
        name: name, 
        maxHp: GetInitHp(job), 
        maxMp: GetInitMp(job), 
        attackPower: GetInitAttack(job), 
        defense: GetInitDefense(job), 
        level: 1)
    {
        Job = job;
        Gold = 1000;
    }

    #endregion

    #region 직업별 초기 스탯

    private static int GetInitHp(JobType job)
    {
        switch (job)
        {
            case JobType.Warrior: return 150;
            case JobType.Archer: return 100;
            case JobType.Wizard: return 80;
            default: return 100;
        }
    }

    private static int GetInitMp(JobType job)
    {
        switch (job)
        {
            case JobType.Warrior: return 30;
            case JobType.Archer: return 50;
            case JobType.Wizard: return 100;
            default: return 30;
        }
    }

    private static int GetInitAttack(JobType job) =>
        job switch
        {
            JobType.Warrior => 20,
            JobType.Archer => 30,
            JobType.Wizard => 40,
            _ => 20
        };

    private static int GetInitDefense(JobType job) =>
        job switch
        {
            JobType.Warrior => 15,
            JobType.Archer => 10,
            JobType.Wizard => 5,
            _ => 15
        };

    #endregion

    #region 메서드
    // 플레이어 정보 출력 (오버라이드)
    public override void DisplayInfo()
    {
        // base.DisplayInfo(); 
        Console.Clear();
        Console.WriteLine($"==== {Name} 정보 ====");
        Console.WriteLine($"레벨 : {Level}");
        Console.WriteLine($"HP : {CurrentHp}/{MaxHp}");
        Console.WriteLine($"MP : {CurrentMp}/{MaxMp}");

        int attackBonus = EquipmentWeapon != null ? EquipmentWeapon.AttackBonus : 0;
        int defenseBonus = EquipmentArmor != null ? EquipmentArmor.DefenseBonus : 0;
        
        Console.WriteLine($"ATK : {AttackPower} (+{attackBonus})");
        Console.WriteLine($"DEF : {Defense} (+{defenseBonus})");
        
        Console.WriteLine($"골드 : {Gold}");
        
        // 장착 아이템 목록 
        if(EquipmentWeapon != null || EquipmentArmor != null)
        {
            Console.WriteLine("\n[장착 중인 장비목록]");
            
            if (EquipmentWeapon != null)
            {
                Console.WriteLine($"- 무기 : {EquipmentWeapon.Name} ");
            }
            
            if (EquipmentArmor != null)
            {
                Console.WriteLine($"- 방어구 : {EquipmentArmor.Name} ");
            }
        }
    }
    
    // 기본 공격 메소드 (오버라이드)
    public override int Attack(Character target)
    {
        // 장착무기 또는 방어구에 따른 추가 데미지 계산 
        int attackDamage = AttackPower;
        
        // ? null도 추가 가능한 경우            int? a = null 
        // null 병합 연산자 : ??              int b = a ?? 100;
        // if (EquipmentWeapon != null)
        // {
        //     attackDamage += EquipmentWeapon.AttackBonus;
        // }
        
        attackDamage += EquipmentWeapon?.AttackBonus ?? 0;
        
        return target.TakeDamage(attackDamage);
    }

    // 스킬 공격(MP 소모) : 플레이어 전용 메소드
    public int SkillAttack(Character target)
    {
        int mpCost = 15;
        
        // 스킬 공격은 기본공격의 1.5 데미지 
        int totalDamage = AttackPower;
        totalDamage += EquipmentWeapon?.AttackBonus ?? 0;
        
        totalDamage = (int)(totalDamage * 1.5f);
        
        // MP 소모 
        CurrentMp -= mpCost;
        
        // 데미지 전달 
        return target.TakeDamage(totalDamage);
    }
    
    // 골드 획득 메서드
    public void GainGoid(int amount)
    {
        Gold += amount;
        Console.WriteLine($"골드 +{amount} 획득! 현재 골드: {Gold}");
    }
    
    // 골드 차감 메서드
    public void SpendGold(int amount)
    {
        if (Gold >= amount)
        {
            Gold -= amount;
        }
    }
    
    // 장비 착용
    public void EquipItem(Equipment newEquipment)
    {
        Equipment? prevEquipment = null;

        switch (newEquipment.Slot)
        {
            case EquipmentSlot.Weapon:
                prevEquipment = EquipmentWeapon;
                EquipmentWeapon = newEquipment;
                break;
            case EquipmentSlot.Armor:
                prevEquipment = EquipmentArmor;
                EquipmentArmor = newEquipment;
                break;
        }
        
        // 이전 장비 해제 메세지 
        if (prevEquipment != null)
        {
            Console.WriteLine($"{prevEquipment.Name} 장착 해제");
        }
        
        Console.WriteLine($"{newEquipment.Name} 장착 완료!");
    }
    
    // 장비 해제
    public Equipment? UnEquipItem(EquipmentSlot slot)
    {
        Equipment? equipment = null;

        switch (slot)
        {
            case EquipmentSlot.Weapon:
                equipment = EquipmentWeapon;
                EquipmentWeapon = null;
                break;
            case EquipmentSlot.Armor:
                equipment = EquipmentArmor;
                EquipmentArmor = null;
                break;
        }

        if (equipment != null)
        {
            Console.WriteLine($"{equipment.Name} 장착 해제");
        }
        
        return equipment;
    }
    
    #endregion
}