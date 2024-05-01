using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using DATN_TRINHVANHUY_99765_65TH2.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_TRINHVANHUY_99765_65TH2.OOP
{
    public class Cls_Cot
    {
        //các thành phần gồm: trường & thuộc tính (feild, propeties), phương thức (method) & phương thức khởi tạo (constructor)

        #region khai báo field

        private ElementId _id; //id Revit
        private Element _element;  //phần tử tương ứng trong revit
        private string _mark; //mã hiệu của cấu kiện
        private ObservableCollection<Element> _danhSachThepDai = new ObservableCollection<Element>();
        private string _name;
        private Instance _instancecolumn;
        #endregion

        #region khai báo prorperty

        public Instance InstanceColumn
        {
            get { return _instancecolumn; } //get mothod
            set { _instancecolumn = value; } //set mothod
        }
        public ObservableCollection<Element> DanhSachThepDai
        {
            get { return _danhSachThepDai; } //get mothod
            set { _danhSachThepDai = value; } //set mothod
        }
        public ElementId ID
        {
            get { return _id; } //get mothod
            set { _id = value; } //set mothod
        }
        public Element Element
        {
            get { return _element; } //get mothod
            set { _element = value; } //set mothod
        }
        public string Mark
        {
            get { return _mark; }
            set { _mark = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion

        #region Khai báo constructor 
        public Cls_Cot(ElementId id)
        {
            _id = id;
        }
        public Cls_Cot(ElementId id, Element element)
        {
            _id = id;
            _element = element;
        }
        #endregion

        #region khai báo hàm (method)
        public void VeThep(float khoangcachdai, RebarBarType rebartype)
        {
            Rebar CreateRebar(Autodesk.Revit.DB.Document document, FamilyInstance column, RebarBarType barType, RebarHookType hookType)
            {
                LocationPoint location = column.Location as LocationPoint;
                XYZ origin = location.Point;

                XYZ normal = new XYZ(1, 0, 0);

                XYZ rebarLineEnd = new XYZ(origin.X, origin.Y, origin.Z + 9);
                Line rebarLine = Line.CreateBound(origin, rebarLineEnd);

                // Create the line rebar
                IList<Curve> curves = new List<Curve>();
                curves.Add(rebarLine);

                Rebar rebar = Rebar.CreateFromCurves(document, Autodesk.Revit.DB.Structure.RebarStyle.Standard, barType, hookType, hookType, column, origin, curves, RebarHookOrientation.Right, RebarHookOrientation.Left, true, true);

                if (null != rebar)
                {
                    // set specific layout for new rebar as fixed number, with 10 bars, distribution path length of 1.5'
                    // with bars of the bar set on the same side of the rebar plane as indicated by normal
                    // and both first and last bar in the set are shown
                    rebar.GetShapeDrivenAccessor().SetLayoutAsFixedNumber(10, 1.5, true, true, true);
                }

                return rebar;
            }
        }        
    }
        #endregion
}
