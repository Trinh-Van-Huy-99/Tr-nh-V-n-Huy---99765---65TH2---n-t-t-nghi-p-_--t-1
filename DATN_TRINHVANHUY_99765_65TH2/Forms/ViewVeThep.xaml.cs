using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using DATN_TRINHVANHUY_99765_65TH2.Command;
using DATN_TRINHVANHUY_99765_65TH2.OOP;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DATN_TRINHVANHUY_99765_65TH2.Forms
{
    /// <summary>
    /// Interaction logic for ViewVeThep.xaml
    /// </summary>
    public partial class ViewVeThep : Window
    {


        public Autodesk.Revit.DB.Document Doc;
        //public Autodesk.Revit.UI.ExternalCommandData Command;

        public ViewVeThep(Autodesk.Revit.DB.Document doc /*, Autodesk.Revit.UI.ExternalCommandData command*/)
        {
            InitializeComponent();
            Doc = doc;
            //Command = command;

            #region Gán thông tin của rebartype vào trong combobox giao diện

            Cls_BienChuongTrinh.Rvt_RebarRebarHookTypes.Insert(0, null);
            cbb_bartypethepdai.ItemsSource = Cls_BienChuongTrinh.Rvt_RebarBarTypes;
            cbb_bartypethepdoc.ItemsSource = Cls_BienChuongTrinh.Rvt_RebarBarTypes;
            cbb_bartypenoichongcotthep.ItemsSource = Cls_BienChuongTrinh.Rvt_RebarBarTypes;
            cbb_shapethepdai.ItemsSource = Cls_BienChuongTrinh.Rvt_RebarBarShapes;
            cbb_hookatstart.ItemsSource = Cls_BienChuongTrinh.Rvt_RebarRebarHookTypes;
            cbb_hookatend.ItemsSource = Cls_BienChuongTrinh.Rvt_RebarRebarHookTypes;
            cbb_tophooks.ItemsSource = Cls_BienChuongTrinh.Rvt_RebarRebarHookTypes;
            cbb_bottomhooks.ItemsSource = Cls_BienChuongTrinh.Rvt_RebarRebarHookTypes;

            label_chieurong.Content = "( " + Cls_BienChuongTrinh.cls_ThepDoc.Chieurong  + " mm )";
            label_chieucao.Content ="( " + Cls_BienChuongTrinh.cls_ThepDoc.Chieucao  + " mm )";
            label_chieudaicot_L.Content= Cls_BienChuongTrinh.cls_ThepDoc.Chieudai + " \n (mm)";
            #endregion
            #region Hoạt ảnh thay đổi số lượng thép dọc của mặt cắt cột 

            //if(cbb_soluongthepdoc_Z.Text == "2")
            //{
            //    thanh1.Fill = new SolidColorBrush(Colors.Red);
            //    thanh2.Fill = new SolidColorBrush(Colors.AliceBlue);
            //    thanh3.Fill = new SolidColorBrush(Colors.AliceBlue);
            //    thanh4.Fill = new SolidColorBrush(Colors.AliceBlue);
            //    thanh5.Fill = new SolidColorBrush(Colors.AliceBlue);
            //    thanh6.Fill = new SolidColorBrush(Colors.AliceBlue);
            //    thanh7.Fill = new SolidColorBrush(Colors.AliceBlue);
            //    thanh8.Fill = new SolidColorBrush(Colors.AliceBlue);
            //    thanh9.Fill = new SolidColorBrush(Colors.Red);

            //    thanh10.Fill = new SolidColorBrush(Colors.Red);
            //    thanh11.Fill = new SolidColorBrush(Colors.AliceBlue);
            //    thanh12.Fill = new SolidColorBrush(Colors.AliceBlue);
            //    thanh13.Fill = new SolidColorBrush(Colors.AliceBlue);
            //    thanh14.Fill = new SolidColorBrush(Colors.AliceBlue);
            //    thanh15.Fill = new SolidColorBrush(Colors.AliceBlue);
            //    thanh16.Fill = new SolidColorBrush(Colors.AliceBlue);
            //    thanh17.Fill = new SolidColorBrush(Colors.AliceBlue);
            //    thanh18.Fill = new SolidColorBrush(Colors.Red);

            //    thanh19.Fill = new SolidColorBrush(Colors.AliceBlue);
            //    thanh20.Fill = new SolidColorBrush(Colors.AliceBlue);
            //    thanh21.Fill = new SolidColorBrush(Colors.AliceBlue);
            //    thanh22.Fill = new SolidColorBrush(Colors.AliceBlue);
            //    thanh23.Fill = new SolidColorBrush(Colors.AliceBlue);
            //    thanh24.Fill = new SolidColorBrush(Colors.AliceBlue);
            //    thanh25.Fill = new SolidColorBrush(Colors.AliceBlue);

            //    thanh26.Fill = new SolidColorBrush(Colors.AliceBlue);
            //    thanh27.Fill = new SolidColorBrush(Colors.AliceBlue);
            //    thanh28.Fill = new SolidColorBrush(Colors.AliceBlue);
            //    thanh29.Fill = new SolidColorBrush(Colors.AliceBlue);
            //    thanh30.Fill = new SolidColorBrush(Colors.AliceBlue);
            //    thanh31.Fill = new SolidColorBrush(Colors.AliceBlue);
            //    thanh32.Fill = new SolidColorBrush(Colors.AliceBlue);
            //}



            #endregion

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            #region lấy thông tin bố trí thép đai trên giao diện

            Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieuthepdai1 = bool.Parse(checkbox_kieuthepdai1.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieuthepdai2 = bool.Parse(checkbox_kieuthepdai2.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieuthepdai3 = bool.Parse(checkbox_kieuthepdai3.IsChecked.ToString());
            //Cls_BienChuongTrinh.cls_ThepDai.Bartypethepdai = ((RebarBarType)cbb_bartypethepdai.SelectedItem).Name;
            //Cls_BienChuongTrinh.cls_ThepDai.Shapethepdai =((RebarShape)cbb_shapethepdai.SelectedItem).Name;
            //Cls_BienChuongTrinh.cls_ThepDai.Hookatstart = ((RebarHookType)cbb_hookatstart.SelectedItem).Name;
            //Cls_BienChuongTrinh.cls_ThepDai.Hookatend = ((RebarHookType)cbb_hookatend.SelectedItem).Name;
            Cls_BienChuongTrinh.cls_ThepDai.Bartypethepdai = (RebarBarType)cbb_bartypethepdai.SelectedItem;
            Cls_BienChuongTrinh.cls_ThepDai.Shapethepdai = (RebarShape)cbb_shapethepdai.SelectedItem;
            Cls_BienChuongTrinh.cls_ThepDai.Hookatstart = (RebarHookType)cbb_hookatstart.SelectedItem;
            Cls_BienChuongTrinh.cls_ThepDai.Hookatend = (RebarHookType)cbb_hookatend.SelectedItem;
            Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieubotrithepdai1 = bool.Parse(checkbox_kieubotrithepdai1.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieubotrithepdai2 = bool.Parse(checkbox_kieubotrithepdai2.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieubotrithepdai3 = bool.Parse(checkbox_kieubotrithepdai3.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieuphantram = bool.Parse(checkbox_botriphantram.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieuchieudai = bool.Parse(checkbox_botrichieudai.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDai.L1phantram = double.Parse(txt_L1_phantram.Text);
            Cls_BienChuongTrinh.cls_ThepDai.L2phantram = double.Parse(txt_L2_phantram.Text);
            //Cls_BienChuongTrinh.cls_ThepDai.L3phantram = double.Parse(txt_L3_phantram.Text);
            Cls_BienChuongTrinh.cls_ThepDai.L1 = double.Parse(txt_L1_length.Text);
            Cls_BienChuongTrinh.cls_ThepDai.L2 = double.Parse(txt_L2_length.Text);
            //Cls_BienChuongTrinh.cls_ThepDai.L3 = double.Parse(txt_L3_length.Text);
            Cls_BienChuongTrinh.cls_ThepDai.S1 = double.Parse(txt_s1.Text);
            Cls_BienChuongTrinh.cls_ThepDai.S2 = double.Parse(txt_s2.Text);
            Cls_BienChuongTrinh.cls_ThepDai.S3 = double.Parse(txt_s3.Text);
            Cls_BienChuongTrinh.cls_ThepDai.Cover = double.Parse(txt_cover.Text);

            #endregion

            #region lấy thông tin bố trí thép dọc trên giao diện

            //Cls_BienChuongTrinh.cls_ThepDoc.Bartypethepdoc = ((RebarBarType)cbb_bartypethepdoc.SelectedItem).Name;
            Cls_BienChuongTrinh.cls_ThepDoc.Bartypethepdoc = ((RebarBarType)cbb_bartypethepdoc.SelectedItem);
            //Cls_BienChuongTrinh.cls_ThepDoc.Tophooks = ((RebarHookType)cbb_tophooks.SelectedItem).Name;
            //Cls_BienChuongTrinh.cls_ThepDoc.Bottomhooks = ((RebarHookType)cbb_bottomhooks.SelectedItem).Name;
            Cls_BienChuongTrinh.cls_ThepDoc.Tophooks = ((RebarHookType)cbb_tophooks.SelectedItem);
            Cls_BienChuongTrinh.cls_ThepDoc.Bottomhooks = ((RebarHookType)cbb_bottomhooks.SelectedItem);
            Cls_BienChuongTrinh.cls_ThepDoc.Soluongphuongz = int.Parse(cbb_soluongthepdoc_Z.Text);
            Cls_BienChuongTrinh.cls_ThepDoc.Soluongphuongy = int.Parse(cbb_soluongthepdoc_Y.Text);
            Cls_BienChuongTrinh.cls_ThepDoc.Cover = double.Parse(txt_cover.Text);
            Cls_BienChuongTrinh.cls_ThepDoc.Checkboxkieunoichong1 = bool.Parse(checkbox_kieunoichongcotthep1.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDoc.Checkboxkieunoichong2 = bool.Parse(checkbox_kieunoichongcotthep2.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDoc.Checkboxkieunoichong3 = bool.Parse(checkbox_kieunoichongcotthep3.IsChecked.ToString());
            //Cls_BienChuongTrinh.cls_ThepDoc.Bartypenoichongthep = ((RebarBarType)cbb_bartypenoichongcotthep.SelectedItem).Name;
            Cls_BienChuongTrinh.cls_ThepDoc.Bartypenoichongthep = ((RebarBarType)cbb_bartypenoichongcotthep.SelectedItem);

            Cls_BienChuongTrinh.cls_ThepDoc.Overlap = double.Parse(txt_overlap.Text);
            Cls_BienChuongTrinh.cls_ThepDoc.Covera1 = double.Parse(txt_a1.Text);
            Cls_BienChuongTrinh.cls_ThepDoc.Covera2 = double.Parse(txt_a2.Text);

            #endregion



            UIDocument uidoc = new UIDocument(Doc);

            Cmd_ChonCot vethep = new Cmd_ChonCot();

            FamilyInstance columnInstance = Cls_BienChuongTrinh.cls_ThepDoc.Element as FamilyInstance;

            //ColumnFramReinMaker newcot = new ColumnFramReinMaker(Command, columnInstance);


            vethep.VethepDoc(uidoc.Document, Cls_BienChuongTrinh.cls_ThepDoc.Element);

            vethep.VethepDai(uidoc.Document, Cls_BienChuongTrinh.cls_ThepDoc.Element);



            this.Close();
        }

        #region tương tác người dùng ẩn hiện các giao diện
        private void checkbox_kieuthepdai1_Checked(object sender, RoutedEventArgs e)
            {
                canvas_kieuthepdai1.Opacity = 1;
                canvas_kieuthepdai2.IsEnabled = false;
                canvas_kieuthepdai3.IsEnabled = false;
                checkbox_kieuthepdai2.IsChecked = false;
                checkbox_kieuthepdai3.IsChecked = false;
            checkboxshapethepdai.IsChecked = false;


        }

        private void checkbox_kieuthepdai1_Unchecked(object sender, RoutedEventArgs e)
            {
                canvas_kieuthepdai1.Opacity = 0.5;
                canvas_kieuthepdai1.IsEnabled = false;
            }

            private void checkbox_kieuthepdai2_Checked(object sender, RoutedEventArgs e)
            {
                canvas_kieuthepdai2.Opacity = 1;
                canvas_kieuthepdai1.IsEnabled = false;
                canvas_kieuthepdai3.IsEnabled = false;
                checkbox_kieuthepdai1.IsChecked = false;
                checkbox_kieuthepdai3.IsChecked = false;
            checkboxshapethepdai.IsChecked = false;



        }

        private void checkbox_kieuthepdai2_Unchecked(object sender, RoutedEventArgs e)
            {
                canvas_kieuthepdai2.Opacity = 0.5;
                canvas_kieuthepdai2.IsEnabled = false;

            }

            private void checkbox_kieuthepdai3_Checked(object sender, RoutedEventArgs e)
            {
                canvas_kieuthepdai3.Opacity = 1;
                canvas_kieuthepdai1.IsEnabled = false;
                canvas_kieuthepdai2.IsEnabled = false;
                checkbox_kieuthepdai2.IsChecked = false;
                checkbox_kieuthepdai1.IsChecked = false;
            checkboxshapethepdai.IsChecked = false;
            }

            private void checkbox_kieuthepdai3_Unchecked(object sender, RoutedEventArgs e)
            {
                canvas_kieuthepdai3.Opacity = 0.5;
                canvas_kieuthepdai3.IsEnabled = false;

            }

            private void checkbox_kieubotrithepdai1_Checked(object sender, RoutedEventArgs e)
            {
                canvas_kieubotrithepdai1.Opacity = 1;
                canvas_kieubotrithepdai2.IsEnabled = false;
                canvas_kieubotrithepdai3.IsEnabled = false;
                checkbox_kieubotrithepdai3.IsChecked = false;
                checkbox_kieubotrithepdai2.IsChecked = false;
            }

            private void checkbox_kieubotrithepdai1_Unchecked(object sender, RoutedEventArgs e)
            {
                canvas_kieubotrithepdai1.Opacity = 0.5;
                canvas_kieubotrithepdai1.IsEnabled = false;
            }

            private void checkbox_kieubotrithepdai2_Checked(object sender, RoutedEventArgs e)
            {
                canvas_kieubotrithepdai2.Opacity = 1;
                canvas_kieubotrithepdai1.IsEnabled = false;
                canvas_kieubotrithepdai3.IsEnabled = false;
                checkbox_kieubotrithepdai1.IsChecked = false;
                checkbox_kieubotrithepdai3.IsChecked = false;
            }

            private void checkbox_kieubotrithepdai2_Unchecked(object sender, RoutedEventArgs e)
            {
                canvas_kieubotrithepdai2.Opacity = 0.5;
                canvas_kieubotrithepdai2.IsEnabled = false;
            }

            private void checkbox_kieubotrithepdai3_Checked(object sender, RoutedEventArgs e)
            {
                canvas_kieubotrithepdai3.Opacity = 1;
                canvas_kieubotrithepdai2.IsEnabled = false;
                canvas_kieubotrithepdai1.IsEnabled = false;
                checkbox_kieubotrithepdai2.IsChecked = false;
                checkbox_kieubotrithepdai1.IsChecked = false;
            }

            private void checkbox_kieubotrithepdai3_Unchecked(object sender, RoutedEventArgs e)
            {
                canvas_kieubotrithepdai3.Opacity = 0.5;
                canvas_kieubotrithepdai3.IsEnabled = false;
            }

            private void checkbox_kieunoichongcotthep1_Checked(object sender, RoutedEventArgs e)
            {
                canvas_noichong1.Opacity = 1;
                checkbox_kieunoichongcotthep2.IsChecked = false;
                checkbox_kieunoichongcotthep3.IsChecked = false;
            }

            private void checkbox_kieunoichongcotthep1_Unchecked(object sender, RoutedEventArgs e)
            {
                canvas_noichong1.Opacity = 0.5;
            }

            private void checkbox_kieunoichongcotthep2_Checked(object sender, RoutedEventArgs e)
            {
                canvas_noichong2.Opacity = 1;
                checkbox_kieunoichongcotthep1.IsChecked = false;
                checkbox_kieunoichongcotthep3.IsChecked = false;
            }

            private void checkbox_kieunoichongcotthep2_Unchecked(object sender, RoutedEventArgs e)
            {
                canvas_noichong2.Opacity = 0.5;
            }

            private void checkbox_kieunoichongcotthep3_Checked(object sender, RoutedEventArgs e)
            {
                canvas_noichong3.Opacity = 1;
                checkbox_kieunoichongcotthep1.IsChecked = false;
                checkbox_kieunoichongcotthep2.IsChecked = false;
            }

            private void checkbox_kieunoichongcotthep3_Unchecked(object sender, RoutedEventArgs e)
            {
                canvas_noichong3.Opacity = 0.5;
            }

            private void checkbox_botriphantram_Checked(object sender, RoutedEventArgs e)
            {
            txt_L1_phantram.Opacity = 1;
            txt_L2_phantram.Opacity = 1;
            //txt_L3_phantram.Opacity = 1;
            checkbox_botrichieudai.IsChecked = false;
            
            }

            private void checkbox_botriphantram_Unchecked(object sender, RoutedEventArgs e)
            {
                txt_L1_phantram.Opacity = 0.5;
                txt_L2_phantram.Opacity = 0.5;
                //txt_L3_phantram.Opacity = 0.5;
            }

            private void checkbox_botrichieudai_Checked(object sender, RoutedEventArgs e)
            {
            txt_L1_length.Opacity = 1;
            txt_L2_length.Opacity = 1;
            //txt_L3_length.Opacity = 1;
            checkbox_botriphantram.IsChecked = false;

            }

            private void checkbox_botrichieudai_Unchecked(object sender, RoutedEventArgs e)
            {
                txt_L1_length.Opacity = 0.5;
                txt_L2_length.Opacity = 0.5;
                //txt_L3_length.Opacity = 0.5;
            }
        #endregion

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //nút chỉ vẽ thép đai
            #region lấy thông tin bố trí thép đai trên giao diện

            Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieuthepdai1 = bool.Parse(checkbox_kieuthepdai1.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieuthepdai2 = bool.Parse(checkbox_kieuthepdai2.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieuthepdai3 = bool.Parse(checkbox_kieuthepdai3.IsChecked.ToString());
            //Cls_BienChuongTrinh.cls_ThepDai.Bartypethepdai = ((RebarBarType)cbb_bartypethepdai.SelectedItem).Name;
            //Cls_BienChuongTrinh.cls_ThepDai.Shapethepdai = ((RebarShape)cbb_shapethepdai.SelectedItem).Name;
            //Cls_BienChuongTrinh.cls_ThepDai.Hookatstart = ((RebarHookType)cbb_hookatstart.SelectedItem).Name;
            //Cls_BienChuongTrinh.cls_ThepDai.Hookatend = ((RebarHookType)cbb_hookatend.SelectedItem).Name;
            Cls_BienChuongTrinh.cls_ThepDai.Bartypethepdai = ((RebarBarType)cbb_bartypethepdai.SelectedItem);
            Cls_BienChuongTrinh.cls_ThepDai.Shapethepdai = ((RebarShape)cbb_shapethepdai.SelectedItem);
            Cls_BienChuongTrinh.cls_ThepDai.Hookatstart = ((RebarHookType)cbb_hookatstart.SelectedItem);
            Cls_BienChuongTrinh.cls_ThepDai.Hookatend = ((RebarHookType)cbb_hookatend.SelectedItem);
            Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieubotrithepdai1 = bool.Parse(checkbox_kieubotrithepdai1.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieubotrithepdai2 = bool.Parse(checkbox_kieubotrithepdai2.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieubotrithepdai3 = bool.Parse(checkbox_kieubotrithepdai3.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieuphantram = bool.Parse(checkbox_botriphantram.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieuchieudai = bool.Parse(checkbox_botrichieudai.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDai.L1phantram = double.Parse(txt_L1_phantram.Text);
            Cls_BienChuongTrinh.cls_ThepDai.L2phantram = double.Parse(txt_L2_phantram.Text);
            //Cls_BienChuongTrinh.cls_ThepDai.L3phantram = double.Parse(txt_L3_phantram.Text);
            Cls_BienChuongTrinh.cls_ThepDai.L1 = double.Parse(txt_L1_length.Text);
            Cls_BienChuongTrinh.cls_ThepDai.L2 = double.Parse(txt_L2_length.Text);
            //Cls_BienChuongTrinh.cls_ThepDai.L3 = double.Parse(txt_L3_length.Text);
            Cls_BienChuongTrinh.cls_ThepDai.S1 = double.Parse(txt_s1.Text);
            Cls_BienChuongTrinh.cls_ThepDai.S2 = double.Parse(txt_s2.Text);
            Cls_BienChuongTrinh.cls_ThepDai.S3 = double.Parse(txt_s3.Text);
            Cls_BienChuongTrinh.cls_ThepDai.Cover = double.Parse(txt_cover.Text);

            #endregion

            #region lấy thông tin bố trí thép dọc trên giao diện

            Cls_BienChuongTrinh.cls_ThepDoc.Bartypethepdoc = ((RebarBarType)cbb_bartypethepdoc.SelectedItem);
            //Cls_BienChuongTrinh.cls_ThepDoc.Bartypethepdoc = ((RebarBarType)cbb_bartypethepdoc.SelectedItem).Name;
            //Cls_BienChuongTrinh.cls_ThepDoc.Tophooks = ((RebarHookType)cbb_tophooks.SelectedItem).Name;
            //Cls_BienChuongTrinh.cls_ThepDoc.Bottomhooks = ((RebarHookType)cbb_bottomhooks.SelectedItem).Name;
            Cls_BienChuongTrinh.cls_ThepDoc.Tophooks = ((RebarHookType)cbb_tophooks.SelectedItem);
            Cls_BienChuongTrinh.cls_ThepDoc.Bottomhooks = ((RebarHookType)cbb_bottomhooks.SelectedItem);

            Cls_BienChuongTrinh.cls_ThepDoc.Soluongphuongz = int.Parse(cbb_soluongthepdoc_Z.Text);
            Cls_BienChuongTrinh.cls_ThepDoc.Soluongphuongy = int.Parse(cbb_soluongthepdoc_Y.Text);
            Cls_BienChuongTrinh.cls_ThepDoc.Cover = double.Parse(txt_cover.Text);
            Cls_BienChuongTrinh.cls_ThepDoc.Checkboxkieunoichong1 = bool.Parse(checkbox_kieunoichongcotthep1.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDoc.Checkboxkieunoichong2 = bool.Parse(checkbox_kieunoichongcotthep2.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDoc.Checkboxkieunoichong3 = bool.Parse(checkbox_kieunoichongcotthep3.IsChecked.ToString());
            //Cls_BienChuongTrinh.cls_ThepDoc.Bartypenoichongthep = ((RebarBarType)cbb_bartypenoichongcotthep.SelectedItem).Name;
            Cls_BienChuongTrinh.cls_ThepDoc.Bartypenoichongthep = ((RebarBarType)cbb_bartypenoichongcotthep.SelectedItem);

            Cls_BienChuongTrinh.cls_ThepDoc.Overlap = double.Parse(txt_overlap.Text);
            Cls_BienChuongTrinh.cls_ThepDoc.Covera1 = double.Parse(txt_a1.Text);
            Cls_BienChuongTrinh.cls_ThepDoc.Covera2 = double.Parse(txt_a2.Text);

            #endregion



            UIDocument uidoc = new UIDocument(Doc);

            Cmd_ChonCot vethep = new Cmd_ChonCot();

            FamilyInstance columnInstance = Cls_BienChuongTrinh.cls_ThepDoc.Element as FamilyInstance;

            //ColumnFramReinMaker newcot = new ColumnFramReinMaker(Command, columnInstance);

            vethep.VethepDai(uidoc.Document, Cls_BienChuongTrinh.cls_ThepDoc.Element);



            this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //nút chỉ vẽ thép chủ
            #region lấy thông tin bố trí thép đai trên giao diện

            Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieuthepdai1 = bool.Parse(checkbox_kieuthepdai1.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieuthepdai2 = bool.Parse(checkbox_kieuthepdai2.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieuthepdai3 = bool.Parse(checkbox_kieuthepdai3.IsChecked.ToString());
            //Cls_BienChuongTrinh.cls_ThepDai.Bartypethepdai = ((RebarBarType)cbb_bartypethepdai.SelectedItem).Name;
            //Cls_BienChuongTrinh.cls_ThepDai.Shapethepdai = ((RebarShape)cbb_shapethepdai.SelectedItem).Name;
            //Cls_BienChuongTrinh.cls_ThepDai.Hookatstart = ((RebarHookType)cbb_hookatstart.SelectedItem).Name;
            //Cls_BienChuongTrinh.cls_ThepDai.Hookatend = ((RebarHookType)cbb_hookatend.SelectedItem).Name;
            Cls_BienChuongTrinh.cls_ThepDai.Bartypethepdai = ((RebarBarType)cbb_bartypethepdai.SelectedItem);
            Cls_BienChuongTrinh.cls_ThepDai.Shapethepdai = ((RebarShape)cbb_shapethepdai.SelectedItem);
            Cls_BienChuongTrinh.cls_ThepDai.Hookatstart = ((RebarHookType)cbb_hookatstart.SelectedItem);
            Cls_BienChuongTrinh.cls_ThepDai.Hookatend = ((RebarHookType)cbb_hookatend.SelectedItem);
            Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieubotrithepdai1 = bool.Parse(checkbox_kieubotrithepdai1.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieubotrithepdai2 = bool.Parse(checkbox_kieubotrithepdai2.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieubotrithepdai3 = bool.Parse(checkbox_kieubotrithepdai3.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieuphantram = bool.Parse(checkbox_botriphantram.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieuchieudai = bool.Parse(checkbox_botrichieudai.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDai.L1phantram = double.Parse(txt_L1_phantram.Text);
            Cls_BienChuongTrinh.cls_ThepDai.L2phantram = double.Parse(txt_L2_phantram.Text);
            //Cls_BienChuongTrinh.cls_ThepDai.L3phantram = double.Parse(txt_L3_phantram.Text);
            Cls_BienChuongTrinh.cls_ThepDai.L1 = double.Parse(txt_L1_length.Text);
            Cls_BienChuongTrinh.cls_ThepDai.L2 = double.Parse(txt_L2_length.Text);
            //Cls_BienChuongTrinh.cls_ThepDai.L3 = double.Parse(txt_L3_length.Text);
            Cls_BienChuongTrinh.cls_ThepDai.S1 = double.Parse(txt_s1.Text);
            Cls_BienChuongTrinh.cls_ThepDai.S2 = double.Parse(txt_s2.Text);
            Cls_BienChuongTrinh.cls_ThepDai.S3 = double.Parse(txt_s3.Text);
            Cls_BienChuongTrinh.cls_ThepDai.Cover = double.Parse(txt_cover.Text);
            Cls_BienChuongTrinh.cls_ThepDai.Checkboxkieushape = bool.Parse(checkboxshapethepdai.IsChecked.ToString());
            #endregion

            #region lấy thông tin bố trí thép dọc trên giao diện

            Cls_BienChuongTrinh.cls_ThepDoc.Bartypethepdoc = ((RebarBarType)cbb_bartypethepdoc.SelectedItem);
            //Cls_BienChuongTrinh.cls_ThepDoc.Bartypethepdoc = ((RebarBarType)cbb_bartypethepdoc.SelectedItem).Name;
            //Cls_BienChuongTrinh.cls_ThepDoc.Tophooks = ((RebarHookType)cbb_tophooks.SelectedItem).Name;
            //Cls_BienChuongTrinh.cls_ThepDoc.Bottomhooks = ((RebarHookType)cbb_bottomhooks.SelectedItem).Name;
            Cls_BienChuongTrinh.cls_ThepDoc.Tophooks = ((RebarHookType)cbb_tophooks.SelectedItem);
            Cls_BienChuongTrinh.cls_ThepDoc.Bottomhooks = ((RebarHookType)cbb_bottomhooks.SelectedItem);

            Cls_BienChuongTrinh.cls_ThepDoc.Soluongphuongz = int.Parse(cbb_soluongthepdoc_Z.Text);
            Cls_BienChuongTrinh.cls_ThepDoc.Soluongphuongy = int.Parse(cbb_soluongthepdoc_Y.Text);
            Cls_BienChuongTrinh.cls_ThepDoc.Cover = double.Parse(txt_cover.Text);
            Cls_BienChuongTrinh.cls_ThepDoc.Checkboxkieunoichong1 = bool.Parse(checkbox_kieunoichongcotthep1.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDoc.Checkboxkieunoichong2 = bool.Parse(checkbox_kieunoichongcotthep2.IsChecked.ToString());
            Cls_BienChuongTrinh.cls_ThepDoc.Checkboxkieunoichong3 = bool.Parse(checkbox_kieunoichongcotthep3.IsChecked.ToString());
            //Cls_BienChuongTrinh.cls_ThepDoc.Bartypenoichongthep = ((RebarBarType)cbb_bartypenoichongcotthep.SelectedItem).Name;
            Cls_BienChuongTrinh.cls_ThepDoc.Bartypenoichongthep = ((RebarBarType)cbb_bartypenoichongcotthep.SelectedItem);

            Cls_BienChuongTrinh.cls_ThepDoc.Overlap = double.Parse(txt_overlap.Text);
            Cls_BienChuongTrinh.cls_ThepDoc.Covera1 = double.Parse(txt_a1.Text);
            Cls_BienChuongTrinh.cls_ThepDoc.Covera2 = double.Parse(txt_a2.Text);

            #endregion



            UIDocument uidoc = new UIDocument(Doc);

            Cmd_ChonCot vethep = new Cmd_ChonCot();

            FamilyInstance columnInstance = Cls_BienChuongTrinh.cls_ThepDoc.Element as FamilyInstance;

            //ColumnFramReinMaker newcot = new ColumnFramReinMaker(Command, columnInstance);


            vethep.VethepDoc(uidoc.Document, Cls_BienChuongTrinh.cls_ThepDoc.Element);

            this.Close();
        }

        private void checkboxshapethepdai_Checked(object sender, RoutedEventArgs e)
        {
            cbb_shapethepdai.Opacity = 1;
            cbb_hookatstart.Opacity = 1;
            cbb_hookatend.Opacity = 1;
            labelhookatend.Opacity = 1;
            labelhookatstart.Opacity = 1;
            labelshape.Opacity = 1;
            checkbox_kieuthepdai1.IsChecked = false;
            checkbox_kieuthepdai2.IsChecked = false;
            checkbox_kieuthepdai3.IsChecked = false;
        }

        private void checkboxshapethepdai_Unchecked(object sender, RoutedEventArgs e)
        {
            cbb_shapethepdai.Opacity = 0.5;
            cbb_hookatstart.Opacity = 0.5;
            cbb_hookatend.Opacity = 0.5;
            labelhookatend.Opacity = 0.5;
            labelhookatstart.Opacity = 0.5;
            labelshape.Opacity = 0.5;
        }
    }
}
