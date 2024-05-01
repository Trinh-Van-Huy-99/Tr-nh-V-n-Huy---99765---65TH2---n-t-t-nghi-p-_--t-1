using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

//using DATN_TRINHVANHUY_99765_65TH2.LoadFilePath;

namespace DATN_TRINHVANHUY_99765_65TH2
{
    [TransactionAttribute(TransactionMode.Manual)]
    static public class GlobalVariable
    {
        //Load the dxf file from its path.
        public static string file = null;
    }
    [TransactionAttribute(TransactionMode.Manual)]
    public class CreateGrids : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                #region UIDocument and Document     
                //Determine the UIDocument and the Document in revit to draw our elements.
                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                #endregion




                //Creating the Revit Grids.

                //using (Transaction t = new Transaction(doc, "Draw Grids from dxf file"))
                //{
                //    t.Start();
                //    foreach (Autodesk.Revit.DB.Line line in GridLines)
                //    {
                //        Grid.Create(doc, line);
                //    }
                //    t.Commit();
                //}

                return Result.Succeeded;
            }
            catch (Exception e)
            {
                return Result.Failed;
            }
        }
    }
}
