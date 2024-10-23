using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using PortalCore.Logic;
using PortalCommon;

namespace PortalCore.Admin
{
    public partial class CoreUpdate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!PermissionControl.CheckPermission("CoreUpdates"))
            {
                Response.Redirect("~/Error/403");
            }
            CheckPermissions();
        }

        public void CheckPermissions()
        {
            btnSubmitUpdate.Enabled = PermissionControl.CheckPermission("SubmitUpdate");
        }

        protected void btnSubmitUpdate_OnClick(object sender, EventArgs e)
        {
            if (fuCoreUpdate.HasFile)
            {
                if (Path.GetExtension(fuCoreUpdate.FileName).ToLower() == ".update")
                {
                    try
                    {
                        var updateFileName = Path.GetFileNameWithoutExtension(fuCoreUpdate.FileName);
                        fuCoreUpdate.SaveAs(Server.MapPath("~/") + updateFileName + ".zip");
                        ExtractZipFile(Server.MapPath("~/") + updateFileName + ".zip", "1568KSUpdate6413",
                            Server.MapPath("~/"));
                    }
                    catch (Exception ex)
                    {
                        var eLogger = new ErrorLogger();
                        eLogger.AddError("خطا در ارسال فایل به روزرسانی",
                            "زمان ارسال فایل به روزرسانی خطای زیر رخ داد:<br/>" + ex.Message,
                            Page.User.Identity.Name + "/btnSubmitUpdate_OnClick",
                            "CoreUpdate.aspx/btnSubmitUpdate_OnClick");
                        ltUpdateMessages.Text = "خطا در ارسال فایل به روزرسانی:<br/>" + ex.Message;
                    }
                    if (
                        File.Exists(Server.MapPath("~/") + Path.GetFileNameWithoutExtension(fuCoreUpdate.FileName) +
                                    ".zip"))
                    {
                        File.Delete(Server.MapPath("~/") + Path.GetFileNameWithoutExtension(fuCoreUpdate.FileName) +
                                    ".zip");
                    }
                    if (File.Exists(Server.MapPath("~/query.sql")))
                    {
                        File.Delete(Server.MapPath("~/query.sql"));
                    }
                }
                else
                {
                    ltUpdateMessages.Text = "فایل انتخابی مربوط به به روزرسانی سیستم نیست.";
                }
            }
            else
            {
                ltUpdateMessages.Text = "فایل به روزرسانی انتخاب نشده است.";
            }

        }


        public bool ExtractZipFile(string archiveFilenameIn, string password, string outFolder)
        {
            ZipFile zf = null;
            try
            {
                var fs = File.OpenRead(archiveFilenameIn);
                zf = new ZipFile(fs);
                if (!String.IsNullOrEmpty(password))
                {
                    zf.Password = password;     // AES encrypted entries are handled automatically
                }

                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile)
                    {
                        continue;           // Ignore directories
                    }
                    var entryFileName = zipEntry.Name;
                    // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    // Optionally match entrynames against a selection list here to skip as desired.
                    // The unpacked length is available in the zipEntry.Size property.

                    var buffer = new byte[4096];     // 4K is optimum
                    var zipStream = zf.GetInputStream(zipEntry);

                    // Manipulate the output filename here as desired.
                    var fullZipToPath = Path.Combine(outFolder, entryFileName);
                    var directoryName = Path.GetDirectoryName(fullZipToPath);
                    if (directoryName != null)
                    {
                        if (directoryName.Length > 0)
                        {
                            Directory.CreateDirectory(directoryName);
                        }
                    }

                    // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    // of the file, but does not waste memory.
                    // The "using" will close the stream even if an exception occurs.
                    using (var streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
                ltUpdateMessages.Text = "به روزرسانی کامل شد.";
                return true;
            }
            catch (Exception ex)
            {
                var eLogger = new ErrorLogger();
                eLogger.AddError("خطا در گشودن فایل به روزرسانی",
                    "زمان گشودن فایل به روزرسانی خطای زیر رخ داد:<br/>" + ex.Message,
                    Page.User.Identity.Name + "/ExtractZipFile", "CoreUpdate.aspx/ExtractZipFile");
                return false;
            }
            finally
            {
                if (zf != null)
                {
                    using (var fs = new FileStream(archiveFilenameIn, FileMode.Open, FileAccess.Read))
                    {
                        using (var zf2 = new ZipFile(fs))
                        {
                            var ze = zf2.GetEntry("query.sql");
                            if (ze != null)
                            {
                                RunSqlQuery();

                            }
                        }
                    }

                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }
            }
        }

        public bool RunSqlQuery()
        {
            string sqlFullFileName = Server.MapPath("~/query.sql");
            if (File.Exists(sqlFullFileName))
            {
                var stream = File.OpenText(sqlFullFileName);
                var sql = stream.ReadToEnd();
                var sqlCommand = sql.Split(new string[] { "GO" }, StringSplitOptions.None);
                var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PortalConnectionString"].ConnectionString);
                try
                {
                    connection.Open();
                    foreach (var cmdText in sqlCommand)
                    {
                        if (!string.IsNullOrEmpty(cmdText))
                        {
                            var cmd = new SqlCommand(cmdText, connection);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    var eLogger = new ErrorLogger();
                    eLogger.AddError("خطا در اجرای به روزرسانی بانک اطلاعاتی",
                        "زمان اجرای کدهای بانک اطلاعاتی به روزرسانی خطای زیر رخ داد:<br/>" + ex.Message,
                        Page.User.Identity.Name + "/RunQuery","CoreUpdate.aspx/RunQuery");
                    return false;
                }
                finally
                {
                    connection.Close();
                    stream.Close();
                }
            }
            else
            {
                return false;
            }
        }

    }
}