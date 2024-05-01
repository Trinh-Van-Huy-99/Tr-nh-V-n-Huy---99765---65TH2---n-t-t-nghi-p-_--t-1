using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DATN_TRINHVANHUY_99765_65TH2.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_TRINHVANHUY_99765_65TH2.Command
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    internal class Cmd_VeThep : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                #region UIDocument and Document     
                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;
                #endregion
                ////goi ham 
                //ViewVeThep frmvethep = new ViewVeThep(doc);
                //frmvethep.ShowDialog();
                TaskDialog.Show("Trợ Giúp", $" Tác giả: Trịnh Văn Huy \n Mã số sinh viên: 99765 \n Lớp: 65TH2 \n Số điện thoại: 0967446817 \n Email: trinhvanhuy.vn@gmail.com ");
                return Result.Succeeded;
            }
            catch (Exception e)
            {
                return Result.Failed;
            }
            //double L1 = DATA.stirrups.l1;
           
        }

    }
}
