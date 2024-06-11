using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetAirDeffuser.GetElementFrome_Model
{
    internal class ElemensCollectionOfType
    {
        public ICollection<Element> GetCollection(Document doc, BuiltInCategory builtInCategory)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<Element> ElemensCollection = collector.OfCategory(builtInCategory).WhereElementIsElementType().ToElements();

            return ElemensCollection;
        }
    }
}
