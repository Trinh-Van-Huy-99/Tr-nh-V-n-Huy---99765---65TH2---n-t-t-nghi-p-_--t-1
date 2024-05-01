using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace DATN_TRINHVANHUY_99765_65TH2
{
    [TransactionAttribute(TransactionMode.Manual)]
    internal class CreateFloor : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                #region UIDocument and Document     
                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;
                #endregion


                return Result.Succeeded;
            }
            catch (Exception e)
            {
                return Result.Failed;
            }

        }

    }
}
