using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DATN_TRINHVANHUY_99765_65TH2.Forms;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI.Selection;
using DATN_TRINHVANHUY_99765_65TH2.OOP;
using static DATN_TRINHVANHUY_99765_65TH2.Command.Cmd_ChonCot;

namespace DATN_TRINHVANHUY_99765_65TH2.Command
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    internal class Cmd_LoadData : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                #region UIDocument and Document     
                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;
                #endregion

                //lấy dữ liệu rebartype
                Cls_BienChuongTrinh.Rvt_RebarBarTypes = Cls_BienChuongTrinh.GetRebarBarTypes(doc);
                Cls_BienChuongTrinh.Rvt_RebarBarShapes = Cls_BienChuongTrinh.GetRebarShapes(doc);
                Cls_BienChuongTrinh.Rvt_RebarRebarHookTypes = Cls_BienChuongTrinh.GetRebarHookTypes(doc);



                ISelectionFilter selFilter = new MassSelectionFilter();
                Selection choices = uidoc.Selection;
                // Pick one object from Revit.
                try
                {
                    Reference pick_column = choices.PickObject(ObjectType.Element, selFilter, "Chọn đối tượng cột cần vẽ thép");
                    if (pick_column != null)
                    {
                        // Tiếp tục xử lý khi đối tượng được chọn
                        foreach (Cls_Cot cot in Cls_BienChuongTrinh.DanhSach_Cot)
                        {
                            if (cot.ID == pick_column.ElementId)
                            {
                                // Cột đã được vẽ thép
                                return Result.Succeeded;
                            }
                        }

                        // Lấy đối tượng cột từ Reference
                        Element column = doc.GetElement(pick_column.ElementId);

                        Cls_BienChuongTrinh.cls_ThepDoc.Element = column;
                        Cls_BienChuongTrinh.cls_ThepDai.Element = column;

                        FamilyInstance columnInstance = column as FamilyInstance;

                        ColumnFramReinMaker m_dataBuffer = new ColumnFramReinMaker(commandData, columnInstance);
                        // Goi form ViewLoadData và hiển thị
                        ViewLoadData frmloaddata = new ViewLoadData(m_dataBuffer);
                        // Gán sự kiện LoadDataClicked cho sự kiện click của nút trong ViewLoadData
                        frmloaddata.LoadDataClicked += Frmloaddata_LoadDataClicked;
                        frmloaddata.ShowDialog();
                    }
                    return Result.Succeeded;
                }

                catch (Exception e)
                {
                    message = e.Message;
                    return Result.Failed;
                }

            }
            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }

        }

        // Phương thức xử lý sự kiện click cho nút trong ViewLoadData
        private void Frmloaddata_LoadDataClicked(object sender, EventArgs e)
        {
            // Xử lý tạo thanh thép ở đây
            // Nơi để đặt mã xử lý tạo thanh thép
            // Ví dụ:
            MessageBox.Show("Sự kiện click của nút đã được kích hoạt!");


            // Gọi mã xử lý tạo thanh thép tại đây
            // Ví dụ:
            // CreateRebars();
        }

        // Tạo phương thức để thực hiện việc tạo thanh thép
        public static Result CreateRebars(ExternalCommandData commandData, Element element, BoundingBoxXYZ boundingBox, LocationPoint locpoint)
        {
            // Get UIDocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            // Get Document
            Document doc = uidoc.Document;

            try
            {
                // Bạn có thể sử dụng commandData ở đây

                // Các bước tạo thanh thép ở đây

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ ở đây
                return Result.Failed;
            }
        }

    }
}
