using BSsenTextRPG.Utils;
using BSsenTextRPG.Data;
using BSsenTextRPG.Systems;

namespace BSsenTextRPG;

class Program
{
    static void Main(string[] args)
    {
        // 콘솔 인코딩 설정(한글 지원) 
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        // 저장된 게임 존재 여부 확인 
        if (SaveLoadSystem.IsSaveFileExists())
        {
            // 메뉴 오픈 (새게임 / 이어서하기 / 종료)
            ShowStartMenu();
        }
        else
        {
            // 게임 시작 
            GameManager.Instance.StartGame();
        }
       
    }

    static void ShowStartMenu()
    {
        Console.Clear();
        ConsoleUI.ShowTitle();
        
        Console.WriteLine("\n╔════════════════════════════════╗");
        Console.WriteLine("║       게임 시작!               ║");
        Console.WriteLine("╚════════════════════════════════╝\n");  
        
        Console.WriteLine("\n1. 새 게임");
        Console.WriteLine("2. 이어서 하기");
        Console.WriteLine("3. 종료");

        while (true)
        {
            Console.WriteLine("\n선택> ");
            
            string? input = Console.ReadLine();
            
            switch (input)
            {
                case "1":
                    // 새 게임 
                    GameManager.Instance.StartGame();
                    return;
                case "2":
                    // TODO : 이어서 하기 
                    if (GameManager.Instance.LoadGame())
                    {
                        GameManager.Instance.StartGame(true);
                    }
                    return;
                case "3":
                    // 종료 
                    Console.WriteLine("게임을 종료합니다.");
                    return;
                default:
                    Console.WriteLine("잘못된 입력입니다. 다시 선택해주세요.");
                    break;
            }
            
        }
    }
    
    
}