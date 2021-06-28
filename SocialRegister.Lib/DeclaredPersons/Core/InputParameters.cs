namespace SocialRegister.Lib
{
    public class InputParameters
    {
        public int DistrictId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        private int _limit;
        public int Limit
        {
            get
            {
                if (_limit == 0)
                    _limit = 100;

                return _limit;
            }
            set => _limit = value;
        }

        public string GroupBy { get; set; } = "y";
    }
}
