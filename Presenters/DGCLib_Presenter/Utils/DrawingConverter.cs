using DGCLib_Base;
using DGCLib_Base.DomainTypes;
using DGCLib_Base.DomainTypes.LinearAlgebra;
using DGCLib_Presenter.VisualizationTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DGCLib_Presenter
{
    public static class DrawingConverter
    {
        private static Dictionary<Type, GeometryConverter> _converterOverides = new Dictionary<Type, GeometryConverter>();
        public static Dictionary<Type, GeometryConverter> ConverterOverides { get { return _converterOverides; } }

        public delegate IEnumerable<IGeoDraw> GeometryConverter(IGeoStructure structure);

        public static IEnumerable<IGeoDraw> ToDrawableGeo(this IEnumerable<IGeoStructure> structs)
        {
            return structs
                .Where(o => o != null)
                .SelectMany<IGeoStructure, IGeoDraw>(o =>
                {
                    if (ConverterOverides.Count != 0)
                    {
                        GeometryConverter converter = null;
                        if (ConverterOverides.TryGetValue(o.GetType(), out converter) && converter != null)
                        {
                            return converter(o);
                        }
                    }

                    if (o is Point)
                        return ToDrawableGeo_dyn((Point)o);
                    if (o is Line)
                        return ToDrawableGeo_dyn(o as Line);
                    if (o is BezierSpline)
                        return ToDrawableGeo_dyn(o as BezierSpline);
                    if (o is Triangle)
                        return ToDrawableGeo_dyn(o as Triangle);
                    if (o is Polygon)
                        return ToDrawableGeo_dyn(o as Polygon);
                    if (o is Rectangle)
                        return ToDrawableGeo_dyn(o as Rectangle);
                    return new IGeoDraw[0];
                })
                .Where(o => o != null);
        }

        public static PointD ToPointD(this Point p)
        {
            return new PointD(p.X, p.Y);
        }

        public static Point ToPoint(this PointD p)
        {
            return new Point(p.X, p.Y);
        }

        private static TextPoint GetLenghtLabel(this Line line)
        {
            return new TextPoint(line.Center.ToPointD(), line.Length.ToString("N3"));
        }

        private static IEnumerable<IGeoDraw> ToDrawableGeo_dyn(Point vector)
        {
            yield return new GeoPoint(vector.ToPointD(), ColorF.Black, 3);
        }

        private static IEnumerable<IGeoDraw> ToDrawableGeo_dyn(Line line)
        {
            yield return new GeoSpline(line.Start.ToPointD(), line.End.ToPointD(), ColorF.Gray, 10, true, line.GetLenghtLabel());
        }

        private static IEnumerable<IGeoDraw> ToDrawableGeo_dyn(BezierSpline spline)
        {
            yield return new GeoSpline(
                spline.PosA.ToPointD(), spline.PosB.ToPointD(), spline.PosC.ToPointD(), spline.PosD.ToPointD(), ColorF.Green.SetAlpha(0.5f), 2, true);
        }

        private static IEnumerable<IGeoDraw> ToDrawableGeo_dyn(Triangle tri)
        {
            foreach (var line in tri.EdgeLines)
            {
                yield return new GeoSpline(line.Start.ToPointD(), line.End.ToPointD(), ColorF.Gray, 10, true, line.GetLenghtLabel());
            }
        }

        private static IEnumerable<IGeoDraw> ToDrawableGeo_dyn(Polygon poly)
        {
            foreach (var line in poly.EdgeLines)
            {
                yield return new GeoSpline(line.Start.ToPointD(), line.End.ToPointD(), ColorF.Gray, 10, true, line.GetLenghtLabel());
            }
        }

        private static IEnumerable<IGeoDraw> ToDrawableGeo_dyn(Rectangle rect)
        {
            foreach (var line in rect.EdgeLines)
            {
                yield return new GeoSpline(line.Start.ToPointD(), line.End.ToPointD(), ColorF.Gray, 10, true, line.GetLenghtLabel());
            }
        }

        private static IEnumerable<IGeoDraw> ToDrawableGeo_dyn(IGeoDraw vector)
        {
            yield return new GeoPoint(new PointD(0, 0), ColorF.Red, 10);
        }
    }
}