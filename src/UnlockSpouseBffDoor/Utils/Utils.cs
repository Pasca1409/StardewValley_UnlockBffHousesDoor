using StardewValley.Mods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnlockBffHousesDoor.Utils
{
  static class Utils
  {
    public static string ModUniqueId = "oatgh.UnlockBffHousesDoor";
    public static string UserKeyGivedFromSaveData = "oatgh.UnlockBffHousesDoor.Keys";
    public static int FriendshieldHeartValue = 250;
    public static int MinimumFriendshipPointsToGiveKey = 8; // 4 hearts

    public static void OverwriteData(this ModDataDictionary dict, string key, string value)
    {
      if(value is null)
        return;

      if (dict.ContainsKey(key))
        dict.Remove(key);

      dict.Add(key, value);
    }
  }
}
