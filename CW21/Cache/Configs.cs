namespace CW21.Cache
{
    public class Configs
    {
        public PatientCache patient { get; set; }
    }

    public class PatientCache
    {
        public int SlidingExpiration { get; set; }
        public int AbsoluteExpiration { get; set; }
    }

}
