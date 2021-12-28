using System;

[Serializable]
public class SaveData
{
    public string UserName;
    public int UserAge;
    public int StarQuantity=10;
    public int TimeToStar = 60;
    public DateTime ExitTime= DateTime.UtcNow;
}
