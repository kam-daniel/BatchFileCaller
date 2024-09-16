// author: Daniel L. Van Den Bosch
using System;
using System.Diagnostics;

namespace YourNamespace
{
    public partial class ExecuteBatchFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Output the current user running the ASP.NET application
                string currentUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                Response.Write("<p>Current User: " + currentUser + "</p>");
                Response.Flush(); // Immediately flush the response to the client

                // Render the page structure with the output container
                RenderPageStructure();

                // Start streaming the batch file output
                RunBatchFile();
            }
        }

        private void RenderPageStructure()
        {
            // Render the basic HTML structure including the output container
            Response.Write("<!DOCTYPE html>");
            Response.Write("<html>");
            Response.Write("<head>");
            Response.Write("<title>Execute Batch File</title>");
            Response.Write("<style>");
            Response.Write("body { background-color: black; color: white; font-family: 'Courier New', Courier, monospace; }");
            Response.Write("#outputContainer { list-style-type: none; padding: 0; }");
            Response.Write("#outputContainer li { margin-bottom: 5px; }");
            Response.Write("</style>");
            Response.Write("</head>");
            Response.Write("<body>");
            Response.Write("<form id=\"form1\" runat=\"server\">");
            Response.Write("<ul id=\"outputContainer\"></ul>");
            Response.Write("</form>");
            Response.Write("</body>");
            Response.Write("</html>");

            // Flush the response to send the structure to the client immediately
            Response.Flush();
        }

        private void RunBatchFile()
        {
            try
            {
                // Retrieve the folder and file name from query string
                string folder = Request.QueryString["folder"];
                string fileName = Request.QueryString["fileName"];

                // Validate folder and fileName (ensure they are not null or empty)
                if (string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(fileName))
                {
                    SendOutputToClient("Error: Folder or file name is not provided.");
                    return;
                }

                // Validate folder and fileName (no slashes allowed, no directory traversal characters)
                if (folder.Contains("/") || folder.Contains("\\") || fileName.Contains("/") || fileName.Contains("\\"))
                {
                    SendOutputToClient("Error: Invalid folder or file name.");
                    return;
                }

                // Hardcoded base directory
                string baseDirectory = @"C:/scripts/Time_Series_Data_Transfer_Toolkit";

                // Combine the base directory with the folder and file name (one level deep)
                string fullBatchFilePath = System.IO.Path.Combine(baseDirectory, folder, fileName);

                // Ensure the file exists in the directory
                if (!System.IO.File.Exists(fullBatchFilePath))
                {
                    SendOutputToClient("Error: The specified batch file does not exist.");
                    return;
                }

                // Set up the process to run the batch file
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = fullBatchFilePath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = new Process())
                {
                    process.StartInfo = psi;

                    process.OutputDataReceived += new DataReceivedEventHandler((s, e) => SendOutputToClient(e.Data));
                    process.ErrorDataReceived += new DataReceivedEventHandler((s, e) => SendOutputToClient(e.Data));

                    process.Start();

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                SendOutputToClient("Error: " + ex.Message);
            }
        }

        private void SendOutputToClient(string output)
        {
            if (!string.IsNullOrEmpty(output))
            {
                // Use JavaScript to prepend output as a list item to the output container
                string script = $"<script>document.getElementById('outputContainer').insertAdjacentHTML('afterbegin', '<li>{Server.HtmlEncode(output)}</li>');</script>";
                Response.Write(script);
                Response.Flush(); // Immediately flush the response to the client
            }
        }
    }
}
