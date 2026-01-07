using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace UnlockBffHouses.Core
{
    public class KeyRingMenu : IClickableMenu
    {
        private List<NPC> unlockedFriends;
        private string hoverText = "";
        private ITranslationHelper i18n; // Hinzugefügt

        private const int SLOT_SIZE = 80;
        private const int COLS = 8;

        public KeyRingMenu(List<NPC> friends, ITranslationHelper translation)
            : base(0, 0, 0, 0, true)
        {
            this.unlockedFriends = friends;
            this.i18n = translation; // Speichern

            int rows = (int)Math.Ceiling(friends.Count / (double)COLS);
            if (rows < 1) rows = 1;

            this.width = COLS * SLOT_SIZE + IClickableMenu.borderWidth * 2;
            this.height = rows * SLOT_SIZE + IClickableMenu.borderWidth * 2 + 80;

            this.xPositionOnScreen = (Game1.viewport.Width - this.width) / 2;
            this.yPositionOnScreen = (Game1.viewport.Height - this.height) / 2;

            this.upperRightCloseButton = new ClickableTextureComponent(
                new Rectangle(this.xPositionOnScreen + this.width - 48, this.yPositionOnScreen - 20, 48, 48),
                Game1.mouseCursors,
                new Rectangle(337, 494, 12, 12),
                4f);
        }

        public override void draw(SpriteBatch b)
        {
            b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.6f);
            Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true);

            // Übersetzung für den Titel nutzen
            string title = this.i18n.Get("menu.title");
            Vector2 titleSize = Game1.dialogueFont.MeasureString(title);

            int titleBoxWidth = (int)titleSize.X + 64;
            int titleBoxHeight = 88;
            int titleX = this.xPositionOnScreen + (this.width - titleBoxWidth) / 2;
            int titleY = this.yPositionOnScreen - 30;

            IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60),
                titleX, titleY, titleBoxWidth, titleBoxHeight, Color.White, 1f, true);

            Utility.drawTextWithShadow(b, title, Game1.dialogueFont,
                new Vector2(titleX + 32, titleY + 28),
                Game1.textColor);

            int startX = this.xPositionOnScreen + IClickableMenu.borderWidth;
            int startY = this.yPositionOnScreen + IClickableMenu.borderWidth + 64;

            hoverText = "";

            for (int i = 0; i < unlockedFriends.Count; i++)
            {
                NPC npc = unlockedFriends[i];
                int col = i % COLS;
                int row = i / COLS;

                int posX = startX + col * SLOT_SIZE;
                int posY = startY + row * SLOT_SIZE;

                b.Draw(Game1.staminaRect, new Rectangle(posX, posY, 64, 64), Color.SaddleBrown * 0.3f);

                if (npc.Portrait != null)
                {
                    b.Draw(npc.Portrait, new Vector2(posX, posY), new Rectangle(0, 0, 64, 64), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.89f);
                }

                float iconScale = 2.0f;
                float xOffset = 64 - (16 * iconScale);
                float yOffset = 80 - (16 * iconScale);

                b.Draw(Game1.objectSpriteSheet,
                       new Vector2(posX + xOffset, posY + yOffset),
                       Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 912, 16, 16),
                       Color.White, 0f, Vector2.Zero, iconScale, SpriteEffects.None, 0.9f);

                if (Game1.getOldMouseX() >= posX && Game1.getOldMouseX() < posX + 64 &&
                    Game1.getOldMouseY() >= posY && Game1.getOldMouseY() < posY + 64)
                {
                    hoverText = npc.displayName;
                }
            }

            base.draw(b);
            this.drawMouse(b);

            if (!string.IsNullOrEmpty(hoverText))
            {
                IClickableMenu.drawHoverText(b, hoverText, Game1.smallFont);
            }
        }
    }
}