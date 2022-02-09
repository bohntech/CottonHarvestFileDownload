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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using CottonHarvestDataTransferApp.Data;
using CottonHarvestDataTransferApp.Remote;
using CottonHarvestDataTransferApp.Classes;
using CottonHarvestDataTransferApp.Remote.Data;
using CottonHarvestDataTransferApp.Logging;

namespace CottonHarvestDataTransferApp
{
    /// <summary>
    /// This form is used to initiate a new partnership request.
    /// </summary>
    public partial class EditPartner : Form
    {
        #region private properties
        int orgId = 0;
        IRemoteDataRepository remoteRepository = null;
        #endregion

        #region private methods
        private bool ValidateOrganization()
        {
            bool result = true;
            if (string.IsNullOrEmpty(tbOrganization.Text))
            {
                result = false;
                formErrorProvider.SetError(tbOrganization, "Organization name is required.");
            }
            else
            {
                using (IUnitOfWorkDataProvider p = AppStorage.GetUnitOfWorkDataProvider())
                {
                    var existingOrg = p.Organizations.FindSingle(o => o.Name == tbOrganization.Text && o.Id != orgId);

                    if (existingOrg != null)
                    {
                        formErrorProvider.SetError(tbOrganization, "A partner with this name has already been added.");
                        result = false;
                    }
                }
            }

            return result;
        }

        private bool ValidateEmail()
        {
            bool result = true;
            Regex expression = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");

            if (chkRequestPermission.Checked == false && string.IsNullOrEmpty(tbEmail.Text.Trim()))
            {
                result = true;
            }
            else if (string.IsNullOrEmpty(tbEmail.Text))
            {
                formErrorProvider.SetError(tbEmail, "Email is required.");
                result = false;
            }
            else if (!expression.IsMatch(tbEmail.Text.Trim()))
            {
                formErrorProvider.SetError(tbEmail, "Email is not valid.");
                result = false;
            }
            else
            {
                using (IUnitOfWorkDataProvider p = AppStorage.GetUnitOfWorkDataProvider())
                {
                    var existingOrg = p.Organizations.FindSingle(o => o.Email == tbEmail.Text && o.Id != orgId);

                    if (existingOrg != null)
                    {
                        formErrorProvider.SetError(tbEmail, "A partner with this email has already been added.");
                        result = false;
                    }
                }
            }

            return result;
        }

        private bool ValidateForm()
        {
            formErrorProvider.Clear();
            bool validOrg = ValidateOrganization();
            bool ValidEmail = ValidateEmail();
            return validOrg && ValidEmail;
        }
        #endregion

        #region private event handlers
        private void tbOrganization_Validating(object sender, CancelEventArgs e)
        {
            ValidateOrganization();
        }

        private void tbEmail_Validating(object sender, CancelEventArgs e)
        {
            ValidateEmail();
        }

        private void EditPartner_Load(object sender, EventArgs e)
        {
            cboSource.SelectedIndex = 0;

            
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;

            string partnerLink = string.Empty;
            string requestingOrgId = (string) cboMyOrg.SelectedValue;
            if (ValidateForm())
            {
                //try to initiate permission request
                if (chkRequestPermission.Checked)
                {
                    //await Task.Run(async () =>
                    //{
                        try
                        {
                            partnerLink = await remoteRepository.RequestPartnerPermission(tbEmail.Text.Trim(), requestingOrgId);
                        }
                        catch (Exception exc)
                        {
                            Logger.Log(exc);
                            partnerLink = string.Empty;
                        }
                    //});

                    if (string.IsNullOrEmpty(partnerLink))
                    {
                        MessageBox.Show("Unable to initiate partner request.  Check your network connection.");
                        return;
                    }
                }

                using (IUnitOfWorkDataProvider p = AppStorage.GetUnitOfWorkDataProvider())
                {
                    Organization org = new Organization();

                    if (orgId == 0)
                    {
                        //initiate contact request
                        org.RemoteID = string.Empty;
                        org.PartnerLink = partnerLink;
                        org.Created = DateTime.Now;
                        org.PermissionGranted = false;
                        org.FilesETag = "";
                        org.FilesETagDate = DateTime.Now;
                        p.Organizations.Add(org);
                    }
                    else
                    {
                        org = p.Organizations.FindSingle(o => o.Id == orgId);
                        org.Updated = DateTime.Now;
                        p.Organizations.Update(org);
                    }

                    org.Email = tbEmail.Text.Trim();
                    org.Name = tbOrganization.Text.Trim();
                    org.DataSourceId = 1;
                    p.SaveChanges();
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region public methods
        public async Task SetRemoteRepository(IRemoteDataRepository repo)
        {
            remoteRepository = repo;

            cboMyOrg.DataSource = null;
            cboMyOrg.Items.Clear();            
            var myOrgs = await remoteRepository.GetMyOrganizations();
            cboMyOrg.DataSource = myOrgs;
            cboMyOrg.DisplayMember = "Name";
            cboMyOrg.ValueMember = "RemoteId";
        }

        public void ShowAdd()
        {
            if (!this.Visible)
            {
                orgId = 0;
                tbOrganization.Text = "";
                tbEmail.Text = "";
                chkRequestPermission.Enabled = false;
                chkRequestPermission.Checked = true;
                cboMyOrg.SelectedIndex = 0;
                btnSave.Enabled = true;
                this.ShowDialog();
            }
        }

        public void ShowEdit(Organization org)
        {
            if (!this.Visible)
            {
                tbEmail.Text = org.Email;
                tbOrganization.Text = org.Name;
                chkRequestPermission.Enabled = true;
                chkRequestPermission.Checked = false;

                if (!string.IsNullOrEmpty(org.MyLinkedOrgId)) {
                    cboMyOrg.SelectedValue = org.MyLinkedOrgId;
                }
                //lblRequestingOrg.Visible = false;
                orgId = org.Id;
                btnSave.Enabled = true;
                this.ShowDialog();
            }
        }
        #endregion

        public EditPartner()
        {
            InitializeComponent();
        }
    }
}
