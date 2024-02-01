using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentPatcher.Framework.Patches;
using StardewModdingAPI;

namespace ContentPatcher.Framework.Migrations
{
    /// <summary>The base implementation for a game version migration. This can rewrite content packs ahead-of-time (like a format migration) or at runtime.</summary>
    internal abstract class GameMigration : BaseMigration, IGameMigration
    {
        /*********
        ** Accessors
        *********/
        /// <summary>The game version for which this migration applies.</summary>
        public ISemanticVersion GameVersion { get; }

        /// <summary>If set, content packs for which this migration applies will be listed in an <c>INFO</c> log with this message.</summary>
        /// <remarks>For example, <c>"Some content packs haven't been updated for Content Patcher 2.0.0 and Stardew Valley 1.6.0. Content Patcher will try to auto-migrate them, but compatibility isn't guaranteed."</c>.</remarks>
        public string? MigrationWarning { get; }

        public GameMigration(SemanticVersion gameVersion, string? warning, SemanticVersion formatVersion)
            : base(formatVersion)
        {
            this.MigrationWarning = warning;
            this.GameVersion = gameVersion;
        }


        /*********
        ** Public methods
        *********/
        /// <summary>Get the actual asset name to edit, if different from the one resolved from the patch data.</summary>
        /// <param name="assetName">The resolved asset name being loaded or edited.</param>
        /// <param name="patch">The load or edit patch being applied.</param>
        /// <returns>Returns the new asset name to load or edit instead, or <c>null</c> to keep the current one as-is.</returns>
        public virtual IAssetName? RedirectTarget(IAssetName assetName, IPatch patch)
        {
            return null;
        }

        /// <summary>Apply a load patch to the asset at runtime, overriding the normal apply log.</summary>
        /// <param name="patch">The load patch to apply, with any contextual values (e.g. token strings) already updated.</param>
        /// <param name="assetName">The resolved asset name being loaded.</param>
        /// <param name="asset">The loaded asset data.</param>
        /// <param name="error">An error message which indicates why migration failed.</param>
        /// <returns>Returns whether the load was overridden, so that the patch isn't applied normally after calling this method.</returns>
        public virtual bool TryApplyLoadPatch(LoadPatch patch, IAssetName assetName, [NotNullWhen(true)] out object? asset, out string? error)
        {
            asset = null;
            error = null;
            return false;
        }

        /// <summary>Apply an edit patch to the asset at runtime, overriding the normal apply log.</summary>
        /// <param name="patch">The edit patch to apply, with any contextual values (e.g. token strings) already updated.</param>
        /// <param name="assetName">The resolved asset name being loaded.</param>
        /// <param name="asset">The loaded asset data, with any previous patches in the list already applied.</param>
        /// <param name="error">An error message which indicates why migration failed.</param>
        /// <returns>Returns whether the edit was overridden, so that the patch isn't applied normally after calling this method.</returns>
        public virtual bool TryApplyEditPatch(IPatch patch, IAssetName assetName, object asset, out string? error)
        {
            error = null;
            return false;
        }


        /*********
        ** Protected methods
        *********/
        // Some helper methods to reduce duplication, like selecting a target field?
        // Or maybe modularize methods on the patch classes so they can be called from game migrations.
    }
}
