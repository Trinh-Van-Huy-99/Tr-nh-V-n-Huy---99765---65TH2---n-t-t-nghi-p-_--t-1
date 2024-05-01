using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_TRINHVANHUY_99765_65TH2.OOP
{
    public class Cls_ThepDoc
    {
        //các thành phần gồm: trường & thuộc tính (feild, propeties), phương thức (method) & phương thức khởi tạo (constructor)

        #region khai báo field

        private ElementId _id; //id Revit
        private Element _element;  //phần tử tương ứng trong revit
        private ElementId _hostID; //host đối tượng đặt thép
        private string _partition;// quản lý tên cấu kiện của cốt thép 
        private double _cover;
        private double _chieudai;
        private double _chieucao;
        private double _chieurong;
        private int _soluongphuongz;
        private int _soluongphuongy;
        private RebarBarType _bartypethepdoc;
        private RebarHookType _tophooks;
        private RebarHookType _bottomhooks;
        private RebarBarType _bartypenoichongthep;
        private bool _checkboxkieunoichong1;
        private bool _checkboxkieunoichong2;
        private bool _checkboxkieunoichong3;
        private double _overlap;
        private double _covera1;
        private double _covera2;

        #endregion

        #region khai báo prorperty
        public double Covera1
        {
            get { return _covera1; } //get mothod
            set { _covera1 = value; } //set mothod
        }

        public double Covera2
        {
            get { return _covera2; } //get mothod
            set { _covera2 = value; } //set mothod
        }
        public double Overlap
        {
            get { return _overlap; } //get mothod
            set { _overlap = value; } //set mothod
        }
        public bool Checkboxkieunoichong3
        {
            get { return _checkboxkieunoichong3; } //get mothod
            set { _checkboxkieunoichong3 = value; } //set mothod
        }
        public bool Checkboxkieunoichong2
        {
            get { return _checkboxkieunoichong2; } //get mothod
            set { _checkboxkieunoichong2 = value; } //set mothod
        }
        public bool Checkboxkieunoichong1
        {
            get { return _checkboxkieunoichong1; } //get mothod
            set { _checkboxkieunoichong1 = value; } //set mothod
        }
        public RebarBarType Bartypenoichongthep
        {
            get { return _bartypenoichongthep; } //get mothod
            set { _bartypenoichongthep = value; } //set mothod
        }
        public RebarHookType Bottomhooks
        {
            get { return _bottomhooks; } //get mothod
            set { _bottomhooks = value; } //set mothod
        }
        public RebarHookType Tophooks
        {
            get { return _tophooks; } //get mothod
            set { _tophooks = value; } //set mothod
        }
        public RebarBarType Bartypethepdoc
        {
            get { return _bartypethepdoc; } //get mothod
            set { _bartypethepdoc = value; } //set mothod
        }
        public int Soluongphuongy
        {
            get { return _soluongphuongy; } //get mothod
            set { _soluongphuongy = value; } //set mothod
        }
        public int Soluongphuongz
        {
            get { return _soluongphuongz; } //get mothod
            set { _soluongphuongz = value; } //set mothod
        }
        public double Cover
        {
            get { return _cover; } //get mothod
            set { _cover = value; } //set mothod
        }
        public double Chieudai
        {
            get { return _chieudai; } //get mothod
            set { _chieudai = value; } //set mothod
        }
        public double Chieucao
        {
            get { return _chieucao; } //get mothod
            set { _chieucao = value; } //set mothod
        }
        public double Chieurong
        {
            get { return _chieurong; } //get mothod
            set { _chieurong = value; } //set mothod
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
        public ElementId HostID
        {
            get { return _hostID; }
            set { _hostID = value; }
        }
        public string Partition
        {
            get { return _partition; }
            set { _partition = value; }
        }
        #endregion

        #region Khai báo constructor (hàm khởi tạo)
        //public Cls_ThepDoc(ElementId id)
        //{
        //    _id = id;
        //}
        public void AssignColumnDimensions(double height, double length, double width)
        {
            Chieucao = height;
            Chieudai = length;
            Chieurong = width;
        }
        #endregion

        #region khai báo hàm (method)
        public Rebar CreateRebar(Autodesk.Revit.DB.Document document, FamilyInstance column, RebarBarType barType, RebarHookType hookType)
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

        #endregion
    }
}
