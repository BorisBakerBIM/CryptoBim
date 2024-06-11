using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetAirDeffuser.GetElementFrome_Model
{
    public class AllElemensCollection
    {
        public ICollection<ElementId> GetCollection(Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<ElementId> ElemenIdsCollection = collector.WhereElementIsNotElementType().ToElementIds();

            return ElemenIdsCollection;
        }

    }
}
