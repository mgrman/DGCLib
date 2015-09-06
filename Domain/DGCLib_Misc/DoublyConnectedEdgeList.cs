using DGCLib_Base;
using DGCLib_Base.DomainTypes;
using DGCLib_Base.DomainTypes.LinearAlgebra;
using System.Collections.Generic;

//WIP

namespace DGCLib_Misc
{
    internal class DoublyConnectedEdgeList
    {
        public List<EL_Point> Points { get; private set; }
        public List<EL_Edge> Edges { get; private set; }
        public List<EL_Polygon> Polygons { get; private set; }

        public DoublyConnectedEdgeList()
        {
            Points = new List<EL_Point>();
            Edges = new List<EL_Edge>();
            Polygons = new List<EL_Polygon>();
        }

        public List<IGeoStructure> ToGeoStruct()
        {
            List<IGeoStructure> lines = new List<IGeoStructure>();
            foreach (var e in Edges)
            {
                Point a = e.Tail.Position;
                Point b = e.Twin.Tail.Position;
                lines.Add(new Line(a, b));
            }
            foreach (var p in Points)
            {
                Point a = p.Position;
                lines.Add(a);
            }
            return lines;
        }
    }

    internal class EL_Point
    {
        public EL_Edge Edge { get; set; }       /* Edge.Tail == this */
        public Point Position { get; set; }

        public EL_Point(Point p)
        {
            Position = p;
        }

        public EL_Point()
        {
        }
    }

    internal class EL_Edge
    {
        public EL_Edge Previous { get; set; }   /* Previous.Next == this */
        public EL_Edge Next { get; set; }       /* Next.Previous == this */
        public EL_Edge Twin { get; set; }       /* Twin.Twin == this */
        public EL_Point Tail { get; set; }      /* Twin.Next.Tail == Tail &&  Previous.Twin.Tail == Tail */
        public EL_Polygon Polygon { get; set; } /* prev->left == left && next->left == left */
    }

    internal class EL_Polygon
    {
        public EL_Edge Edge { get; set; }       /* Edge.Left == this */
    }
}