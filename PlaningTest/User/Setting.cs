using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaningTest.User
{
    public class setting
    {
        public setting()
        {
            ArcLength = 1;
            ArcDeGreeMode = true;
        }
        public bool ArcDeGreeMode { get; set; }
        private double ArcLength { get; set; }
        private double ArcDegree { get; set; }
        public double arcLength()
        {
            return ArcLength;
        }
        public void setArcLength(double arclength)
        {
            ArcLength = arclength;
        }
        public double arcDegree()
        {
            return ArcDegree;
        }
        public void setArcDegree(double arcDegree)
        {
            ArcDegree = arcDegree;
        }
    }
}
