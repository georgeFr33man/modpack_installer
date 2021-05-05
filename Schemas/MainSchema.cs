using System.Collections.Generic;

namespace ModpackInstaller.Schemas
{
    public class MainSchema
    {
        public string MinecraftVersion { get; set; }
        public ModSchema Forge  { get; set; }
        public ModSchema Shaders  { get; set; }
        public ModSchema[] Mods { get; set; }
    }
}