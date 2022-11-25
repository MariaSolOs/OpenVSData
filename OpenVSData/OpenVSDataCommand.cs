using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;

namespace OpenVSData
{
    internal sealed class OpenVSDataCommand
    {
        internal static OpenVSDataCommand Instance
        {
            get;
            private set;
        }

        private readonly AsyncPackage serviceProvider;

        private OpenVSDataCommand(OleMenuCommandService commandService, AsyncPackage package)
        {
            this.serviceProvider = package;

            var commandID = new CommandID(PackageGuids.guidOpenVSDataPackageCmdSet, PackageIds.OpenVSDataId);
            var button = new MenuCommand(this.OpenVSDataFolder, commandID);
            commandService.AddCommand(button);
        }

        internal static async Task InitializeAsync(AsyncPackage package)
        {
            var commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new OpenVSDataCommand(commandService, package);
        }

        private void OpenVSDataFolder(object sender, EventArgs e)
        {
            string folder = this.serviceProvider.UserLocalDataPath;

            if (Directory.Exists(folder))
            {
                var startInfo = new ProcessStartInfo
                {
                    Arguments = folder,
                    FileName = "explorer.exe"
                };
                Process.Start(startInfo);
            }
            else
            {
                MessageBox.Show($"Visual Studio is broken, {folder} does not exist!");
            }
        }
    }
}
