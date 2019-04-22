using System;
using System.IO;
using System.Reflection;

namespace OfflineMapDemo.Helpers
{
    public static class MbTilesHelper
    {
        public static void DeployMbTilesFile(Func<string, Stream> createFile)
        {
            var embeddedResourcesPath = "OfflineMapDemo.EmbeddedResources.";
            var mbTileFiles = new[] { "world.mbtiles", "el-molar.mbtiles", "torrejon-de-ardoz.mbtiles" };

            foreach (var mbTileFile in mbTileFiles)
            {
                CopyEmbeddedResourceToStorage(embeddedResourcesPath, mbTileFile, createFile);
            }
        }

        private static void CopyEmbeddedResourceToStorage(string embeddedResourcesPath, string mbTilesFile,
            Func<string, Stream> createFile)
        {
            var assembly = typeof(MbTilesSample).GetTypeInfo().Assembly;

            using (var image = assembly.GetManifestResourceStream(embeddedResourcesPath + mbTilesFile))
            {
                if (image == null) throw new ArgumentException("EmbeddedResource not found");
                using (var dest = createFile(mbTilesFile))
                {
                    image.CopyTo(dest);
                }
            }
        }
    }
}
