using Autodesk.Revit.DB;
using GetAirDeffuser.GetElementFrome_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GetAirDeffuser.UtilOperation
{
    public class GetSolid
    {
        GeometryElement geomElem = null;
        Solid geosolid1 = null;
        double originZ = -9999;
        double volume = 0;
        PlanarFace ductFace = null;
        XYZ directionX = null;
        XYZ directionY = null;
        XYZ directionZ = null;

        XYZ direction = null;
        public static string []terminalGrillName = new string[]
            {   
                "АМР",
                "АМН",
                "АДР"
                
            };
        public static string[] terminalDiffusorlName = new string[]
           {
                "ДПУ","4АП","VB"
           };
        public XYZ GetSolidForm(Document doc, Element elem, Level elementLevel, string elName)
        {
            FamilyInstance elem1 = elem as FamilyInstance;
            geomElem = elem.get_Geometry(new Options());
            geosolid1 = geomElem.FirstOrDefault() as Solid;
            //new PrintString().Print(elName);
            foreach (PlanarFace face in geosolid1.Faces)
            {
                if (face.Area  > volume)
                {
                    volume = face.Area;
                    directionX = face.XVector;
                    directionY = face.YVector;
                    directionZ = face.FaceNormal;
                    ductFace = face;
                }                
            }
            if (!terminalGrillName.Contains(elName.Substring(0, 3)))
            {
                foreach (Curve line in ductFace.GetEdgesAsCurveLoops().FirstOrDefault())
                {
                    Line line01 = line as Line;
                    directionZ = line01.Direction;
                    break;

                }
            }
           
           
            return directionZ;
        }
               
    }
}
