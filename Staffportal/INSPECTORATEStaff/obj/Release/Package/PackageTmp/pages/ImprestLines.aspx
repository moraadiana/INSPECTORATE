<%@ Page Language="C#" Title="Imprest Requisition" MasterPageFile="~/pages/Main.Master" AutoEventWireup="true" CodeBehind="ImprestLines.aspx.cs" Inherits="INSPECTORATEStaff.pages.ImprestLines" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="Server">
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <h1>Imprest Requisition
       
                <%--<small>Version 2.0</small>--%>
            </h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Imprest Requisition</li>
            </ol>
        </section>

        <!-- Main content -->
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <asp:MultiView ID="MultiView1" runat="server">
                        <asp:View ID="View1" runat="server">
                            <div class="box box-info box-solid">
                                <div class="box-header with-border">
                                    <h3 class="box-title">New Imprest Requisition</h3>
                                </div>
                                <div class="box-body">
                                    <table class="table">
                                        <thead>
                                            <tr>
                                                <th></th>
                                                <th></th>
                                                <th></th>
                                                <th>
                                                   
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <th>No:</th>
                                                <td>
                                                    <asp:Label ID="lblNo" runat="server" Text="Label"></asp:Label>
                                                </td>
                                                <th>Required Date:</th>
                                                <td>
                                                    <asp:TextBox ID="dtRequiredDate" CssClass="form-control" runat="server" Width="350px"></asp:TextBox>
                                                  
                                                    <script>
                                                        $j('#Main1_dtRequiredDate').Zebra_DatePicker({
                                                            // remember that the way you write down dates
                                                            // depends on the value of the "format" property!
                                                            //direction: [1, false],
                                                            //disabled_dates: ['* * * 0,6']
                                                        });</script>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th style="height: 82px">Department Name:</th>
                                                <td style="height: 82px">
                                                    <asp:DropDownList ID="ddlregions" runat="server" CssClass="form-control select2">
                                                        <asp:ListItem Value="1" Text="INSPECTORATE"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <%--
                                                <th style="height: 82px">--------</th>
                                                <td style="height: 82px">
                                                    <asp:DropDownList ID="ddlAccounytTypes" runat="server" CssClass="form-control select2" Width="350px">
                                                        <asp:ListItem Value="0" Text="G/L Account"></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="Customer"></asp:ListItem>
                                                        <%--<asp:ListItem Value="2" Text="Vendor"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="Bank Account"></asp:ListItem>
                                                    <asp:ListItem Value="4" Text="Fixed Asset"></asp:ListItem>
                                                    <asp:ListItem Value="5" Text="IC Partner"></asp:ListItem>
                                                    </asp:DropDownList></td>
                                            </tr>--%>
                                            <tr>
                                                <th>Responsibility Centre:</th>
                                                <td>
                                                    <asp:DropDownList ID="ddlresponibilitycentres" runat="server" CssClass="form-control select2">
                                                    </asp:DropDownList></td>
                                                <th>Requestor or User Id:</th>
                                                <td>
                                                    <asp:Label ID="lbluserId" runat="server" Text="User Id" CssClass="text-info"></asp:Label></td>
                                            </tr>
                                           <%-- <tr>
                                                <th>----:</th>
                                                <td>
                                                    <asp:DropDownList ID="ddlPrograms" runat="server" CssClass="form-control select2">
                                                        <asp:ListItem Value="1" Text="INSPECTORATE"></asp:ListItem>
                                                    </asp:DropDownList></td>--%>
                                                <th>Status:</th>
                                                <td>
                                                    <asp:Label ID="lblstatus" runat="server" Text="Status" CssClass="text-success"></asp:Label></td>
                                            </tr>
                                            <%--<tr>
                                                        <th>Currency:</th>
                                                        <td>
                                                            <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control" Width="350px"></asp:DropDownList></td>
                                                        <th></th>
                                                        <td></td>
                                                    </tr>--%>
                                            <tr>
                                                <th>Purpose:</th>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtHdescription" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox></td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <th></th>
                                                <td></td>
                                                <td></td>
                                        <div class="row">
                                 <div class="col-sm-12">
                              <div class="form-group">
                                           Attach Document:
                              <asp:FileUpload ID="FileUpload1" runat="server" />
                              </div>
                            </div>
                                  </div>
                                                <td>
                                                    <asp:Button ID="lbtnSubmit" class="btn btn-primary pull-right" runat="server" Text="Send for Approval" OnClick="btnSubmit_Click"/>&nbsp;
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </asp:View>
                    </asp:MultiView>
                </div>
            </div>
        </section>
    </div>
    <!-- BEGIN: modal -->
    <div class="modal fade" id="eventModal" tabindex="-1" role="dialog" aria-labelledby="eventModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content bg-emerland" runat="server" id="dvMdlContentPass" visible="false">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="eventModalLabel">Success!!</h4>
                </div>
                <div class="modal-body">
                    <div>
                        <p>Operation successful.</p>
                    </div>
                </div>
            </div>
            <div class="modal-content bg-alizarin text-white" runat="server" id="dvMdlContentFail" visible="false">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="eventModalLabelf">Error!</h4>
                </div>
                <div class="modal-body">
                    <div>
                        <p id="pMsg" runat="server">Operation failed.</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END: modal -->
</asp:Content>

