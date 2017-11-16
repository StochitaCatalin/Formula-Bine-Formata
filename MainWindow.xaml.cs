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
using System.Windows.Interop;
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
        public class Nod
        {
            public List<Nod> nods = new List<Nod>();
            public List<char> ch = new List<char>();
        }
        Nod MyNod;
        string text;
        List<int> posindex = new List<int>();
        int pos = 0;
        int cnt = 0;
        string conectori = "¬∧∨→↔";
        string conector = "∧∨→↔";
        bool Isprop(char ch)
        {
            return (ch >= 'A' && ch <= 'Z' || ch >= 'a' && ch <= 'z');
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        string tostrict(string text)
        {
            foreach (char ch in conectori)
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
                                        if ((j+1) < text.Length && text[j + 1] == ')')
                                            ep[1] = true;
                                        ins[1] = j+1;
                                        //text = text.Insert(j, ")");
                                        break;
                                    }
                                }
                            }
                            //if (cnt != 0)
                            //    ins[1] = text.Length;
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
                                            if ((j-1) >= 0 && text[j - 1] == '(')
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
                        if (!(ep[0] && ep[1]))
                        {
                            text = text.Insert(ins[1], ")");
                            posindex.Insert(ins[1], -1);
                            text = text.Insert(ins[0], "(");
                            posindex.Insert(ins[0], -1);
                            //for (int n = ins[0] + 1; n < ins[1]; n++)
                            //    posindex[n] = -1;
                        }
                        //else
                        //{
                         //   posindex[ins[0]-1] = -1;
                         //   posindex[ins[1]] = -1;
                        //}
                        //else
                        //    for (int n = ins[0] - 1; n <= ins[1]; n++)
                        //        posindex[n] = -1;
                    }
                }
            }
            return text;
        }
        Nod createnod(char cha)
        {
            Nod tmp = new Nod();
            tmp.ch.Add(cha);
            return tmp;
        }
        List<Nod> Compressnods(List<Nod> nc,int nr = 0)
        {
            int j = 0;
            if (nr == 0)
                nr = nc.Count - 1;
            for (int i = nc.Count - 2; i >= 0 && j < nr; i--,j++)
            {
                nc[i].nods.Add(nc[i + 1]);
                nc.RemoveAt(i + 1);
            }
            return nc;
        }
        Nod tonod()
        {
            List<char> need = new List<char>();
            List<Nod> buffernods = new List<Nod>();
            need.Add('p');
            //(P→(Q∧(S→T)))
            //
            while (pos < text.Length)
            {
                if(need.Count == 0)
                {
                    Console.WriteLine("Not expected anything");
                    break;
                }
                if (need[need.Count - 1] == 'p')
                {
                    if (Isprop(text[pos]))
                    {
                        need.RemoveAt(need.Count - 1);
                        if (buffernods.Count > 0)
                            buffernods[buffernods.Count - 1].nods.Add(createnod(text[pos]));
                        else
                            buffernods.Add(createnod(text[pos]));
                    }
                    else if (text[pos] == '(')
                    {
                        need.RemoveAt(need.Count - 1);
                        need.Add(')');
                        need.Add('p');
                        need.Add('c');
                        need.Add('p');
                        buffernods.Add(createnod(text[pos]));
                    }
                    else if(text[pos] == conectori[0])
                    {
                        if (need[need.Count - 2] == 'c')
                        {
                            need.RemoveAt(need.Count - 1);
                            need.RemoveAt(need.Count - 1);
                            buffernods[buffernods.Count - 1].ch.Add(text[pos]);
                        }
                    }
                }
                else if (need[need.Count - 1] == text[pos])
                {
                    need.RemoveAt(need.Count - 1);
                    buffernods[buffernods.Count - 1].ch.Add(')');
                    buffernods = Compressnods(buffernods, 1);
                }
                else if(need[need.Count - 1] == 'c' && conector.Contains(text[pos]))
                {
                    need.RemoveAt(need.Count - 1);
                    buffernods[buffernods.Count - 1].ch.Add(text[pos]);
                }
                else
                {
                    Console.WriteLine("Problema " + text[pos]);
                    break;
                }
                pos++;
            }
            Nod tmp = buffernods[0];
            return tmp;
        }
        private void reset()
        {
            pos = 0;
            cnt = 0;
            posindex.Clear();
            for (int i = 0; i < text.Length; i++)
                posindex.Add(i);
        }
        private void Formula_TextChanged(object sender, TextChangedEventArgs e)
        {
            text = formula.Text;
            reset();
            
            text = tostrict(text);
            Console.WriteLine(text);
            MyNod = tonod();
        }

        private void aratanoduri_Click(object sender, RoutedEventArgs e)
        {
            MainForm form = new MainForm(MyNod);
            form.ShowDialog();
        }
    }
}
