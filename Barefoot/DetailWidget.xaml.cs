using System.ComponentModel;
using System.Windows.Controls;

namespace Barefoot
{
    public partial class DetailWidget : UserControl
    {
        private string caption = "caption";
        private string text = "TEXT";

        [DefaultValue("caption")]
        public string Caption { 
            get { return this.caption; } 
            set
            {
                this.caption = value;
                widgetLabelTextBlock.Text = this.caption; 
            } 
        }

        [DefaultValue("TEXT")]
        public string Text
        {
            get { return this.text; }
            set
            {
                this.text = value;
                widgetTextBlock.Text = this.text;
            }
        }

        public DetailWidget()
        {
            InitializeComponent();
        }
    }
}