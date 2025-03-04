<%@ Page Language="C#" Title="Venue Booking" MasterPageFile="~/pages/Main.Master" AutoEventWireup="true" CodeBehind="VenueListing.aspx.cs" Inherits="INSPECTORATEStaff.pages.VenueListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="Server">
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <h1>Venue Booking
       
                <%--<small>Version 2.0</small>--%>
            </h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Venue Booking</li>
            </ol>
        </section>

        <!-- Main content -->
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-success">
                        <div class="box-header with-border">
                            <h3 class="box-title">Venue BooKing List</h3>

                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body">
                             <p class="text-center"><a id="btnTravelRequest" runat="server" onserverclick="btnTravelRequest_ServerClick"><span class="btn btn-pill btn-success u-posRelative"><i class="fa fa-building-o"></i> Request for venue</span></a></p>
                            <%=Session["ResponseMsg"] %>
                            <br />
                            <div class="table-responsive">
                                <table id="example1" class="table no-margin">
                                    <thead>
                                        <tr>
                                            <th class="small">Ref No</th>
                                            <th class="small">Description</th>
                                            <th class="small">Venue</th>
                                            <th class="small">Request Date</th>
                                            <th class="small">Booking Date</th>
                                            <th class="small">Required Time</th>
                                            <th class="small">No of People</th>
                                            <th class="small">Status</th>
                                            <th class="small">Action</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <%=Jobs()%>
                                    </tbody>
                                </table>
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
    <div class="modal fade msg-modal" tabindex="-1" role="dialog" aria-labelledby="delModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content" visible="false">
                <div class="modal-header bg-success padding-10">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h5 class="modal-title">Success!</h5>
                </div>
                <div class="modal-body padding-10">
                    <p>Your Venue booking has been successfully sent for Approval.</p>
                </div>
            </div>
            <div class="modal-content" visible="false">
                <div class="modal-header bg-danger padding-10">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h5 class="modal-title" id="hTitle" runat="server">Failed!</h5>
                </div>
                <div class="modal-body padding-10">
                </div>
            </div>
        </div>
    </div>
    <!-- BEGIN: modal -->
    <div class="modal fade" id="eventModal" tabindex="-1" role="dialog" aria-labelledby="eventModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content bg-emerland" runat="server" id="dvMdlContentPass" visible="false">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="eventModalLabel">Suucess!!</h4>
                </div>
                <div class="modal-body">
                    <div>
                        <p>Your Venue Request has been successfully sent for Approval.</p>
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
                        <p id="pMsg" runat="server">You have a Pending venue Application.</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- /.content-wrapper -->
</asp:Content>

