using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace UnlockBffHouses.Core
{
    public class ModEntry : Mod
    {
        private ModConfig Config;

        public override void Entry(IModHelper helper)
        {
            this.Config = helper.ReadConfig<ModConfig>();
            helper.Events.Input.ButtonPressed += OnButtonPressed;
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu == null) return;

            configMenu.Register(
                mod: this.ModManifest,
                reset: () => this.Config = new ModConfig(),
                save: () => this.Helper.WriteConfig(this.Config)
            );

            // Visueller Titel & Beschreibung (Übersetzt!)
            configMenu.AddSectionTitle(
                mod: this.ModManifest,
                text: () => this.Helper.Translation.Get("mod.name")
            );

            configMenu.AddParagraph(
                mod: this.ModManifest,
                text: () => this.Helper.Translation.Get("mod.description")
            );

            configMenu.AddNumberOption(
                mod: this.ModManifest,
                getValue: () => this.Config.RequiredHearts,
                setValue: value => this.Config.RequiredHearts = value,
                name: () => this.Helper.Translation.Get("config.hearts.name"),
                min: 0, max: 14
            );

            configMenu.AddKeybind(
                mod: this.ModManifest,
                getValue: () => this.Config.MenuKey,
                setValue: value => this.Config.MenuKey = value,
                name: () => this.Helper.Translation.Get("config.key.name")
            );
        }

        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady || !Context.IsPlayerFree) return;

            // Menü öffnen
            if (e.Button == this.Config.MenuKey)
            {
                OpenKeyRingMenu();
                return;
            }

            // Interaktion mit Türen
            if (!e.Button.IsActionButton()) return;

            Vector2 tile = Game1.player.GetGrabTile();
            GameLocation location = Game1.currentLocation;

            string action = location.doesTileHaveProperty((int)tile.X, (int)tile.Y, "Action", "Buildings");
            if (string.IsNullOrWhiteSpace(action)) return;

            string[] parts = action.Split(' ');
            if (parts[0] != "LockedDoorWarp" || parts.Length < 4) return;

            if (!int.TryParse(parts[1], out int targetX) || !int.TryParse(parts[2], out int targetY)) return;
            string targetMapName = parts[3];

            // 1. Hole ALLE passenden Freunde als Liste
            List<NPC> friends = GetFriendsForMap(targetMapName);

            if (friends.Count > 0)
            {
                this.Helper.Input.Suppress(e.Button);
                location.playSound("doorClose");
                Game1.warpFarmer(targetMapName, targetX, targetY, false);

                // 2. Namen extrahieren
                List<string> names = new List<string>();
                foreach (NPC friend in friends)
                {
                    names.Add(friend.displayName);
                }

                // 3. Formatierung: "A, B & C"
                string allNames;
                if (names.Count == 1)
                {
                    allNames = names[0];
                }
                else
                {
                    // Alle Namen außer dem letzten mit ", " verbinden
                    string allButLast = string.Join(", ", names.GetRange(0, names.Count - 1));
                    // Den letzten Namen holen
                    string lastOne = names[names.Count - 1];
                    // Zusammenfügen mit " & "
                    allNames = allButLast + " & " + lastOne;
                }

                // 4. Anzeigen (mit dem Replace-Trick, damit es sicher funktioniert)
                string text = this.Helper.Translation.Get("message.used-key").ToString();
                text = text.Replace("{name}", allNames);
                Game1.showGlobalMessage(text);
            }
        }

        private void OpenKeyRingMenu()
        {
            List<NPC> keys = new List<NPC>();
            int requiredPoints = this.Config.RequiredHearts * 250;

            foreach (NPC npc in Utility.getAllCharacters())
            {
                if (!npc.isVillager()) continue;
                if (Game1.player.getFriendshipLevelForNPC(npc.Name) >= requiredPoints)
                {
                    keys.Add(npc);
                }
            }

            if (keys.Count > 0)
                // Wir geben den Translation-Helper an das Menü weiter
                Game1.activeClickableMenu = new KeyRingMenu(keys, this.Helper.Translation);
            else
                Game1.showGlobalMessage(this.Helper.Translation.Get("menu.no-keys"));
        }

        // Hilfsmethode: Gibt jetzt eine LISTE zurück (Mehrzahl)
        private List<NPC> GetFriendsForMap(string mapName)
        {
            List<NPC> residents = new List<NPC>();
            int requiredPoints = this.Config.RequiredHearts * 250;

            foreach (NPC npc in Utility.getAllCharacters())
            {
                if (!npc.isVillager()) continue;

                bool isResident = (npc.DefaultMap == mapName) ||
                                  (mapName.Contains(npc.Name)) ||
                                  (npc.getHome()?.Name == mapName);

                if (isResident && Game1.player.getFriendshipLevelForNPC(npc.Name) >= requiredPoints)
                {
                    residents.Add(npc);
                }
            }
            return residents;
        }
    }
}