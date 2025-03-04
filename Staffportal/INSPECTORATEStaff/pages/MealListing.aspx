<%@ Page Language="C#" Title="Meal Listing" MasterPageFile="~/pages/Main.Master" AutoEventWireup="true" CodeBehind="MealListing.aspx.cs" Inherits="INSPECTORATEStaff.pages.MealListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="Server">
      <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <h1>Meal Listing
       
                <%--<small>Version 2.0</small>--%>
            </h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Meal Listing</li>
            </ol>
        </section>

        <!-- Main content -->
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-info">
                        <div class="box-header with-border">
                            <h3 class="box-title"><i class="fa fa-coffee"></i> My Meal Listing</h3>

                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body">
                             <p class="text-center">
                                <a class="btn btn-pill btn-info u-posRelative" href="MealBookingNew.aspx?Tp=new"><i class="fa fa-coffee"></i> New Meal Booking<span class="waves"></span></a>
                                <%=Session["ResponseMsg"] %>
                            </p>
                             <div class="col-md-12">
                                <div class="box">
                                    <div class="box-header with-border">
                                        <h4 class="box-title"><i class="fa fa-list-ul"></i> Meal Booking List</h4>
                                    </div>
                                    <div class="box-body collapse in">
                                          <asp:GridView ID="gvMealBookings" runat="server" AllowPaging="true" AllowSorting="true" AutoGenerateColumns="false"
                                DataKeyNames="Booking Id,StatusDesc" EmptyDataText="You have no Meal Bookings, Add a Meal Booking" ShowHeaderWhenEmpty="true"
                                OnRowCommand="gvMealBookings_RowCommand" OnRowDeleting="gvMealBookings_RowDeleting" OnRowCreated="gvMealBookings_RowCreated"
                                CssClass="table table-bordered" BorderWidth="0" ShowFooter="true" PageSize="20">
                                <Columns>
                                    <asp:TemplateField HeaderText="#">
                                        <ItemTemplate><%# Container.DataItemIndex + 1 %> </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Booking Id" HeaderText="Ref No" ItemStyle-CssClass="text-uppercase" SortExpression="Booking Id" ReadOnly="true" />
                                    <asp:BoundField DataField="Request Date" HeaderText="Request Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="Request Date" ReadOnly="true" />
                                    <asp:TemplateField HeaderText="Booking Date" ItemStyle-CssClass="text-uppercase" SortExpression="Booking Date">
                                        <ItemTemplate><%# Eval("Booking Date","{0:dd/MM/yyyy}") + ' ' + Eval("Booking Time","{0:t}") %> </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Required Time" DataFormatString="{0:hh:mm}" HeaderText="Duration(hrs)" ItemStyle-CssClass="text-uppercase" SortExpression="Required Time" ReadOnly="true" />
                                    <asp:BoundField DataField="Meeting Name" HeaderText="Meeting Name" ItemStyle-CssClass="text-capitalize" SortExpression="Meeting Name" ReadOnly="true" />
                                    <asp:BoundField DataField="Venue" HeaderText="Venue" ItemStyle-CssClass="text-capitalize" SortExpression="Venue" ReadOnly="true" />
                                    <asp:BoundField DataField="Pax" HeaderText="Pax" ItemStyle-CssClass="text-uppercase" SortExpression="Pax" ReadOnly="true" />
                                    <asp:TemplateField HeaderText="Status" ItemStyle-CssClass="text-capitalize" SortExpression="Status">
                                        <ItemTemplate>
                                            <span class='label label-<%# getCssClass(Eval("StatusDesc").ToString(),"sdesc") %>'><%# Eval("StatusDesc")  %></span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actions" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="small text-center">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblEdt">
                                                <i class='fa fa-coffee text-primary' title='edit'></i>
                                                <asp:LinkButton ID="btnEditMealBooking" runat="server" CssClass="text-info" CommandName="Redirect" Style="padding-right: 5px;">Edit Booking</asp:LinkButton>
                                                &nbsp;|&nbsp;
                                            </asp:Label>
                                            <asp:Label runat="server" ID="lblDel">
                                                <i class='fa fa-trash text-danger' title='delete'></i>
                                                <asp:LinkButton ID="btnDeleteMealBooking" runat="server" modalID="del-modal" CssClass="text-danger" OnClientClick="return getModal(event)" CommandName="Delete">Delete Booking</asp:LinkButton>
                                            </asp:Label>
                                            <asp:Label runat="server" ID="lblView" Visible="false">
                                                <i class='fa fa-coffee text-primary' title='view details'></i>
                                                <asp:LinkButton ID="btnViewMealBooking" runat="server" CssClass="pointer text-primary" CommandName="Redirect">View Details</asp:LinkButton>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataRowStyle CssClass="small text-center" ForeColor="#ff3300" />
                                <HeaderStyle CssClass="small" BorderWidth="0" ForeColor="#525252" />
                                <RowStyle CssClass="small" BorderWidth="0" ForeColor="#337ab7" />
                                <FooterStyle CssClass="small" Font-Bold="true" />
                            </asp:GridView>
                                    </div>
                                </div>
                            </div>
                          
                            <%--success, delete confirm modals--%>
                            <div class="modal fade del-modal" tabindex="-1" role="dialog" aria-labelledby="delModalLabel">
                                <div class="modal-dialog" role="document">
                                    <div class="modal-content">
                                        <div class="modal-header bg-danger padding-10">
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                            <h5 class="modal-title">Delete Meal Bookings?</h5>
                                        </div>
                                        <div class="modal-body padding-10">
                                            <p>Deleting this Booking will permanently remove it from your List of Meal Bookings.</p>
                                        </div>
                                        <div class="modal-footer padding-5">
                                            <button type="button" class="btn btn-sm btn-default" data-dismiss="modal">No, keep this Booking</button>
                                            <button id="btnModalConfirm" type="button" class="btn btn-sm btn-danger">Yes, delete this Booking</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal fade msg-modal" tabindex="-1" role="dialog" aria-labelledby="delModalLabel">
                                <div class="modal-dialog" role="document">
                                    <div runat="server" id="dvMdlContentPass" class="modal-content" visible="false">
                                        <div class="modal-header bg-success padding-10">
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                            <h5 class="modal-title">Success!</h5>
                                        </div>
                                        <div class="modal-body padding-10">
                                            <p>Your Meal Booking has been successfully sent for Approval.</p>
                                        </div>
                                    </div>
                                    <div runat="server" id="dvMdlContentFail" class="modal-content" visible="false">
                                        <div class="modal-header bg-danger padding-10">
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                            <h5 class="modal-title">Failed!</h5>
                                        </div>
                                        <div class="modal-body padding-10">
                                            <p>Your Meal Booking has not been successfully sent for Approval. Please check and Try Again.</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- /.table-responsive -->
                        </div>
                        <!-- /.box-body -->
                      <%--  <div class="box-footer clearfix">
                            <a href="javascript:void(0)" class="btn btn-sm btn-info btn-flat pull-left">Place New Order</a>
                            <a href="javascript:void(0)" class="btn btn-sm btn-default btn-flat pull-right">View All Orders</a>
                        </div>--%>
                        <!-- /.box-footer -->
                    </div>
                </div>
                <!-- /.col -->
            </div>
        </section>
        <!-- /.content -->
    </div>
    <!-- /.content-wrapper -->
    </asp:Content>

