using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.Structure;
using GetAirDeffuser.GetElementFrome_Model;
using GetAirDeffuser.UtilOperation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using GetAirDeffuser.UtilOperation;

namespace GetAirDeffuser.DiffuserOpperation
{
   public class GetDiffuserForm
    {
        GeometryElement geomElem = null;
        GeometryInstance geomElem1 = null;
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
        string magiTypeComponent = string.Empty;
        string magiNameType = string.Empty;
        string magiSize = string.Empty;
        string magiSystem = string.Empty;
        double magiLinght = 0.3;
        double magiUpp = 0.3;
        double magiDown = 0.3;
        double magiIzol = 0;
        XYZ direction = XYZ.Zero;
        string familyName = string.Empty;
        string familyLongName = string.Empty;
        Element newOpening = null;
        string name= string.Empty;
        FamilySymbol familySymbol= null;


        public void GetDiffuser(Document linkdoc, Document doc, View view)
        {
            
            Level elementLevel = new ElemensCollection().GetCollection(doc, BuiltInCategory.OST_Levels)
                .Where(v => v.LookupParameter("Фасад").AsDouble() == 0).FirstOrDefault() as Level;
            
            //FamilySymbol familySymbol = new ElemensCollectionOfType().GetCollection(doc, BuiltInCategory.OST_DuctTerminal).Where(v => v.Name.Contains("4АПН")).FirstOrDefault() as FamilySymbol;
            
            List<string> list = new List<string>();
            //FamilySymbol familySymbol = FamilyOpeningType.openingFamily;
            Dictionary<string, FamilySymbol> grilDictionaryP = new Dictionary<string, FamilySymbol>();
            Dictionary<string, FamilySymbol> grilDictionaryS = new Dictionary<string, FamilySymbol>();
            Dictionary<string, FamilySymbol> gril4APDictionaryP = new Dictionary<string, FamilySymbol>();
            Dictionary<string, FamilySymbol> gril4APDictionaryS = new Dictionary<string, FamilySymbol>();
            Dictionary<string, FamilySymbol> grilVBDDictionaryP = new Dictionary<string, FamilySymbol>();
            Dictionary<string, FamilySymbol> grilVBDDictionaryS = new Dictionary<string, FamilySymbol>();
            FamilySymbol grilADN_S= null;
            FamilySymbol grilADN_E = null;
            FamilySymbol diffDpu_S = null;
            FamilySymbol diffDpu_E = null;
            foreach (Element el in new ElemensCollectionOfType().GetCollection(doc, BuiltInCategory.OST_DuctTerminal))
                {
                    try
                    {
                        FamilySymbol familySymbol=el as FamilySymbol;
                        if (el.Name.Contains("4АП") &&  familySymbol.Family.Name.Contains("прит"))
                        {
                            grilDictionaryP.Add(el.LookupParameter("Длина итог").AsValueString(), familySymbol);
                        }
                        if (el.Name.Contains("4АП")  && familySymbol.Family.Name.Contains("выт"))
                        {
                            grilDictionaryS.Add(el.LookupParameter("Длина итог").AsValueString(), familySymbol);
                        }
                       

                    if (el.Name.Contains("ВБ")  && familySymbol.Family.Name.Contains("прит"))
                        {
                            grilVBDDictionaryP.Add(el.LookupParameter("Длина итог").AsValueString(), familySymbol);
                        }
                        if (el.Name.Contains("ВБ")  && familySymbol.Family.Name.Contains("выт"))
                        {
                            grilVBDDictionaryS.Add(el.LookupParameter("Длина итог").AsValueString(), familySymbol);
                        }
                    if (GetSolid.terminalGrillName.Contains(el.Name.Substring(0, 3))  && familySymbol.Family.Name.Contains("выт"))
                        {
                            grilADN_E= familySymbol;
                        }
                        if (GetSolid.terminalGrillName.Contains(el.Name.Substring(0, 3)) && familySymbol.Family.Name.Contains("прит"))
                        {
                            grilADN_S = familySymbol;
                        }
                        if (el.Name.Contains("ДПУ") && familySymbol.Family.Name.Contains("выт"))
                        {
                            diffDpu_E = familySymbol;
                        }
                        if (el.Name.Contains("ДПУ") && familySymbol.Family.Name.Contains("прит"))
                        {
                            diffDpu_S = familySymbol;
                        }
                }
                    catch
                    {

                    }
                }
            
           

            foreach (Element elem in new ElemensCollection().GetCollection(linkdoc, BuiltInCategory.OST_GenericModel)
                    .Where(v => v.GetParameters("MagiCAD.Тип компонента")[0].AsString() != null && v.GetParameters("MagiCAD.Тип компонента")[0].AsString().Contains("Приточное ВРУ")))//BuiltInCategory.OST_DuctTerminal))
                {
                //familySymbol = new ElemensCollectionOfType().GetCollection(doc, BuiltInCategory.OST_DuctTerminal).Where(v => v.Name.Contains("4АПН + 3КСД 450х450 верх круг П")).FirstOrDefault() as FamilySymbol;
                //familyName = familySymbol.Name;
                
                try
                {
                    name = elem.GetParameters("MagiCAD.Имя типа")[0].AsString();
                   
                    familyName = elem.GetParameters("MagiCAD.Имя типа")[0].AsString().Substring(0, 3);
                    if (familyName.Contains("4А"))
                    {
                        foreach (var el in grilDictionaryP)
                        {
                            if (name.Contains(el.Key))
                            {
                                familySymbol = el.Value;
                                familySymbol.Activate();
                                break;

                            }

                        }
                    }
                    if (familyName.Contains("2V"))
                    {
                        foreach (var el in grilVBDDictionaryP)
                        {
                            if (name.Contains(el.Key))
                            {
                                familySymbol = el.Value;
                                familySymbol.Activate();
                                break;

                            }

                        }
                    }
                    if (GetSolid.terminalGrillName.Contains(familyName))
                    {
                        familySymbol = grilADN_S;
                        familySymbol.Activate();

                    }
                    if (familyName.Contains("ДПУ"))
                    {
                        familySymbol = diffDpu_S;
                        familySymbol.Activate();

                    }
                    new PrintString().Print(familyName);

                    familyName = familySymbol.Name;
                      
                        new CreateElement().CreateDiffuser(doc, elem, view, familySymbol, elementLevel, familyName);
                }
                catch { }

                //familySymbol = new ElemensCollectionOfType().GetCollection(doc, BuiltInCategory.OST_DuctTerminal)
                //    .Where(v => !v.Name.Contains(elem.GetParameters("MagiCAD.Компонент(вытяжной)")[0].AsString().Split(' ')[0])).FirstOrDefault() as FamilySymbol;
            }
            
            foreach (Element elem in new ElemensCollection().GetCollection(linkdoc, BuiltInCategory.OST_GenericModel)
                .Where(v => v.GetParameters("MagiCAD.Тип компонента")[0].AsString() != null && v.GetParameters("MagiCAD.Тип компонента")[0].AsString().Contains("Вытяжное устройство")))
                {
                
                //familySymbol = new ElemensCollectionOfType().GetCollection(doc, BuiltInCategory.OST_DuctTerminal).Where(v => v.Name.Contains("4АПН + 3КСД 450х450 верх круг В")).FirstOrDefault() as FamilySymbol;
                //familyName = familySymbol.Name;
                try
                {
                    name = elem.GetParameters("MagiCAD.Имя типа")[0].AsString();
                    familyName = elem.GetParameters("MagiCAD.Имя типа")[0].AsString().Substring(0, 3);
                    new PrintString().Print(familyName);

                    if (familyName.Contains("4А"))
                    {
                        foreach (var el in grilDictionaryS)
                        {
                            if (name.Contains(el.Key))
                            {
                                familySymbol = el.Value;
                                familySymbol.Activate();
                                break;

                            }

                        }
                    }

                    if (familyName.Contains("2V"))
                    {
                        foreach (var el in grilVBDDictionaryS)
                        {
                            if (name.Contains(el.Key))
                            {
                                familySymbol = el.Value;
                                familySymbol.Activate();
                                break;

                            }

                        }
                    }
                    if (GetSolid.terminalGrillName.Contains(familyName))
                    {
                        familySymbol = grilADN_E;
                        familySymbol.Activate();

                    }
                    if (familyName.Contains("ДПУ"))
                    {
                        familySymbol = diffDpu_E;
                        familySymbol.Activate();

                    }
                    //familySymbol = new ElemensCollectionOfType().GetCollection(doc, BuiltInCategory.OST_DuctTerminal)
                    //        .Where(v => v.Name.Contains(familyName)).FirstOrDefault() as FamilySymbol;
                    familyName = familySymbol.Name;
                        new CreateElement().CreateDiffuser(doc, elem, view, familySymbol, elementLevel, familyName);
                }
                catch { }

                //familySymbol = new ElemensCollectionOfType().GetCollection(doc, BuiltInCategory.OST_DuctTerminal)
                //   .Where(v => !v.Name.Contains(elem.GetParameters("MagiCAD.Компонент(приточный)")[0].AsString().Split(' ')[0])).FirstOrDefault() as FamilySymbol;
            }

                    
               
           
        

        }
    }
}
