using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_TRINHVANHUY_99765_65TH2.OOP
{
    public class Cls_ThepDai
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
        private bool _checkboxkieuthepdai1;
        private bool _checkboxkieuthepdai2;
        private bool _checkboxkieuthepdai3;
        private RebarBarType _bartypethepdai;
        private RebarShape _shapethepdai;
        private RebarHookType _hookatstart;
        private RebarHookType _hookatend;
        private bool _checkboxkieubotrithepdai1;
        private bool _checkboxkieubotrithepdai2;
        private bool _checkboxkieubotrithepdai3;
        private bool _checkboxkieuphantram;
        private bool _checkboxkieuchieudai;
        private bool _checkboxkieushape;
        private double _l1phantram;
        private double _l2phantram;
        //private double _l3phantram;
        private double _l1;
        private double _l2;
        //private double _l3;
        private double _s1;
        private double _s2;
        private double _s3;
        #endregion

        #region khai báo prorperty
        public bool Checkboxkieushape
        {
            get { return _checkboxkieushape; } //get mothod
            set { _checkboxkieushape = value; } //set mothod
        }
        public double S1
        {
            get { return _s1; } //get mothod
            set { _s1 = value; } //set mothod
        }
        public double S2
        {
            get { return _s2; } //get mothod
            set { _s2 = value; } //set mothod
        }
        public double S3
        {
            get { return _s3; } //get mothod
            set { _s3 = value; } //set mothod
        }
        public double L1
        {
            get { return _l1; } //get mothod
            set { _l1 = value; } //set mothod
        }
        public double L2
        {
            get { return _l2; } //get mothod
            set { _l2 = value; } //set mothod
        }
        //public double L3
        //{
        //    get { return _l3; } //get mothod
        //    set { _l3 = value; } //set mothod
        //}
        public double L1phantram
        {
            get { return _l1phantram; } //get mothod
            set { _l1phantram = value; } //set mothod
        }
        public double L2phantram
        {
            get { return _l2phantram; } //get mothod
            set { _l2phantram = value; } //set mothod
        }
        //public double L3phantram
        //{
        //    get { return _l3phantram; } //get mothod
        //    set { _l3phantram = value; } //set mothod
        //}
        public bool Checkboxkieuchieudai
        {
            get { return _checkboxkieuchieudai; } //get mothod
            set { _checkboxkieuchieudai = value; } //set mothod
        }
        public bool Checkboxkieuphantram
        {
            get { return _checkboxkieuphantram; } //get mothod
            set { _checkboxkieuphantram = value; } //set mothod
        }
        public bool Checkboxkieubotrithepdai1
        {
            get { return _checkboxkieubotrithepdai1; } //get mothod
            set { _checkboxkieubotrithepdai1 = value; } //set mothod
        }
        public bool Checkboxkieubotrithepdai2
        {
            get { return _checkboxkieubotrithepdai2; } //get mothod
            set { _checkboxkieubotrithepdai2 = value; } //set mothod
        }
        public bool Checkboxkieubotrithepdai3
        {
            get { return _checkboxkieubotrithepdai3; } //get mothod
            set { _checkboxkieubotrithepdai3 = value; } //set mothod
        }
        public RebarHookType Hookatend
        {
            get { return _hookatend; } //get mothod
            set { _hookatend = value; } //set mothod
        }
        public RebarHookType Hookatstart
        {
            get { return _hookatstart; } //get mothod
            set { _hookatstart = value; } //set mothod
        }
        public RebarShape Shapethepdai
        {
            get { return _shapethepdai; } //get mothod
            set { _shapethepdai = value; } //set mothod
        }
        public RebarBarType Bartypethepdai
        {
            get { return _bartypethepdai; } //get mothod
            set { _bartypethepdai = value; } //set mothod
        }
        public bool Checkboxkieuthepdai1
        {
            get { return _checkboxkieuthepdai1; } //get mothod
            set { _checkboxkieuthepdai1 = value; } //set mothod
        }
        public bool Checkboxkieuthepdai2
        {
            get { return _checkboxkieuthepdai2; } //get mothod
            set { _checkboxkieuthepdai2 = value; } //set mothod
        }
        public bool Checkboxkieuthepdai3
        {
            get { return _checkboxkieuthepdai3; } //get mothod
            set { _checkboxkieuthepdai3 = value; } //set mothod
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

        #region Khai báo constructor
        //public Cls_ThepDai(ElementId id)
        //{
        //    _id = id;
        //}

        //public Cls_ThepDai()
        //{
        //}

        #endregion

        #region khai báo hàm (method)

        #endregion
    }
}
