using MahApps.Metro.Controls;
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

namespace FormulaBineFormata
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        string contectori = "¬∧∨→↔";
        public MainWindow()
        {
            InitializeComponent();
        }

        string tostrict(string text)
        {
            foreach (char ch in contectori)
            {
                for (int i = text.Length - 1; i >= 0; i--)
                {
                    if (text[i] == ch)
                    {
                        int[] ins = { -1, -1 };
                        bool[] ep = { false, false };
                        if (text[i + 1] == '(')
                        {
                            int cnt = 1;
                            for (int j = i + 2; j < text.Length; j++)
                            {
                                if (text[j] == '(')
                                    cnt++;
                                else if (text[j] == ')')
                                {
                                    cnt--;
                                    if (cnt == 0)
                                    {
                                        if (text[j + 1] == ')')
                                            ep[1] = true;
                                        ins[1] = j;
                                        //text = text.Insert(j, ")");
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if ((i+2) < text.Length && text[i + 2] == ')')
                                ep[1] = true;
                            ins[1] = i + 2;
                            //text = text.Insert(i + 2, ")");
                        }
                        if (ch != '¬')
                        {
                            if (text[i - 1] == ')')
                            {
                                int cnt = 1;
                                for (int j = i - 2; j >= 0; j--)
                                {
                                    if (text[j] == ')')
                                        cnt++;
                                    else if (text[j] == '(')
                                    {
                                        cnt--;
                                        if (cnt == 0)
                                        {
                                            if (text[j - 1] == '(')
                                                ep[0] = true;
                                            ins[0] = j;
                                            //text = text.Insert(j, "(");
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if ((i-2)>=0 && text[i - 2] == '(')
                                    ep[0] = true;
                                ins[0] = i - 1;
                                //text = text.Insert(i - 1, "(");
                            }
                        }
                        else
                        {
                            if ((i-1) >= 0 && text[i - 1] == '(')
                                ep[0] = true;
                            ins[0] = i;
                            //text = text.Insert(i, "(");
                        }
                        if(!ep[0] || !ep[1])
                        {
                            text = text.Insert(ins[1], ")");
                            text = text.Insert(ins[0], "(");
                        }
                    }
                }
            }
            return text;
        }

        private void Formula_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine(tostrict(formula.Text));
        }
    }
}
