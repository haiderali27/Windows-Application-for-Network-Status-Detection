using Network_Detection.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Network_Detection
{
    class nw_Detection: IDisposable
    {
        BackgroundWorker bgWorker_STATUS= new BackgroundWorker();
        
        NotifyIcon ni= new NotifyIcon();
      public  nw_Detection()
        {
            this.bgWorker_STATUS.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorker_STATUS_DoWork);
            this.bgWorker_STATUS.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgWorker_STATUS_ProgressChanged);
            bgWorker_STATUS.WorkerReportsProgress = true;
            bgWorker_STATUS.WorkerSupportsCancellation = true;         
        }
        public void Load()
        {
            ni.Icon = Resources.Base;
            ni.Text = "Network Detection";
            ni.Visible = true;
            ni.ContextMenuStrip = Create();
            bgWorker_STATUS.RunWorkerAsync();
        }

        public ContextMenuStrip Create()
        {

            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item;

            item = new ToolStripMenuItem();
            item.Text = "Exit";
            item.Click += new System.EventHandler(Exit_Click);
            item.Image = Resources.Exit;
            menu.Items.Add(item);

            return menu;
        }
        void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        public System.Net.HttpWebResponse resp;

        public bool CheckNet()
        {

            int desc;
            return InternetGetConnectedState(out desc, 0);
        }
        public bool http_test()
        {
            try
            {

                string website_address;
                website_address = "http://www.google.com/";
                System.Net.WebRequest myWebrequest = System.Net.WebRequest.Create(website_address);
                myWebrequest.Timeout = 3000;
                resp = (System.Net.HttpWebResponse)myWebrequest.GetResponse();

                if (resp.StatusCode.ToString().Equals("OK"))
                {

                    return true;
                }
                else
                {

                    MessageBox.Show("Connecting to the internet failed , please check your internet connection or the website " + website_address);
                    return false;
                }
            }
            catch
            {

                return false;
            }
            finally
            {
                if (resp != null) resp.Close();
            }

        }

        private void bgWorker_STATUS_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            while (true)
            {
                Thread.Sleep(500);
                worker.ReportProgress(0);
            }
        }
        private void bgWorker_STATUS_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (CheckNet() && http_test())
            {

                ni.Icon = Resources.online1;


            }
            else
            {
                ni.Icon = Resources.offline1;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~nw_Detection() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
