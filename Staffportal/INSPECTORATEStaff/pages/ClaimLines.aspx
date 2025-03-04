<%@ Page Language="C#" Title="Claim Requisition" MasterPageFile="~/pages/Main.Master" AutoEventWireup="true" CodeBehind="ClaimLines.aspx.cs" Inherits="INSPECTORATEStaff.pages.ClaimLines"%>

<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="Server">
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <h1>Claim Requisition
       
                <%--<small>Version 2.0</small>--%>
            </h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Claim Requisition</li>
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
                                    <h3 class="box-title">New Claim Requisition</h3>
                                </div>
                                <div class="box-body">
                                    <table class="table">
                                       
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
                                                <th>Department Name:</th>
                                                <td>
                                                    <asp:DropDownList ID="ddlregions" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                               <%-- </td>
                                                <th>Account Type:</th>
                                                <td>
                                                    <asp:DropDownList ID="ddlAccounytTypes" runat="server" CssClass="form-control select2" Width="350px">
                                                        <%--<asp:ListItem Value="0" Text="G/L Account"></asp:ListItem>--%>
                                                        <%--<asp:ListItem Value="1" Text="Customer"></asp:ListItem>--%>
                                                        <%--<asp:ListItem Value="2" Text="Vendor"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="Bank Account"></asp:ListItem>
                                                    <asp:ListItem Value="4" Text="Fixed Asset"></asp:ListItem>
                                                    <asp:ListItem Value="5" Text="IC Partner"></asp:ListItem>--%>
                                                    <%--</asp:DropDownList></td>--%>
                                            </tr>
                                            <tr>
                                                <th>Responsibility Centre:</th>
                                                <td>
                                                    <asp:DropDownList ID="ddlresponibilitycentres" runat="server" CssClass="form-control select2"></asp:DropDownList></td>
                                                <th>Requestor or User Id:</th>
                                                <td>
                                                    <asp:Label ID="lbluserId" runat="server" Text="User Id" CssClass="text-info"></asp:Label></td>
                                            </tr>
                                            <tr>
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
                                            <div class="row">
                                 <div class="col-sm-12">
                              <div class="form-group">
                                           Attach Document:
                              <asp:FileUpload ID="FileUpload1" runat="server" />
                              </div>
                            </div>
                                  </div>
                                            <tr>
                                                <th></th>
                                                <td></td>
                                                <td></td>
                                                <td>
                                                    <asp:Button ID="btnSubmit" class="btn btn-primary pull-right" runat="server" Text="Next" OnClick="btnSubmit_Click" /></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </asp:View>
                        <asp:View ID="View2" runat="server">
                            <div class="box box-info box-solid">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Claim Lines</h3>
                                </div>
                                <div class="box-body">
                                    <div id="newLines" runat="server" visible="false">
                                        <asp:LinkButton ID="lbnClose" ToolTip="Close Lines" class="pull-right" runat="server" OnClick="lbnClose_Click"><i class="fa fa-minus-circle"></i> Close lines</asp:LinkButton>
                                        <table class="table table-hover">
                                            <thead>
                                                <tr>
                                                    <th>No:</th>
                                                    <th>Claim Type</th>
                                                    <th>Amount:</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <th>
                                                        <asp:Label ID="lblLNo" runat="server" Text="Label"></asp:Label></th>
                                                    <td>
                                                        <asp:DropDownList ID="ddlAdvancType" runat="server" CssClass="form-control select2"></asp:DropDownList></td>
                                                    <td>
                                                        <asp:TextBox ID="txtAmnt" CssClass="form-control" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th></th>
                                                    <td></td>
                                                    <td>
                                                        <asp:Button ID="btnLine" class="btn btn-primary pull-right" runat="server" Text="Add" OnClick="btnLine_Click"/></td>
                                                    <td></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <asp:LinkButton ID="lbnAddLine" ToolTip="Add New Lines" class="pull-right text-info" runat="server" OnClick="lbnAddLine_Click" ><i class="fa fa-plus-circle"></i> add Line</asp:LinkButton>
                                    <asp:GridView ID="gvLines" AutoGenerateColumns="false" DataKeyNames="No" class="table table-responsive no-padding table-bordered table-hover" runat="server"
                                        AllowSorting="True" AllowPaging="true" ShowFooter="true" PageSize="5">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="#No" SortExpression="">
                                                <HeaderStyle Width="30px" />
                                                <ItemTemplate>
                                                    <%# string.Format("{0}",Container.DataItemIndex + 1 +".") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="No" HeaderText="Number" />
                                            <asp:BoundField DataField="Advance Type" HeaderText="Advance Type" />
                                            <asp:BoundField DataField="Global Dimension 1 Code" HeaderText="Dimension" />
                                            <asp:BoundField DataField="varAmount" HeaderText="Amount" />
                                            <asp:BoundField DataField="Due Date" HeaderText="Due Date" />
                                             <asp:TemplateField HeaderText="Action" SortExpression="" HeaderStyle-HorizontalAlign="Left">
                                                <ItemStyle Width="110px" HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                     <asp:LinkButton ID="lbtnCancll" CssClass="label label-danger" runat="server" ToolTip="Click to Remove line" OnClick="cancelz" CommandArgument='<%# Eval("Advance Type") %>'><i class="fa fa-remove"></i> Remove</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <EmptyDataTemplate>
                                            <span style="color: red">No Recods</span>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <table class="table table-hover">
                                        <thead>
                                            <tr>
                                                <th></th>
                                                <th></th>
                                                <th></th>
                                                <th></th>
                                                <th>&nbsp;</th>
                                                <th>
                                                    <asp:Button ID="btnApproval" CssClass="btn btn-success pull-right" runat="server" Text="Send For Approval" OnClick="btnApproval_Click" />&nbsp;
                                        <asp:Button ID="btncancel" runat="server" CssClass="btn btn-danger pull-right" OnClick="btncancel_Click" Text="Cancel Approval Request" />
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
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

