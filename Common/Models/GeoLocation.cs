using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    /// <summary>
    /// Class for geo location
    /// </summary>
    public class GeoLocation
    {
        /// <summary>
        /// Gets and sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets and sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets latitude.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets and sets longitude.
        /// </summary>
        public double Longitude { get; set; }
    }
}