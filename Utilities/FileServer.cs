using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace Utilities {

    public class FileServer {

        public static string GetUNCPath(string path) {
            if (!IsNetworkDrive(path)) return path;   // If it's not a network path, just return the path unchanged

            string rval = path;
            string driveprefix = path.Substring(0, 2);
            string unc;

            if (driveprefix != "\\") {
                ManagementObject mo = new();
                try {
                    mo.Path = new ManagementPath(string.Format("Win32_LogicalDisk='{0}'", driveprefix));
                    unc = (string)mo["ProviderName"];
                    rval = path.Replace(driveprefix, unc);
                } catch {
                    throw;
                }
            }

            if (rval == null) { rval = path; }

            return rval;
        }

        public static bool IsNetworkDrive(string path) {
            FileInfo f = new FileInfo(path);
            string driveRoot = Path.GetPathRoot(f.FullName); // Example return "C:\"
            // find the drive 
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            foreach (var drive in drives) {
                string driveName = drive.Name; // C:\, E:\, etc:\
                if (driveName == driveRoot) // if this is the drive 
                {
                    System.IO.DriveType driveType = drive.DriveType;
                    if (driveType == System.IO.DriveType.Network) return true;
                }
            }
            return false;
        }

    }

}
