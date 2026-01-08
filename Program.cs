using BSsenTextRPG.Utils;
using BSsenTextRPG.Data;

namespace BSsenTextRPG;

class Program
{
    static void Main(string[] args)
    {
        // 콘솔 인코딩 설정(한글 지원) 
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        // TODO : 저장된 게임 존재 여부 확인 
        // TODO : 게임 로드 및 새 게임 시작 
        
        // 게임 시작 
        GameManager.Instance.StartGame();
    }
}