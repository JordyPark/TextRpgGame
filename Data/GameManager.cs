using System;
using BSsenTextRPG.Models;
using BSsenTextRPG.Utils;
using BSsenTextRPG.Models;
using BSsenTextRPG.Systems;
using Microsoft.VisualBasic;

namespace BSsenTextRPG.Data;

public class GameManager
{
    // 싱글톤 패턴(Singleton Pattern) 구현 

    #region 싱글톤 패턴
    // 싱글톤 인스 (내부 접근 용 변수 : 필드)
    private static GameManager instance;
    
    // 외부에서 인스턴스에 접근할 수 있는 정적 속성(프로퍼티)
    public static GameManager Instance
    {
        get
        {
            // 인스턴스가 없으면 새로 생성 
            if (instance == null)
            {
                instance = new GameManager();
            }
            
            return instance;
        }
    }

    private GameManager()
    {
        // 클래스가 생성될 때 초기화 작업 수행 
        
        // 전투 시스템 초기화 
        BattleSystem = new BattleSystem();
        
        // 상점 시스템 초기화 
        ShopSystem = new ShopSystem();
    }
    #endregion

    #region 프로퍼티
    //  플레이어 캐릭터 
    public Player? Player { get; private set; }
    
    // 전투시스템 
    public BattleSystem BattleSystem { get; private set; } 
    
    // 인벤토리 시스템 
    public InventorySystem InventorySystem { get; private set; }
    
    // 상점 시스템 
    public ShopSystem ShopSystem { get; private set; }
    
    // 게임 실행 여부 
    public bool IsRunning { get; private set; } = true;
    
    #endregion

    #region 게임 시작/ 종료
    // 게임 시작 메서드 
    public void StartGame(bool loadedGame = false)
    {
        // 타이틀 표시 
        ConsoleUI.ShowTitle();
        Console.WriteLine("빡센 게임에 오신것을 환영합니다. \n ");
        
        // 새로 시작하는 게임일 경우 새 캐릭터 및 설정을 처리 
        if (!loadedGame)
        {
            // 캐릭터 생성
            CreateCharacter();
        
            // 인벤토리 초기화 
            InventorySystem = new InventorySystem();
        
            // 초기 아이템 지급 
            SetupInitialItems();
        }
       
        
        // 메인 게임 루프 
        IsRunning = true;
        while (IsRunning)
        {
            ShowMainMenu();
        }

        if (!IsRunning)
        {
            ConsoleUI.ShowGameOver();
        }
    }
    
    #endregion

    #region 캐릭터 생성

    private void CreateCharacter()
    {
        // 이름 입력 
        Console.Write("캐릭터의 이름을 입력하세요: ");
        string? name = Console.ReadLine();          // nullable 허용

        if (string.IsNullOrWhiteSpace(name))
        {
            name = "무명용사";
        }
        
        Console.WriteLine($"{name}님, 모험을 시작하겠습니다!");
        
        // 직업 선택 
        Console.WriteLine("직업을 선택하세요: ");
        Console.WriteLine("1: 전사");
        Console.WriteLine("2: 궁수");
        Console.WriteLine("3: 마법사");

        // 직업 기본값 설정 
        JobType job = JobType.Warrior;

        while (true)
        {
            Console.WriteLine("선택 (1-3): ");
            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    job = JobType.Warrior;
                    break;
                case "2":
                    job = JobType.Archer;
                    break;
                case "3":
                    job = JobType.Wizard;
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다. 다시 선택해주세요.");
                    continue;
            }

            break;
        }
        
        // 입력한 이름과 선택한 직업으로 플레이어 캐릭터 생성 
        Player = new Player(name, job);
        Console.WriteLine($"\n{name}님, {job}직업으로 캐릭터가 생성되었습니다.");
        
        // // 적 캐릭터 생성 
        // Enemy enemy = Enemy.CreateEnemy(Player.Level);
        // enemy.DisplayInfo();
        //
        // // 전투테스트 
        // BattleSystem baaBattleSystem = new BattleSystem();
        // bool playerWin = baaBattleSystem.StartBattle(Player, enemy);
        
        ConsoleUI.PressAnyKey();
    }
    
    // 초기 아이템 지급 
    private void SetupInitialItems()
    {
        // 기본 장비 
        // InventorySystem.AddItem(Equipment.CreateWeapon("목검")); 
        // InventorySystem.AddItem(Equipment.CreateArmor("천갑옷"));
        var weapon = Equipment.CreateWeapon("목검");
        var armor = Equipment.CreateArmor("천갑옷");
        
        InventorySystem.AddItem(weapon);
        InventorySystem.AddItem(armor);
        
        // 포션 지급 
        InventorySystem.AddItem(Consumable.CreatePotion("체력포션"));
        InventorySystem.AddItem(Consumable.CreatePotion("체력포션"));
        InventorySystem.AddItem(Consumable.CreatePotion("마나포션"));

        // 기본 장비 착용 
        Player.EquipItem(weapon);
        Player.EquipItem(armor);
        
        Console.WriteLine("\n초기 장비를 지급했습니다.");
        ConsoleUI.PressAnyKey();
    }
    
    
    #endregion

    #region 메인 메뉴

    public void ShowMainMenu()
    {
        Console.Clear();
        
        Console.WriteLine("\n╔══════════════════════════════════════════╗");
        Console.WriteLine("║              메인 메뉴                   ║");
        Console.WriteLine("╚══════════════════════════════════════════╝\n");
        
        Console.WriteLine("\n1. 상태보기");
        Console.WriteLine("2. 인벤토리");
        Console.WriteLine("3. 상점");
        Console.WriteLine("4. 던전입장 (전투)");
        Console.WriteLine("5. 휴식 (HP/MP 회복)");
        Console.WriteLine("6. 저장");
        Console.WriteLine("0. 게임 종료");
        
        Console.Write("\n선택 (1-6): ");
        string? input = Console.ReadLine();

        switch (input)
        {
            case "1" :
                Player.DisplayInfo();
                ConsoleUI.PressAnyKey();
                break;  
            case "2" :
                // 인벤토리 기능 구현 
                InventorySystem.ShowInventoryMenu(Player);
                break;
            case "3" :
                // 상점 기능 구현 
                ShopSystem.ShowShopMenu(Player, InventorySystem);
                break;
            case "4" :
                // 던전 입장 및 전투 기능 구현 
                EnterDungeon();
                break;
            case "5" :
                // 휴식 기능 구현
                Rest();
                break;
            case "6" :
                // 저장 기능 구현 
                SaveGame();
                break;
            case "0" :
                IsRunning = false;
                Console.WriteLine("\n게임을 종료합니다. 다음에 또 오세요~");
                break;
            default:
                Console.WriteLine("\n잘못된 입력입니다. 다시 선택해주세요");
                ConsoleUI.PressAnyKey();
                break;
        }
    }

    #endregion

    #region 메뉴 기능 

    // 던전입장 
    public void EnterDungeon()
    {
       Console.Clear();
       Console.WriteLine("\n던전에 입장했습니다!");
       
       // 적 캐릭터 생성
       Enemy enemy = Enemy.CreateEnemy(Player.Level);
       ConsoleUI.PressAnyKey();
       
       // 전투 시작
       BattleSystem.StartBattle(Player, enemy);
       
       Console.WriteLine("\n던전 탐험을 마치고 마을로 돌아갑니다....");
       ConsoleUI.PressAnyKey();
    }
    
    // 휴식 (HP/MP 회복)
    private void Rest()
    {
        // 상수(Constant) : 변경되지 않는 값
        const int restCost = 50; // 휴식 비용
        
        Console.Clear();
        Console.WriteLine("\n마을의 여관에서 휴식을 취합니다...");
        Console.WriteLine($"\n휴식 비용: {restCost} 골드");

        if (Player.Gold < restCost)
        {
            Console.WriteLine("\n골드가 부족합니다..ㅠ_ㅠ");
            ConsoleUI.PressAnyKey();
            return;
        }
        
        Console.Write("\n휴식을 취하시겠습니까? (Y/N): ");
        
        if (Console.ReadLine().ToUpper() == "Y")
        {
            Player.SpendGold(restCost);
            Player.HealHp(Player.MaxHp);
            Player.HealMp(Player.MaxMp);
            Console.WriteLine("\n휴식을 취했습니다. HP와 MP가 모두 회복되었습니다.");
            ConsoleUI.PressAnyKey();
        }
    }

    #endregion

    #region 저장 / 로드 기능 
    
    // 게임 저장
    public void SaveGame()
    {
        if(Player == null || InventorySystem == null)
        {
            Console.WriteLine("\n저장할 게임 데이터가 없습니다.");
            ConsoleUI.PressAnyKey();
            return;
        }

        if (SaveLoadSystem.SaveGame(Player, InventorySystem))
        {
            Console.WriteLine("\n정상적으로 게임이 저장 되었습니다.");
            ConsoleUI.PressAnyKey();
            
        }
    }
    
    // 게임 로드
    public bool LoadGame()
    {
        var savedData = SaveLoadSystem.LoadGame();
        if(savedData == null)       return false;
        
        // 1. 플레이어 데이터 복원
        Player = SaveLoadSystem.LoadPlayer(savedData.Player);
        
        // 2. 인벤토리 데이터 복원
        InventorySystem = SaveLoadSystem.LoadInventorySystem(savedData.Inventory, Player);
        
        // 3. 장착 아이템 복원
        SaveLoadSystem.LoadEquippedItems(Player, savedData.Player, InventorySystem);
        
        Console.WriteLine("\n게임 데이터를 불러왔습니다.");
        ConsoleUI.PressAnyKey();
        return true;
     
    }

    #endregion
}