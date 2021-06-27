namespace CataclysmMod.Common.ArmorSets
{
    public readonly struct ArmorSet
    {
        public readonly int HeadType;
        public readonly int BodyType;
        public readonly int LegsType;
        public readonly string SetId;

        public ArmorSet(int headType, int bodyType, int legsType, string setId)
        {
            HeadType = headType;
            BodyType = bodyType;
            LegsType = legsType;
            SetId = setId;
        }
    }
}