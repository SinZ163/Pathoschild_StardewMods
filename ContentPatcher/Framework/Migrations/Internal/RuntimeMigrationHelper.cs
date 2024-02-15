using System.Collections.Generic;
using StardewValley;
using StardewValley.ItemTypeDefinitions;

namespace ContentPatcher.Framework.Migrations.Internal
{
    /// <summary>Provides utility methods for implementing runtime migrations.</summary>
    internal static class RuntimeMigrationHelper
    {
        /*********
        ** Fields
        *********/
        /// <summary>The backing cache for <see cref="ParseObjectId"/>.</summary>
        private static readonly Dictionary<string, string?> ParseObjectIdCache = new();


        /*********
        ** Public methods
        *********/
        /// <summary>Get the unqualified object ID, if it's a valid object ID.</summary>
        /// <param name="rawItemId">The raw item ID, which may be an item query or non-object ID.</param>
        /// <returns>Returns the unqualified object ID, or <c>null</c> if it's not a valid object ID.</returns>
        public static string? ParseObjectId(string? rawItemId)
        {
            // skip null
            if (rawItemId is null)
                return null;

            // skip cached
            var cache = RuntimeMigrationHelper.ParseObjectIdCache;
            {
                if (cache.TryGetValue(rawItemId, out string? cached))
                    return cached;
            }

            // skip non-object-ID value
            ItemMetadata metadata = ItemRegistry.GetMetadata(rawItemId);
            if (metadata?.Exists() is not true || metadata.TypeIdentifier != ItemRegistry.type_object)
            {
                cache[rawItemId] = null;
                return null;
            }

            // apply
            cache[rawItemId] = metadata.LocalItemId;
            return metadata.LocalItemId;
        }

        /// <summary>Count the number of fields in a delimited string.</summary>
        /// <param name="row">The row in which to count fields.</param>
        /// <param name="delimiter">The character which delimits fields in the row.</param>
        public static int CountFields(string row, char delimiter = '/')
        {
            int count = 1; // count field before first delimiter

            int lastIndex = -1;
            while (true)
            {
                lastIndex = row.IndexOf('/', lastIndex + 1);

                if (lastIndex == -1)
                    break;

                count++;
            }

            return count;
        }
    }
}