using BespokeFusion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace KSHWP
{
    class Error
    {

     public Error(Exception error) {

            var msgerror = new CustomMaterialMessageBox
            {
                TxtMessage = { Text = "ERROR : " + error, Foreground = Brushes.White },
                TxtTitle = { Text = "ERROR", Foreground = Brushes.White },
                BtnCancel = { Content = "OK", Background = Brushes.Red, BorderBrush = Brushes.Gray },
                BtnOk = { Visibility = Visibility.Hidden },
                MainContentControl = { Background = Brushes.Gray },
                TitleBackgroundPanel = { Background = Brushes.Red },
                BorderBrush = Brushes.Red


            };

            msgerror.Show();

        }
    }
}
