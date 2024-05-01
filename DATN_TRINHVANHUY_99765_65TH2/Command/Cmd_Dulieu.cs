using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DATN_TRINHVANHUY_99765_65TH2.Forms;
using DATN_TRINHVANHUY_99765_65TH2.OOP;
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
    internal class Cmd_Dulieu : IExternalCommand
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
                //TaskDialog.Show("Trợ Giúp", $" Tác giả: Trịnh Văn Huy \n Mã số sinh viên: 99765 \n Lớp: 65TH2 \n Số điện thoại: 0967446817 \n Email: trinhvanhuy.vn@gmail.com ");

                if (Cls_BienChuongTrinh.cls_ThepDoc.Checkboxkieunoichong1 == true || Cls_BienChuongTrinh.cls_ThepDoc.Checkboxkieunoichong2 == true || Cls_BienChuongTrinh.cls_ThepDoc.Checkboxkieunoichong3 == true)
                {
                    TaskDialog.Show("Dữ Liệu Của Cột",
                        $"ID: {Cls_BienChuongTrinh.cls_ThepDoc.ID}\n" +
                        $"Element: {Cls_BienChuongTrinh.cls_ThepDoc.Element.Category.Name}\n" +
                        $"Chiều cao: {Cls_BienChuongTrinh.cls_ThepDoc.Chieucao} mm\n" +
                        $"Chiều rộng: {Cls_BienChuongTrinh.cls_ThepDoc.Chieurong} mm\n" +
                        $"Chiều dài: {Cls_BienChuongTrinh.cls_ThepDoc.Chieudai} mm\n" +
                        $"Host ID: {Cls_BienChuongTrinh.cls_ThepDoc.HostID}\n" +
                        $"Số lượng thanh theo phương z: {Cls_BienChuongTrinh.cls_ThepDoc.Soluongphuongz} thanh\n" +
                        $"Số lượng thanh theo phương y: {Cls_BienChuongTrinh.cls_ThepDoc.Soluongphuongy} thanh\n" +
                        $"Chiều dày lớp bảo vệ cốt thép: {Cls_BienChuongTrinh.cls_ThepDoc.Cover} mm\n" +
                        $"Chiều dài thép nối chồng: {Cls_BienChuongTrinh.cls_ThepDoc.Overlap} mm\n" 
                        );
                   
                }
                else
                {
                    TaskDialog.Show("Dữ Liệu Của Cột",
                       $"ID: {Cls_BienChuongTrinh.cls_ThepDoc.ID}\n" +
                       $"Element: {Cls_BienChuongTrinh.cls_ThepDoc.Element.Category.Name}\n" +
                       $"Chiều cao: {Cls_BienChuongTrinh.cls_ThepDoc.Chieucao} mm\n" +
                       $"Chiều rộng: {Cls_BienChuongTrinh.cls_ThepDoc.Chieurong} mm\n" +
                       $"Chiều dài: {Cls_BienChuongTrinh.cls_ThepDoc.Chieudai} mm\n" +
                       $"Host ID: {Cls_BienChuongTrinh.cls_ThepDoc.HostID}\n" +
                       $"Số lượng thanh theo phương z: {Cls_BienChuongTrinh.cls_ThepDoc.Soluongphuongz} thanh\n" +
                       $"Số lượng thanh theo phương y: {Cls_BienChuongTrinh.cls_ThepDoc.Soluongphuongy} thanh\n" +
                       $"Chiều dày lớp bảo vệ cốt thép: {Cls_BienChuongTrinh.cls_ThepDoc.Cover} mm\n" 
                       //$"Chiều dài thép nối chồng: {Cls_BienChuongTrinh.cls_ThepDoc.Overlap} mm\n" +
                       );
                 

                }



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
