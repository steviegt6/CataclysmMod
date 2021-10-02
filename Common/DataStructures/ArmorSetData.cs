namespace CataclysmMod.Common.DataStructures
{
    public readonly struct ArmorSetData
    {
        public readonly int HeadType;
        public readonly int BodyType;
        public readonly int LegsType;
        public readonly string SetId;

        public ArmorSetData(int headType, int bodyType, int legsType, string setId)
        {
            HeadType = headType;
            BodyType = bodyType;
            LegsType = legsType;
            SetId = setId;
        }
    }
}