using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using JearCarrier.webforms.Models;
using Newtonsoft.Json;

namespace JearCarrier.webforms
{
    public partial class Carriers : Page
    {
        private static readonly HttpClient http = new HttpClient();
        private string ApiBase { get { return ConfigurationManager.AppSettings["ApiBaseUrl"]; } }

        class PagedResult<T>
        {
            public List<T> Items { get; set; }
            public int Total { get; set; }
            public int Page { get; set; }
            public int PageSize { get; set; }
        }

        static Carriers()
        {
            http.Timeout = TimeSpan.FromSeconds(15);
            http.DefaultRequestHeaders.Accept.Clear();
            http.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblFormTitle.Text = "Add carrier";
                btnSave.Text = "Add carrier";
                await LoadTable((txtSearch.Text ?? "").Trim());
            }
        }

        private const int PageSize = 10; // keep in sync with GridView PageSize

        private async Task LoadTable(string q = "")
        {
            try
            {
                int page = gvCarriers.PageIndex + 1;
                string url = $"{ApiBase}/Carrier?page={page}&pageSize={PageSize}";
                if (!string.IsNullOrWhiteSpace(q))
                    url += "&q=" + Uri.EscapeDataString(q);

                using (var res = await http.GetAsync(url))
                {
                    var body = await res.Content.ReadAsStringAsync();

                    if (!res.IsSuccessStatusCode)
                    {
                        lblMsg.Text = $"HTTP {(int)res.StatusCode} {res.ReasonPhrase}. Body: {body}";
                        gvCarriers.DataSource = null;
                        gvCarriers.DataBind();
                        lblCount.Text = "0 carriers";
                        return;
                    }

                    var result = JsonConvert.DeserializeObject<PagedResult<Carrier>>(
                        body,
                        new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Ignore }
                    ) ?? new PagedResult<Carrier> { Items = new List<Carrier>(), Total = 0, Page = 1, PageSize = PageSize };

                    gvCarriers.DataSource = result.Items;
                    gvCarriers.VirtualItemCount = result.Total;   // informs the pager of total rows
                    gvCarriers.DataBind();

                    lblCount.Text = result.Total == 1 ? "1 carrier" : result.Total + " carriers";
                    lblMsg.Text = result.Total == 0 ? "No carriers found." : "";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Load error: " + ex.GetBaseException().Message;
                gvCarriers.DataSource = null;
                gvCarriers.DataBind();
                lblCount.Text = "0 carriers";
            }
        }

        protected async void btnSearch_Click(object sender, EventArgs e)
        {
            gvCarriers.PageIndex = 0; // reset paging on new search
            await LoadTable((txtSearch.Text ?? "").Trim());
        }

        protected async void gvCarriers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCarriers.PageIndex = e.NewPageIndex;
            await LoadTable((txtSearch.Text ?? "").Trim());
        }

        protected async void btnSave_Click(object sender, EventArgs e)
        {
            var carrier = new Carrier
            {
                Id = string.IsNullOrWhiteSpace(hfId.Value) ? 0 : int.Parse(hfId.Value),
                CarrierName = (txtName.Text ?? "").Trim(),
                Address = (txtAddress.Text ?? "").Trim(),
                Address2 = (txtAddress2.Text ?? "").Trim(),
                City = (txtCity.Text ?? "").Trim(),
                State = (txtState.Text ?? "").Trim().ToUpper(),
                Zip = (txtZip.Text ?? "").Trim(),
                Contact = (txtContact.Text ?? "").Trim(),
                Phone = (txtPhone.Text ?? "").Trim(),
                Fax = (txtFax.Text ?? "").Trim(),
                Email = (txtEmail.Text ?? "").Trim()
            };

            try
            {
                string json = JsonConvert.SerializeObject(carrier);
                using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    if (carrier.Id == 0)
                    {
                        using (HttpResponseMessage res = await http.PostAsync($"{ApiBase}/Carrier", content))
                        {
                            string body = await res.Content.ReadAsStringAsync();
                            if (!res.IsSuccessStatusCode)
                            {
                                lblMsg.Text = "POST " + (int)res.StatusCode + " " + res.ReasonPhrase + ". " + body;
                                return;
                            }
                            lblMsg.Text = "Carrier added.";
                        }
                    }
                    else
                    {
                        using (HttpResponseMessage res = await http.PutAsync($"{ApiBase}/Carrier/{carrier.Id}", content))
                        {
                            string body = await res.Content.ReadAsStringAsync();
                            if (!res.IsSuccessStatusCode)
                            {
                                lblMsg.Text = "PUT " + (int)res.StatusCode + " " + res.ReasonPhrase + ". " + body;
                                return;
                            }
                            lblMsg.Text = "Carrier updated.";
                        }
                    }
                }

                ClearForm();
                await LoadTable((txtSearch.Text ?? "").Trim());
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Save error: " + ex.GetBaseException().Message;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
            lblMsg.Text = "";
        }

        private void ClearForm()
        {
            hfId.Value = "";
            txtName.Text = txtAddress.Text = txtAddress2.Text = txtCity.Text =
            txtState.Text = txtZip.Text = txtContact.Text = txtPhone.Text =
            txtFax.Text = txtEmail.Text = "";
            lblFormTitle.Text = "Add carrier";
            btnSave.Text = "Add carrier";
        }

        protected async void gvCarriers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = (int)gvCarriers.DataKeys[e.RowIndex].Value;
                using (HttpResponseMessage res = await http.DeleteAsync($"{ApiBase}/Carrier/{id}"))
                {
                    string body = await res.Content.ReadAsStringAsync();
                    if (!res.IsSuccessStatusCode)
                    {
                        lblMsg.Text = "Delete failed: " + (int)res.StatusCode + " " + res.ReasonPhrase + ". " + body;
                        return;
                    }
                }

                lblMsg.Text = "Carrier deleted.";
                await LoadTable((txtSearch.Text ?? "").Trim());
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Delete error: " + ex.GetBaseException().Message;
            }
        }

        protected async void gvCarriers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "EditRow") return;

            try
            {
                if (!int.TryParse(e.CommandArgument?.ToString(), out var id))
                {
                    lblMsg.Text = "Invalid row id.";
                    return;
                }

                using (HttpResponseMessage res = await http.GetAsync($"{ApiBase}/Carrier/{id}"))
                {
                    string body = await res.Content.ReadAsStringAsync();
                    if (!res.IsSuccessStatusCode)
                    {
                        lblMsg.Text = "Load item failed: " + (int)res.StatusCode + " " + res.ReasonPhrase + ". " + body;
                        return;
                    }

                    Carrier c = JsonConvert.DeserializeObject<Carrier>(body);
                    if (c == null)
                    {
                        lblMsg.Text = "Could not parse carrier.";
                        return;
                    }

                    hfId.Value = c.Id.ToString();
                    txtName.Text = c.CarrierName;
                    txtAddress.Text = c.Address;
                    txtAddress2.Text = c.Address2;
                    txtCity.Text = c.City;
                    txtState.Text = c.State;
                    txtZip.Text = c.Zip;
                    txtContact.Text = c.Contact;
                    txtPhone.Text = c.Phone;
                    txtFax.Text = c.Fax;
                    txtEmail.Text = c.Email;

                    lblFormTitle.Text = "Edit carrier";
                    btnSave.Text = "Save";
                    lblMsg.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Load item error: " + ex.GetBaseException().Message;
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            // Let the API generate CSV so client stays simple
            string q = (txtSearch.Text ?? "").Trim();
            string url = string.IsNullOrWhiteSpace(q)
                ? $"{ApiBase}/Carrier/export"
                : $"{ApiBase}/Carrier/export?q={Uri.EscapeDataString(q)}";

            Response.Redirect(url, false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
