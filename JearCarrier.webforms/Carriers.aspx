<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="Carriers.aspx.cs" Inherits="JearCarrier.webforms.Carriers" %>
<!DOCTYPE html>
<html>
<head runat="server">
  <title>JEAR Carrier</title>
  <meta name="viewport" content="width=device-width, initial-scale=1" />
  <style>
    :root{
      --bg:#f5f6f7;
      --card:#ffffff;
      --ink:#0b0c0f;
      --muted:#6b7280;
      --line:#eceef0;
      --accent:#000000;      /* black */
      --danger:#ff3b30;
      --btn:#f2f3f5;
      --radius:16px;
      --shadow:0 8px 30px rgba(0,0,0,.05);
    }
    *{box-sizing:border-box}
    html,body{height:100%}
    body{margin:0;background:var(--bg);color:var(--ink);font:16px/1.4 -apple-system,BlinkMacSystemFont,"Segoe UI",Roboto,Helvetica,Arial,sans-serif}

    .page{padding:24px}
    .topbar{display:flex;gap:16px;justify-content:space-between;align-items:center;padding:12px 16px;background:var(--card);border-bottom:1px solid var(--line)}
    .logo-container img{height:64px} /* increased size */

    .search-container{display:flex;gap:8px;align-items:center}
    .input{
      padding:10px 12px;border:1px solid var(--line);border-radius:10px;background:#fff;min-width:280px;outline:none
    }
    .btn{padding:10px 14px;border-radius:999px;border:1px solid transparent;background:var(--btn);cursor:pointer}
    .btn:focus{outline:none;box-shadow:0 0 0 3px rgba(0,0,0,.15)}
    .btn-primary{background:var(--accent);color:#fff} /* black */
    .btn-danger{background:var(--danger);color:#fff}
    .btn-ghost{background:#fff;border-color:var(--line);color:var(--ink)}
    .btn-sm{padding:8px 12px;font-size:14px}

    .card{background:var(--card);border:1px solid var(--line);border-radius:var(--radius);box-shadow:var(--shadow);max-width:1250px;margin:25px auto;padding:25px}
    .grid{display:grid;grid-template-columns:repeat(3,1fr);gap:16px}

    /* labels + asterisks */
    .field label{display:block;margin-bottom:6px;font-size:13px;color:var(--muted)}
    .field label .ast{margin-left:4px;color:#9ca3af} /* default gray asterisk */
    .field.invalid label .ast{color:var(--danger)}

    /* input + invalid look */
    .field input[type=text]{width:100%;padding:12px;border:1px solid var(--line);border-radius:12px;background:#fff;outline:none}
    .field input[type=text]:focus{border-color:var(--accent);box-shadow:0 0 0 3px rgba(0,0,0,.12)}
    .field.invalid input[type=text]{border-color:var(--danger);box-shadow:0 0 0 3px rgba(239,68,68,.15)}

    .section-title{margin:0 0 14px;font-weight:700;letter-spacing:.2px}
    .actions{margin-top:12px;display:flex;gap:10px}
    #lblMsg{display:block;margin-top:10px;color:#d00}

    /* table */
    .table{width:100%;border-collapse:separate;border-spacing:0}
    .table thead th{
      text-align:left;font-size:12px;color:var(--muted);font-weight:600;
      padding:12px;border-bottom:1px solid var(--line);background:var(--card)
    }
    .table tbody td{padding:14px 12px;border-bottom:1px solid var(--line);vertical-align:middle}
    .row{background:var(--card)}
    .row:hover{background:#fbfbfc}

    .pill{display:inline-flex;align-items:center;gap:6px;padding:6px 12px;border-radius:999px;border:1px solid var(--line);background:#fff}
    .count{color:var(--muted);font-size:14px}

    .actions-cell{display:flex;gap:8px;justify-content:flex-end}
    .mono{font-variant-numeric:tabular-nums}
    .strong{font-weight:700}

    /* pager */
    .gv-pager{padding:10px;text-align:right}
    .gv-pager a, .gv-pager span{margin:0 4px;padding:6px 10px;border-radius:8px;border:1px solid var(--line);background:#fff;color:var(--ink);text-decoration:none}
    .gv-pager span{background:var(--accent);border-color:var(--accent);color:#fff}
  </style>
</head>
<body>
<form id="form1" runat="server" class="page">
  <!-- topbar -->
  <div class="topbar">
    <div class="logo-container">
      <asp:Image ID="imgLogo" runat="server" ImageUrl="~/Content/Images/jear_logo.png" AlternateText="JEAR Logistics" />
    </div>
    <div class="search-container">
      <asp:TextBox ID="txtSearch" runat="server" CssClass="input" placeholder="Search name, city, or state" />
      <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
    </div>
  </div>

  <!-- form card -->
  <div class="card">
    <h2 class="section-title"><asp:Label runat="server" ID="lblFormTitle" Text="Add Carrier" /></h2>
    <asp:HiddenField ID="hfId" runat="server" />
    <div class="grid">
      <div class="field"><label>Carrier Name<span class="ast">*</span></label><asp:TextBox ID="txtName" runat="server" /></div>
      <div class="field"><label>Address<span class="ast">*</span></label><asp:TextBox ID="txtAddress" runat="server" /></div>
      <div class="field"><label>Address2</label><asp:TextBox ID="txtAddress2" runat="server" /></div>
      <div class="field"><label>City<span class="ast">*</span></label><asp:TextBox ID="txtCity" runat="server" /></div>
      <div class="field"><label>State<span class="ast">*</span></label><asp:TextBox ID="txtState" runat="server" MaxLength="2" /></div>
      <div class="field"><label>Zip<span class="ast">*</span></label><asp:TextBox ID="txtZip" runat="server" CssClass="mono" /></div>
      <div class="field"><label>Contact<span class="ast">*</span></label><asp:TextBox ID="txtContact" runat="server" /></div>
      <div class="field"><label>Phone<span class="ast">*</span></label><asp:TextBox ID="txtPhone" runat="server" CssClass="mono" /></div>
      <div class="field"><label>Fax</label><asp:TextBox ID="txtFax" runat="server" CssClass="mono" /></div>
      <div class="field"><label>Email<span class="ast">*</span></label><asp:TextBox ID="txtEmail" runat="server" /></div>
    </div>
    <div class="actions">
      <asp:Button ID="btnSave" runat="server" Text="Add carrier"
        CssClass="btn btn-primary"
        OnClientClick="return validateCarrierForm();"
        OnClick="btnSave_Click" />
      <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-ghost" OnClick="btnClear_Click" CausesValidation="false" />
    </div>
    <asp:Label ID="lblMsg" runat="server" />
  </div>

  <!-- directory card -->
  <div class="card">
    <div style="display:flex;justify-content:space-between;align-items:center;margin-bottom:8px;">
      <h2 class="section-title" style="margin:0">Directory</h2>
      <div style="display:flex;align-items:center;gap:12px;">
        <span class="pill count"><asp:Label ID="lblCount" runat="server" Text="0 carriers" /></span>
        <asp:Button ID="btnExport" runat="server" Text="Export CSV" CssClass="btn btn-ghost btn-sm" OnClick="btnExport_Click" />
      </div>
    </div>

    <asp:GridView ID="gvCarriers" runat="server"
      AutoGenerateColumns="False"
      DataKeyNames="Id"
      GridLines="None"
      CssClass="table"
      AllowPaging="true"
      AllowCustomPaging="true"
      PageSize="10"
      OnPageIndexChanging="gvCarriers_PageIndexChanging"
      OnRowCommand="gvCarriers_RowCommand"
      OnRowDeleting="gvCarriers_RowDeleting"
      EmptyDataText="No carriers found.">
      <HeaderStyle CssClass="gv-header" />
      <RowStyle CssClass="row" />
      <PagerStyle CssClass="gv-pager" />

      <Columns>
        <asp:TemplateField HeaderText="Carrier Name">
          <ItemTemplate><span class="strong"><%# Eval("CarrierName") %></span></ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Address" HeaderText="Address" />
        <asp:BoundField DataField="City" HeaderText="City" />
        <asp:BoundField DataField="State" HeaderText="State" />
        <asp:BoundField DataField="Zip" HeaderText="Zip" />
        <asp:BoundField DataField="Contact" HeaderText="Contact" />
        <asp:TemplateField HeaderText="Phone">
          <ItemTemplate><a class="mono" href='tel:<%# Eval("Phone") %>'><%# Eval("Phone") %></a></ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Email">
          <ItemTemplate><a href='mailto:<%# Eval("Email") %>'><%# Eval("Email") %></a></ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Fax" HeaderText="Fax" />
        <asp:TemplateField HeaderText="Actions">
          <ItemStyle HorizontalAlign="Right" />
          <ItemTemplate>
            <div class="actions-cell">
              <asp:LinkButton ID="btnEdit" runat="server" Text="Edit"
                CommandName="EditRow"
                CommandArgument='<%# Eval("Id") %>'
                CssClass="btn btn-ghost btn-sm" />
              <asp:LinkButton ID="btnDelete" runat="server" Text="Delete"
                CommandName="Delete"
                OnClientClick="return confirm('Delete this carrier?');"
                CssClass="btn btn-danger btn-sm" />
            </div>
          </ItemTemplate>
        </asp:TemplateField>
      </Columns>
    </asp:GridView>
  </div>

  <script>
      // validation
      function validateCarrierForm() {
          const fields = [
              { id: 'txtName', min: 1 },
              { id: 'txtAddress', min: 1 },
              { id: 'txtCity', min: 1 },
              { id: 'txtState', min: 2, max: 2 },
              { id: 'txtZip', min: 1 },
              { id: 'txtContact', min: 1 },
              { id: 'txtPhone', min: 1 },
              { id: 'txtEmail', min: 1, email: true }
          ];
          let ok = true;

          fields.forEach(f => {
              const input = document.getElementById(f.id);
              if (!input) return;
              const wrapper = input.parentElement; // .field
              const value = (input.value || '').trim();
              let valid = value.length >= (f.min || 0);
              if (valid && f.max) valid = value.length <= f.max;
              if (valid && f.email) valid = /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);
              wrapper.classList.toggle('invalid', !valid);
              if (!valid) ok = false;
          });
          return ok; // block postback if false
      }

      // clean invalid when typing
      document.addEventListener('input', e => {
          const el = e.target;
          if (el && el.closest('.field')) {
              el.closest('.field').classList.remove('invalid');
          }
      });

      // keep focus ring only for keyboard nav
      (function () {
          let mouseDown = false;
          document.addEventListener('mousedown', () => mouseDown = true);
          document.addEventListener('mouseup', () => mouseDown = false);
          document.addEventListener('focusin', e => { if (mouseDown) e.target.style.outline = 'none'; });
      })();
  </script>
</form>
</body>
</html>

