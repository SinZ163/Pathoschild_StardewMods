using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.Common.UI;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace Pathoschild.Stardew.Automate.Framework
{
    /// <summary>The overlay which highlights automatable machines.</summary>
    internal class OverlayMenu : BaseOverlay
    {
        /*********
        ** Fields
        *********/
        /// <summary>The padding to apply to tile backgrounds to make the grid visible.</summary>
        private readonly int TileGap = 1;

        /// <summary>The machine data for the current location.</summary>
        private readonly MachineDataForLocation? MachineData;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="events">The SMAPI events available for mods.</param>
        /// <param name="inputHelper">An API for checking and changing input state.</param>
        /// <param name="reflection">Simplifies access to private code.</param>
        /// <param name="machineData">The machine groups to display.</param>
        public OverlayMenu(IModEvents events, IInputHelper inputHelper, IReflectionHelper reflection, MachineDataForLocation? machineData)
            : base(events, inputHelper, reflection)
        {
            this.MachineData = machineData;
        }


        /*********
        ** Protected
        *********/
        /// <summary>Draw the overlay to the screen under the UI.</summary>
        /// <param name="spriteBatch">The sprite batch being drawn.</param>
        [SuppressMessage("ReSharper", "PossibleLossOfFraction", Justification = "Deliberate discarded for conversion to tile coordinates.")]
        protected override void DrawWorld(SpriteBatch spriteBatch)
        {
            if (!Context.IsPlayerFree)
                return;

            // draw each tile
            foreach (Vector2 tile in TileHelper.GetVisibleTiles(expand: 1))
            {
                // get tile's screen coordinates
                float screenX = tile.X * Game1.tileSize - Game1.viewport.X;
                float screenY = tile.Y * Game1.tileSize - Game1.viewport.Y;
                int tileSize = Game1.tileSize;

                // get machine group
                IMachineGroup? group = null;
                Color? color = null;
                if (this.MachineData is not null)
                {
                    if (this.MachineData.ActiveTiles.TryGetValue(tile, out group))
                        color = Color.Green * 0.2f;
                    else if (this.MachineData.DisabledTiles.TryGetValue(tile, out group) || this.MachineData.OutdatedTiles.ContainsKey(tile))
                        color = Color.Red * 0.2f;
                }
                color ??= Color.Black * 0.5f;

                // draw background
                spriteBatch.DrawLine(screenX + this.TileGap, screenY + this.TileGap, new Vector2(tileSize - this.TileGap * 2, tileSize - this.TileGap * 2), color);

                // draw group edge borders
                if (group != null)
                    this.DrawEdgeBorders(spriteBatch, group, tile, group.HasInternalAutomation ? Color.Green : Color.Red);
            }

            // draw cursor
            this.DrawCursor();
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Draw borders for each unconnected edge of a tile.</summary>
        /// <param name="spriteBatch">The sprite batch being drawn.</param>
        /// <param name="group">The machine group.</param>
        /// <param name="tile">The group tile.</param>
        /// <param name="color">The border color.</param>
        private void DrawEdgeBorders(SpriteBatch spriteBatch, IMachineGroup group, Vector2 tile, Color color)
        {
            int borderSize = 3;
            float screenX = tile.X * Game1.tileSize - Game1.viewport.X;
            float screenY = tile.Y * Game1.tileSize - Game1.viewport.Y;
            float tileSize = Game1.tileSize;

            // top
            if (!group.Tiles.Contains(new Vector2(tile.X, tile.Y - 1)))
                spriteBatch.DrawLine(screenX, screenY, new Vector2(tileSize, borderSize), color); // top

            // bottom
            if (!group.Tiles.Contains(new Vector2(tile.X, tile.Y + 1)))
                spriteBatch.DrawLine(screenX, screenY + tileSize, new Vector2(tileSize, borderSize), color); // bottom

            // left
            if (!group.Tiles.Contains(new Vector2(tile.X - 1, tile.Y)))
                spriteBatch.DrawLine(screenX, screenY, new Vector2(borderSize, tileSize), color); // left

            // right
            if (!group.Tiles.Contains(new Vector2(tile.X + 1, tile.Y)))
                spriteBatch.DrawLine(screenX + tileSize, screenY, new Vector2(borderSize, tileSize), color); // right
        }
    }
}
