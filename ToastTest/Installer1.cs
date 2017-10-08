using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace ToastTest
{
    [RunInstaller(true)]
    public partial class Installer1 : Installer
    {
        public Installer1() : base() {
            
        }
    }
}
