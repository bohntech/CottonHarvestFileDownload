/****************************************************************************
The MIT License(MIT)

Copyright(c) 2016 Bohn Technology Solutions, LLC

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
****************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CottonHarvestDataTransferApp.Data;
using CottonHarvestDataTransferApp.Remote;
using CottonHarvestDataTransferApp.Configuration;
using System.Threading;
using System.Diagnostics;
using System.Net.NetworkInformation;
using CottonHarvestDataTransferApp.Helpers;

namespace CottonHarvestDataTransferApp.UserControls
{
    public partial class ManagePartners : UserControl
    {
        #region private properties
        IRemoteDataRepository remoteDataRepository = null;
        EditPartner partnerForm = new EditPartner();
        bool loading = false;
        #endregion

        #region private methods
        private void showLoadingMessage(string message)
        {
            loading = true;
            pnlButtonBar.Visible = false;
            dgPartners.Visible = false;
            pnlLoading.Visible = true;
            lblLoadingMessage.Text = message;
        }

        private void hideLoadingMessage()
        {
            loading = false;
            pnlButtonBar.Visible = true;
            dgPartners.Visible = true;
            pnlLoading.Visible = false;
            lblLoadingMessage.Text = "Loading";
        }

        private async Task loadPartnerGrid()
        {
            showLoadingMessage("Loading partner data");
            IEnumerable<Organization> orgs = null;
            dgPartners.Rows.Clear();
            await Task.Run(() =>
            {
                //load partners     
                using (IUnitOfWorkDataProvider p = AppStorage.GetUnitOfWorkDataProvider())
                {
                    orgs = p.Organizations.GetAll();
                }
            });

            foreach (var org in orgs)
            {
                DataGridViewRow row = new DataGridViewRow();
                dgPartners.Rows.Add(org.Id.ToString(), 
                    (!string.IsNullOrEmpty(org.RemoteID) ? org.RemoteID.ToString() : "PENDING"), 
                    org.Name, 
                    org.Email, (org.PermissionGranted) ? "Granted" : "Not Granted" ,
                    org.SharedFiles.ToString(), "MyJohnDeere");
            }           
        }
        #endregion

        #region private event handlers
        private async void PartnerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (partnerForm.DialogResult == DialogResult.OK)
            {
                await loadPartnerGrid();
                hideLoadingMessage();
            }
        }

        private async void btnImportPartners_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to import partnerships from your MyJohnDeere account?  Existing partners will be updated and no partners will be deleted.", "Import Partners", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                List<Partner> results = null;                
                ImportPartnerResult importResult = new ImportPartnerResult();
                showLoadingMessage("Retrieving partnership data");

                await Task.Run(async () => { importResult = await DataHelper.ImportPartners(results, remoteDataRepository, true); });

                if (importResult.Status == ImportStatus.SUCCESS)
                {
                    await loadPartnerGrid();
                }
                else
                {
                    MessageBox.Show(importResult.Message);
                }

                hideLoadingMessage();
            }
        }

        private void btnDeletePartner_Click(object sender, EventArgs e)
        {
            if (dgPartners.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a partner to delete.");
            }
            else
            {
                var rowToDelete = dgPartners.SelectedRows[0];
                int id = Convert.ToInt32(rowToDelete.Cells[0].Value);
                string orgName = (string)rowToDelete.Cells[2].Value;
                if (MessageBox.Show("Are you sure you want to delete " + orgName + "? This does not delete this partner from your MyJohnDeere account. Previously downloaded files will not be deleted.", "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (IUnitOfWorkDataProvider p = AppStorage.GetUnitOfWorkDataProvider())
                    {
                        //delete all recent files
                        var files = p.SourceFiles.FindMatching(f => f.OrganizationId == id).ToList();
                        files.ForEach(f => p.SourceFiles.Delete(f));
                        var org = p.Organizations.FindSingle(f => f.Id == id);
                        p.Organizations.Delete(org);
                        p.SaveChanges();
                    }

                    dgPartners.Rows.Remove(rowToDelete);
                }
            }
        }

        private async void btnEdit_Click(object sender, EventArgs e)
        {
            await partnerForm.SetRemoteRepository(remoteDataRepository);
            if (dgPartners.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a partner to edit.");
            }
            else
            {
                var selectedRow = dgPartners.SelectedRows[0];
                int id = Convert.ToInt32(selectedRow.Cells[0].Value);

                using (IUnitOfWorkDataProvider p = AppStorage.GetUnitOfWorkDataProvider())
                {
                    //delete all recent files                   
                    var org = p.Organizations.FindSingle(f => f.Id == id);
                    partnerForm.ShowEdit(org);   
                }                
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {            
            await partnerForm.SetRemoteRepository(remoteDataRepository);
            partnerForm.ShowAdd();
        }
        #endregion

        #region public properties
        public async Task LoadDataAsync()
        {
            if (!loading)
            {
                await loadPartnerGrid();
                hideLoadingMessage();
            }
        }

        public void SetRemoteDataRepository(IRemoteDataRepository repo)
        {
            remoteDataRepository = repo;
        }
        #endregion

        public ManagePartners()
        {
            InitializeComponent();

            partnerForm.FormClosed += PartnerForm_FormClosed;      
        }
    }
}
