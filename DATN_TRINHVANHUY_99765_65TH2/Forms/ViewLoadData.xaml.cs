using DATN_TRINHVANHUY_99765_65TH2.Command;
using System;
using System.Collections.Generic;
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
using static MaterialDesignThemes.Wpf.Theme;

namespace DATN_TRINHVANHUY_99765_65TH2.Forms
{
    /// <summary>
    /// Interaction logic for ViewLoadData.xaml
    /// </summary>
    public partial class ViewLoadData : Window
    {
        ColumnFramReinMaker m_dataBuffer = null;
        // Định nghĩa một delegate để sử dụng cho sự kiện
        public delegate void LoadDataClickedEventHandler(object sender, EventArgs e);

        // Định nghĩa sự kiện LoadDataClicked
        public event EventHandler LoadDataClicked;

        public ViewLoadData(ColumnFramReinMaker dataBuffer)
        {
            InitializeComponent();
            btnLuuVaDong.Click += button_click;
            m_dataBuffer = dataBuffer;
        }
        private void button_click(object sender, RoutedEventArgs e)
        {
            // Kích hoạt sự kiện LoadDataClicked khi nút được nhấn
            LoadDataClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnLoadData_Click(object sender, RoutedEventArgs e)
        {




        }
    }
}
