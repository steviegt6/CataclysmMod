using Terraria.ModLoader;

namespace CataclysmMod
{
	public partial class CataclysmMod : Mod
	{
		public static CataclysmMod Instance { get; private set; }

		public CataclysmMod()
        {
			Instance = this;

			Properties = new ModProperties
			{
				Autoload = true,
				AutoloadBackgrounds = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
        }

		public override void Load() => LoadIL();

		public override void Unload() => UnloadIL();
    }
}