using ContentPatcher.Framework.Patches;
using StardewModdingAPI;
using System.Diagnostics.CodeAnalysis;

namespace ContentPatcher.Framework.Migrations
{
    internal interface IGameMigration
    {
        /*********
        ** Accessors
        *********/
        /// <summary>The game version for which this migration applies.</summary>
        public ISemanticVersion GameVersion { get; }

        /// <summary>If set, content packs for which this migration applies will be listed in an <c>INFO</c> log with this message.</summary>
        /// <remarks>For example, <c>"Some content packs haven't been updated for Content Patcher 2.0.0 and Stardew Valley 1.6.0. Content Patcher will try to auto-migrate them, but compatibility isn't guaranteed."</c>.</remarks>
        public string? MigrationWarning { get; }


        /*********
        ** Public methods
        *********/
        /// <summary>Get the actual asset name to edit, if different from the one resolved from the patch data.</summary>
        /// <param name="assetName">The resolved asset name being loaded or edited.</param>
        /// <param name="patch">The load or edit patch being applied.</param>
        /// <returns>Returns the new asset name to load or edit instead, or <c>null</c> to keep the current one as-is.</returns>
        public IAssetName? RedirectTarget(IAssetName assetName, IPatch patch);

        /// <summary>Apply a load patch to the asset at runtime, overriding the normal apply log.</summary>
        /// <param name="patch">The load patch to apply, with any contextual values (e.g. token strings) already updated.</param>
        /// <param name="assetName">The resolved asset name being loaded.</param>
        /// <param name="asset">The loaded asset data.</param>
        /// <param name="error">An error message which indicates why migration failed.</param>
        /// <returns>Returns whether the load was overridden, so that the patch isn't applied normally after calling this method.</returns>
        public bool TryApplyLoadPatch(LoadPatch patch, IAssetName assetName, [NotNullWhen(true)] out object? asset, out string? error);

        /// <summary>Apply an edit patch to the asset at runtime, overriding the normal apply log.</summary>
        /// <param name="patch">The edit patch to apply, with any contextual values (e.g. token strings) already updated.</param>
        /// <param name="assetName">The resolved asset name being loaded.</param>
        /// <param name="asset">The loaded asset data, with any previous patches in the list already applied.</param>
        /// <param name="error">An error message which indicates why migration failed.</param>
        /// <returns>Returns whether the edit was overridden, so that the patch isn't applied normally after calling this method.</returns>
        public bool TryApplyEditPatch(IPatch patch, IAssetName assetName, object asset, out string? error);
    }
}
