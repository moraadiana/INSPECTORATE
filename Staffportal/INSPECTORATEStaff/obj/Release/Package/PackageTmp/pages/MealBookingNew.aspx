<%@ Page Language="C#" Title="Meal Booking" MasterPageFile="~/pages/Main.Master" AutoEventWireup="true" CodeBehind="MealBookingNew.aspx.cs" Inherits="INSPECTORATEStaff.pages.MealBookingNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="Server">
     <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <h1>Meal Booking
       
                <%--<small>Version 2.0</small>--%>
            </h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Meal Booking</li>
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
                                <h3 class="box-title">New Meal Booking</h3>
                            </div>
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-lg-4 col-md-6">
                                        <div class="form-group">
                                            <label>Department</label>
                                            <asp:DropDownList ID="ddlregions" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-lg-4 col-md-6">
                                        <div class="form-group">
                                            <label>Venue</label>
                                            <asp:TextBox ID="txtVenue" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-4 col-md-6">
                                        <div class="form-group">
                                            <label>Pax (No. of People)</label>
                                            <asp:TextBox ID="txtPax" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-6 col-md-6">
                                        <div class="form-group">
                                            <label>Contact Number</label>
                                            <asp:TextBox ID="txtCntct" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 col-md-6">
                                        <div class="form-group">
                                            <label>Email address</label>
                                            <asp:TextBox ID="txtmail" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4 col-md-6">
                                        <div class="form-group">
                                            <label>Booking Date</label>
                                            <asp:TextBox ID="txtbd" class="form-control" runat="server"></asp:TextBox>
                                            <script>
                                                $j('#Main1_txtbd').Zebra_DatePicker({
                                                    // remember that the way you write down dates
                                                    // depends on the value of the "format" property!
                                                    direction: [1, false],
                                                    //format: 'Y-m-d h:i A'
                                                });</script>

                                            <%--<asp:HiddenField runat="server" ID="txtBookingDate" ClientIDMode="Static"></asp:HiddenField>--%>
                                            <br />
                                        </div>
                                    </div>
                                    <div class="col-lg-4 col-md-6">
                                        <div class="form-group">
                                            <label>Required Time (hours:minutes)</label>
                                            <asp:TextBox ID="txtrt" class="form-control" runat="server"></asp:TextBox>
                                            <script>
                                                $j('#Main1_txtrt').Zebra_DatePicker({
                                                    // remember that the way you write down dates
                                                    // depends on the value of the "format" property!
                                                    direction: [1, false],
                                                    format: 'H:i'
                                                });</script>
                                            <%--<asp:HiddenField runat="server" ID="txtRequiredTime" ClientIDMode="Static"></asp:HiddenField>--%>
                                            <br />
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-md-6">
                                        <div class="form-group">
                                            <label>User ID</label><br />
                                            <asp:Label ID="lbluserId" runat="server" Text="User Id" CssClass="text-info"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-md-6">
                                        <div class="form-group">
                                            <label>Status</label><br />
                                            <asp:Label ID="lblstatus" runat="server" Text="Status" CssClass="text-success"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-12 col-md-12">
                                        <label>Description of Meeting</label>
                                        <asp:TextBox ID="txtDesc" class="form-control" TextMode="MultiLine" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-12 col-md-12">
                                        <br />
                                        <asp:LinkButton ID="lbtnApply" class="btn btn-line btn-info pull-right" runat="server" OnClick="lbtnApply_Click"> <i class="fa fa-arrow-circle-o-right"></i> Next</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="View2" runat="server">
                        <div class="box box-warning box-solid">
                            <div class="box-header with-border">
                                <h3 class="box-title">Meal Booking Lines</h3>
                            </div>
                            <div class="box-body">
                                <div id="newLines" runat="server" visible="false">
                                    <asp:LinkButton ID="lbnClose" ToolTip="Close Lines" class="pull-right text-danger" runat="server" OnClick="lbnClose_Click"><i class="fa fa-minus-circle"></i> Close lines</asp:LinkButton>
                                    <div class="row">
                                        <div class="col-lg-2 col-md-4">
                                            <label>No.</label><br />
                                            <asp:Label ID="lblLNo" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-lg-4 col-md-6">
                                            <label>Meal Name</label><br />
                                            <asp:DropDownList ID="ddlMeals" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlMeals_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                        <div class="col-lg-2 col-md-6">
                                            <label>Quantity</label><br />
                                            <asp:TextBox ID="txtQnty" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-lg-2 col-md-6">
                                            <label>Unit price</label><br />
                                            <asp:Label ID="lblPrice" CssClass="label label-info" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                    <div class="col-lg-12 col-md-12">
                                        <br />
                                        <asp:LinkButton ID="lbtnLine" class="btn btn-line btn-info pull-right" runat="server" OnClick="btnLine_Click"> <i class="fa fa-arrow-circle-o-right"></i> Add</asp:LinkButton>
                                    </div>
                                </div>
                                </div>
                                <asp:LinkButton ID="lbnAddLine" ToolTip="Add New Lines" class="pull-right text-info" runat="server" OnClick="lbnAddLine_Click"><i class="fa fa-plus-circle "></i> Add New item</asp:LinkButton>
                                <asp:GridView ID="gvLines" AutoGenerateColumns="false" DataKeyNames="Booking Id" class="table table-responsive no-padding table-bordered table-hover" runat="server"
                                    AllowSorting="True" AllowPaging="true" ShowFooter="true" PageSize="5">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="#No" SortExpression="">
                                            <HeaderStyle Width="30px" />
                                            <ItemTemplate>
                                                <%# string.Format("{0}",Container.DataItemIndex + 1 +".") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Booking Id" HeaderText="Number" />
                                        <asp:BoundField DataField="MealNamee" HeaderText="Description" />
                                        <asp:TemplateField HeaderText="Quantity">
                                            <ItemTemplate>
                                                <%# string.Format("{0:#,##0.00}", ((decimal)Eval("[Quantity]")))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unit Cost">
                                            <ItemTemplate>
                                                <%# string.Format("{0:#,##0.00}", ((decimal)Eval("[Unit Price]")))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Cost">
                                            <ItemTemplate>
                                                <%# string.Format("{0:#,##0.00}", ((decimal)Eval("[varAmount]")))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="Quantity Requested" HeaderText="Quantity" />--%>
                                        <%--<asp:BoundField DataField="varAmount" HeaderText="Cost" />--%>
                                        <%--<asp:BoundField DataField="NumOfDays" HeaderText="Days" />--%>
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
                                        <asp:Button ID="btncancel" runat="server" CssClass="btn btn-danger pull-right" Text="Cancel Approval Request" OnClick="btncancel_Click" />
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
            <div class="modal-content bg-success"  runat="server" id="dvMdlContentPass"  visible="false">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="eventModalLabel">Success!!</h4>
                </div>
                <div class="modal-body">
                    <div>
                         <p>Operation successfull.</p>
                    </div>
                </div>
            </div>
              <div class="modal-content bg-alizarin text-white"  runat="server" id="dvMdlContentFail" visible="false">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="eventModalLabelf">Error!!</h4>
                </div>
                <div class="modal-body">
                    <div>
                         <p id="pMsg" runat="server">The operation has failed.</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
  
    <!-- END: modal -->
    </asp:Content>

