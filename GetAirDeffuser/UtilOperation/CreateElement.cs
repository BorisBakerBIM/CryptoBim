using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetAirDeffuser.UtilOperation
{

    public class CreateElement
    {        
        BoundingBoxXYZ boxmin = null;
        double boxminx = 0;
        double boxminy = 0;
        double boxminz = 0;
        double boxmaxx = 0;
        double boxmaxy = 0;
        double boxmaxz = 0;
        double boxcentx = 0;
        double boxcenty = 0;
        double boxcentz = 0;
        XYZ center = null;       
        XYZ direction = XYZ.Zero;
        string familyName = string.Empty;
        Element newOpening = null;
        string name = string.Empty;
        Line lineX = null;
        Line lineY = null;
        Line lineZ = null;
        Line lineDiog = null;
        string strW = string.Empty;
        string strH = string.Empty;
        string flow = string.Empty;
        double w = 0;
        double h = 0;
        double flowVolum = 0;
        public void CreateDiffuser(Document doc, Element elem, View view, FamilySymbol familySymbol, Level elementLevel, string familyName)
        {
            boxmin = elem.get_BoundingBox(view);
            boxminx = boxmin.Min.X;
            boxminy = boxmin.Min.Y;
            boxminz = boxmin.Min.Z;
            boxmaxx = boxmin.Max.X;
            boxmaxy = boxmin.Max.Y;
            boxmaxz = boxmin.Max.Z;
            boxcentx = (boxminx + boxmaxx) / 2;
            boxcenty = (boxminy + boxmaxy) / 2;
            boxcentz = (boxminz + boxmaxz) / 2;

            try
            {
                lineX = Line.CreateBound(new XYZ(boxminx, 0, 0), new XYZ(boxmaxx, 0, 0));
                lineY = Line.CreateBound(new XYZ(0, boxminy, 0), new XYZ(0, boxmaxy, 0));
                lineZ = Line.CreateBound(new XYZ(0, 0, boxminz), new XYZ(0, 0, boxmaxz));
                lineDiog = Line.CreateBound(new XYZ(boxminx, boxminy, 0), new XYZ(boxmaxx, boxmaxy, 0));
            }
            catch { }
            if (GetSolid.terminalGrillName.Contains(familyName.Substring(0, 3)))
            {
                center = new XYZ(boxcentx, boxcenty, boxcentz);
            }
            else if (familyName.Contains("ДПУ"))
            {
                center = new XYZ(boxcentx, boxcenty, boxcentz );
                direction = new XYZ(0, 0, 0);
            }
            else if (familyName.Contains("4АП"))
            {
                center = new XYZ(boxcentx, boxcenty, boxcentz + lineZ.Length);
                
            }
            else
            {
                center = new XYZ(boxcentx, boxcenty, boxcentz);// - lineZ.Length);
            }

            familyName = familySymbol.Name;
            direction = new GetSolid().GetSolidForm(doc, elem, elementLevel, familyName);
            newOpening = doc.Create.NewFamilyInstance(center, familySymbol, direction, elementLevel, StructuralType.NonStructural);
            try
            {
                flow = elem.LookupParameter("MagiCAD.Расход").AsString().Split(' ')[0];
                flowVolum = Convert.ToDouble(flow) / 101.94;
                try
                {
                    newOpening.LookupParameter("ADSK_Расход воздуха").Set(flowVolum);
                }
                catch { }
                try
                {
                    newOpening.LookupParameter("Расход").Set(flowVolum);
                }
                catch { }
                newOpening.LookupParameter("Комментарии").Set("string:::" + flow + "после формулы" + flowVolum.ToString());
            }
            catch { }

            if (GetSolid.terminalGrillName.Contains(familyName))
            { 
                try
                {
                    strW = elem.LookupParameter("MagiCAD.Размер соединения (ВРУ)").AsString().Split('x')[0];
                    strH = elem.LookupParameter("MagiCAD.Размер соединения (ВРУ)").AsString().Split('x')[1];
                    w = Convert.ToDouble(strW) / 304.8;
                    h = Convert.ToDouble(strH) / 304.8;
                    if (lineZ.Length > lineX.Length && lineZ.Length > lineY.Length)
                    {
                        newOpening.LookupParameter("A Ширина решетки").Set(h);
                        newOpening.LookupParameter("B Высота решетки").Set(w);
                    }
                    else
                    {
                        newOpening.LookupParameter("A Ширина решетки").Set(w);
                        newOpening.LookupParameter("B Высота решетки").Set(h);
                    }
                }
                catch { }
            }
            if (familyName.Contains("ДПУ"))
            {
                try
                {
                    strW = elem.LookupParameter("MagiCAD.Размер соединения (ВРУ)").AsString().Split('x')[0];                    
                    w = Convert.ToDouble(strW) / 304.8;
                    newOpening.LookupParameter("D").Set(w);

                }
                catch { }
            }
        }
    }
}
