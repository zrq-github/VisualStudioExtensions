﻿using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using Microsoft.Web.WebView2.Wpf;

namespace FirstWindow
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("c2a5f827-8d25-485d-868b-d8e0879f39db")]
    public class MyWindow : ToolWindowPane
    {
        private readonly WebView2 webView;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyWindow"/> class.
        /// </summary>
        public MyWindow() : base(null)
        {
            this.Caption = "MyWindow";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new MyWindowControl();
        }
    }
}
