using Autodesk.Revit.UI;
using System;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace DATN_TRINHVANHUY_99765_65TH2
{
    public class ExternalApplication : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            //Create The Plugin Tab.
            application.CreateRibbonTab("TVH TOOLS");

            #region TẠO PANEL vẽ thép cột
            //Create The Plugin Panel.
            RibbonPanel Panel = application.CreateRibbonPanel("TVH TOOLS", "Vẽ Thép Cột");
            Assembly assembly = Assembly.GetExecutingAssembly();
            #endregion

            #region TẠO PANEL tính toán
            //Create The Plugin Panel.
            RibbonPanel Panel1 = application.CreateRibbonPanel("TVH TOOLS", "Tính Toán Cốt Thép");
            Assembly assembly1 = Assembly.GetExecutingAssembly();
            #endregion

            #region Create chọn cột
            //Create chọn cột

            PushButtonData Button1 = new PushButtonData("Button1", "Triển Khai", assembly.Location, "DATN_TRINHVANHUY_99765_65TH2.Command.Cmd_ChonCot");
            PushButton pushButton1 = Panel.AddItem(Button1) as PushButton;

            Uri UriPath1 = new Uri("pack://application:,,,/DATN_TRINHVANHUY_99765_65TH2;component/PluginIcons/Columns.png");
            BitmapImage Image1 = new BitmapImage(UriPath1);
            pushButton1.LargeImage = Image1;
            #endregion

            #region Create dữ liệu
            //Create chọn cột

            PushButtonData Button0 = new PushButtonData("Button0", "  Dữ Liệu  ", assembly.Location, "DATN_TRINHVANHUY_99765_65TH2.Command.Cmd_Dulieu");
            PushButton pushButton0 = Panel.AddItem(Button0) as PushButton;

            Uri UriPath0 = new Uri("pack://application:,,,/DATN_TRINHVANHUY_99765_65TH2;component/PluginIcons/LoadFile.png");
            BitmapImage Image0 = new BitmapImage(UriPath0);
            pushButton0.LargeImage = Image0;
            #endregion

            #region Create trợ giúp
            //Create The Plugin Button.
            PushButtonData Button3 = new PushButtonData("Button3", "Trợ Giúp", assembly.Location, "DATN_TRINHVANHUY_99765_65TH2.Command.Cmd_VeThep");
            PushButton pushButton3 = Panel.AddItem(Button3) as PushButton;

            Uri UriPath3 = new Uri("pack://application:,,,/DATN_TRINHVANHUY_99765_65TH2;component/PluginIcons/Help.png");
            BitmapImage Image3 = new BitmapImage(UriPath3);
            pushButton3.LargeImage = Image3;
            #endregion



            #region Create tải dữ liệu
            //Create The Plugin Button.
            PushButtonData Button4 = new PushButtonData("btn_Loc1", "Tải Dữ Liệu Tính Toán", assembly1.Location, "DATN_TRINHVANHUY_99765_65TH2.Command.Cmd_LoadData");
            PushButton pushButton4 = Panel1.AddItem(Button4) as PushButton;

            Uri UriPath4 = new Uri("pack://application:,,,/DATN_TRINHVANHUY_99765_65TH2;component/PluginIcons/loaddata.ico");
            BitmapImage Image4 = new BitmapImage(UriPath4);
            pushButton4.LargeImage = Image4;
            #endregion



            #region Create tính toán
            //Create The Plugin Button.
            PushButtonData Button2 = new PushButtonData("Button2", "Tính Toán", assembly1.Location, "DATN_TRINHVANHUY_99765_65TH2.Command.Cmd_Tinhtoan");
            PushButton pushButton2 = Panel1.AddItem(Button2) as PushButton;

            Uri UriPath2 = new Uri("pack://application:,,,/DATN_TRINHVANHUY_99765_65TH2;component/PluginIcons/TinhToan.png");
            BitmapImage Image2 = new BitmapImage(UriPath2);
            pushButton2.LargeImage = Image2;
            #endregion 
            return Result.Succeeded;
        }
    }
}
