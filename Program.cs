﻿using System;
using System.Windows.Forms;
using ProjektWektor3D.Forms;

namespace ProjektWektor3D
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}