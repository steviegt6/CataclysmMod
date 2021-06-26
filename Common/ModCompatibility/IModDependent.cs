namespace CataclysmMod.Common.ModCompatibility
{
    public interface IModDependent
    {
        bool LoadWithValidMods();

        bool DependsOnMod();
    }
}