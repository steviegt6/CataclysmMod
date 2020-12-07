using Terraria.ModLoader;

namespace CataclysmMod
{
	public class CataclysmMod : Mod
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
	}
}