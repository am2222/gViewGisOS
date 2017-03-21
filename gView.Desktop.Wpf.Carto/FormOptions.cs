﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using gView.Framework.UI;

namespace gView.Desktop.Wpf.Carto
{
    public partial class FormOptions : Form
    {
        private IMapDocument _doc;
        private IMapOptionPage _page;

        public FormOptions(IMapDocument document, IMapOptionPage page)
        {
            _doc = document;
            _page = page;

            InitializeComponent();

            Panel panel = _page.OptionPage(_doc);
            if (panel != null)
            {
                this.Width = panel.Width + 10;
                this.Height = panel.Height + 100;
                panelContainer.Controls.Add(panel);

                this.Text += ": " + page.Title;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (_page != null)
                _page.Commit();
        }
    }
}