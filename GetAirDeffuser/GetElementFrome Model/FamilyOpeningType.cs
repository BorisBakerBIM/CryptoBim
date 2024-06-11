using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetAirDeffuser.GetElementFrome_Model
{
    public static class FamilyOpeningType
    {
        public static FamilySymbol openingFamily = null;
        public static List<FamilySymbol> familyList = new List<FamilySymbol>();
        public static void GetOpeningFamily(Document doc)
        {
            foreach (var element in new ElemensCollectionOfType().GetCollection(doc, BuiltInCategory.OST_GenericModel))
            {
                if (element.Name.Contains("Отверстие_Не учитывать"))
                {
                    openingFamily = element as FamilySymbol;
                        openingFamily.Activate();
                    familyList.Add(openingFamily);

                }
            }

        }


    }
}
