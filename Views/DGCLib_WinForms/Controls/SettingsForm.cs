using System.Security.Permissions;
using System.Windows.Forms;

namespace DGCLib_WinForms.Controls
{
    public partial class SettingsForm : Form
    {
        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        public SettingsForm()
        {
            InitializeComponent();

            _close.Click += (o, e) =>
            {
                Close();
            };
        }

        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        public void SetObject(object o)
        {
            _propertyGrid.SelectedObject = o;
        }
    }
}