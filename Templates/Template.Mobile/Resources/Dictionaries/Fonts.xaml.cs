﻿using System;
using Template.Mobile.Helpers;

namespace Template.Mobile.Resources.Dictionaries
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Fonts
    {
        public Fonts()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }
        }
    }
}