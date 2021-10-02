namespace CataclysmMod.Core.ModCompatibility
{
    public interface IModDependent
    {
        bool LoadWithValidMods();

        bool DependsOnMod();
    }
}