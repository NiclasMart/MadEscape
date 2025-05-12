using Stats;

namespace CharacterProgressionMatrix
{
    public class Mod : ISocketable
    {
        Stat _stat;
        float _value;

        public Mod(ModTemplate modTemplate)
        {
            _stat = modTemplate.ModifiedStat;
            _value = modTemplate.Value;
        }

    }
}