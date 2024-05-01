using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using DATN_TRINHVANHUY_99765_65TH2.Forms;
using DATN_TRINHVANHUY_99765_65TH2.OOP;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Xml.Linq;

namespace DATN_TRINHVANHUY_99765_65TH2.Command
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    internal class Cmd_ChonCot : IExternalCommand
    {
        public Autodesk.Revit.DB.Transform transform;
        #region chọn cột
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Get UIDocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            // Get Document
            Document doc = uidoc.Document;

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

                    Cls_BienChuongTrinh.cls_ThepDoc.ID = column.Id;
                    Cls_BienChuongTrinh.cls_ThepDai.ID = column.Id;

                    Cls_BienChuongTrinh.cls_ThepDoc.HostID = column.Id;
                    Cls_BienChuongTrinh.cls_ThepDai.HostID = column.Id;

                    //string mark = column.LookupParameter("Mark").AsString();
                    //string mark = column.GetParameter(ParameterTypeId.AllModelInstanceComments).AsString();
                    string mark = column.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).AsString();
                    if (mark != null)
                    {
                        Cls_BienChuongTrinh.Cls_Cot.Mark = mark;
                    }
                    else
                    {
                        TaskDialog.Show("Thông báo", "Chưa gán Mark cho cột");
                    }

                    // Lấy bounding box của cột
                    BoundingBoxXYZ boundingBox = column.get_BoundingBox(null);


                    #region nháp
                    //// Lấy vị trí của phần tử
                    //Location location = column.Location;


                    //XYZ locationPoint = null;

                    //// Kiểm tra nếu vị trí là kiểu LocationPoint
                    //if (location is LocationPoint)
                    //{
                    //    // Ép kiểu vị trí thành LocationPoint
                    //    LocationPoint locationPointLocation = location as LocationPoint;

                    //    // Kiểm tra xem locationPointLocation có khác null không
                    //    if (locationPointLocation != null)
                    //    {
                    //        // Lấy điểm vị trí của phần tử
                    //        locationPoint = locationPointLocation.Point;

                    //        // Tạo ma trận biến đổi từ điểm vị trí của phần tử
                    //        Autodesk.Revit.DB.Transform transform = Autodesk.Revit.DB.Transform.Identity;
                    //        transform.Origin = locationPoint;

                    //        // Sử dụng transform để xoay bounding box hoặc thực hiện các thao tác khác
                    //    }
                    //}



                    //// Lấy hình dạng của phần tử
                    //GeometryElement geometryElement = column.get_Geometry(new Options());

                    //// Khởi tạo các biến cho bounding box mới
                    //XYZ minPoint = new XYZ(double.MaxValue, double.MaxValue, double.MaxValue);
                    //XYZ maxPoint = new XYZ(double.MinValue, double.MinValue, double.MinValue);




                    //// Duyệt qua tất cả các hình dạng trong geometry element
                    //foreach (GeometryObject geomObject in geometryElement)
                    //{
                    //    // Kiểm tra nếu là một instance của một hình dạng đã được xoay
                    //    if (geomObject is GeometryInstance geomInstance)
                    //    {
                    //        // Lấy ma trận biến đổi của instance
                    //         transform = geomInstance.Transform;

                    //        // Lấy hình dạng của instance
                    //        GeometryElement instanceGeometry = geomInstance.GetInstanceGeometry();

                    //        // Duyệt qua tất cả các hình dạng trong instance
                    //        foreach (GeometryObject instanceGeomObject in instanceGeometry)
                    //        {
                    //            // Kiểm tra nếu là một solid
                    //            if (instanceGeomObject is Solid solid)
                    //            {
                    //                // Duyệt qua tất cả các cạnh của solid
                    //                foreach (Edge edge in solid.Edges)
                    //                {
                    //                    // Lấy điểm đầu và điểm cuối của cạnh
                    //                    XYZ startPoint = edge.AsCurve().GetEndPoint(0);
                    //                    XYZ endPoint = edge.AsCurve().GetEndPoint(1);

                    //                    // So sánh và cập nhật tọa độ nhỏ nhất
                    //                    minPoint = new XYZ(Math.Min(minPoint.X, Math.Min(startPoint.X, endPoint.X)),
                    //                                       Math.Min(minPoint.Y, Math.Min(startPoint.Y, endPoint.Y)),
                    //                                       Math.Min(minPoint.Z, Math.Min(startPoint.Z, endPoint.Z)));

                    //                    // So sánh và cập nhật tọa độ lớn nhất
                    //                    maxPoint = new XYZ(Math.Max(maxPoint.X, Math.Max(startPoint.X, endPoint.X)),
                    //                                       Math.Max(maxPoint.Y, Math.Max(startPoint.Y, endPoint.Y)),
                    //                                       Math.Max(maxPoint.Z, Math.Max(startPoint.Z, endPoint.Z)));
                    //                }

                    //            }
                    //        }
                    //    }
                    //}

                    //XYZ newMinPoint = null;

                    //// Kiểm tra các giá trị minPoint
                    //if (IsFiniteNumber(minPoint.X) && IsFiniteNumber(minPoint.Y) && IsFiniteNumber(minPoint.Z))
                    //{
                    //    // Tạo điểm mới từ ma trận biến đổi
                    //    newMinPoint = transform.OfPoint(minPoint);
                    //}
                    //else
                    //{
                    //    // Xử lý trường hợp các giá trị không hợp lệ
                    //    // Ví dụ: Gán giá trị mặc định hoặc thông báo lỗi
                    //}
                    //XYZ newMaxPoint = transform.OfPoint(maxPoint);

                    //// Tạo bounding box mới từ minPoint và maxPoint
                    //BoundingBoxXYZ rotatedBoundingBox = new BoundingBoxXYZ();
                    //rotatedBoundingBox.Min = newMinPoint;
                    //rotatedBoundingBox.Max = newMaxPoint;
                    #endregion


                    if (boundingBox != null)
                    {
                        // Tính toán kích thước
                        double length = boundingBox.Max.Z - boundingBox.Min.Z; // Chiều dài
                        double width = boundingBox.Max.X - boundingBox.Min.X;  // Chiều rộng
                        double height = boundingBox.Max.Y - boundingBox.Min.Y; // Chiều cao

                        // Chuyển đổi đơn vị nếu cần

                        // Ví dụ: Chuyển từ feet sang mm (1 feet = 304.8 mm)
                        const double FeetToMmFactor = 304.8;

                        Cls_BienChuongTrinh.cls_ThepDai.Chieucao = Math.Round(height * FeetToMmFactor, 0);
                        Cls_BienChuongTrinh.cls_ThepDai.Chieurong = Math.Round(width * FeetToMmFactor, 0);
                        Cls_BienChuongTrinh.cls_ThepDai.Chieudai = Math.Round(length * FeetToMmFactor, 0);

                        Cls_BienChuongTrinh.cls_ThepDoc.Chieucao = Math.Round(height * FeetToMmFactor, 0);
                        Cls_BienChuongTrinh.cls_ThepDoc.Chieurong = Math.Round(width * FeetToMmFactor, 0);
                        Cls_BienChuongTrinh.cls_ThepDoc.Chieudai = Math.Round(length * FeetToMmFactor, 0);

                        

                        //TaskDialog.Show("thông báo", $"chieu cao : {Cls_BienChuongTrinh.cls_ThepDoc.Chieucao}\n chieu rong : {Cls_BienChuongTrinh.cls_ThepDoc.Chieurong}\nchieu dai : {Cls_BienChuongTrinh.cls_ThepDoc.Chieudai} ");

                    }
                    else
                    {
                        TaskDialog.Show("thông báo", $" chưa lấy được thông tin h, b, l ");
                    }


                    FamilyInstance columnInstance = column as FamilyInstance;

                    //Nếu cột chưa được vẽ thép, hiển thị form và thêm vào danh sách sau khi xử lý
                    ViewVeThep frm_vethep = new ViewVeThep(doc);
                    bool? _result = frm_vethep.ShowDialog();
                    if (_result == true)
                    {
                        Cls_Cot pick_cot = new Cls_Cot(pick_column.ElementId);
                        Autodesk.Revit.DB.Instance instanceOfPickCot = pick_cot.InstanceColumn;
                        Cls_BienChuongTrinh.DanhSach_Cot.Add(pick_cot);
                        // Gọi chương trình vẽ thép và lưu vào class

                        //ColumnFramReinMaker newcot = new ColumnFramReinMaker(commandData,columnInstance);

                        //Cls_ThepDoc thepDoc = new Cls_ThepDoc();

                    }
                }
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException ex)
            {
                TaskDialog.Show("Error", ex.Message);
                return Result.Succeeded;
            }
            return Result.Succeeded;
        }

        public class MassSelectionFilter : ISelectionFilter
        {
            public bool AllowElement(Element element)
            {
                // Kiểm tra xem phần tử có tồn tại và có thuộc tính Category không
                if (element != null && element.Category != null)
                {
                    //// Kiểm tra xem phần tử có thuộc loại "Structural Column" không
                    //if (element.family.Name == "Structural Column")
                    //{
                    //    return true;
                    //}
                    return element.Category.Name.Contains("Structural Column");
                }
                return false;
            }

            public bool AllowReference(Reference refer, XYZ point)
            {
                return false;
            }
        }
        #endregion

        #region vẽ thép dọc
        public void VethepDoc(Document doc, Element cot)
        {
            #region khai bao bien va doi don vi
            double b = Cls_BienChuongTrinh.cls_ThepDoc.Chieurong;
            double h = Cls_BienChuongTrinh.cls_ThepDoc.Chieucao;
            double l = Cls_BienChuongTrinh.cls_ThepDoc.Chieudai;
            double cover = Cls_BienChuongTrinh.cls_ThepDoc.Cover;
            double a1 = Cls_BienChuongTrinh.cls_ThepDoc.Covera1;
            double a2 = Cls_BienChuongTrinh.cls_ThepDoc.Covera2;
            double soluongthanh_z = Cls_BienChuongTrinh.cls_ThepDoc.Soluongphuongz;
            double soluongthanh_y = Cls_BienChuongTrinh.cls_ThepDoc.Soluongphuongy;
            bool kieubotrithepnoichong1 = Cls_BienChuongTrinh.cls_ThepDoc.Checkboxkieunoichong1;
            bool kieubotrithepnoichong2 = Cls_BienChuongTrinh.cls_ThepDoc.Checkboxkieunoichong2;
            bool kieubotrithepnoichong3 = Cls_BienChuongTrinh.cls_ThepDoc.Checkboxkieunoichong3;
            double overlap = Cls_BienChuongTrinh.cls_ThepDoc.Overlap;

            cover = UnitUtils.ConvertToInternalUnits(cover, UnitTypeId.Millimeters);
            b = UnitUtils.ConvertToInternalUnits(b, UnitTypeId.Millimeters);
            l = UnitUtils.ConvertToInternalUnits(l, UnitTypeId.Millimeters);
            h = UnitUtils.ConvertToInternalUnits(h, UnitTypeId.Millimeters);
            a1 = UnitUtils.ConvertToInternalUnits(a1, UnitTypeId.Millimeters);
            a2 = UnitUtils.ConvertToInternalUnits(a2, UnitTypeId.Millimeters);
            overlap = UnitUtils.ConvertToInternalUnits(overlap, UnitTypeId.Millimeters);
            double so50 = UnitUtils.ConvertToInternalUnits(50, UnitTypeId.Millimeters);
            double hesokieunoichong2 = UnitUtils.ConvertToInternalUnits(18, UnitTypeId.Millimeters);


            #endregion
            using (Transaction trans = new Transaction(doc, "Tạo thanh thép"))
            {
                if (soluongthanh_z >= 2 && soluongthanh_y >= 2)
                {
                    trans.Start();

                    #region thông tin thép

                    RebarBarType Bartypethepdoc = Cls_BienChuongTrinh.cls_ThepDoc.Bartypethepdoc;
                    RebarHookType startHooks = Cls_BienChuongTrinh.cls_ThepDoc.Bottomhooks;
                    RebarHookType endHooks = Cls_BienChuongTrinh.cls_ThepDoc.Tophooks;
                    RebarBarType Bartypethepnoichong = Cls_BienChuongTrinh.cls_ThepDoc.Bartypenoichongthep;

                    double D = Bartypethepdoc.BarModelDiameter;
                    double D1 = Bartypethepnoichong.BarModelDiameter;

                    double R = D / 2;
                    cover = cover + R;
                    double Dtb=(D1+D)/2;
                    #endregion

                    LocationPoint locpoint = cot.Location as LocationPoint;
                    BoundingBoxXYZ boundingBox = cot.get_BoundingBox(null);
                    XYZ centerPoint = locpoint.Point;
                    XYZ diemdich = new XYZ(boundingBox.Min.X, boundingBox.Min.Y, boundingBox.Min.Z);

                    #region kiểu không nối chồng
                    if (kieubotrithepnoichong1 == false && kieubotrithepnoichong2 == false && kieubotrithepnoichong3 == false)
                    {
                        try
                        {
                            XYZ diembatdau = diemdich + new XYZ(cover, cover, 0);
                            XYZ endPt = diembatdau + new XYZ(0, 0, l - cover);
                            Line curve = Line.CreateBound(diembatdau, endPt);
                            IList<Curve> curves = new List<Curve>() { curve };
                            Rebar verticalRebar1 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepdoc, startHooks, endHooks,
                                                                          cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                            Element e_rebar = doc.GetElement(verticalRebar1.Id);
                            Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                            if (pra_partition != null)
                            {
                                pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                            }
                            else
                            {
                                TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                            }

                            diembatdau = diemdich + new XYZ(b - cover, cover, 0);
                            endPt = diembatdau + new XYZ(0, 0, l - cover);
                            curve = Line.CreateBound(diembatdau, endPt);
                            curves = new List<Curve>() { curve };
                            Rebar verticalRebar2 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepdoc, startHooks, endHooks,
                                                                          cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                            e_rebar = doc.GetElement(verticalRebar2.Id);
                            pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                            if (pra_partition != null)
                            {
                                pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                            }
                            else
                            {
                                TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                            }

                            diembatdau = diemdich + new XYZ(cover, h - cover, 0);
                            endPt = diembatdau + new XYZ(0, 0, l - cover);
                            curve = Line.CreateBound(diembatdau, endPt);
                            curves = new List<Curve>() { curve };
                            Rebar verticalRebar3 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepdoc, startHooks, endHooks,
                                                                          cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                            e_rebar = doc.GetElement(verticalRebar3.Id);
                            pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                            if (pra_partition != null)
                            {
                                pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                            }
                            else
                            {
                                TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                            }

                            diembatdau = diemdich + new XYZ(b - cover, h - cover, 0);
                            endPt = diembatdau + new XYZ(0, 0, l - cover);
                            curve = Line.CreateBound(diembatdau, endPt);
                            curves = new List<Curve>() { curve };
                            Rebar verticalRebar4 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepdoc, startHooks, endHooks,
                                                                          cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                            e_rebar = doc.GetElement(verticalRebar4.Id);
                            pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                            if (pra_partition != null)
                            {
                                pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                            }
                            else
                            {
                                TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                            }

                            if (soluongthanh_z > 2)
                            {
                                for (int i = 1; i < soluongthanh_z - 1; i++)
                                {
                                    diembatdau = diemdich + new XYZ(cover + i * (b - 2 * cover) / (soluongthanh_z - 1), cover, 0);
                                    endPt = diembatdau + new XYZ(0, 0, l - cover);
                                    curve = Line.CreateBound(diembatdau, endPt);
                                    curves = new List<Curve>() { curve };
                                    Rebar verticalRebar5 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepdoc, startHooks, endHooks,
                                                                                  cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                                    e_rebar = doc.GetElement(verticalRebar5.Id);
                                    pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                    if (pra_partition != null)
                                    {
                                        pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                    }
                                    else
                                    {
                                        TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                    }

                                    diembatdau = diemdich + new XYZ(cover + i * (b - 2 * cover) / (soluongthanh_z - 1), h - cover, 0);
                                    endPt = diembatdau + new XYZ(0, 0, l - cover);
                                    curve = Line.CreateBound(diembatdau, endPt);
                                    curves = new List<Curve>() { curve };
                                    Rebar verticalRebar6 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepdoc, startHooks, endHooks,
                                                                                  cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                                    e_rebar = doc.GetElement(verticalRebar6.Id);
                                    pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                    if (pra_partition != null)
                                    {
                                        pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                    }
                                    else
                                    {
                                        TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                    }
                                }
                            }

                            if (soluongthanh_y > 2)
                            {
                                for (int i = 1; i < soluongthanh_y - 1; i++)
                                {
                                    diembatdau = diemdich + new XYZ(cover, cover + i * (h - 2 * cover) / (soluongthanh_y - 1), 0);
                                    endPt = diembatdau + new XYZ(0, 0, l - cover);
                                    curve = Line.CreateBound(diembatdau, endPt);
                                    curves = new List<Curve>() { curve };
                                    Rebar verticalRebar5 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepdoc, startHooks, endHooks,
                                                                                  cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                                    e_rebar = doc.GetElement(verticalRebar5.Id);
                                    pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                    if (pra_partition != null)
                                    {
                                        pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                    }
                                    else
                                    {
                                        TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                    }

                                    diembatdau = diemdich + new XYZ(b - cover, cover + i * (h - 2 * cover) / (soluongthanh_y - 1), 0);
                                    endPt = diembatdau + new XYZ(0, 0, l - cover);
                                    curve = Line.CreateBound(diembatdau, endPt);
                                    curves = new List<Curve>() { curve };
                                    Rebar verticalRebar6 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepdoc, startHooks, endHooks,
                                                                                  cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                                    e_rebar = doc.GetElement(verticalRebar6.Id);
                                    pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                    if (pra_partition != null)
                                    {
                                        pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                    }
                                    else
                                    {
                                        TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                    }
                                }
                            }
                            //Element e_rebar = doc.GetElement(verticalRebar1.Id);
                            //Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                            //if (pra_partition != null)
                            //{
                            //    pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                            //}
                            //else
                            //{
                            //    TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                            //}






                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            // Roll back the transaction if there's an exception
                            trans.RollBack();
                            // Handle the exception
                            TaskDialog.Show("Error", ex.Message);
                        }
                    }
                    #endregion
                    #region kiểu nối chồng 1
                    if (kieubotrithepnoichong1 == true)
                    {
                        try
                        {

                            XYZ diembatdau = diemdich + new XYZ(cover, cover, 0);
                            XYZ endPt = diembatdau + new XYZ(0, 0, l + overlap);
                            Line curve = Line.CreateBound(diembatdau, endPt);
                            IList<Curve> curves = new List<Curve>() { curve };
                            Rebar verticalRebar1 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepdoc, startHooks, endHooks,
                                                                          cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                            Element e_rebar = doc.GetElement(verticalRebar1.Id);
                            Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                            if (pra_partition != null)
                            {
                                pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                            }
                            else
                            {
                                TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                            }

                            diembatdau = diemdich + new XYZ(b - cover, cover, 0);
                            endPt = diembatdau + new XYZ(0, 0, l + overlap);
                            curve = Line.CreateBound(diembatdau, endPt);
                            curves = new List<Curve>() { curve };
                            Rebar verticalRebar2 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepdoc, startHooks, endHooks,
                                                                          cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                             e_rebar = doc.GetElement(verticalRebar2.Id);
                             pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                            if (pra_partition != null)
                            {
                                pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                            }
                            else
                            {
                                TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                            }

                            diembatdau = diemdich + new XYZ(cover, h - cover, 0);
                            endPt = diembatdau + new XYZ(0, 0, l + overlap);
                            curve = Line.CreateBound(diembatdau, endPt);
                            curves = new List<Curve>() { curve };
                            Rebar verticalRebar3 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepdoc, startHooks, endHooks,
                                                                          cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                            e_rebar = doc.GetElement(verticalRebar3.Id);
                            pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                            if (pra_partition != null)
                            {
                                pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                            }
                            else
                            {
                                TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                            }

                            diembatdau = diemdich + new XYZ(b - cover, h - cover, 0);
                            endPt = diembatdau + new XYZ(0, 0, l + overlap);
                            curve = Line.CreateBound(diembatdau, endPt);
                            curves = new List<Curve>() { curve };
                            Rebar verticalRebar4 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepdoc, startHooks, endHooks,
                                                                          cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                            e_rebar = doc.GetElement(verticalRebar4.Id);
                            pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                            if (pra_partition != null)
                            {
                                pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                            }
                            else
                            {
                                TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                            }

                            if (soluongthanh_z > 2)
                            {
                                for (int i = 1; i < soluongthanh_z - 1; i++)
                                {
                                    diembatdau = diemdich + new XYZ(cover + i * (b - 2 * cover) / (soluongthanh_z - 1), cover, 0);
                                    endPt = diembatdau + new XYZ(0, 0, l + overlap);
                                    curve = Line.CreateBound(diembatdau, endPt);
                                    curves = new List<Curve>() { curve };
                                    Rebar verticalRebar5 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepdoc, startHooks, endHooks,
                                                                                  cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                                    e_rebar = doc.GetElement(verticalRebar5.Id);
                                    pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                    if (pra_partition != null)
                                    {
                                        pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                    }
                                    else
                                    {
                                        TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                    }

                                    diembatdau = diemdich + new XYZ(cover + i * (b - 2 * cover) / (soluongthanh_z - 1), h - cover, 0);
                                    endPt = diembatdau + new XYZ(0, 0, l + overlap);
                                    curve = Line.CreateBound(diembatdau, endPt);
                                    curves = new List<Curve>() { curve };
                                    Rebar verticalRebar6 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepdoc, startHooks, endHooks,
                                                                                  cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                                    e_rebar = doc.GetElement(verticalRebar6.Id);
                                    pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                    if (pra_partition != null)
                                    {
                                        pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                    }
                                    else
                                    {
                                        TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                    }
                                }
                            }

                            if (soluongthanh_y > 2)
                            {
                                for (int i = 1; i < soluongthanh_y - 1; i++)
                                {
                                    diembatdau = diemdich + new XYZ(cover, cover + i * (h - 2 * cover) / (soluongthanh_y - 1), 0);
                                    endPt = diembatdau + new XYZ(0, 0, l + overlap);
                                    curve = Line.CreateBound(diembatdau, endPt);
                                    curves = new List<Curve>() { curve };
                                    Rebar verticalRebar5 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepdoc, startHooks, endHooks,
                                                                                  cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                                    e_rebar = doc.GetElement(verticalRebar5.Id);
                                    pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                    if (pra_partition != null)
                                    {
                                        pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                    }
                                    else
                                    {
                                        TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                    }

                                    diembatdau = diemdich + new XYZ(b - cover, cover + i * (h - 2 * cover) / (soluongthanh_y - 1), 0);
                                    endPt = diembatdau + new XYZ(0, 0, l + overlap);
                                    curve = Line.CreateBound(diembatdau, endPt);
                                    curves = new List<Curve>() { curve };
                                    Rebar verticalRebar6 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepdoc, startHooks, endHooks,
                                                                                  cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                                    e_rebar = doc.GetElement(verticalRebar6.Id);
                                    pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                    if (pra_partition != null)
                                    {
                                        pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                    }
                                    else
                                    {
                                        TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                    }
                                }
                            }

                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            // Roll back the transaction if there's an exception
                            trans.RollBack();
                            // Handle the exception
                            TaskDialog.Show("Error", ex.Message);
                        }
                    }
                    #endregion
                    #region kiểu nối chồng 2
                    if (kieubotrithepnoichong2 == true)
                    {
                        try
                        {
                            XYZ diembatdau = diemdich + new XYZ(cover, cover, 0);
                            XYZ endPt = diembatdau + new XYZ(0, 0, l);
                            Line curve = Line.CreateBound(diembatdau, endPt);
                            IList<Curve> curves = new List<Curve>() { curve };
                            Rebar verticalRebar1 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepdoc, startHooks, endHooks,
                                                                          cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                            Element e_rebar = doc.GetElement(verticalRebar1.Id);
                            Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                            if (pra_partition != null)
                            {
                                pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                            }
                            else
                            {
                                TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                            }

                            diembatdau = diemdich + new XYZ(cover + Dtb, cover + Dtb, l - overlap);
                            endPt = diembatdau + new XYZ(0, 0, 2 * overlap);
                            curve = Line.CreateBound(diembatdau, endPt);
                            curves = new List<Curve>() { curve };
                            Rebar verticalRebar2 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepnoichong, startHooks, endHooks,
                                                                          cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                            e_rebar = doc.GetElement(verticalRebar2.Id);
                            pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                            if (pra_partition != null)
                            {
                                pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                            }
                            else
                            {
                                TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                            }

                            diembatdau = diemdich + new XYZ(b - cover, cover, 0);
                            endPt = diembatdau + new XYZ(0, 0, l);
                            curve = Line.CreateBound(diembatdau, endPt);
                            curves = new List<Curve>() { curve };
                            Rebar verticalRebar3 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepdoc, startHooks, endHooks,
                                                                          cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                            e_rebar = doc.GetElement(verticalRebar3.Id);
                            pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                            if (pra_partition != null)
                            {
                                pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                            }
                            else
                            {
                                TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                            }

                            diembatdau = diemdich + new XYZ(b - cover - Dtb, cover + Dtb, l - overlap);
                            endPt = diembatdau + new XYZ(0, 0, 2 * overlap);
                            curve = Line.CreateBound(diembatdau, endPt);
                            curves = new List<Curve>() { curve };
                            Rebar verticalRebar4 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepnoichong, startHooks, endHooks,
                                                                          cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                            e_rebar = doc.GetElement(verticalRebar4.Id);
                            pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                            if (pra_partition != null)
                            {
                                pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                            }
                            else
                            {
                                TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                            }

                            diembatdau = diemdich + new XYZ(cover, h - cover, 0);
                            endPt = diembatdau + new XYZ(0, 0, l);
                            curve = Line.CreateBound(diembatdau, endPt);
                            curves = new List<Curve>() { curve };
                            Rebar verticalRebar5 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepdoc, startHooks, endHooks,
                                                                          cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                            e_rebar = doc.GetElement(verticalRebar5.Id);
                            pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                            if (pra_partition != null)
                            {
                                pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                            }
                            else
                            {
                                TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                            }

                            diembatdau = diemdich + new XYZ(cover + Dtb, h - cover - Dtb, l - overlap);
                            endPt = diembatdau + new XYZ(0, 0, 2 * overlap);
                            curve = Line.CreateBound(diembatdau, endPt);
                            curves = new List<Curve>() { curve };
                            Rebar verticalRebar6 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepnoichong, startHooks, endHooks,
                                                                          cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                            e_rebar = doc.GetElement(verticalRebar6.Id);
                            pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                            if (pra_partition != null)
                            {
                                pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                            }
                            else
                            {
                                TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                            }

                            diembatdau = diemdich + new XYZ(b - cover, h - cover, 0);
                            endPt = diembatdau + new XYZ(0, 0, l);
                            curve = Line.CreateBound(diembatdau, endPt);
                            curves = new List<Curve>() { curve };
                            Rebar verticalRebar7 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepdoc, startHooks, endHooks,
                                                                          cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                            e_rebar = doc.GetElement(verticalRebar7.Id);
                            pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                            if (pra_partition != null)
                            {
                                pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                            }
                            else
                            {
                                TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                            }

                            diembatdau = diemdich + new XYZ(b - cover - Dtb, h - cover - Dtb, l - overlap);
                            endPt = diembatdau + new XYZ(0, 0, 2 * overlap);
                            curve = Line.CreateBound(diembatdau, endPt);
                            curves = new List<Curve>() { curve };
                            Rebar verticalRebar8 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepnoichong, startHooks, endHooks,
                                                                          cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                            e_rebar = doc.GetElement(verticalRebar8.Id);
                            pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                            if (pra_partition != null)
                            {
                                pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                            }
                            else
                            {
                                TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                            }


                            if (soluongthanh_z > 2)
                            {
                                for (int i = 1; i < soluongthanh_z - 1; i++)
                                {
                                    diembatdau = diemdich + new XYZ(cover + i * (b - 2 * cover) / (soluongthanh_z - 1), cover, 0);
                                    endPt = diembatdau + new XYZ(0, 0, l);
                                    curve = Line.CreateBound(diembatdau, endPt);
                                    curves = new List<Curve>() { curve };
                                    Rebar verticalRebar9 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepdoc, startHooks, endHooks,
                                                                                  cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                                    e_rebar = doc.GetElement(verticalRebar9.Id);
                                    pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                    if (pra_partition != null)
                                    {
                                        pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                    }
                                    else
                                    {
                                        TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                    }

                                    diembatdau = diemdich + new XYZ(cover + i * (b - 2 * cover - 2 * Dtb) / (soluongthanh_z - 1) + Dtb, cover + Dtb, l - overlap);
                                    endPt = diembatdau + new XYZ(0, 0, 2 * overlap);
                                    curve = Line.CreateBound(diembatdau, endPt);
                                    curves = new List<Curve>() { curve };
                                    Rebar verticalRebar10 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepnoichong, startHooks, endHooks,
                                                                                  cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                                    e_rebar = doc.GetElement(verticalRebar10.Id);
                                    pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                    if (pra_partition != null)
                                    {
                                        pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                    }
                                    else
                                    {
                                        TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                    }

                                    diembatdau = diemdich + new XYZ(cover + i * (b - 2 * cover) / (soluongthanh_z - 1), h - cover, 0);
                                    endPt = diembatdau + new XYZ(0, 0, l);
                                    curve = Line.CreateBound(diembatdau, endPt);
                                    curves = new List<Curve>() { curve };
                                    Rebar verticalRebar11 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepdoc, startHooks, endHooks,
                                                                                  cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                                    e_rebar = doc.GetElement(verticalRebar11.Id);
                                    pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                    if (pra_partition != null)
                                    {
                                        pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                    }
                                    else
                                    {
                                        TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                    }

                                    diembatdau = diemdich + new XYZ(cover + i * (b - 2 * cover - 2 * Dtb) / (soluongthanh_z - 1) + Dtb, h - cover - Dtb, l - overlap);
                                    endPt = diembatdau + new XYZ(0, 0, 2 * overlap);
                                    curve = Line.CreateBound(diembatdau, endPt);
                                    curves = new List<Curve>() { curve };
                                    Rebar verticalRebar12 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepnoichong, startHooks, endHooks,
                                                                                  cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                                    e_rebar = doc.GetElement(verticalRebar12.Id);
                                    pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                    if (pra_partition != null)
                                    {
                                        pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                    }
                                    else
                                    {
                                        TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                    }
                                }
                            }

                            if (soluongthanh_y > 2)
                            {
                                for (int i = 1; i < soluongthanh_y - 1; i++)
                                {
                                    diembatdau = diemdich + new XYZ(cover, cover + i * (h - 2 * cover) / (soluongthanh_y - 1), 0);
                                    endPt = diembatdau + new XYZ(0, 0, l);
                                    curve = Line.CreateBound(diembatdau, endPt);
                                    curves = new List<Curve>() { curve };
                                    Rebar verticalRebar9 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepdoc, startHooks, endHooks,
                                                                                  cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                                    e_rebar = doc.GetElement(verticalRebar9.Id);
                                    pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                    if (pra_partition != null)
                                    {
                                        pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                    }
                                    else
                                    {
                                        TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                    }

                                    diembatdau = diemdich + new XYZ(cover + Dtb, cover + i * (h - 2 * cover - 2 * Dtb) / (soluongthanh_y - 1) + Dtb, l - overlap);
                                    endPt = diembatdau + new XYZ(0, 0, 2 * overlap);
                                    curve = Line.CreateBound(diembatdau, endPt);
                                    curves = new List<Curve>() { curve };
                                    Rebar verticalRebar10 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepnoichong, startHooks, endHooks,
                                                                                  cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                                    e_rebar = doc.GetElement(verticalRebar10.Id);
                                    pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                    if (pra_partition != null)
                                    {
                                        pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                    }
                                    else
                                    {
                                        TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                    }

                                    diembatdau = diemdich + new XYZ(b - cover, cover + i * (h - 2 * cover) / (soluongthanh_y - 1), 0);
                                    endPt = diembatdau + new XYZ(0, 0, l);
                                    curve = Line.CreateBound(diembatdau, endPt);
                                    curves = new List<Curve>() { curve };
                                    Rebar verticalRebar11 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepdoc, startHooks, endHooks,
                                                                                  cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                                    e_rebar = doc.GetElement(verticalRebar11.Id);
                                    pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                    if (pra_partition != null)
                                    {
                                        pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                    }
                                    else
                                    {
                                        TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                    }

                                    diembatdau = diemdich + new XYZ(b - cover - Dtb, cover + i * (h - 2 * cover - 2 * Dtb) / (soluongthanh_y - 1) + Dtb, l - overlap);
                                    endPt = diembatdau + new XYZ(0, 0, 2 * overlap);
                                    curve = Line.CreateBound(diembatdau, endPt);
                                    curves = new List<Curve>() { curve };
                                    Rebar verticalRebar12 = Rebar.CreateFromCurves(doc, RebarStyle.Standard, Bartypethepnoichong, startHooks, endHooks,
                                                                                  cot, centerPoint, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);
                                    e_rebar = doc.GetElement(verticalRebar12.Id);
                                    pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                    if (pra_partition != null)
                                    {
                                        pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                    }
                                    else
                                    {
                                        TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                    }
                                }
                            }
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            // Roll back the transaction if there's an exception
                            trans.RollBack();
                            // Handle the exception
                            TaskDialog.Show("Error", ex.Message);
                        }
                    }
                    #endregion
                    #region kiểu nối chồng 3
                    if (kieubotrithepnoichong3 == true)
                    {
                        try
                        {
                            XYZ diembatdau = diemdich + new XYZ(cover, cover, 0);
                            List<XYZ> points = new List<XYZ>
                                {
                                        new XYZ(diembatdau.X, diembatdau.Y, 0),
                                        new XYZ(diembatdau.X, diembatdau.Y, l -2*so50 ),
                                        new XYZ(diembatdau.X + a1-cover, diembatdau.Y + a2- cover, l),
                                        new XYZ(diembatdau.X + a1 - cover, diembatdau.Y + a2 - cover, l + overlap - so50),};
                            CurveLoop curveLoop = new CurveLoop();
                            for (int i = 0; i < points.Count - 1; i++)
                            {
                                Line line = Line.CreateBound(points[i], points[i + 1]);
                                curveLoop.Append(line);
                            }
                            RebarFreeFormValidationResult validationResult;
                            Rebar verticalRebar1 = Rebar.CreateFreeForm(doc, Bartypethepdoc, cot, new List<CurveLoop> { curveLoop }, out validationResult);
                            // Sao chép thanh thép tự do hiện tại
                            Rebar copiedRebar1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, verticalRebar1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                            // Dịch chuyển bản sao theo phương z
                            ElementTransformUtils.MoveElement(doc, copiedRebar1.Id, new XYZ(0, 0, boundingBox.Min.Z));
                            copiedRebar1.SetHostId(doc, cot.Id);
                            doc.Delete(verticalRebar1.Id);
                            Element e_rebar = doc.GetElement(copiedRebar1.Id);
                            Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                            if (pra_partition != null)
                            {
                                pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                            }
                            else
                            {
                                TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                            }

                            points = new List<XYZ>
                                {
                                        new XYZ(    diembatdau.X+b-2*cover,                       diembatdau.Y,                              0),
                                        new XYZ(    diembatdau.X+b-2*cover                        ,diembatdau.Y,                             l -2*so50 ),
                                        new XYZ(    diembatdau.X+b-cover -a1,             diembatdau.Y + a2- cover,                  l),
                                        new XYZ(    diembatdau.X +b -cover - a1 ,       diembatdau.Y + a2 - cover,                 l + overlap - so50),};
                            curveLoop = new CurveLoop();
                            for (int i = 0; i < points.Count - 1; i++)
                            {
                                Line line = Line.CreateBound(points[i], points[i + 1]);
                                curveLoop.Append(line);
                            }
                            Rebar verticalRebar2 = Rebar.CreateFreeForm(doc, Bartypethepdoc, cot, new List<CurveLoop> { curveLoop }, out validationResult);
                            // Sao chép thanh thép tự do hiện tại
                            Rebar copiedRebar2 = doc.GetElement(ElementTransformUtils.CopyElement(doc, verticalRebar2.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                            // Dịch chuyển bản sao theo phương z
                            ElementTransformUtils.MoveElement(doc, copiedRebar2.Id, new XYZ(0, 0, boundingBox.Min.Z));
                            copiedRebar2.SetHostId(doc, cot.Id);
                            doc.Delete(verticalRebar2.Id);
                            e_rebar = doc.GetElement(copiedRebar2.Id);
                            pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                            if (pra_partition != null)
                            {
                                pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                            }
                            else
                            {
                                TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                            }

                            points = new List<XYZ>
                                {
                                        new XYZ(    diembatdau.X  ,                    diembatdau.Y +h -2*cover,                              0),
                                        new XYZ(    diembatdau.X                        ,diembatdau.Y +h -2*cover,                             l -2*so50 ),
                                        new XYZ(    diembatdau.X - cover + a1,             diembatdau.Y +h -cover-a2,                  l),
                                        new XYZ(    diembatdau.X - cover + a1,             diembatdau.Y +h -cover-a2,                  l + overlap - so50),};
                            curveLoop = new CurveLoop();
                            for (int i = 0; i < points.Count - 1; i++)
                            {
                                Line line = Line.CreateBound(points[i], points[i + 1]);
                                curveLoop.Append(line);
                            }
                            Rebar verticalRebar3 = Rebar.CreateFreeForm(doc, Bartypethepdoc, cot, new List<CurveLoop> { curveLoop }, out validationResult);
                            // Sao chép thanh thép tự do hiện tại
                            Rebar copiedRebar3 = doc.GetElement(ElementTransformUtils.CopyElement(doc, verticalRebar3.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                            // Dịch chuyển bản sao theo phương z
                            ElementTransformUtils.MoveElement(doc, copiedRebar3.Id, new XYZ(0, 0, boundingBox.Min.Z));
                            copiedRebar3.SetHostId(doc, cot.Id);
                            doc.Delete(verticalRebar3.Id);
                            e_rebar = doc.GetElement(copiedRebar3.Id);
                            pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                            if (pra_partition != null)
                            {
                                pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                            }
                            else
                            {
                                TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                            }

                            points = new List<XYZ>
                                {
                                        new XYZ(    diembatdau.X + b -2*cover  ,                  diembatdau.Y +h -2*cover,                              0),
                                        new XYZ(   diembatdau.X + b -2*cover  ,                     diembatdau.Y +h -2*cover,                             l -2*so50 ),
                                        new XYZ(     diembatdau.X + b -cover - a1,             diembatdau.Y +h -cover-a2,                  l),
                                        new XYZ(     diembatdau.X + b -cover - a1,             diembatdau.Y +h -cover-a2,                  l + overlap - so50),};
                            curveLoop = new CurveLoop();
                            for (int i = 0; i < points.Count - 1; i++)
                            {
                                Line line = Line.CreateBound(points[i], points[i + 1]);
                                curveLoop.Append(line);
                            }
                            Rebar verticalRebar4 = Rebar.CreateFreeForm(doc, Bartypethepdoc, cot, new List<CurveLoop> { curveLoop }, out validationResult);
                            // Sao chép thanh thép tự do hiện tại
                            Rebar copiedRebar4 = doc.GetElement(ElementTransformUtils.CopyElement(doc, verticalRebar4.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                            // Dịch chuyển bản sao theo phương z
                            ElementTransformUtils.MoveElement(doc, copiedRebar4.Id, new XYZ(0, 0, boundingBox.Min.Z));
                            copiedRebar4.SetHostId(doc, cot.Id);
                            doc.Delete(verticalRebar4.Id);
                            e_rebar = doc.GetElement(copiedRebar4.Id);
                            pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                            if (pra_partition != null)
                            {
                                pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                            }
                            else
                            {
                                TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                            }

                            if (soluongthanh_z > 2)
                            {
                                for (int j = 1; j < soluongthanh_z - 1; j++)
                                {
                                    diembatdau = diemdich + new XYZ(cover, cover, 0);
                                    points = new List<XYZ>
                                {
                                       new XYZ(    diembatdau.X + j * (b - 2 *cover) / (soluongthanh_z - 1)  ,                  diembatdau.Y ,                              0),
                                        new XYZ(   diembatdau.X + j * (b - 2 *cover ) / (soluongthanh_z - 1),                     diembatdau.Y ,                             l -2*so50 ),
                                        new XYZ(   diembatdau.X + j * (b - 2 *cover ) / (soluongthanh_z - 1),             diembatdau.Y  -cover+a2,                  l),
                                        new XYZ(    diembatdau.X + j * (b - 2 *cover) / (soluongthanh_z - 1),             diembatdau.Y -cover+a2,                  l + overlap - so50),};
                                    curveLoop = new CurveLoop();
                                    for (int i = 0; i < points.Count - 1; i++)
                                    {
                                        Line line = Line.CreateBound(points[i], points[i + 1]);
                                        curveLoop.Append(line);
                                    }
                                    verticalRebar1 = Rebar.CreateFreeForm(doc, Bartypethepdoc, cot, new List<CurveLoop> { curveLoop }, out validationResult);
                                    // Sao chép thanh thép tự do hiện tại
                                    Rebar copiedRebar5 = doc.GetElement(ElementTransformUtils.CopyElement(doc, verticalRebar1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                    // Dịch chuyển bản sao theo phương z
                                    ElementTransformUtils.MoveElement(doc, copiedRebar5.Id, new XYZ(0, 0, boundingBox.Min.Z));
                                    copiedRebar5.SetHostId(doc, cot.Id);
                                    doc.Delete(verticalRebar1.Id);
                                    e_rebar = doc.GetElement(copiedRebar5.Id);
                                    pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                    if (pra_partition != null)
                                    {
                                        pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                    }
                                    else
                                    {
                                        TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                    }

                                    diembatdau = diemdich + new XYZ(cover, cover, 0);
                                    points = new List<XYZ>
                                {
                                       new XYZ(    diembatdau.X + j * (b - 2 *cover) / (soluongthanh_z - 1)  ,                  diembatdau.Y +h -2*cover,                              0),
                                        new XYZ(   diembatdau.X + j * (b - 2 *cover ) / (soluongthanh_z - 1)  ,                     diembatdau.Y +h -2*cover,                             l -2*so50 ),
                                        new XYZ(     diembatdau.X + j * (b - 2 *cover ) / (soluongthanh_z - 1),             diembatdau.Y +h -cover-a2,                  l),
                                        new XYZ(    diembatdau.X + j * (b - 2 *cover) / (soluongthanh_z - 1) ,             diembatdau.Y +h -cover-a2,                  l + overlap - so50),};
                                    curveLoop = new CurveLoop();
                                    for (int i = 0; i < points.Count - 1; i++)
                                    {
                                        Line line = Line.CreateBound(points[i], points[i + 1]);
                                        curveLoop.Append(line);
                                    }
                                    verticalRebar1 = Rebar.CreateFreeForm(doc, Bartypethepdoc, cot, new List<CurveLoop> { curveLoop }, out validationResult);
                                    // Sao chép thanh thép tự do hiện tại
                                    Rebar copiedRebar6 = doc.GetElement(ElementTransformUtils.CopyElement(doc, verticalRebar1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                    // Dịch chuyển bản sao theo phương z
                                    ElementTransformUtils.MoveElement(doc, copiedRebar6.Id, new XYZ(0, 0, boundingBox.Min.Z));
                                    copiedRebar6.SetHostId(doc, cot.Id);
                                    doc.Delete(verticalRebar1.Id);
                                    e_rebar = doc.GetElement(copiedRebar6.Id);
                                    pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                    if (pra_partition != null)
                                    {
                                        pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                    }
                                    else
                                    {
                                        TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                    }
                                }
                            }

                            if (soluongthanh_y > 2)
                            {
                                for (int j = 1; j < soluongthanh_y - 1; j++)
                                {
                                    diembatdau = diemdich + new XYZ(cover, cover, 0);
                                    points = new List<XYZ>
                                {
                                       new XYZ(    diembatdau.X                 ,             diembatdau.Y + j * (h - 2* cover) / (soluongthanh_y - 1)             ,                              0),
                                        new XYZ(   diembatdau.X         ,                     diembatdau.Y + j * (h - 2* cover) / (soluongthanh_y - 1)        ,                             l -2*so50 ),
                                        new XYZ(     diembatdau.X - cover + a1,           diembatdau.Y + j * (h - 2* cover) / (soluongthanh_y - 1)         ,         l),
                                        new XYZ(     diembatdau.X - cover + a1 ,           diembatdau.Y + j * (h - 2* cover) / (soluongthanh_y - 1)         ,         l + overlap - so50),};
                                    curveLoop = new CurveLoop();
                                    for (int i = 0; i < points.Count - 1; i++)
                                    {
                                        Line line = Line.CreateBound(points[i], points[i + 1]);
                                        curveLoop.Append(line);
                                    }
                                    verticalRebar1 = Rebar.CreateFreeForm(doc, Bartypethepdoc, cot, new List<CurveLoop> { curveLoop }, out validationResult);
                                    // Sao chép thanh thép tự do hiện tại
                                    Rebar copiedRebar5 = doc.GetElement(ElementTransformUtils.CopyElement(doc, verticalRebar1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                    // Dịch chuyển bản sao theo phương z
                                    ElementTransformUtils.MoveElement(doc, copiedRebar5.Id, new XYZ(0, 0, boundingBox.Min.Z));
                                    copiedRebar5.SetHostId(doc, cot.Id);
                                    doc.Delete(verticalRebar1.Id);
                                    e_rebar = doc.GetElement(copiedRebar5.Id);
                                    pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                    if (pra_partition != null)
                                    {
                                        pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                    }
                                    else
                                    {
                                        TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                    }

                                    diembatdau = diemdich + new XYZ(cover, cover, 0);
                                    points = new List<XYZ>
                                {
                                       new XYZ(    diembatdau.X  + b - 2 *cover               ,             diembatdau.Y + j * (h - 2* cover ) / (soluongthanh_y - 1)             ,                              0),
                                        new XYZ(   diembatdau.X  + b - 2 * cover       ,                     diembatdau.Y + j * (h -2*  cover ) / (soluongthanh_y - 1)        ,                             l -2*so50 ),
                                        new XYZ(     diembatdau.X + b - cover - a1,           diembatdau.Y + j * (h - 2* cover ) / (soluongthanh_y - 1)         ,         l),
                                        new XYZ(     diembatdau.X + b - cover - a1 ,           diembatdau.Y + j * (h - 2* cover ) / (soluongthanh_y - 1)         ,         l + overlap - so50),};
                                    curveLoop = new CurveLoop();
                                    for (int i = 0; i < points.Count - 1; i++)
                                    {
                                        Line line = Line.CreateBound(points[i], points[i + 1]);
                                        curveLoop.Append(line);
                                    }
                                    verticalRebar1 = Rebar.CreateFreeForm(doc, Bartypethepdoc, cot, new List<CurveLoop> { curveLoop }, out validationResult);
                                    // Sao chép thanh thép tự do hiện tại
                                    Rebar copiedRebar6 = doc.GetElement(ElementTransformUtils.CopyElement(doc, verticalRebar1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                    // Dịch chuyển bản sao theo phương z
                                    ElementTransformUtils.MoveElement(doc, copiedRebar6.Id, new XYZ(0, 0, boundingBox.Min.Z));
                                    copiedRebar6.SetHostId(doc, cot.Id);
                                    doc.Delete(verticalRebar1.Id);
                                    e_rebar = doc.GetElement(copiedRebar6.Id);
                                    pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                    if (pra_partition != null)
                                    {
                                        pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                    }
                                    else
                                    {
                                        TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                    }
                                }
                            }
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            // Roll back the transaction if there's an exception
                            trans.RollBack();
                            // Handle the exception
                            TaskDialog.Show("Error", ex.Message);
                        }
                    }
                    #endregion

                }
                else
                {
                    TaskDialog.Show("Thông báo", "Số lượng thép theo phương z và phương y cần lớn hơn 2 thanh.");
                }    

            }
        }
        #endregion

        #region vẽ thép đai
        public void VethepDai(Document doc, Element cot)
        {

            #region khai bao bien va doi don vi
            double b = Cls_BienChuongTrinh.cls_ThepDoc.Chieurong;
            double h = Cls_BienChuongTrinh.cls_ThepDoc.Chieucao;
            double l = Cls_BienChuongTrinh.cls_ThepDoc.Chieudai;
            double hesokieunoichong2 = UnitUtils.ConvertToInternalUnits(18, UnitTypeId.Millimeters);

            double cover = Cls_BienChuongTrinh.cls_ThepDai.Cover - hesokieunoichong2;
            double l1phantram = Cls_BienChuongTrinh.cls_ThepDai.L1phantram;
            double l2phantram = Cls_BienChuongTrinh.cls_ThepDai.L1phantram;
            double l3phantram = Cls_BienChuongTrinh.cls_ThepDai.L1phantram;
            double l1 = Cls_BienChuongTrinh.cls_ThepDai.L1;
            double l2 = Cls_BienChuongTrinh.cls_ThepDai.L2;
            //double l3 = Cls_BienChuongTrinh.cls_ThepDai.L3;
            bool kieuthepdai1 = Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieuthepdai1;
            bool kieuthepdai2 = Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieuthepdai2;
            bool kieuthepdai3 = Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieuthepdai3;
            bool kieubotrithepdai1 = Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieubotrithepdai1;
            bool kieubotrithepdai2 = Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieubotrithepdai2;
            bool kieubotrithepdai3 = Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieubotrithepdai3;
            bool kieuphantram = Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieuphantram;
            bool kieuchieudai = Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieuchieudai;
            double s1 = Cls_BienChuongTrinh.cls_ThepDai.S1;
            double s2 = Cls_BienChuongTrinh.cls_ThepDai.S2;
            double s3 = Cls_BienChuongTrinh.cls_ThepDai.S3;
            bool kieushape = Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieushape;

            cover = UnitUtils.ConvertToInternalUnits(cover, UnitTypeId.Millimeters);
            //l1phantram = UnitUtils.ConvertToInternalUnits(l1phantram, UnitTypeId.Millimeters);
            //l2phantram = UnitUtils.ConvertToInternalUnits(l2phantram, UnitTypeId.Millimeters);
            //l3phantram = UnitUtils.ConvertToInternalUnits(l3phantram, UnitTypeId.Millimeters);
            l1 = UnitUtils.ConvertToInternalUnits(l1, UnitTypeId.Millimeters);
            l2 = UnitUtils.ConvertToInternalUnits(l2, UnitTypeId.Millimeters);
            //l3 = UnitUtils.ConvertToInternalUnits(l3, UnitTypeId.Millimeters);
            s1 = UnitUtils.ConvertToInternalUnits(s1, UnitTypeId.Millimeters);
            s2 = UnitUtils.ConvertToInternalUnits(s2, UnitTypeId.Millimeters);
            s3 = UnitUtils.ConvertToInternalUnits(s3, UnitTypeId.Millimeters);
            double so50 = UnitUtils.ConvertToInternalUnits(50, UnitTypeId.Millimeters);
            b = UnitUtils.ConvertToInternalUnits(b, UnitTypeId.Millimeters);
            l = UnitUtils.ConvertToInternalUnits(l, UnitTypeId.Millimeters);
            h = UnitUtils.ConvertToInternalUnits(h, UnitTypeId.Millimeters);

            #endregion
            using (Transaction trans = new Transaction(doc, "Tạo thanh thép"))
            {
                trans.Start();

                #region thông tin thép
                RebarBarType Bartypethepdoc = Cls_BienChuongTrinh.cls_ThepDoc.Bartypethepdoc;
                RebarBarType Bartypethepdai = Cls_BienChuongTrinh.cls_ThepDai.Bartypethepdai;
                RebarHookType startHooks = Cls_BienChuongTrinh.cls_ThepDai.Hookatstart;
                RebarHookType endHooks = Cls_BienChuongTrinh.cls_ThepDai.Hookatend;
                RebarShape shapethepdai = Cls_BienChuongTrinh.cls_ThepDai.Shapethepdai;
                double D = Bartypethepdoc.BarModelDiameter;
                double D1 = Bartypethepdai.BarModelDiameter;
                double R = D / 2;
                double R1 = D1 / 2;
                cover = cover + R;
                D = 2 * D;
                #endregion

                LocationPoint locpoint = cot.Location as LocationPoint;
                BoundingBoxXYZ boundingBox = cot.get_BoundingBox(null);
                XYZ centerPoint = locpoint.Point;
                XYZ diemdich = new XYZ(boundingBox.Min.X, boundingBox.Min.Y, boundingBox.Min.Z);
                #region vẽ đai bằng create free form
                if (kieushape == false)
                {

                    #region kiểu THEP DAI 1
                    if (kieuthepdai1 == true)
                    {
                        try
                        {
                            XYZ diembatdau = diemdich + new XYZ(cover, cover, 0);
                            List<XYZ> points = new List<XYZ>
                        {
                                            new XYZ(diembatdau.X - D, diembatdau.Y -D + 20*R1 ,cover),
                                            new XYZ(diembatdau.X - D, diembatdau.Y -D ,cover),
                                            new XYZ(diembatdau.X + b - 2 * cover +D , diembatdau.Y - D , cover ),
                                            new XYZ(diembatdau.X + b - 2 * cover +D, diembatdau.Y + h -2* cover + D,cover),
                                            new XYZ(diembatdau.X - D, diembatdau.Y + h - 2* cover + D,cover),
                                            new XYZ(diembatdau.X - D, diembatdau.Y - D,cover),
                                            new XYZ(diembatdau.X - D +20*R1, diembatdau.Y -D ,cover),

                        };
                            CurveLoop curveLoop = new CurveLoop();
                            for (int i = 0; i < points.Count - 1; i++)
                            {
                                Line line = Line.CreateBound(points[i], points[i + 1]);
                                curveLoop.Append(line);
                            }
                            RebarFreeFormValidationResult validationResult;
                            Rebar freeFormRebarthepdai = Rebar.CreateFreeForm(doc, Bartypethepdai, cot, new List<CurveLoop> { curveLoop }, out validationResult);
                            #region kiểu bố trí thép đai 1
                            if (kieubotrithepdai1 == true)
                            {
                                double l_kiemtra = 0;
                                for (int i = 0; l > l_kiemtra + 5 * cover; i++)
                                {
                                    // Sao chép thanh thép tự do hiện tại
                                    Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                    // Dịch chuyển bản sao theo phương z
                                    ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover));
                                    l_kiemtra = l_kiemtra + s1;
                                    copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                    Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                    Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                    if (pra_partition != null)
                                    {
                                        pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                    }
                                    else
                                    {
                                        TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                    }
                                }
                                doc.Delete(freeFormRebarthepdai.Id);
                                trans.Commit();
                            }

                            #endregion
                            #region kiểu bố trí thép đai 2
                            if (kieubotrithepdai2 == true)
                            {
                                #region bố trí theo kiểu phần trăm
                                if (kieuphantram == true)
                                {
                                    double l_kiemtra = 0;
                                    double soluongthanhthepdai1 = 0;
                                    for (int i = 0; l * l1phantram / 100 > l_kiemtra+ 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover));
                                        l_kiemtra = l_kiemtra + s1;
                                        soluongthanhthepdai1 = soluongthanhthepdai1 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; l > l_kiemtra+ 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai1 * s1 + (s2 * i) + cover));
                                        l_kiemtra = l_kiemtra + s2;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    doc.Delete(freeFormRebarthepdai.Id);
                                    trans.Commit();

                                }
                                #endregion
                                #region bố trí theo kiểu chiều dài
                                if (kieuchieudai == true)
                                {
                                    double l_kiemtra = 0;
                                    double soluongthanhthepdai1 = 0;
                                    for (int i = 0; l1 > l_kiemtra + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover));
                                        l_kiemtra = l_kiemtra + s1;
                                        soluongthanhthepdai1 = soluongthanhthepdai1 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; l > l_kiemtra + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai1 * s1 + (s2 * i) + cover));
                                        l_kiemtra = l_kiemtra + s2;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    doc.Delete(freeFormRebarthepdai.Id);
                                    trans.Commit();

                                }
                                #endregion
                            }
                            #endregion
                            #region kiểu bố trí thép đai 3
                            if (kieubotrithepdai3 == true)
                            {
                                #region bố trí theo kiểu phần trăm
                                if (kieuphantram == true)
                                {

                                    double l_kiemtra = 0;
                                    double soluongthanhthepdai1 = 0;
                                    double soluongthanhthepdai2 = 0;
                                    for (int i = 0; l * l1phantram / 100 > l_kiemtra + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover));
                                        l_kiemtra = l_kiemtra + s1;
                                        soluongthanhthepdai1 = soluongthanhthepdai1 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; (l * (l1phantram + l2phantram) / 100) > l_kiemtra+ 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai1 * s1 + (s2 * i) + cover));
                                        l_kiemtra = l_kiemtra + s2;
                                        soluongthanhthepdai2 = soluongthanhthepdai2 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; l > l_kiemtra + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai2 * s2 + soluongthanhthepdai1 * s1 + (s3 * i) + cover));
                                        l_kiemtra = l_kiemtra + s3;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                    }
                                    doc.Delete(freeFormRebarthepdai.Id);
                                    trans.Commit();
                                }
                                #endregion
                                #region bố trí theo kiểu chiều dài
                                if (kieuchieudai == true)
                                {
                                    double l_kiemtra = 0;
                                    double soluongthanhthepdai1 = 0;
                                    double soluongthanhthepdai2 = 0;
                                    for (int i = 0; l1 > l_kiemtra + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover));
                                        l_kiemtra = l_kiemtra + s1;
                                        soluongthanhthepdai1 = soluongthanhthepdai1 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; l1 + l2 > l_kiemtra + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai1 * s1 + (s2 * i) + cover));
                                        l_kiemtra = l_kiemtra + s2;
                                        soluongthanhthepdai2 = soluongthanhthepdai2 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; l > l_kiemtra + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai2 * s2 + soluongthanhthepdai1 * s1 + (s3 * i) + cover));
                                        l_kiemtra = l_kiemtra + s3;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    doc.Delete(freeFormRebarthepdai.Id);
                                    trans.Commit();
                                }
                                #endregion
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            trans.Start();
                            TaskDialog.Show("Error", ex.Message);
                        }
                    }
                    #endregion

                    #region kiểu THEP DAI 2
                    if (kieuthepdai2 == true)
                    {
                        try
                        {
                            XYZ diembatdau = diemdich + new XYZ(cover, cover, 0);
                            List<XYZ> points = new List<XYZ>
                        {
                                            new XYZ(diembatdau.X - D, diembatdau.Y -D +20*R1 ,cover),
                                            new XYZ(diembatdau.X - D, diembatdau.Y -D  ,cover),
                                            new XYZ(diembatdau.X +0.75*b - 2 * cover +D ,  diembatdau.Y -D, cover ),
                                            new XYZ(diembatdau.X +0.75*b - 2 * cover +D, diembatdau.Y +h-2*cover +D,cover),
                                            new XYZ(diembatdau.X - D,  diembatdau.Y +h-2*cover +D,cover),
                                            new XYZ(diembatdau.X - D, diembatdau.Y - D ,cover),
                                            new XYZ(diembatdau.X - D +20*R1, diembatdau.Y -D ,cover),

                        };
                            CurveLoop curveLoop = new CurveLoop();
                            for (int i = 0; i < points.Count - 1; i++)
                            {
                                Line line = Line.CreateBound(points[i], points[i + 1]);
                                curveLoop.Append(line);
                            }
                            RebarFreeFormValidationResult validationResult;
                            Rebar freeFormRebarthepdai = Rebar.CreateFreeForm(doc, Bartypethepdai, cot, new List<CurveLoop> { curveLoop }, out validationResult);

                            List<XYZ> points1 = new List<XYZ>
                        {
                                             new XYZ(diembatdau.X + 0.25 *b - D, diembatdau.Y -D +20*R1 ,cover),
                                            new XYZ(diembatdau.X + 0.25 *b - D, diembatdau.Y -D ,cover),
                                            new XYZ(diembatdau.X +b - 2 * cover +D , diembatdau.Y -D  , cover ),
                                            new XYZ(diembatdau.X +b - 2 * cover +D, diembatdau.Y + h -2* cover + D,cover),
                                            new XYZ(diembatdau.X +0.25*b - D, diembatdau.Y + h - 2* cover + D,cover),
                                            new XYZ(diembatdau.X +0.25*b - D, diembatdau.Y -D,cover),
                                             new XYZ(diembatdau.X +0.25*b - D +20*R1, diembatdau.Y -D,cover),
                        };
                            CurveLoop curveLoop1 = new CurveLoop();
                            for (int i = 0; i < points1.Count - 1; i++)
                            {
                                Line line = Line.CreateBound(points1[i], points1[i + 1]);
                                curveLoop1.Append(line);
                            }
                            RebarFreeFormValidationResult validationResult1;
                            Rebar freeFormRebarthepdai1 = Rebar.CreateFreeForm(doc, Bartypethepdai, cot, new List<CurveLoop> { curveLoop1 }, out validationResult1);

                            #region kiểu bố trí thép đai 1
                            if (kieubotrithepdai1 == true)
                            {
                                double l_kiemtra = 0;
                                for (int i = 0; l > l_kiemtra + 5 * cover; i++)
                                {
                                    // Sao chép thanh thép tự do hiện tại
                                    Rebar copiedRebarthepdai = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                    // Dịch chuyển bản sao theo phương z
                                    ElementTransformUtils.MoveElement(doc, copiedRebarthepdai.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover));
                                    l_kiemtra = l_kiemtra + s1;
                                    copiedRebarthepdai.SetHostId(doc, cot.Id);
                                    Element e_rebar = doc.GetElement(copiedRebarthepdai.Id);
                                    Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                    if (pra_partition != null)
                                    {
                                        pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                    }
                                    else
                                    {
                                        TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                    }
                                }
                                doc.Delete(freeFormRebarthepdai.Id);

                                double l_kiemtra1 = 0;
                                for (int i = 0; l > l_kiemtra1 + 5 * cover; i++)
                                {
                                    // Sao chép thanh thép tự do hiện tại
                                    Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                    // Dịch chuyển bản sao theo phương z
                                    ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover + 2 * R));
                                    l_kiemtra1 = l_kiemtra1 + s1;
                                    copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                    Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                    Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                    if (pra_partition != null)
                                    {
                                        pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                    }
                                    else
                                    {
                                        TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                    }
                                }
                                doc.Delete(freeFormRebarthepdai1.Id);

                                trans.Commit();
                            }

                            #endregion
                            #region kiểu bố trí thép đai 2
                            if (kieubotrithepdai2 == true)
                            {
                                #region bố trí theo kiểu phần trăm
                                if (kieuphantram == true)
                                {
                                    double l_kiemtra = 0;
                                    double soluongthanhthepdai1 = 0;
                                    for (int i = 0; l * l1phantram / 100 > l_kiemtra + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover));
                                        l_kiemtra = l_kiemtra + s1;
                                        soluongthanhthepdai1 = soluongthanhthepdai1 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; l > l_kiemtra + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai1 * s1 + (s2 * i) + cover));
                                        l_kiemtra = l_kiemtra + s2;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    doc.Delete(freeFormRebarthepdai.Id);
                                    ////////////////////////////////////////
                                    double l_kiemtra1 = 0;
                                    double soluongthanhthepdai2 = 0;
                                    for (int i = 0; l * l1phantram / 100 > l_kiemtra1 + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai2 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai2.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover + R));
                                        l_kiemtra1 = l_kiemtra1 + s1;
                                        soluongthanhthepdai2 = soluongthanhthepdai2 + 1;
                                        copiedRebarthepdai2.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai2.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; l > l_kiemtra1 + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai2 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai2.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai2 * s1 + (s2 * i) + cover + 2 * R));
                                        l_kiemtra1 = l_kiemtra1 + s2;
                                        copiedRebarthepdai2.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai2.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    doc.Delete(freeFormRebarthepdai1.Id);

                                    trans.Commit();

                                }
                                #endregion
                                #region bố trí theo kiểu chiều dài
                                if (kieuchieudai == true)
                                {
                                    double l_kiemtra = 0;
                                    double soluongthanhthepdai1 = 0;
                                    for (int i = 0; l1 > l_kiemtra + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover));
                                        l_kiemtra = l_kiemtra + s1;
                                        soluongthanhthepdai1 = soluongthanhthepdai1 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; l > l_kiemtra + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai1 * s1 + (s2 * i) + cover));
                                        l_kiemtra = l_kiemtra + s2;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    doc.Delete(freeFormRebarthepdai.Id);

                                    ///////////////
                                    l_kiemtra = 0;
                                    soluongthanhthepdai1 = 0;
                                    for (int i = 0; l1 > l_kiemtra + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover + 2 * R));
                                        l_kiemtra = l_kiemtra + s1;
                                        soluongthanhthepdai1 = soluongthanhthepdai1 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; l > l_kiemtra + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai1 * s1 + (s2 * i) + cover + 2 * R));
                                        l_kiemtra = l_kiemtra + s2;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    doc.Delete(freeFormRebarthepdai1.Id);


                                    trans.Commit();
                                }
                                #endregion
                            }
                            #endregion
                            #region kiểu bố trí thép đai 3
                            if (kieubotrithepdai3 == true)
                            {
                                #region bố trí theo kiểu phần trăm
                                if (kieuphantram == true)
                                {

                                    double l_kiemtra = 0;
                                    double soluongthanhthepdai1 = 0;
                                    double soluongthanhthepdai2 = 0;
                                    for (int i = 0; l * l1phantram / 100 > l_kiemtra + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover));
                                        l_kiemtra = l_kiemtra + s1;
                                        soluongthanhthepdai1 = soluongthanhthepdai1 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; (l * (l1phantram + l2phantram) / 100) > l_kiemtra + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai1 * s1 + (s2 * i) + cover));
                                        l_kiemtra = l_kiemtra + s2;
                                        soluongthanhthepdai2 = soluongthanhthepdai2 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; l > l_kiemtra + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai2 * s2 + soluongthanhthepdai1 * s1 + (s3 * i) + cover));
                                        l_kiemtra = l_kiemtra + s3;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    doc.Delete(freeFormRebarthepdai.Id);
                                    ///////////////////////
                                    ///
                                    l_kiemtra = 0;
                                    soluongthanhthepdai1 = 0;
                                    soluongthanhthepdai2 = 0;
                                    for (int i = 0; l * l1phantram / 100 > l_kiemtra + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover + 2 * R));
                                        l_kiemtra = l_kiemtra + s1;
                                        soluongthanhthepdai1 = soluongthanhthepdai1 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; (l * (l1phantram + l2phantram) / 100) > l_kiemtra + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai1 * s1 + (s2 * i) + cover + 2 * R));
                                        l_kiemtra = l_kiemtra + s2;
                                        soluongthanhthepdai2 = soluongthanhthepdai2 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; l > l_kiemtra + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai2 * s2 + soluongthanhthepdai1 * s1 + (s3 * i) + cover + 2 * R));
                                        l_kiemtra = l_kiemtra + s3;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    doc.Delete(freeFormRebarthepdai1.Id);
                                    trans.Commit();
                                }
                                #endregion
                                #region bố trí theo kiểu chiều dài
                                if (kieuchieudai == true)
                                {
                                    double l_kiemtra = 0;
                                    double soluongthanhthepdai1 = 0;
                                    double soluongthanhthepdai2 = 0;
                                    for (int i = 0; l1 > l_kiemtra + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover));
                                        l_kiemtra = l_kiemtra + s1;
                                        soluongthanhthepdai1 = soluongthanhthepdai1 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; l1 + l2 > l_kiemtra + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai1 * s1 + (s2 * i) + cover));
                                        l_kiemtra = l_kiemtra + s2;
                                        soluongthanhthepdai2 = soluongthanhthepdai2 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; l > l_kiemtra + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai2 * s2 + soluongthanhthepdai1 * s1 + (s3 * i) + cover));
                                        l_kiemtra = l_kiemtra + s3;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    doc.Delete(freeFormRebarthepdai.Id);
                                    ////////////////////////
                                    l_kiemtra = 0;
                                    soluongthanhthepdai1 = 0;
                                    soluongthanhthepdai2 = 0;
                                    for (int i = 0; l1 > l_kiemtra + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover + 2 * R));
                                        l_kiemtra = l_kiemtra + s1;
                                        soluongthanhthepdai1 = soluongthanhthepdai1 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; l1 + l2 > l_kiemtra + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai1 * s1 + (s2 * i) + cover + 2 * R));
                                        l_kiemtra = l_kiemtra + s2;
                                        soluongthanhthepdai2 = soluongthanhthepdai2 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; l > l_kiemtra + 5 * cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai2 * s2 + soluongthanhthepdai1 * s1 + (s3 * i) + cover + 2 * R));
                                        l_kiemtra = l_kiemtra + s3;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    doc.Delete(freeFormRebarthepdai1.Id);
                                    trans.Commit();
                                }
                                #endregion
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            trans.RollBack();
                            TaskDialog.Show("Error", ex.Message);
                        }


                    }
                    #endregion

                    #region kiểu THEP DAI 3
                    if (kieuthepdai3 == true)
                    {
                        try
                        {
                            XYZ diembatdau = diemdich + new XYZ(cover, cover, 0);
                            List<XYZ> points = new List<XYZ>
                        {
                             new XYZ(diembatdau.X - D, diembatdau.Y -D +20*R1  ,cover),
                                            new XYZ(diembatdau.X - D, diembatdau.Y -D  ,cover),
                                            new XYZ(diembatdau.X +b - 2 * cover +D , diembatdau.Y -D, cover ),
                                            new XYZ(diembatdau.X +b - 2 * cover +D, diembatdau.Y +0.75*h -D,cover),
                                            new XYZ(diembatdau.X - D,  diembatdau.Y +0.75*h -D,cover),
                                            new XYZ(diembatdau.X - D, diembatdau.Y - D ,cover),
                                            new XYZ(diembatdau.X - D + 20 * R1, diembatdau.Y - D ,cover),
                        };
                            CurveLoop curveLoop = new CurveLoop();
                            for (int i = 0; i < points.Count - 1; i++)
                            {
                                Line line = Line.CreateBound(points[i], points[i + 1]);
                                curveLoop.Append(line);
                            }
                            RebarFreeFormValidationResult validationResult;
                            Rebar freeFormRebarthepdai = Rebar.CreateFreeForm(doc, Bartypethepdai, cot, new List<CurveLoop> { curveLoop }, out validationResult);

                            List<XYZ> points1 = new List<XYZ>
                        {
                            new XYZ(diembatdau.X - D, diembatdau.Y -D +0.25*h +20*R1,cover),
                                            new XYZ(diembatdau.X - D, diembatdau.Y -D +0.25*h ,cover),
                                            new XYZ(diembatdau.X +b - 2 * cover +D , diembatdau.Y -D  +0.25*h , cover ),
                                            new XYZ(diembatdau.X +b - 2 * cover +D, diembatdau.Y + h -2* cover + D,cover),
                                            new XYZ(diembatdau.X - D, diembatdau.Y + h - 2* cover + D,cover),
                                            new XYZ(diembatdau.X - D, diembatdau.Y -D + 0.25 * h,cover),
                                             new XYZ(diembatdau.X - D +20*R1, diembatdau.Y -D + 0.25 * h,cover),
                        };
                            CurveLoop curveLoop1 = new CurveLoop();
                            for (int i = 0; i < points1.Count - 1; i++)
                            {
                                Line line = Line.CreateBound(points1[i], points1[i + 1]);
                                curveLoop1.Append(line);
                            }
                            RebarFreeFormValidationResult validationResult1;
                            Rebar freeFormRebarthepdai1 = Rebar.CreateFreeForm(doc, Bartypethepdai, cot, new List<CurveLoop> { curveLoop1 }, out validationResult1);

                            #region kiểu bố trí thép đai 1
                            if (kieubotrithepdai1 == true)
                            {
                                double l_kiemtra = 0;
                                for (int i = 0; l > l_kiemtra + cover; i++)
                                {
                                    // Sao chép thanh thép tự do hiện tại
                                    Rebar copiedRebarthepdai = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                    // Dịch chuyển bản sao theo phương z
                                    ElementTransformUtils.MoveElement(doc, copiedRebarthepdai.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover));
                                    l_kiemtra = l_kiemtra + s1;
                                    copiedRebarthepdai.SetHostId(doc, cot.Id);
                                    Element e_rebar = doc.GetElement(copiedRebarthepdai.Id);
                                    Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                    if (pra_partition != null)
                                    {
                                        pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                    }
                                    else
                                    {
                                        TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                    }
                                }
                                doc.Delete(freeFormRebarthepdai.Id);

                                double l_kiemtra1 = 0;
                                for (int i = 0; l > l_kiemtra1 + cover; i++)
                                {
                                    // Sao chép thanh thép tự do hiện tại
                                    Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                    // Dịch chuyển bản sao theo phương z
                                    ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover + 2 * R));
                                    l_kiemtra1 = l_kiemtra1 + s1;
                                    copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                    Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                    Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                    if (pra_partition != null)
                                    {
                                        pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                    }
                                    else
                                    {
                                        TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                    }
                                }
                                doc.Delete(freeFormRebarthepdai1.Id);

                                trans.Commit();
                            }

                            #endregion
                            #region kiểu bố trí thép đai 2
                            if (kieubotrithepdai2 == true)
                            {
                                #region bố trí theo kiểu phần trăm
                                if (kieuphantram == true)
                                {
                                    double l_kiemtra = 0;
                                    double soluongthanhthepdai1 = 0;
                                    for (int i = 0; l * l1phantram / 100 > l_kiemtra + cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover));
                                        l_kiemtra = l_kiemtra + s1;
                                        soluongthanhthepdai1 = soluongthanhthepdai1 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; l > l_kiemtra + cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai1 * s1 + (s2 * i) + cover));
                                        l_kiemtra = l_kiemtra + s2;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    doc.Delete(freeFormRebarthepdai.Id);
                                    ////////////////////////////////////////
                                    double l_kiemtra1 = 0;
                                    double soluongthanhthepdai2 = 0;
                                    for (int i = 0; l * l1phantram / 100 > l_kiemtra1 + cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai2 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai2.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover + R));
                                        l_kiemtra1 = l_kiemtra1 + s1;
                                        soluongthanhthepdai2 = soluongthanhthepdai2 + 1;
                                        copiedRebarthepdai2.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai2.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; l > l_kiemtra1 + cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai2 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai2.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai2 * s1 + (s2 * i) + cover + 2 * R));
                                        l_kiemtra1 = l_kiemtra1 + s2;
                                        copiedRebarthepdai2.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai2.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    doc.Delete(freeFormRebarthepdai1.Id);

                                    trans.Commit();

                                }
                                #endregion
                                #region bố trí theo kiểu chiều dài
                                if (kieuchieudai == true)
                                {
                                    double l_kiemtra = 0;
                                    double soluongthanhthepdai1 = 0;
                                    for (int i = 0; l1 > l_kiemtra + cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover));
                                        l_kiemtra = l_kiemtra + s1;
                                        soluongthanhthepdai1 = soluongthanhthepdai1 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; l > l_kiemtra + cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai1 * s1 + (s2 * i) + cover));
                                        l_kiemtra = l_kiemtra + s2;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    doc.Delete(freeFormRebarthepdai.Id);

                                    ///////////////
                                    l_kiemtra = 0;
                                    soluongthanhthepdai1 = 0;
                                    for (int i = 0; l1 > l_kiemtra + cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover + 2 * R));
                                        l_kiemtra = l_kiemtra + s1;
                                        soluongthanhthepdai1 = soluongthanhthepdai1 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; l > l_kiemtra + cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai1 * s1 + (s2 * i) + cover + 2 * R));
                                        l_kiemtra = l_kiemtra + s2;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    doc.Delete(freeFormRebarthepdai1.Id);


                                    trans.Commit();
                                }
                                #endregion
                            }
                            #endregion
                            #region kiểu bố trí thép đai 3
                            if (kieubotrithepdai3 == true)
                            {
                                #region bố trí theo kiểu phần trăm
                                if (kieuphantram == true)
                                {

                                    double l_kiemtra = 0;
                                    double soluongthanhthepdai1 = 0;
                                    double soluongthanhthepdai2 = 0;
                                    for (int i = 0; l * l1phantram / 100 > l_kiemtra + cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover));
                                        l_kiemtra = l_kiemtra + s1;
                                        soluongthanhthepdai1 = soluongthanhthepdai1 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; (l * (l1phantram + l2phantram) / 100) > l_kiemtra + cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai1 * s1 + (s2 * i) + cover));
                                        l_kiemtra = l_kiemtra + s2;
                                        soluongthanhthepdai2 = soluongthanhthepdai2 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; l > l_kiemtra + cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai2 * s2 + soluongthanhthepdai1 * s1 + (s3 * i) + cover));
                                        l_kiemtra = l_kiemtra + s3;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    doc.Delete(freeFormRebarthepdai.Id);
                                    ///////////////////////
                                    ///
                                    l_kiemtra = 0;
                                    soluongthanhthepdai1 = 0;
                                    soluongthanhthepdai2 = 0;
                                    for (int i = 0; l * l1phantram / 100 > l_kiemtra + cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover + 2 * R));
                                        l_kiemtra = l_kiemtra + s1;
                                        soluongthanhthepdai1 = soluongthanhthepdai1 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; (l * (l1phantram + l2phantram) / 100) > l_kiemtra + cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai1 * s1 + (s2 * i) + cover + 2 * R));
                                        l_kiemtra = l_kiemtra + s2;
                                        soluongthanhthepdai2 = soluongthanhthepdai2 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; l > l_kiemtra + cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai2 * s2 + soluongthanhthepdai1 * s1 + (s3 * i) + cover + 2 * R));
                                        l_kiemtra = l_kiemtra + s3;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    doc.Delete(freeFormRebarthepdai1.Id);
                                    trans.Commit();
                                }
                                #endregion
                                #region bố trí theo kiểu chiều dài
                                if (kieuchieudai == true)
                                {
                                    double l_kiemtra = 0;
                                    double soluongthanhthepdai1 = 0;
                                    double soluongthanhthepdai2 = 0;
                                    for (int i = 0; l1 > l_kiemtra + cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover));
                                        l_kiemtra = l_kiemtra + s1;
                                        soluongthanhthepdai1 = soluongthanhthepdai1 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; l1 + l2 > l_kiemtra + cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai1 * s1 + (s2 * i) + cover));
                                        l_kiemtra = l_kiemtra + s2;
                                        soluongthanhthepdai2 = soluongthanhthepdai2 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; l > l_kiemtra + cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai2 * s2 + soluongthanhthepdai1 * s1 + (s3 * i) + cover));
                                        l_kiemtra = l_kiemtra + s3;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    doc.Delete(freeFormRebarthepdai.Id);
                                    ////////////////////////
                                    l_kiemtra = 0;
                                    soluongthanhthepdai1 = 0;
                                    soluongthanhthepdai2 = 0;
                                    for (int i = 0; l1 > l_kiemtra + cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover + 2 * R));
                                        l_kiemtra = l_kiemtra + s1;
                                        soluongthanhthepdai1 = soluongthanhthepdai1 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; l1 + l2 > l_kiemtra + cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai1 * s1 + (s2 * i) + cover + 2 * R));
                                        l_kiemtra = l_kiemtra + s2;
                                        soluongthanhthepdai2 = soluongthanhthepdai2 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    for (int i = 0; l > l_kiemtra + cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai1.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai2 * s2 + soluongthanhthepdai1 * s1 + (s3 * i) + cover + 2 * R));
                                        l_kiemtra = l_kiemtra + s3;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                        Element e_rebar = doc.GetElement(copiedRebarthepdai1.Id);
                                        Autodesk.Revit.DB.Parameter pra_partition = e_rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM);
                                        if (pra_partition != null)
                                        {
                                            pra_partition.Set(Cls_BienChuongTrinh.Cls_Cot.Mark);
                                        }
                                        else
                                        {
                                            TaskDialog.Show("thông báo", $" chưa lấy được thông tin Partition");
                                        }
                                    }
                                    doc.Delete(freeFormRebarthepdai1.Id);
                                    trans.Commit();
                                }
                                #endregion
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            trans.RollBack();
                            TaskDialog.Show("Error", ex.Message);
                        }
                    }
                    #endregion
                }
                #endregion
                #region vẽ đai bằng shape
                if(kieushape == true)
                {
                        try
                        {
                        XYZ diembatdau = diemdich + new XYZ(cover, cover, 0);
                        XYZ endPt = diembatdau + new XYZ(0, 1, 0);
                        Line curve = Line.CreateBound(diembatdau, endPt);
                        IList<Curve> curves = new List<Curve>() { curve };

                        Rebar freeFormRebarthepdai = Rebar.CreateFromCurvesAndShape(doc, shapethepdai, Bartypethepdai, startHooks, endHooks, cot,
                                                                                    centerPoint,curves,RebarHookOrientation.Right,RebarHookOrientation.Left);
                            #region kiểu bố trí thép đai 1
                            if (kieubotrithepdai1 == true)
                            {
                                double l_kiemtra = 0;
                                for (int i = 0; l > l_kiemtra + cover; i++)
                                {
                                    // Sao chép thanh thép tự do hiện tại
                                    Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                    // Dịch chuyển bản sao theo phương z
                                    ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover));
                                    l_kiemtra = l_kiemtra + s1;
                                    copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                }
                                doc.Delete(freeFormRebarthepdai.Id);
                                trans.Commit();
                            }

                            #endregion
                            #region kiểu bố trí thép đai 2
                            if (kieubotrithepdai2 == true)
                            {
                                #region bố trí theo kiểu phần trăm
                                if (kieuphantram == true)
                                {
                                    double l_kiemtra = 0;
                                    double soluongthanhthepdai1 = 0;
                                    for (int i = 0; l * l1phantram / 100 > l_kiemtra + cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover));
                                        l_kiemtra = l_kiemtra + s1;
                                        soluongthanhthepdai1 = soluongthanhthepdai1 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                    }
                                    for (int i = 0; l > l_kiemtra + cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai1 * s1 + (s2 * i) + cover));
                                        l_kiemtra = l_kiemtra + s2;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                    }
                                    doc.Delete(freeFormRebarthepdai.Id);
                                    trans.Commit();

                                }
                                #endregion
                                #region bố trí theo kiểu chiều dài
                                if (kieuchieudai == true)
                                {
                                    double l_kiemtra = 0;
                                    double soluongthanhthepdai1 = 0;
                                    for (int i = 0; l1 > l_kiemtra  +cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover));
                                        l_kiemtra = l_kiemtra + s1;
                                        soluongthanhthepdai1 = soluongthanhthepdai1 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                    }
                                    for (int i = 0; l > l_kiemtra+cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai1 * s1 + (s2 * i) + cover));
                                        l_kiemtra = l_kiemtra + s2;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                    }
                                    doc.Delete(freeFormRebarthepdai.Id);
                                    trans.Commit();

                                }
                                #endregion
                            }
                            #endregion
                            #region kiểu bố trí thép đai 3
                            if (kieubotrithepdai3 == true)
                            {
                                #region bố trí theo kiểu phần trăm
                                if (kieuphantram == true)
                                {

                                    double l_kiemtra = 0;
                                    double soluongthanhthepdai1 = 0;
                                    double soluongthanhthepdai2 = 0;
                                    for (int i = 0; l * l1phantram / 100 > l_kiemtra + cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover));
                                        l_kiemtra = l_kiemtra + s1;
                                        soluongthanhthepdai1 = soluongthanhthepdai1 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                    }
                                    for (int i = 0; (l * (l1phantram + l2phantram) / 100) > l_kiemtra+cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai1 * s1 + (s2 * i) + cover));
                                        l_kiemtra = l_kiemtra + s2;
                                        soluongthanhthepdai2 = soluongthanhthepdai2 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                    }
                                    for (int i = 0; l > l_kiemtra+cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai2 * s2 + soluongthanhthepdai1 * s1 + (s3 * i) + cover));
                                        l_kiemtra = l_kiemtra + s3;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                    }
                                    doc.Delete(freeFormRebarthepdai.Id);
                                    trans.Commit();
                                }
                                #endregion
                                #region bố trí theo kiểu chiều dài
                                if (kieuchieudai == true)
                                {
                                    double l_kiemtra = 0;
                                    double soluongthanhthepdai1 = 0;
                                    double soluongthanhthepdai2 = 0;
                                    for (int i = 0; l1 > l_kiemtra+cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + (s1 * i) + cover));
                                        l_kiemtra = l_kiemtra + s1;
                                        soluongthanhthepdai1 = soluongthanhthepdai1 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                    }
                                    for (int i = 0; l1 + l2 > l_kiemtra+cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai1 * s1 + (s2 * i) + cover));
                                        l_kiemtra = l_kiemtra + s2;
                                        soluongthanhthepdai2 = soluongthanhthepdai2 + 1;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                    }
                                    for (int i = 0; l > l_kiemtra+cover; i++)
                                    {
                                        // Sao chép thanh thép tự do hiện tại
                                        Rebar copiedRebarthepdai1 = doc.GetElement(ElementTransformUtils.CopyElement(doc, freeFormRebarthepdai.Id, new XYZ(0, 0, 0)).First()) as Rebar;
                                        // Dịch chuyển bản sao theo phương z
                                        ElementTransformUtils.MoveElement(doc, copiedRebarthepdai1.Id, new XYZ(0, 0, boundingBox.Min.Z + soluongthanhthepdai2 * s2 + soluongthanhthepdai1 * s1 + (s3 * i) + cover));
                                        l_kiemtra = l_kiemtra + s3;
                                        copiedRebarthepdai1.SetHostId(doc, cot.Id);
                                    }
                                    doc.Delete(freeFormRebarthepdai.Id);
                                    trans.Commit();
                                }
                                #endregion
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            trans.Commit();
                        TaskDialog.Show("Error", ex.Message);
                    }
                }
                #endregion
            }
        }
    }
    #endregion
}