using DGCLib_Base;
using DGCLib_Base.DomainTypes;
using DGCLib_Base.DomainTypes.LinearAlgebra;
using System.Collections.Generic;
using System.Linq;

namespace DGCLib_Misc
{
    public class Convex_Triangulation : IAlgorithm<Triangle>
    {
        [BindInGUI]
        public IEnumerable<Point> Points { get; set; }

        public List<Triangle> Compute()
        {
            if (!Points.HasAtLeast(3))
                return new List<Triangle>();

            List<Triangle> triangles = new List<Triangle>();

            var s = Points.First();

            var prev = Points.Skip(1).First();
            foreach (var p in Points.Skip(2))
            {
                triangles.Add(new Triangle(s, prev, p));
                prev = p;
            }

            return triangles;
        }
        
        IEnumerable<Triangle> IAlgorithm<Triangle>.Compute()
        {
            return Compute();
        }
    }
}