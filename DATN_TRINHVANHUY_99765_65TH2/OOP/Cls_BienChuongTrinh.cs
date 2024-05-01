using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DATN_TRINHVANHUY_99765_65TH2.OOP
{
    public static class Cls_BienChuongTrinh
    {
        public static Document Revit_Document;
        public static ObservableCollection<Cls_Cot> DanhSach_Cot=new ObservableCollection<Cls_Cot>();

        public static Cls_ThepDai cls_ThepDai = new Cls_ThepDai();
        public static Cls_ThepDoc cls_ThepDoc = new Cls_ThepDoc();
        public static Cls_Cot Cls_Cot = new Cls_Cot(cls_ThepDoc.ID);

        //lưu các dữ liệu vẽ thép của dự án

        public static ObservableCollection<RebarBarType> Rvt_RebarBarTypes = new ObservableCollection<RebarBarType>();
        public static ObservableCollection<RebarShape> Rvt_RebarBarShapes = new ObservableCollection<RebarShape>();
        public static ObservableCollection<RebarHookType> Rvt_RebarRebarHookTypes = new ObservableCollection<RebarHookType>();
        //Function Get RebarBarType
        public static ObservableCollection<RebarBarType> GetRebarBarTypes(Document _doc)
        {
            FilteredElementCollector FillterCollector_RebarBarTypes = new FilteredElementCollector(_doc).OfClass(typeof(RebarBarType));
            
            List<RebarBarType> List_RebarBarTypes = FillterCollector_RebarBarTypes.Cast<RebarBarType>().ToList();

            ObservableCollection<RebarBarType> Revit_RebarBarTypes = new ObservableCollection<RebarBarType>(List_RebarBarTypes);
       
            return Revit_RebarBarTypes;
        }

        //Function Get RebarShape
        public static ObservableCollection<RebarShape> GetRebarShapes(Document _doc)
        {
            FilteredElementCollector FillterCollector_RebarBarShapes = new FilteredElementCollector(_doc).OfClass(typeof(RebarShape));

            List<RebarShape> List_RebarBarShapes = FillterCollector_RebarBarShapes.Cast<RebarShape>().ToList();

            ObservableCollection<RebarShape> Revit_RebarBarShapes = new ObservableCollection<RebarShape>(List_RebarBarShapes);

            return Revit_RebarBarShapes;
        }

        //Function Get RebarHookType
        public static ObservableCollection<RebarHookType> GetRebarHookTypes(Document _doc)
        {
            FilteredElementCollector FillterCollector_RebarHookTypes = new FilteredElementCollector(_doc).OfClass(typeof(RebarHookType));

            List<RebarHookType> List_RebarHookTypes = FillterCollector_RebarHookTypes.Cast<RebarHookType>().ToList();

            ObservableCollection<RebarHookType> Revit_RebarHookTypes = new ObservableCollection<RebarHookType>(List_RebarHookTypes);

            return Revit_RebarHookTypes;
        }
    }
}
