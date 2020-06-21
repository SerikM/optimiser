using System.Collections.Generic;

namespace Optimiser.Models
{
    public class Break : IData
    {
        public int Id { get; set; }
        public List<Rating> Ratings { get; set; }
        public List<CommercialType> DisallowedCommTypes { get; set; }
        public List<Commercial> Commercials { get; set; }
    }
}