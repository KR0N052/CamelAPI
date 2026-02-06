namespace CamelAPI
{
    public class Camel
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = null!;

        public string? Color { get; set; }

        private int _humpCount;
        public int HumpCount { 
            get => _humpCount;
            set
            {
                if (value != 1 && value != 2)
                    throw new ArgumentOutOfRangeException(nameof(HumpCount), "HumpCount must be 1 or 2.");

                _humpCount = value;
            }
        }

        public DateTime? LastFed { get; set; }

    }
}
