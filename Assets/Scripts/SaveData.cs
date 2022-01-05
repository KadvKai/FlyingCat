using System;

[Serializable]
public class SaveData
{
    public string UserName=null;
    public bool AdForChild=true;
    public bool PersonalizationAds = false;
    public int StarQuantity=10;
    public int TimeToStar = 60;
    public DateTime ExitTime= DateTime.UtcNow;
}
