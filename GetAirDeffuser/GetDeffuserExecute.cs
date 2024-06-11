using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using GetAirDeffuser.GetElementFrome_Model;
using GetAirDeffuser.DiffuserOpperation;
using Autodesk.Revit.DB.Structure;
using System.Linq;




namespace GetAirDeffuser
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]

    public class GetDeffuserExecute : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication App = commandData.Application;
            Document doc = App.ActiveUIDocument.Document;
            UIDocument uidoc = new UIDocument(doc);
            View view = doc.ActiveView;           
            FamilySymbol familySymbol = new ElemensCollectionOfType().GetCollection(doc, BuiltInCategory.OST_DuctTerminal).Where(v => !v.Name.Contains("ДПУ")).FirstOrDefault() as FamilySymbol;
            using (Transaction tx01 = new Transaction(doc))
            {
                tx01.Start("Family activate");
                GetLinkDocument.GetLink(doc);
                FamilyOpeningType.GetOpeningFamily(doc);
                tx01.Commit();
            }



            foreach (Document linkDocument in GetLinkDocument.linkDocument)
            {
                using (Transaction tx01 = new Transaction(doc))
                {
                    tx01.Start("Family activate");
                        new GetDiffuserForm().GetDiffuser(linkDocument,doc, doc.ActiveView);
                    tx01.Commit();
                }
            }

                return Result.Succeeded;
           
        }
    }
}
