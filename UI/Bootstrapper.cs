﻿using System.Windows;
using Caliburn.Micro;
using UI.ViewModels;

namespace UI
{
    class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<EmployeeViewModel>();
        }
    }
}
