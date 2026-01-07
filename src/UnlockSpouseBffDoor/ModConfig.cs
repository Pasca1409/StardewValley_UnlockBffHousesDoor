using StardewModdingAPI;

namespace UnlockBffHouses.Core
{
    public class ModConfig
    {
        // Standard: 8 Herzen
        public int RequiredHearts { get; set; } = 8;

        // Standard: Taste K
        public SButton MenuKey { get; set; } = SButton.K;
    }
}