namespace BSsenTextRPG.Models;

// 캐릭터 기본 추상 클래스 
public abstract class Character
{
    #region 프로퍼티
    public string Name { get; protected set; }
    public int CurrentHp { get; protected set; }
    public int MaxHp { get; protected set; }
    public int CurrentMp { get; protected set; }
    public int MaxMp { get; protected set; }
    public int AttackPower { get; protected set; }
    public int Defense{ get; protected set; }
    public int Level { get; protected set; }
    // 생존여부 
    public bool IsAlive => CurrentHp > 0;
    #endregion

    #region 생성자

    protected Character(string name, int maxHp, int maxMp, int attackPower, int defense, int level)
    {
        Name = name;
        MaxHp = maxHp;
        CurrentHp = maxHp;
        MaxMp = maxMp;
        CurrentMp = maxMp;
        AttackPower = attackPower;
        Defense = defense;
        Level = level;
    }
    #endregion

    #region 메서드

    

    #endregion
}